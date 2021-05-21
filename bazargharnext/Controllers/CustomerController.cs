
using bazargharnext.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
namespace bazargharnext.Controllers
{
    public class CustomerController : Controller

    {
        // GET: Home
        public ActionResult Index()
        {
            DataContext entities = new DataContext();
            return View(entities.Customers);
        }

        public JsonResult InsertCustomers(List<Customer> customers)
        {
            using DataContext entities = new DataContext();

            //Check for NULL.
            if (customers == null)
            {
                customers = new List<Customer>();


            }

            //Loop and insert records.
            foreach (Customer customer in customers)
            {
                entities.Customers.Add(customer);
            }
            int insertedRecords = entities.SaveChanges();
            return Json(insertedRecords);
        }
    }

}