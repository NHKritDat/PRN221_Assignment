using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using Hotel_Daos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Repositories
{
    public class CustomerRepo : ICustomerRepo
    {
        public bool AddCustomer(Customer customer) => CustomerDao.Instance.AddCustomer(customer);

        public bool AddCustomer(string path, string type, CustomerDto customer) => CustomerDao.Instance.AddCustomer(path, type, customer);

        public Customer? GetCustomer(int id) => CustomerDao.Instance.GetCustomer(id);

        public CustomerDto? GetCustomer(string path, string type, int id) => CustomerDao.Instance.GetCustomer(path, type, id);

        public List<Customer> GetCustomers() => CustomerDao.Instance.GetCustomers();

        public List<CustomerDto>? GetCustomers(string path, string type) => CustomerDao.Instance.GetCustomers(path, type);

        public bool RemoveCustomer(int id) => CustomerDao.Instance.RemoveCustomer(id);

        public bool RemoveCustomer(string path, string type, int id) => CustomerDao.Instance.RemoveCustomer(path, type, id);

        public bool UpdateCustomer(Customer customer) => CustomerDao.Instance.UpdateCustomer(customer);

        public bool UpdateCustomer(string path, string type, Customer customer) => CustomerDao.Instance.UpdateCustomer(path, type, customer);

        public void WriteFile(List<CustomerDto> customers, string filePath, string type) => CustomerDao.Instance.WriteFile(customers, filePath, type);
    }
}
