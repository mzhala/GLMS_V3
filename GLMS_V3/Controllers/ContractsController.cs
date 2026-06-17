using GLMS.Data;
using GLMS.Models;
using GLMS.Models.Enums;
using GLMS.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace GLMS.Controllers
{
    public class ContractsController : Controller
    {
        private readonly IWebHostEnvironment _environment;
        private readonly ContractApiService _apiService;
        private readonly ClientApiService _clientApiService;

        public ContractsController(
            IWebHostEnvironment environment,
            ContractApiService apiService,
            ClientApiService clientApiService)
        {
            _environment = environment;
            _apiService = apiService;
            _clientApiService = clientApiService;
        }

        // GET: Contracts
        public async Task<IActionResult> Index(
            ContractStatus? status,
            DateTime? startDate,
            DateTime? endDate)
        {
            ViewBag.Status = status;
            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;

            var contracts =
                await _apiService.GetContractsAsync(
                    status,
                    startDate,
                    endDate);

            if (!contracts.Any())
            {
                ViewBag.ApiMessage =
                    "No contracts available or API service is unavailable.";
            }
            return View(contracts);
        }

        // GET: Contracts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _apiService.GetContractAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // GET: Contracts/Create
        public async Task<IActionResult> Create()
        {
            await LoadClientsDropdown();
            return View();
        }

        // POST: Contracts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Id,ClientId,StartDate,EndDate,Status,ServiceLevel")]
            Contract contract,
            IFormFile? agreementFile)
        {
            if (agreementFile == null)
            {
                ModelState.AddModelError(
                    "",
                    "Agreement PDF is required.");
            }

            if (ModelState.IsValid)
            {
                var extension = Path.GetExtension(agreementFile.FileName);

                if (extension.ToLower() != ".pdf")
                {
                    ModelState.AddModelError(
                        "",
                        "Only PDF files are allowed.");

                    await LoadClientsDropdown();
                    return View(contract);
                }

                var fileName =
                    Guid.NewGuid().ToString() + ".pdf";

                var uploadPath = Path.Combine(
                    _environment.WebRootPath,
                    "uploads",
                    "contracts");

                var filePath = Path.Combine(
                    uploadPath,
                    fileName);

                using (var stream =
                       new FileStream(filePath, FileMode.Create))
                {
                    await agreementFile.CopyToAsync(stream);
                }

                contract.AgreementFilePath = fileName;

                var success = await _apiService.CreateContractAsync(contract);

                if (!success)
                {
                    ModelState.AddModelError(
                        "",
                        "Failed to create contract.");

                    await LoadClientsDropdown();

                    return View(contract);
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadClientsDropdown();

            return View(contract);
        }

        // GET: Contracts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _apiService.GetContractAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            await LoadClientsDropdown();

            return View(contract);
        }

        // POST: Contracts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
        int id,
        [Bind("Id,ClientId,StartDate,EndDate,Status,ServiceLevel,AgreementFilePath")]
        Contract contract,
        IFormFile? agreementFile)
        {
            if (id != contract.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (agreementFile != null)
                {
                    if (agreementFile == null)
                    {
                        ModelState.AddModelError(
                            "",
                            "Agreement PDF is required.");

                        await LoadClientsDropdown();

                        return View(contract);
                    }

                    var extension = Path.GetExtension(agreementFile.FileName);

                    if (extension.ToLower() != ".pdf")
                    {
                        ModelState.AddModelError("", "Only PDF files are allowed.");

                        await LoadClientsDropdown();

                        return View(contract);
                    }

                    var fileName = Guid.NewGuid().ToString() + ".pdf";

                    var uploadPath = Path.Combine(
                        _environment.WebRootPath,
                        "uploads",
                        "contracts");

                    var filePath = Path.Combine(uploadPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await agreementFile.CopyToAsync(stream);
                    }

                    contract.AgreementFilePath = fileName;
                }

                if (agreementFile == null)
                {
                    var existingContract =
                        await _apiService.GetContractAsync(id);

                    if (existingContract != null)
                    {
                        contract.AgreementFilePath =
                            existingContract.AgreementFilePath;
                    }
                }

                var success = await _apiService.UpdateContractAsync(id,contract);

                if (!success)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            await LoadClientsDropdown();

            return View(contract);
        }

        // GET: Contracts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contract = await _apiService.GetContractAsync(id.Value);

            if (contract == null)
            {
                return NotFound();
            }

            return View(contract);
        }

        // POST: Contracts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteContractAsync(id);

            return RedirectToAction(nameof(Index));
        }

        private async Task LoadClientsDropdown()
        {
            var clients =
                await _clientApiService.GetClientsAsync();

            if (!clients.Any())
            {
                ViewBag.ApiMessage =
                    "Unable to load clients. API service may be unavailable.";
            }

            ViewData["ClientId"] =
                new SelectList(
                    clients,
                    "Id",
                    "Name");
        }
    }
}