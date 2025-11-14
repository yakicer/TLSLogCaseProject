using Businness.Application;
using Businness.Interface.API;
using Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;

namespace Web.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool onlyActives = true)
        {
            var resp = await _orderService.GetAllAsync(onlyActives);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resp = await _orderService.GetByIdAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }

        [HttpGet("by-min-quantity")]
        public async Task<IActionResult> GetOrdersByMinQuantity([FromQuery] decimal minAmount)
        {
            var resp = await _orderService.GetOrdersWithMinQuantityAsync(minAmount);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpGet("count-by-city")]
        public async Task<IActionResult> GetOrderCountByCity([FromQuery] string city = "Istanbul")
        {
            var resp = await _orderService.GetOrderCountByCityAsync(city);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderCreateRequest dto)
        {
            if (dto == null)
            {
                return BadRequest(new BaseResponse<string>
                {
                    Success = false,
                    Response = "Sipariş verisi boş olamaz."
                });
            }

            var resp = await _orderService.CreateAsync(dto);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] OrderUpdateRequest dto)
        {
            if (dto is null || id <= 0 || id != dto.Id)
                return BadRequest(new BaseResponse<int> { Success = false, Response = "Geçersiz istek." });

            var resp = await _orderService.UpdateAsync(dto);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpPatch("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus([FromRoute] int id, [FromBody] OrderStatusUpdateRequest dto)
        {
            if (dto is null || id <= 0 || id != dto.Id)
                return BadRequest(new BaseResponse<int> { Success = false, Response = "Geçersiz istek." });

            var resp = await _orderService.UpdateStatusAsync(dto);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _orderService.DeleteAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }
    }

}
