using DeependAncestry.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace DeependAncestry.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Advanced()
        {
            return View();
        }

        //public ActionResult DisplayResultPartialView([FromBody]dynamic People)
        //{
        //    //if (viewName == "CustomerDetails")
        //    //{
        //    //    using (NorthwindEntities db = new NorthwindEntities())
        //    //    {
        //    //        model = db.Customers.Find(customerID);
        //    //    }
        //    //}
        //    //if (viewName == "OrderDetails")
        //    //{
        //    //    using (NorthwindEntities db = new NorthwindEntities())
        //    //    {
        //    //        model = db.Orders.Where(o => o.CustomerID == customerID)
        //    //                  .OrderBy(o => o.OrderID).ToList();
        //    //    }
        //    //}
        //    return PartialView(People);
        //}
    }
}
