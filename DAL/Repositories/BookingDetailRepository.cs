using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BookingDetailRepository
    {
        private FuminiHotelManagementContext _context;
        public List<BookingDetail> GetAll()
        {
            _context = new();
            return _context.BookingDetails
                .Include(bd => bd.BookingReservation)
                .Include(bd => bd.Room)
                .ToList();
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
            var bookingDetail = _context.BookingDetails
                .FirstOrDefault(bd => bd.BookingReservationId == BookingDetail.BookingReservationId && bd.RoomId == BookingDetail.RoomId);
            bookingDetail.StartDate = BookingDetail.StartDate;
            bookingDetail.EndDate = BookingDetail.EndDate;
            bookingDetail.ActualPrice = BookingDetail.ActualPrice;
            _context.BookingDetails.Update(bookingDetail);
            _context.SaveChanges();
        }

        public void Delete(BookingDetail BookingDetail)
        {
            _context = new();
            _context.BookingDetails.Remove(BookingDetail);
            _context.SaveChanges();
        }

        public BookingDetail? GetByRoomAndReservationId(int roomId, int bookingReservationId)
        {
            _context = new();
            return _context.BookingDetails.FirstOrDefault(bd => bd.RoomId == roomId && bd.BookingReservationId == bookingReservationId);
        }
    }
}
