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

        public List<BookingDetail> GetAll()
        {
            return _BookingDetailRepo.GetAll();
        }

        public void AddBookingDetail(BookingDetail BookingDetail)
        {
            _BookingDetailRepo.Add(BookingDetail);
        }

        public void UpdateBookingDetail(BookingDetail BookingDetail)
        {
            _BookingDetailRepo.Update(BookingDetail);
        }

        public BookingDetail? GetBookingDetailByRoomId(int id)
        {
            return _BookingDetailRepo.GetByRoomId(id);
        }

        public void DeleteBookingDetail(BookingDetail BookingDetail)
        {
            _BookingDetailRepo.Delete(BookingDetail);
        }
    }
}
