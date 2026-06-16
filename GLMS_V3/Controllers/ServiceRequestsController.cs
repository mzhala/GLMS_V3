using GLMS.Data;
using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ServiceRequestService _service;
        private readonly ApplicationDbContext _context;

        public ServiceRequestsController(
            ServiceRequestService service,
            ApplicationDbContext context)
        {
            _service = service;
            _context = context;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var serviceRequests = await _service.GetAllAsync();
            return View(serviceRequests);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _service.GetByIdAsync(id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public IActionResult Create()
        {
            LoadContractsDropdown();
            return View();
        }

        // POST: ServiceRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,ContractId,Description,CostUSD,CostZAR,Status")]
            ServiceRequest serviceRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateAsync(serviceRequest);

                if (!result.Success)
                {
                    ModelState.AddModelError("", result.Message);

                    LoadContractsDropdown();

                    return View(serviceRequest);
                }

                return RedirectToAction(nameof(Index));
            }

            LoadContractsDropdown();
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _service.GetByIdAsync(id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            LoadContractsDropdown();

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Id,ContractId,Description,CostUSD,CostZAR,Status")]
            ServiceRequest serviceRequest)
        {
            if (id != serviceRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _service.UpdateAsync(serviceRequest);

                    if (!result.Success)
                    {
                        ModelState.AddModelError("", result.Message);

                        LoadContractsDropdown();

                        return View(serviceRequest);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_service.Exists(serviceRequest.Id))
                    {
                        return NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            LoadContractsDropdown();

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _service.GetByIdAsync(id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // POST: ServiceRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _service.DeleteAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private void LoadContractsDropdown()
        {
            var contracts = _context.Contracts
                .Include(c => c.Client)
                .Select(c => new
                {
                    c.Id,
                    DisplayText =
                        $"Contract {c.Id} | Service Level: {c.ServiceLevel} | Status: {c.Status} | Client: {c.Client.Name}"
                })
                .ToList();

            ViewData["ContractId"] =
                new SelectList(contracts, "Id", "DisplayText");
        }
    }
}