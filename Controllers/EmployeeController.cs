using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using Test_App.Models;

namespace Test_App.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public EmployeeController(IConfiguration config, IWebHostEnvironment env)
        {
            _config = config;
            _env = env;
        }

        private int UserId =>
            HttpContext.Session.GetInt32("UserId").Value;

        // ================= LIST =================
        //public IActionResult Index()
        //{
        //    List<Employee> list = new();

        //    using SqlConnection con =
        //        new SqlConnection(_config.GetConnectionString("dbcs"));

        //    SqlCommand cmd = new SqlCommand("sp_Employee_GetByUser", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@UserId", UserId);

        //    con.Open();
        //    SqlDataReader dr = cmd.ExecuteReader();

        //    while (dr.Read())
        //    {
        //        list.Add(new Employee
        //        {
        //            EmployeeId = (int)dr["EmployeeId"],
        //            FullName = dr["FullName"].ToString(),
        //            Email = dr["Email"].ToString(),
        //            Phone = dr["Phone"].ToString(),
        //            Salary = (decimal)dr["Salary"],
        //            Photo = dr["Photo"].ToString()
        //        });
        //    }
        //    return View(list);
        //}
        public IActionResult Index()
        {
            List<Employee> list = new();

            string role = HttpContext.Session.GetString("Role");
            int userId = HttpContext.Session.GetInt32("UserId").Value;

            using SqlConnection con =
                new SqlConnection(_config.GetConnectionString("dbcs"));

            SqlCommand cmd;

            // ===== ADMIN: SEE ALL =====
            if (role == "Admin")
            {
                cmd = new SqlCommand("sp_Employee_GetAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
            }
            // ===== USER: SEE OWN DATA =====
            else
            {
                cmd = new SqlCommand("sp_Employee_GetByUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
            }

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            while (dr.Read())
            {
                list.Add(new Employee
                {
                    EmployeeId = (int)dr["EmployeeId"],
                    FullName = dr["FullName"].ToString(),
                    Email = dr["Email"].ToString(),
                    Phone = dr["Phone"].ToString(),
                    Salary = (decimal)dr["Salary"],
                    Photo = dr["Photo"].ToString()
                });
            }

            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee emp)
        {
            if (!ModelState.IsValid)
                return View(emp);

            string fileName = null;

            if (emp.PhotoFile != null)
            {
                fileName = Guid.NewGuid() + Path.GetExtension(emp.PhotoFile.FileName);
                string path = Path.Combine(_env.WebRootPath, "EmployeeImages", fileName);
                using FileStream fs = new(path, FileMode.Create);
                emp.PhotoFile.CopyTo(fs);
            }

            using SqlConnection con =
                new SqlConnection(_config.GetConnectionString("dbcs"));

            SqlCommand cmd = new SqlCommand("sp_Employee_Insert", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@FullName", emp.FullName);
            cmd.Parameters.AddWithValue("@Email", emp.Email);
            cmd.Parameters.AddWithValue("@Phone", emp.Phone);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            cmd.Parameters.AddWithValue("@Photo", fileName);
            cmd.Parameters.AddWithValue("@UserId", UserId);

            con.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        // ================= EDIT =================
        [HttpGet]
        public IActionResult Edit(int id)
        {
            Employee emp = new();

            using SqlConnection con =
                new SqlConnection(_config.GetConnectionString("dbcs"));

            SqlCommand cmd = new SqlCommand("sp_Employee_GetById", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", id);

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            if (dr.Read())
            {
                emp.EmployeeId = id;
                emp.FullName = dr["FullName"].ToString();
                emp.Email = dr["Email"].ToString();
                emp.Phone = dr["Phone"].ToString();
                emp.Salary = (decimal)dr["Salary"];
                emp.Photo = dr["Photo"].ToString();
            }
            return View(emp);
        }

        [HttpPost]
        public IActionResult Edit(Employee emp)
        {
            string fileName = emp.Photo;

            if (emp.PhotoFile != null)
            {
                fileName = Guid.NewGuid() + Path.GetExtension(emp.PhotoFile.FileName);
                string path = Path.Combine(_env.WebRootPath, "EmployeeImages", fileName);
                using FileStream fs = new(path, FileMode.Create);
                emp.PhotoFile.CopyTo(fs);
            }

            using SqlConnection con =
                new SqlConnection(_config.GetConnectionString("dbcs"));

            SqlCommand cmd = new SqlCommand("sp_Employee_Update", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@EmployeeId", emp.EmployeeId);
            cmd.Parameters.AddWithValue("@FullName", emp.FullName);
            cmd.Parameters.AddWithValue("@Phone", emp.Phone);
            cmd.Parameters.AddWithValue("@Salary", emp.Salary);
            cmd.Parameters.AddWithValue("@Photo", fileName);

            con.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }

        // ================= DELETE =================
        //public IActionResult Delete(int id)
        //{
        //    using SqlConnection con =
        //        new SqlConnection(_config.GetConnectionString("dbcs"));

        //    SqlCommand cmd = new SqlCommand("sp_Employee_Delete", con);
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.Parameters.AddWithValue("@EmployeeId", id);

        //    con.Open();
        //    cmd.ExecuteNonQuery();

        //    return RedirectToAction("Index");
        //}

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Role") != "Admin")
            {
                return Unauthorized();
            }

            using SqlConnection con =
                new SqlConnection(_config.GetConnectionString("dbcs"));

            SqlCommand cmd = new SqlCommand("sp_Employee_Delete", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@EmployeeId", id);

            con.Open();
            cmd.ExecuteNonQuery();

            return RedirectToAction("Index");
        }
    }
}
