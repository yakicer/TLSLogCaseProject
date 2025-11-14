using Contracts.DTO.Customers;
using Contracts.DTO.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.UI.Services.Api.Interface;

[Authorize]
public class OrdersController : Controller
{
    private readonly IOrderApiService _api;
    public OrdersController(IOrderApiService api)
    {
        _api = api;
    }

    public IActionResult Index() => View();

    [HttpGet]
    public async Task<IActionResult> GetAll(bool onlyActives = false)
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
    public async Task<IActionResult> ByMinQuantity(decimal minAmount)
    {
        var resp = await _api.GetOrdersWithMinQuantityAsync(minAmount);
        if (!resp.Success)
            return Json(new { success = false, message = resp.Response, errors = resp.Errors, data = Array.Empty<CustomerOrderDto>() });
        return Json(new { success = true, data = resp.Data ?? new List<CustomerOrderDto>() });
    }

    [HttpGet]
    public async Task<IActionResult> CountByCity(string city = "Istanbul")
    {
        if (string.IsNullOrWhiteSpace(city))
            return Json(new { success = false, message = "Şehir zorunludur.", data = (CityOrderCountDto?)null });

        var resp = await _api.GetOrderCountByCityAsync(city);
        if (!resp.Success)
            return Json(new { success = false, message = resp.Response, errors = resp.Errors, data = (CityOrderCountDto?)null });

        return Json(new { success = true, data = resp.Data });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderCreateRequestDto dto)
    {
        if (!ModelState.IsValid)
            return Json(new { success = false, message = "Form geçersiz." });

        var resp = await _api.CreateAsync(dto);
        return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(OrderUpdateRequestDto dto)
    {
        if (!ModelState.IsValid || dto.Id <= 0)
            return Json(new { success = false, message = "Form geçersiz." });

        var resp = await _api.UpdateAsync(dto);
        return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateStatus(OrderStatusUpdateRequestDto dto)
    {
        if (!ModelState.IsValid || dto.Id <= 0)
            return Json(new { success = false, message = "Geçersiz istek." });

        var resp = await _api.UpdateStatusAsync(dto);
        return Json(new { success = resp.Success, message = resp.Response, data = resp.Data });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var resp = await _api.DeleteAsync(id);
        return Json(new { success = resp.Success, message = resp.Response });
    }
}
