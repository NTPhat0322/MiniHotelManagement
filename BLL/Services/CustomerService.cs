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
    
        public void AddCustomer(Customer customer)
        {
            _customerRepo.Add(customer);
        }

        public void UpdateCustomer(Customer customer)
        {
            _customerRepo.Update(customer);
        }

        public Customer? GetCustomerById(int id)
        {
            return _customerRepo.GetById(id);
        }

        public void DeleteCustomer(Customer customer)
        {
            customer.CustomerStatus = 0; // Set status to 0 (inactive) instead of deleting
            _customerRepo.Update(customer); // Update the status in the database
        }

        public List<Customer> SearchByName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return GetAll(); // Return all customers if name is empty
            }
            return GetAll().Where(c => c.CustomerFullName != null && c.CustomerFullName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
        }
    }
}
