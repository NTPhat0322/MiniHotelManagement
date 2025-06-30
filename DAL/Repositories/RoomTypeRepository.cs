using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories
{
    public class RoomTypeRepository
    {
        private FuminiHotelManagementContext _context; 
        public List<RoomType> GetAll()
        {
            _context = new();
            return _context.RoomTypes.ToList();
        }
        public RoomType? GetById(int id)
        {
            _context = new();
            return _context.RoomTypes.FirstOrDefault(rt => rt.RoomTypeId == id);
        }
        public void Add(RoomType roomType)
        {
            _context = new();
            _context.RoomTypes.Add(roomType);
            _context.SaveChanges();
        }
        public void Update(RoomType roomType)
        {
            _context = new();
            _context.RoomTypes.Update(roomType);
            _context.SaveChanges();
        }
        public void Delete(RoomType roomType)
        {
            _context = new();
            _context.RoomTypes.Remove(roomType);
            _context.SaveChanges();
        }
    }
}
