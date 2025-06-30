using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories
{
    public class RoomRepository
    {
        private FuminiHotelManagementContext _context;
        public List<RoomInformation> GetAll()
        {
            _context = new();
            return _context.RoomInformations.Include(c => c.RoomType).ToList();
        }
        public RoomInformation? GetById(int id)
        {
            _context = new();
            return _context.RoomInformations.Include(c => c.RoomType).FirstOrDefault(c => c.RoomId == id);
        }

        public void Add(RoomInformation RoomInformation)
        {
            _context = new();
            _context.RoomInformations.Add(RoomInformation);
            _context.SaveChanges();
        }

        public void Update(RoomInformation RoomInformation)
        {
            _context = new();
            _context.RoomInformations.Update(RoomInformation);
            _context.SaveChanges();
        }

        public void Delete(RoomInformation RoomInformation)
        {
            _context = new();
            _context.RoomInformations.Remove(RoomInformation);
            _context.SaveChanges();
        }
    }
}
