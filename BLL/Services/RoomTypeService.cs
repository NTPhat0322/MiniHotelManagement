using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services
{
    public class RoomTypeService
    {
        private RoomTypeRepository RoomTypeRepository = new();
        public List<RoomType> GetAll()
        {
            return RoomTypeRepository.GetAll();
        }
    }
}
