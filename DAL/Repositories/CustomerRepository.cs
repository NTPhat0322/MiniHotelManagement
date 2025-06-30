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

        public void Add(Customer customer)
        {
            _context = new();
            _context.Customers.Add(customer);
            _context.SaveChanges();
        }

        public void Update(Customer customer)
        {
            _context = new();
            _context.Customers.Update(customer);
            _context.SaveChanges();
        }

        public void Delete(Customer customer)
        {
            _context = new();
            _context.Customers.Remove(customer);
            _context.SaveChanges();
        }
    }
}
