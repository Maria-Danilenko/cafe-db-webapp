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
    public class ProvidersController : Controller
    {
        DateTime date;

        private readonly ProvidersContext _context;

        public ProvidersController(ProvidersContext context)
        {
            _context = context;
        }

        // GET: Providers
        public async Task<IActionResult> Index(string searchStr)
        {
            ViewData["CurrentFilter"] = searchStr;

            var providers = from p in _context.Provider select p;

            if (!String.IsNullOrEmpty(searchStr))
            {
                providers = providers.Where(p => p.Name.Contains(searchStr));
            }

            return View(await providers.AsNoTracking().ToListAsync());
        }

        // GET: Providers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Provider == null)
            {
                return NotFound();
            }

            var provider = await _context.Provider
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        // GET: Providers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Providers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Company_name,Date_of_contract_sign,Ingredients_count")] Provider provider)
        {
            if (provider.Date_of_contract_sign.CompareTo(DateTime.Now) > date.CompareTo(DateTime.Now))
            {
                ModelState.AddModelError("Date_of_contract_sign", "The date of signing the contract cannot be in the future");
            }

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(provider);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(provider);
            }
            catch (Exception ex)
            {
                TempData["ErrorMes"] = ex.Message;
                return RedirectToAction("Create", "Providers");
            }
        }

        // GET: Providers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Provider == null)
            {
                return NotFound();
            }

            var provider = await _context.Provider.FindAsync(id);
            if (provider == null)
            {
                return NotFound();
            }
            return View(provider);
        }

        // POST: Providers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Company_name,Date_of_contract_sign,Ingredients_count")] Provider provider)
        {
            if (id != provider.Id)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(provider);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ProviderExists(provider.Id))
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
                return View(provider);
            }
            catch(Exception ex)
            {
                TempData["ErrorMes"] = ex.Message;
                return RedirectToAction("Edit", "Providers");
            }
        }

        // GET: Providers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Provider == null)
            {
                return NotFound();
            }

            var provider = await _context.Provider
                .FirstOrDefaultAsync(m => m.Id == id);
            if (provider == null)
            {
                return NotFound();
            }

            return View(provider);
        }

        // POST: Providers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Provider == null)
            {
                return Problem("Entity set 'ProvidersContext.Provider'  is null.");
            }
            var provider = await _context.Provider.FindAsync(id);
            if (provider != null)
            {
                _context.Provider.Remove(provider);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProviderExists(int id)
        {
          return _context.Provider.Any(e => e.Id == id);
        }
    }
}
