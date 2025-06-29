using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;

namespace DAL.Repositories
{
    public class CustomerRepository
    {
        private FuminiHotelManagementContext _context; 
        public List<Customer> GetAll()
        {
            _context = new();
            return _context.Customers.ToList();
        }
        public Customer? GetById(int id)
        {
            _context = new();
            return _context.Customers.FirstOrDefault(c => c.CustomerId == id);
        }

        public Customer? GetByEmail(string email)
        {
            _context = new();
            return _context.Customers.FirstOrDefault(c => c.EmailAddress == email);
        }
    }
}
