using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Hotel_BusinessObjects;
using Hotel_BusinessObjects.Models;
using Hotel_Services;
using Hotel_BusinessObjects.Dtos;

namespace HotelManagementJsonXml.Pages.CustomerXml
{
    public class IndexModel : PageModel
    {
        private readonly ICustomerService _customerService;

        public IndexModel(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        public IList<Customer> Customer { get; set; } = default!;
        private string type = ".xml";
        private string pathCustomer = "..\\Hotel_Daos\\customer";

        public async Task OnGetAsync()
        {
            if (_customerService.GetCustomers(pathCustomer, type) != null)
            {
                Customer = ToListModel(_customerService.GetCustomers(pathCustomer, type));
            }
        }
        private Customer ToModel(CustomerDto customerDto)
        {
            Customer customer = new Customer();
            customer.CustomerId = customerDto.CustomerId;
            customer.CustomerFullName = customerDto.CustomerFullName;
            customer.Telephone = customerDto.Telephone;
            customer.EmailAddress = customerDto.EmailAddress;
            customer.CustomerBirthday = customerDto.CustomerBirthday;
            customer.CustomerStatus = customerDto.CustomerStatus;
            customer.Password = customerDto.Password;
            return customer;
        }
        private List<Customer> ToListModel(List<CustomerDto> customerList)
        {
            List<Customer> result = new List<Customer>();
            foreach (var customer in customerList)
                result.Add(ToModel(customer));
            return result;
        }
    }
}
