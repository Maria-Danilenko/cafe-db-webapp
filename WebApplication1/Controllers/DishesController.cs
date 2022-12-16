using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataContext;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class DishesController : Controller
    {
        private readonly DishesContext _context;

        public DishesController(DishesContext context)
        {
            _context = context;
        }
        public static string CountDishes()
        {
            SqlConnection conn = new SqlConnection("Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False");

            string command = "select p from dbo.getCount()";
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            conn.Open();
            return cmd.ExecuteScalar().ToString();
            conn.Close();
        }

        public static string AvgPrice()
        {
            SqlConnection conn = new SqlConnection("Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False");

            string command = "select p from dbo.getAvgPrice2()";
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            conn.Open();
            return cmd.ExecuteScalar().ToString();
            conn.Close();
        }

        public static string GetTypeName(int dish_id)
        {
            SqlConnection conn = new SqlConnection("Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False");

            string command = $"select t.name from type t inner join dish d on t.id = d.type_id where d.type_id = t.id and d.id = {dish_id}";
            SqlCommand cmd = new SqlCommand(command, conn);
            cmd.CommandType = System.Data.CommandType.Text;
            conn.Open();
            return cmd.ExecuteScalar().ToString();
            conn.Close();
        }

        // GET: Dishes
        public async Task<IActionResult> Index(string searchStr)
        {
            ViewData["CurrentFilter"] = searchStr;
            
            var dishes = from d in _context.Dish select d;

            if (!String.IsNullOrEmpty(searchStr))
            {
                dishes = dishes.Where(d => d.Type_id.ToString() == searchStr);
            }

            dishes = dishes.OrderBy(d => d.Id);

            return View(await dishes.AsNoTracking().ToListAsync());
        }

        // GET: Dishes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // GET: Dishes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Dishes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Type_id")] Dish dish)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(dish);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(dish);
            }
            catch (Exception ex)
            {
                TempData["ErrorMes"] = ex.Message;
                return RedirectToAction("Create", "Dishes");
            }
        }

        // GET: Dishes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish.FindAsync(id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        // POST: Dishes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Type_id")] Dish dish)
        {
            if (id != dish.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(dish);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DishExists(dish.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(dish);
            }
            catch (Exception ex)
            {
                TempData["ErrorMes"] = ex.Message;
                return RedirectToAction("Edit", "Dishes");
            }
        }

        // GET: Dishes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Dish == null)
            {
                return NotFound();
            }

            var dish = await _context.Dish
                .FirstOrDefaultAsync(m => m.Id == id);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        // POST: Dishes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Dish == null)
            {
                return Problem("Entity set 'DishesContext.Dish'  is null.");
            }
            var dish = await _context.Dish.FindAsync(id);
            if (dish != null)
            {
                _context.Dish.Remove(dish);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DishExists(int id)
        {
          return _context.Dish.Any(e => e.Id == id);
        }
    }
}
