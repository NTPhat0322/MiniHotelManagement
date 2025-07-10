using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services
{
    public class BookingDetailService
    {
        private BookingDetailRepository _BookingDetailRepo = new();
        private BookingReservationRepository _BookingReservationRepo = new();

        public List<BookingDetail> GetAll()
        {
            return _BookingDetailRepo.GetAll();
        }

        public void AddBookingDetail(BookingDetail bookingDetail)
        {
            _BookingDetailRepo.Add(bookingDetail);
        }

        public void UpdateBookingDetail(BookingDetail bookingDetail)
        {
            _BookingDetailRepo.Update(bookingDetail);
        }

        public BookingDetail? GetBookingDetailByRoomId(int id)
        {
            return _BookingDetailRepo.GetByRoomId(id);
        }

        public List<BookingDetail> GetBookingDetailsByRoomId(int id)
        {
            return _BookingDetailRepo.GetAll().Where(bd => bd.RoomId == id).ToList();
        }

        public void DeleteBookingDetail(BookingDetail bookingDetail)
        {
            bool isDeleted = false;
            var bookingReservation = _BookingReservationRepo.GetById(bookingDetail.BookingReservationId);
            if (bookingReservation!.BookingDetails.Count == 1)
            {
                isDeleted = true;
            }
            _BookingDetailRepo.Delete(bookingDetail);
            //sau khi xóa booking detail, cần xóa luôn booking reservation nếu không còn booking detail nào liên kết với nó
            if (isDeleted)
            {
                _BookingReservationRepo.Delete(bookingReservation);
            }
        }

        public BookingDetail? GetByRoomAndReservationId(int roomId, int bookingReservationId)
        {
            return _BookingDetailRepo.GetByRoomAndReservationId(roomId, bookingReservationId);
        }
    }
}
