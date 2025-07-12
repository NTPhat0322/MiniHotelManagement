using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.DTOs
{
    public class StatisticDTO
    {
        public string RoomNumber { get; set; } = null!;
        public string RoomTypeName { get; set; } = null!;
        public int NumberOfBookings { get; set; }
        public decimal TotalRevenue { get; set; }
        public byte? RoomStatus { get; set; }
    }
}
