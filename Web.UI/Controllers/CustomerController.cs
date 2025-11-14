using Contracts.DTO.CustomerAddresses;
using Contracts.DTO.Customers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Controllers
{
    [Authorize]
    public class CustomerController : Controller
    {
        private readonly ICustomerApiService _api;
        public CustomerController(ICustomerApiService api)
        {
            _api = api;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> List(bool onlyActives = false)
        {
            var resp = await _api.GetAllAsync(onlyActives);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new
            {
                success = resp.Success,
                message = resp.Response,
                data = resp.Data ?? new List<CustomerDto>()
            });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var resp = await _api.GetByIdAsync(id);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new { success = true, data = resp.Data });
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses(int customerId)
        {
            var resp = await _api.GetAddressesAsync(customerId);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new { success = true, data = resp.Data });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(CustomerAddressCreateRequestDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Form geçersiz." });

            var resp = await _api.AddAddressAsync(dto);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateRequestDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Form geçersiz." });

            var resp = await _api.CreateAsync(dto);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CustomerUpdateRequestDto dto)
        {
            if (!ModelState.IsValid || dto.Id <= 0)
                return Json(new { success = false, message = "Form geçersiz." });

            var resp = await _api.UpdateAsync(dto);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new { success = resp.Success, message = resp.Response });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _api.DeleteAsync(id);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new { success = resp.Success, message = resp.Response });
        }
    }
}
