using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Dtos;
using Hotel_BusinessObjects.Models;
using HotelUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Hotel_Daos
{
    public class CustomerDao
    {
        private static CustomerDao? instance;
        private FUMiniHotelManagementContext _context;
        public CustomerDao()
        {
            _context = new FUMiniHotelManagementContext();
        }
        public static CustomerDao Instance
        {
            get
            {
                if (instance == null)
                    instance = new CustomerDao();
                return instance;
            }
        }
        public List<Customer> GetCustomers() => _context.Customers.ToList();
        public Customer? GetCustomer(int id) => _context.Customers.FirstOrDefault(c => c.CustomerId == id);
        public bool AddCustomer(Customer customer)
        {
            try
            {
                _context.Add(customer);
                _context.SaveChanges();
                _context.Entry<Customer>(customer).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveCustomer(int id)
        {
            try
            {
                var trackingEntity = _context.ChangeTracker.Entries<Customer>().FirstOrDefault(c => c.Entity.CustomerId == id);
                Customer? customer;
                if (trackingEntity != null)
                    customer = trackingEntity.Entity;
                else
                    customer = GetCustomer(id);
                if (customer != null)
                {
                    _context.Customers.Remove(customer);
                    _context.SaveChanges();
                    _context.Entry<Customer>(customer).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool UpdateCustomer(Customer customer)
        {
            try
            {
                var trackingEntity = _context.ChangeTracker.Entries<Customer>().FirstOrDefault(c => c.Entity.CustomerId == customer.CustomerId);
                Customer? old;
                if (trackingEntity != null)
                    old = trackingEntity.Entity;
                else
                    old = GetCustomer(customer.CustomerId);
                if (old != null)
                {
                    _context.Customers.Update(customer);
                    _context.SaveChanges();
                    _context.Entry<Customer>(customer).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public void WriteFile(List<CustomerDto> customers, string filePath, string type) => FileUtil<CustomerDto>.WriteFile(customers, filePath, type);
        public List<CustomerDto>? GetCustomers(string path, string type) => FileUtil<CustomerDto>.ReadFile(path, type);
        public CustomerDto? GetCustomer(string path, string type, int id) => FileUtil<CustomerDto>.ReadFile(path, type).FirstOrDefault(c => c.CustomerId == id);
        public bool UpdateCustomer(string path, string type, Customer customer)
        {
            try
            {
                var customers = GetCustomers(path, type);
                customers.ForEach(c =>
                {
                    if (c.CustomerId == customer.CustomerId)
                    {
                        c.CustomerFullName = customer.CustomerFullName;
                        c.Telephone = customer.Telephone;
                        c.EmailAddress = customer.EmailAddress;
                        c.CustomerBirthday = customer.CustomerBirthday;
                        c.CustomerStatus = customer.CustomerStatus;
                        c.Password = customer.Password;
                    }
                });
                WriteFile(customers, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool AddCustomer(string path, string type, CustomerDto customer)
        {
            try
            {
                var customers = GetCustomers(path, type);
                customer.CustomerId = customers.Last().CustomerId + 1;
                customers.Add(customer);
                WriteFile(customers, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool RemoveCustomer(string path, string type, int id)
        {
            try
            {
                var customers = GetCustomers(path, type);
                var customer = customers.Find(c => c.CustomerId == id);
                customers.Remove(customer);
                WriteFile(customers, path, type);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
