using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Mvc;
using Inventory_module.Models;

namespace Inventory_module.Controllers
{
    public class ProductController : Controller
    {
        // Database connection string
        string cs = @"Data Source=(localdb)\MSSQLLocalDB;
                      Initial Catalog=Inventory_DB;
                      Integrated Security=True";

        // INDEX - Display all products
        public ActionResult Index()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products";
                SqlCommand cmd = new SqlCommand(query, con);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                while (dr.Read())
                {
                    products.Add(new Product
                    {
                        ProductId = Convert.ToInt32(dr["ProductId"]),
                        ProductName = dr["ProductName"].ToString(),
                        Quantity = Convert.ToInt32(dr["Quantity"]),
                        Price = Convert.ToDecimal(dr["Price"])
                    });
                }
            }

            return View(products);
        }

        // CREATE - Display form (GET)
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        // CREATE - Save product (POST)
        [HttpPost]
        public ActionResult Create(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"INSERT INTO Products
                                 (ProductName, Quantity, Price)
                                 VALUES (@name, @qty, @price)";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@name", p.ProductName);
                cmd.Parameters.AddWithValue("@qty", p.Quantity);
                cmd.Parameters.AddWithValue("@price", p.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // EDIT - Display existing product (GET)
        [HttpGet]
        public ActionResult Edit(int id)
        {
            Product p = new Product();

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM Products WHERE ProductId=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.Read())
                {
                    p.ProductId = Convert.ToInt32(dr["ProductId"]);
                    p.ProductName = dr["ProductName"].ToString();
                    p.Quantity = Convert.ToInt32(dr["Quantity"]);
                    p.Price = Convert.ToDecimal(dr["Price"]);
                }
                else
                {
                    dr.Close();
                    return HttpNotFound();
                }
            }

            return View(p);
        }

        // EDIT - Update product (POST)
        [HttpPost]
        public ActionResult Edit(Product p)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = @"UPDATE Products SET
                                 ProductName=@name,
                                 Quantity=@qty,
                                 Price=@price
                                 WHERE ProductId=@id";

                SqlCommand cmd = new SqlCommand(query, con);

                cmd.Parameters.AddWithValue("@id", p.ProductId);
                cmd.Parameters.AddWithValue("@name", p.ProductName);
                cmd.Parameters.AddWithValue("@qty", p.Quantity);
                cmd.Parameters.AddWithValue("@price", p.Price);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }

        // DELETE - Remove product (POST)
        [HttpPost]
        public ActionResult Delete(int id)
        {
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "DELETE FROM Products WHERE ProductId=@id";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index");
        }
    }
}
