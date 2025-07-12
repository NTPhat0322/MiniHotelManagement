using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.DTOs;
using DAL.Entities;
using DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services
{
    public class BookingReservationService
    {
        private BookingReservationRepository _bookingReservationRepo = new();
        private BookingDetailRepository _bookingDetailRepo = new();

        public List<BookingReservation> GetAll()
        {
            return _bookingReservationRepo.GetAll();
        }
        public void AddBookingReservation(BookingReservation bookingReservation)
        {
            _bookingReservationRepo.Add(bookingReservation);
        }

        public void UpdateBookingReservation(BookingReservation bookingReservation)
        {
            _bookingReservationRepo.Update(bookingReservation);
        }

        public BookingReservation? GetbookingReservationById(int id)
        {
            return _bookingReservationRepo.GetById(id);
        }

        public List<BookingInformationDTO> CreateBookingInforList()
        {
            List<BookingInformationDTO> bookingInformationList = new();
            var bookingDetails = _bookingDetailRepo.GetAll();
            foreach(var booking in bookingDetails)
            {
                BookingInformationDTO bookingInformation = new()
                {
                    BookingReservationId = booking.BookingReservation.BookingReservationId,
                    BookingDate = booking.BookingReservation.BookingDate,
                    TotalPrice = booking.BookingReservation.TotalPrice,
                    CustomerId = booking.BookingReservation.CustomerId,
                    BookingStatus = booking.BookingReservation.BookingStatus,
                    RoomId = booking.RoomId,
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate,
                    ActualPrice = booking.ActualPrice,
                    RoomNumber = booking.Room.RoomNumber
                };
                bookingInformationList.Add(bookingInformation);
            }
            return bookingInformationList;
        }
        public List<BookingReservation> GetBookingReservationByCustomerId(int customerId)
        {
            return _bookingReservationRepo.GetAll()
                .Where(b => b.CustomerId == customerId)
                .ToList();
        }
    }
}
