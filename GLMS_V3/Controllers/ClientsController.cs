using GLMS.Models;
using GLMS.Services;
using Microsoft.AspNetCore.Mvc;


namespace GLMS.Controllers
{
    public class ClientsController : Controller
    {
        private readonly ClientApiService _apiService;

        public ClientsController(
            ClientApiService apiService)
        {
            _apiService = apiService;
        }

        // GET: Clients
        public async Task<IActionResult> Index()
        {
            var clients =
                await _apiService.GetClientsAsync();

            if (!clients.Any())
            {
                ViewBag.ApiMessage =
                    "No clients available or API service is unavailable.";
            }

            return View(clients);
        }

        // GET: Clients/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _apiService.GetClientAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // GET: Clients/Create
        public async Task<IActionResult> Create()
        {
            var apiAvailable =
                await _apiService.IsApiAvailableAsync();

            if (!apiAvailable)
            {
                ViewBag.ApiMessage =
                    "Unable to connect to the API service. Saving clients is currently unavailable.";
            }

            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,ContactDetails,Region")] Client client)
        {
            if (ModelState.IsValid)
            {
                var success =
                    await _apiService.CreateClientAsync(client);

                if (!success)
                {
                    ModelState.AddModelError(
                        "",
                        "Unable to contact API service.");

                    return View(client);
                }

                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // GET: Clients/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _apiService.GetClientAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,ContactDetails,Region")] Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (ModelState.IsValid)
                {
                    var success =
                        await _apiService.UpdateClientAsync(
                            id,
                            client);

                    if (!success)
                    {
                        ModelState.AddModelError(
                            "",
                            "Unable to contact API service.");

                        return View(client);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var client = await _apiService.GetClientAsync(id.Value);
            if (client == null)
            {
                return NotFound();
            }

            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _apiService.DeleteClientAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool ClientExists(int id)
        {
            return true;
        }
    }
}
