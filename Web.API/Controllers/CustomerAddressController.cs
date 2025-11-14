using Businness.Application;
using Businness.Interface.API;
using Contracts.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Controllers;

namespace Web.API.Controllers
{
    //ihtiyac halinda kullanilmasi icin ornek bir backoffice senaryosu olmasi icin ekledim
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Administrator")]
    public class CustomerAddressController : BaseController
    {
        private readonly ICustomerAddressService _service;

        public CustomerAddressController(ICustomerAddressService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] bool onlyActives = false)
        {
            var resp = await _service.GetAllAddressesAsync(onlyActives);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpGet("GetAllDetailed")]
        public async Task<IActionResult> GetAllDetailed([FromQuery] bool onlyActives = false)
        {
            var resp = await _service.GetAllAddressesDetailedAsync(onlyActives);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpGet("customer/{customerId:int}")]
        public async Task<IActionResult> GetByCustomer(int customerId, [FromQuery] bool onlyActives = false)
        {
            var resp = await _service.GetByCustomerAsync(customerId, onlyActives);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var resp = await _service.GetByIdAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerAddressCreateRequest dto)
        {
            var resp = await _service.CreateAsync(dto);
            return resp.Success ? Ok(resp) : BadRequest(resp);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerAddressUpdateRequest dto)
        {
            if (id != dto.Id)
                return BadRequest(new BaseResponse<string> { Success = false, Response = "Güncellenecek Id mevcut Id ile uyuşmuyor." });

            var resp = await _service.UpdateAsync(dto);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var resp = await _service.DeleteAsync(id);
            return resp.Success ? Ok(resp) : NotFound(resp);
        }
    }
}
