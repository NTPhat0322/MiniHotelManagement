using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class ReservationCustomerDTO
    {
        public int BookingReservationId { get; set; }
        public DateOnly? BookingDate { get; set; }
        public decimal? TotalPrice { get; set; }
        public byte? BookingStatus { get; set; }
        public string RoomNumber { get; set; } = null!;
        public DateOnly StartDate { get; set; }
        public DateOnly EndDate { get; set; }
        public decimal? ActualPrice { get; set; }
    }
}
