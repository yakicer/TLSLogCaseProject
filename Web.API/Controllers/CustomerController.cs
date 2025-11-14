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
    public class CustomerController : BaseController
    {
        private readonly ICustomerService _customerService;
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool onlyActives = false)
        {
            var resp = await _customerService.GetAllAsync(onlyActives);

            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var resp = await _customerService.GetByIdAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }

        [HttpPost("{id:int}/add-address")]
        public async Task<IActionResult> AddAddress(int id, [FromBody] CustomerAddressCreateRequest dto)
        {
            if (dto == null)
                return BadRequest(new BaseResponse<string> { Success = false, Response = "Adres bilgileri boş olamaz." });

            dto.CustomerId = id;

            var resp = await _customerService.AddAddressAsync(dto);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpGet("{id:int}/getaddresses")]
        public async Task<IActionResult> GetAddresses(int id)
        {
            var resp = await _customerService.GetAddressesAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }
        [HttpGet("orders-by-name")]
        public async Task<IActionResult> GetOrdersByCustomerName([FromQuery] string name)
        {
            var resp = await _customerService.GetOrdersByCustomerNameAsync(name);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }
        [HttpGet("different-addresses")]
        public async Task<IActionResult> GetCustomersWithDifferentAddresses()
        {
            var resp = await _customerService.GetCustomersWithDifferentAddressesAsync();
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerCreateRequest dto)
        {
            if (dto == null)
                return BadRequest(new BaseResponse<string> { Success = false, Response = "Veri boş." });

            var resp = await _customerService.CreateAsync(dto);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerUpdateRequest dto)
        {
            if (dto == null || id != dto.Id)
                return BadRequest(new BaseResponse<string> { Success = false, Response = "Geçersiz veri." });
            //servise mi alsan acaba ?!!!!
            //var customerResp = await _customerService.GetByIdAsync(id);
            //if (!customerResp.Success)
            //    return NotFound(customerResp);

            var resp = await _customerService.UpdateAsync(dto);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _customerService.DeleteAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }
    }
}
