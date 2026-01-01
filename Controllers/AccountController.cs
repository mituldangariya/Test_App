using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Test_App.Models;

namespace Test_App.Controllers
{
    public class AccountController : Controller
    {
        private readonly IConfiguration _config;

        public AccountController(IConfiguration config)
        {
            _config = config;
        }


        // ================= REGISTER =================

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                string hash = PasswordHelper.Hash(model.Password);

                using SqlConnection con =
                    new SqlConnection(_config.GetConnectionString("dbcs"));

                SqlCommand cmd = new SqlCommand("sp_User_Register", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FullName", model.FullName);
                cmd.Parameters.AddWithValue("@Email", model.Email);
                cmd.Parameters.AddWithValue("@PasswordHash", hash);
                cmd.Parameters.AddWithValue("@Role", model.Role);

                con.Open();
                cmd.ExecuteNonQuery();

                return RedirectToAction("Login");
            }
            catch (SqlException ex)
            {
                ViewBag.Error = ex.Message;
                return View(model);
            }
        }

        // ================= LOGIN =================

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            using SqlConnection con =
                new SqlConnection(_config.GetConnectionString("dbcs"));

            SqlCommand cmd = new SqlCommand("sp_User_Login", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", model.Email);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                string dbHash = dr["PasswordHash"].ToString();
                string inputHash = PasswordHelper.Hash(model.Password);

                if (dbHash == inputHash)
                {
                    HttpContext.Session.SetInt32("UserId", (int)dr["UserId"]);
                    HttpContext.Session.SetString("UserName", dr["FullName"].ToString());
                    HttpContext.Session.SetString("Role", dr["Role"].ToString());

                    return RedirectToAction("Index", "Employee");
                }
            }

            ViewBag.Error = "Invalid Email or Password";
            return View(model);
        }


        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }


    }
}
