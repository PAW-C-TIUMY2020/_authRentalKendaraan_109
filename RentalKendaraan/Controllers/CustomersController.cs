using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RentalKendaraan.Models;
using RentalKendaraan_109.Models;

namespace RentalKendaraan_109.Controllers
{
    public class CustomersController : Controller
    {
        private readonly RentKendaraanContext _context;

        public CustomersController(RentKendaraanContext context)
        {
            _context = context;
        }

        // GET: Customers
        public async Task<IActionResult> Index(string ktsd, string searchString, string currentFilter, int? pageNumber, string sortOrder)
        {
            //Buat list menyimpan ketersediaan
            var ktsdList = new List<string>();

            //Query mengambil data
            var ktsdQuery = from d in _context.Customer orderby d.NamaCustomer select d.NamaCustomer;
            ktsdList.AddRange(ktsdQuery.Distinct());

            //Untuk menampilkan di view
            ViewBag.ktsd = new SelectList(ktsdList);

            //Panggil db content
            var menu = from m in _context.Customer.Include(k => k.IdGenderNavigation) select m;

            //Untuk memilih dropdownlist ketersediaan
            if (!string.IsNullOrEmpty(ktsd))
            {
                menu = menu.Where(x => x.NamaCustomer == ktsd);
            }

            //Untuk search data
            if (!string.IsNullOrEmpty(searchString))
            {
                menu = menu.Where(s => s.NamaCustomer.Contains(searchString) || s.Nik.Contains(searchString)
                || s.Alamat.Contains(searchString));
            }

            //Membuat pagelist
            ViewData["CurentSort"] = sortOrder;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;

            //Untuk Sorting
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Data" ? "date_desc" : "Date";

            switch (sortOrder)
            {
                case "name_desc":
                    menu = menu.OrderByDescending(s => s.NamaCustomer);
                    break;
                case "Date":
                    menu = menu.OrderBy(s => s.Nik);
                    break;
                case "date_desc":
                    menu = menu.OrderByDescending(s => s.Nik);
                    break;
                default: //name ascending
                    menu = menu.OrderBy(s => s.NamaCustomer);
                    break;
            }

            //Definisi jumlah data pada halaman
            int pageSize = 5;
            return View(await PaginatedList<Customer>.CreateAsync(menu.AsNoTracking(), pageNumber ?? 1, pageSize));
            
            //var rentKendaraanContext = _context.Customer.Include(c => c.IdGenderNavigation);
            //return View(await rentKendaraanContext.ToListAsync());
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.IdGenderNavigation)
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create()
        {
            ViewData["NamaGender"] = new SelectList(_context.Gender, "NamaGender", "NamaGender");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdCustomer,NamaCustomer,Nik,Alamat,NoHp,IdGender")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["NamaGender"] = new SelectList(_context.Gender, "NamaGender", "NamaGender", customer.IdGenderNavigation.NamaGender);
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            ViewData["IdGender"] = new SelectList(_context.Gender, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdCustomer,NamaCustomer,Nik,Alamat,NoHp,IdGender")] Customer customer)
        {
            if (id != customer.IdCustomer)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.IdCustomer))
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
            ViewData["IdGender"] = new SelectList(_context.Gender, "IdGender", "IdGender", customer.IdGender);
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customer
                .Include(c => c.IdGenderNavigation)
                .FirstOrDefaultAsync(m => m.IdCustomer == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var customer = await _context.Customer.FindAsync(id);
            _context.Customer.Remove(customer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int id)
        {
            return _context.Customer.Any(e => e.IdCustomer == id);
        }
    }
}
