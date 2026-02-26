using HotelManagmentAPI.DTOs.Bill;

namespace HotelManagmentAPI.Services.Bill
{
    public interface IBillService
    {
        Task<IEnumerable<BillDto>> GetAllBillsAsync();
        Task<BillDto?> GetBillByIdAsync(int id);
        Task<BillDto?> GetBillByReservationIdAsync(int reservationId);
        Task<BillDto> CreateBillAsync(CreateBillDto dto);
        Task<BillDto?> UpdateBillAsync(int id, UpdateBillDto dto);
    }
}