using Contracts.DTO.Customers;
using Contracts.DTO.Stocks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.UI.Services.Api.Interface;

namespace Web.UI.Controllers
{
    [Authorize]
    public class StockController : Controller
    {
        private readonly IStockApiService _api;
        public StockController(IStockApiService api)
        {
            _api = api;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> List(bool onlyActives = false)
        {
            var resp = await _api.GetAllAsync(onlyActives);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data ?? new() });
        }

        [HttpGet]
        public async Task<IActionResult> Detail(int id)
        {
            var resp = await _api.GetByIdAsync(id);
            if (!resp.Success) return Json(new { success = false, message = resp.Response });
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomersByStock(int stockId)
        {
            var resp = await _api.GetCustomersByStock(stockId);
            if (!resp.Success)
                return Json(new { success = false, message = resp.Response, errors = resp.Errors, data = Array.Empty<CustomerDetailDto>() });
            return Json(new { success = true, data = resp.Data ?? new List<CustomerDetailDto>() });
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StockCreateDto dto)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Form geçersiz." });

            var resp = await _api.CreateAsync(dto);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(StockUpdateDto dto)
        {
            if (!ModelState.IsValid || dto.Id <= 0)
                return Json(new { success = false, message = "Form geçersiz." });

            var resp = await _api.UpdateAsync(dto);
            return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _api.DeleteAsync(id);
            return Json(new { success = resp.Success, message = resp.Response });
        }
    }
}
