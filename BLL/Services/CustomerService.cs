using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Entities;
using DAL.Repositories;

namespace BLL.Services
{
    public class CustomerService
    {
        private CustomerRepository _customerRepo = new();

        public List<Customer> GetAll()
        {
            return _customerRepo.GetAll();
        }

        public Customer? Login(string email, string password)
        {
            var customer = _customerRepo.GetByEmail(email);
            if(customer == null)
            {
                return null; // Email not found
            }
            if (customer.Password == null)
            {
                return null; // Password not set
            }

            if (customer.Password.Equals(password, StringComparison.Ordinal)) // Case-sensitive comparison
            {
                return customer; // Login successful
            }
            return null;
        }
    }
}
