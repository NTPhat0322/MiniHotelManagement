using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class BookingReservationRepository
    {
        private FuminiHotelManagementContext _context;
        public List<BookingReservation> GetAll()
        {
            _context = new();
            return _context.BookingReservations
                .Include(b => b.BookingDetails)
                .ThenInclude(b => b.Room)
                .ToList();
        }
        public BookingReservation? GetById(int id)
        {
            _context = new();
            return _context.BookingReservations
                .Include(b => b.BookingDetails)
                .FirstOrDefault(c => c.BookingReservationId == id);
        }

        public void Add(BookingReservation bookingReservation)
        {
            _context = new();
            _context.BookingReservations.Add(bookingReservation);
            _context.SaveChanges();
        }

        public void Update(BookingReservation bookingReservation)
        {
            _context = new();
            _context.BookingReservations.Update(bookingReservation);
            _context.SaveChanges();
        }

        public void Delete(BookingReservation bookingReservation)
        {
            _context = new();
            var tmp = _context.BookingReservations.FirstOrDefault(b => b.BookingReservationId == bookingReservation.BookingReservationId);
            if(tmp is null)
                throw new Exception("BookingReservation not found in deletion action");
            _context.BookingReservations.Remove(tmp);
            _context.SaveChanges();
        }
    }
}
