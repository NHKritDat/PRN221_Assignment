using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Services
{
    public interface ICustomerService
    {
        public List<Customer> GetCustomers();
        public Customer? GetCustomer(int id);
        public bool AddCustomer(Customer customer);
        public bool UpdateCustomer(Customer customer);
        public bool RemoveCustomer(int id);
        public void WriteFile(List<CustomerDto> customers, string filePath, string type);
        public List<CustomerDto>? GetCustomers(string path, string type);
        public CustomerDto? GetCustomer(string path, string type, int id);
        public bool AddCustomer(string path, string type, CustomerDto customer);
        public bool UpdateCustomer(string path, string type, Customer customer);
        public bool RemoveCustomer(string path, string type, int id);
    }
}
