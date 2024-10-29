using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using Hotel_Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Services
{
    public class CustomerService : ICustomerService
    {
        private ICustomerRepo customerRepo;
        public CustomerService()
        {
            customerRepo = new CustomerRepo();
        }

        public bool AddCustomer(Customer customer) => customerRepo.AddCustomer(customer);

        public bool AddCustomer(string path, string type, CustomerDto customer) => customerRepo.AddCustomer(path, type, customer);

        public Customer? GetCustomer(int id) => customerRepo.GetCustomer(id);

        public CustomerDto? GetCustomer(string path, string type, int id) => customerRepo.GetCustomer(path, type, id);

        public List<Customer> GetCustomers() => customerRepo.GetCustomers();

        public List<CustomerDto>? GetCustomers(string path, string type) => customerRepo.GetCustomers(path, type);

        public bool RemoveCustomer(int id) => customerRepo.RemoveCustomer(id);

        public bool RemoveCustomer(string path, string type, int id) => customerRepo.RemoveCustomer(path, type, id);

        public bool UpdateCustomer(Customer customer) => customerRepo.UpdateCustomer(customer);

        public bool UpdateCustomer(string path, string type, Customer customer) => customerRepo.UpdateCustomer(path, type, customer);

        public void WriteFile(List<CustomerDto> customers, string filePath, string type) => customerRepo.WriteFile(customers, filePath, type);
    }
}
