using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models.CourierViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.WayBill.RoleName)]
    public class WayBillController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WayBillController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()    
        {

            var GetAllRequests = _context.CourierRequests.Include(c => c.CourierDestination).Select(c => new CourierRequestViewModel { CourierId = c.Id, CourierNumber = (c.CourierNumber.ToString() + "-" + c.CourierDestination.Name) }).ToList();
            ViewBag.DropDownlistRequests = GetAllRequests;
            return View();
        }

       
    }
}