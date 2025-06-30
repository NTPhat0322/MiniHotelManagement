using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories
{
    public class BookingDetailRepository
    {
        private FuminiHotelManagementContext _context;
        public List<BookingDetail> GetAll()
        {
            _context = new();
            return _context.BookingDetails.ToList();
        }

        public BookingDetail? GetByRoomId(int id)
        {
            _context = new();
            return _context.BookingDetails.FirstOrDefault(c => c.RoomId == id);
        }

        public void Add(BookingDetail BookingDetail)
        {
            _context = new();
            _context.BookingDetails.Add(BookingDetail);
            _context.SaveChanges();
        }

        public void Update(BookingDetail BookingDetail)
        {
            _context = new();
            _context.BookingDetails.Update(BookingDetail);
            _context.SaveChanges();
        }

        public void Delete(BookingDetail BookingDetail)
        {
            _context = new();
            _context.BookingDetails.Remove(BookingDetail);
            _context.SaveChanges();
        }
    }
}
