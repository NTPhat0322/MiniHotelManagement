using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services
{
    public class RoomInformationService
    {
        private RoomRepository _RoomInformationRepo = new();

        public List<RoomInformation> GetAll()
        {
            return _RoomInformationRepo.GetAll();
        }


        public void AddRoomInformation(RoomInformation RoomInformation)
        {
            _RoomInformationRepo.Add(RoomInformation);
        }

        public void UpdateRoomInformation(RoomInformation RoomInformation)
        {
            _RoomInformationRepo.Update(RoomInformation);
        }

        public RoomInformation? GetRoomInformationById(int id)
        {
            return _RoomInformationRepo.GetById(id);
        }

        public void DeleteRoomInformation(RoomInformation RoomInformation)
        {
            _RoomInformationRepo.Delete(RoomInformation);
        }

        public List<RoomInformation> GetByMaxCapacity(int maxCapacity)
        {
            return _RoomInformationRepo.GetAll().Where(r => r.RoomMaxCapacity == maxCapacity).ToList();
        }
    }
}
