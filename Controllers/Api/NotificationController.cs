using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Notification")]
    public class NotificationController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        public NotificationController(UserManager<ApplicationUser> _userManager, ApplicationDbContext _context)
        {
            this._context = _context;
            this._userManager = _userManager;
        }
        [HttpGet]
        public async Task<IEnumerable<Notification>> Notfications()
        {

            var user =await _userManager.GetUserAsync(User);

            var userid = user.Id;

            
            var notification = _context.UserNotifications.Where(c => c.UserId == userid && c.IsRead == false )
                .Select(C => C.Notification)
                .Include(C => C.CourierRequest).ThenInclude(c => c.User)
                .Include(C=>C.WayBill).ThenInclude(c=>c.User)
                .Include(C => C.Delivery).ThenInclude(c => c.User)
                .ToList();
           
          

            return notification;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> MarkAsRead()
        {
            var user = await _userManager.GetUserAsync(User);
            var notifications = _context.UserNotifications
                .Where(un => un.UserId == user.Id && !un.IsRead)
                .ToList();

            notifications.ForEach(n => n.IsRead = true);

            _context.SaveChanges();

            return Ok();
        }
    }
}