using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataContext;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class SalesController : Controller
    {
        private readonly SalesContext _context;

        public SalesController(SalesContext context)
        {
            _context = context;
        }

        // GET: Sales
        public async Task<IActionResult> Index()
        {
              return View(await _context.Sales.ToListAsync());
        }

        // GET: Sales/Create
        public IActionResult Create()
        {
            return View();
        }

        public static string GetTotalRevenue()
        {
            SqlConnection conn = new SqlConnection("Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False");

            string command = $"declare @a int exec @a = totalRevenue select @a";
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            return cmd.ExecuteScalar().ToString();
            conn.Close();
        }
        public static string GetPrice(int dish_id)
        {
            SqlConnection conn = new SqlConnection("Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False");

            string command = $"select price from dish where id = {dish_id}";
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            return cmd.ExecuteScalar().ToString();
            conn.Close();
        }
        public static string GetDishName(int dish_id)
        {
            SqlConnection conn = new SqlConnection("Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False");

            string command = $"select name from dish where id = {dish_id}";
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            return cmd.ExecuteScalar().ToString();
            conn.Close();
        }
        public static string GetTypeName(int type_id)
        {
            SqlConnection conn = new SqlConnection("Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False");

            string command = $"select name from type where id = {type_id}";
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = CommandType.Text;
            conn.Open();
            return cmd.ExecuteScalar().ToString();
            conn.Close();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Dish_id,Type_id,Date_of_sale")] Sales sale)
        {
            try
            {
                SqlConnection conn = new SqlConnection("Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False");
                if (ModelState.IsValid)
                {
                    string command = $"declare @dishId int = {sale.Dish_id}, @typeId int = {sale.Type_id}, @date datetime = getdate() exec addSale @dishId, @typeId, @date";
                    SqlCommand cmd = new SqlCommand(command, conn);
                    cmd.CommandType = CommandType.Text;
                    conn.Open();
                    cmd.ExecuteScalar();
                    conn.Close();
                    return RedirectToAction(nameof(Index));
                }
                return View(sale);
            }
            catch (Exception ex)
            {
                TempData["ErrorMes"] = ex.Message;
                return RedirectToAction("Create", "Sales");
            }
        }

        // GET: Sales/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sales = await _context.Sales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(sales);
        }

        // GET: Sales/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Sales == null)
            {
                return NotFound();
            }

            var sales = await _context.Sales
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sales == null)
            {
                return NotFound();
            }

            return View(sales);
        }

        // POST: Sales/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Sales == null)
            {
                return Problem("Entity set 'SalesContext.Sales'  is null.");
            }
            var sales = await _context.Sales.FindAsync(id);
            if (sales != null)
            {
                _context.Sales.Remove(sales);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SalesExists(int id)
        {
          return _context.Sales.Any(e => e.Id == id);
        }
    }
}
