using Contracts.DTO.CustomerAddresses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CustomerAddressController : Controller
    {
        private readonly ICustomerAddressApiService _api;

        public CustomerAddressController(ICustomerAddressApiService api)
        {
            _api = api;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> List(bool onlyActives = false)
        {
            var resp = await _api.GetAllAsync(onlyActives);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }
        [HttpGet]
        public async Task<IActionResult> ListAllDetailed(bool onlyActives = false)
        {
            var resp = await _api.GetAllDetailedAsync(onlyActives);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpGet]
        public async Task<IActionResult> ListByCustomer(int customerId, bool onlyActives = false)
        {
            if (customerId <= 0)
                return Json(new { success = false, message = "Müşteri seçiniz." });

            var resp = await _api.GetByCustomerAsync(customerId, onlyActives);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var resp = await _api.GetAsync(id);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerAddressCreateRequestDto dto)
        {
            var resp = await _api.CreateAsync(dto);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(CustomerAddressUpdateRequestDto dto)
        {
            var resp = await _api.UpdateAsync(dto);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _api.DeleteAsync(id);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }
    }
}
