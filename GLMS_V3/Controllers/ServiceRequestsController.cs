using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GLMS.Controllers
{
    public class ServiceRequestsController : Controller
    {
        private readonly ServiceRequestApiService _apiService;
        private readonly ContractApiService _contractApiService;

        public ServiceRequestsController(
            ServiceRequestApiService apiService,
            ContractApiService contractApiService)
        {
            _apiService = apiService;
            _contractApiService = contractApiService;
        }

        // GET: ServiceRequests
        public async Task<IActionResult> Index()
        {
            var serviceRequests = await _apiService.GetServiceRequestsAsync()?? new List<ServiceRequest>();

            return View(serviceRequests);
        }

        // GET: ServiceRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _apiService.GetServiceRequestAsync(id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Create
        public async Task<IActionResult> Create()
        {
            await LoadContractsDropdown();
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
                var success =
                    await _apiService.CreateServiceRequestAsync(
                        serviceRequest);

                if (!success)
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to contact API service.");

                    await LoadContractsDropdown();

                    return View(serviceRequest);
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadContractsDropdown();
            return View(serviceRequest);

            await LoadContractsDropdown();
            return View(serviceRequest);
        }

        // GET: ServiceRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _apiService.GetServiceRequestAsync(id.Value);

            if (serviceRequest == null)
            {
                return NotFound();
            }

            await LoadContractsDropdown();

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
                var success = await _apiService.UpdateServiceRequestAsync(id, serviceRequest);

                if (!success)
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to contact API service.");

                    await LoadContractsDropdown();

                    return View(serviceRequest);
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadContractsDropdown();

            return View(serviceRequest);
        }

        // GET: ServiceRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var serviceRequest = await _apiService.GetServiceRequestAsync(id.Value);

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
            await _apiService.DeleteServiceRequestAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadContractsDropdown()
        {
            var contracts = await _contractApiService.GetContractsAsync(
                    null,
                    null,
                    null);

            var contractList = contracts.Select(c => new
            {
                c.Id,
                DisplayText =
                    $"Contract {c.Id} | Service Level: {c.ServiceLevel} | Status: {c.Status} | Client: {c.Client?.Name}"
            });

            ViewData["ContractId"] =
                new SelectList(
                    contractList,
                    "Id",
                    "DisplayText");
        }
    }
}