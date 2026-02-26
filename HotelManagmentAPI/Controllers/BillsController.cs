using HotelManagmentAPI.DTOs.Bill;
using HotelManagmentAPI.Services.Bill;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BillsController : ControllerBase
    {
        private readonly IBillService _billService;

        public BillsController(IBillService billService)
        {
            _billService = billService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var bills = await _billService.GetAllBillsAsync();
            return Ok(bills);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bill = await _billService.GetBillByIdAsync(id);
            if (bill == null) return NotFound();
            return Ok(bill);
        }

        [HttpGet("reservation/{reservationId}")]
        public async Task<IActionResult> GetByReservationId(int reservationId)
        {
            var bill = await _billService.GetBillByReservationIdAsync(reservationId);
            if (bill == null) return NotFound();
            return Ok(bill);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBillDto dto)
        {
            try
            {
                var bill = await _billService.CreateBillAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = bill.BillId }, bill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateBillDto dto)
        {
            try
            {
                var bill = await _billService.UpdateBillAsync(id, dto);
                if (bill == null) return NotFound();
                return Ok(bill);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}