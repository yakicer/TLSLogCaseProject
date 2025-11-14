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
    public class StockController : BaseController
    {
        private readonly IStockService _stockService;

        public StockController(IStockService stockService)
        {
            _stockService = stockService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool onlyActives)
        {
            var resp = await _stockService.GetAllAsync(onlyActives);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var resp = await _stockService.GetByIdAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }
        [HttpGet("{stockId:int}/customers")]
        public async Task<IActionResult> GetCustomersByStock(int stockId)
        {
            var resp = await _stockService.GetCustomersWhoBoughtStockAsync(stockId);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StockCreateRequest dto)
        {
            if (dto == null)
                return BadRequest(new BaseResponse<string> { Success = false, Response = "Veri boş olamaz." });

            var resp = await _stockService.CreateAsync(dto);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] StockUpdateRequest dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest(new BaseResponse<string> { Success = false, Response = "Geçersiz veri." });

            var resp = await _stockService.UpdateAsync(dto);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _stockService.DeleteAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }
    }

}
