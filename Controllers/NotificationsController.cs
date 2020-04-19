using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public NotificationsController(UserManager<ApplicationUser> _userManager, ApplicationDbContext _context)
        {
            this._context = _context;
            this._userManager = _userManager;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            var userid = user.Id;


            var notification = _context.UserNotifications.Where(c => c.UserId == userid)
                .Select(C => C.Notification)
                .Include(c=>c.CourierRequest).ThenInclude(c=>c.CourierDestination)
                .Include(c => c.CourierRequest).ThenInclude(c => c.User)
                .Include(C => C.WayBill).ThenInclude(c => c.CourierDestination)
                .Include(C => C.WayBill).ThenInclude(c => c.User)
                .Include(C => C.Delivery).ThenInclude(c => c.CourierDestination)
                .Include(C => C.Delivery).ThenInclude(c => c.User)
                .ToList();


            List<Notification> ordreringnotification = notification.OrderBy(c => c.DateTime.Date).ThenBy(c => c.DateTime.TimeOfDay).ToList();
                //            .OrderByDescending(c => c.DateTime.Date)
                //.ThenBy(c => c.DateTime.TimeOfDay)
            return View(ordreringnotification);
        }
    }
}