using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace coderush.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Courier")]
    public class CourierController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        
        public CourierController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet("{id}")]
        [Route("api/Courier/{id}")]
        //This For Remove The Request
        public async Task<IActionResult> GetRequestremove(string id)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                CourierRequest Courierrequest = _context.CourierRequests.Single(c => c.Id == int.Parse(id));
                Courierrequest.DeletedBy = AppUser.Id;
                Courierrequest.DeletedDate = DateTime.UtcNow.AddHours(2);
                Courierrequest.IsActive = false;
                Courierrequest.IsDeleted = true;
                _context.CourierRequests.Update(Courierrequest);
                _context.SaveChanges();

                var notification = new Notification
                {
                    DateTime = DateTime.UtcNow.AddHours(2),
                    CourierRequestId = Convert.ToInt32(id),
                    CreatedBy = AppUser.Id,
                    CreatedDate = DateTime.UtcNow.AddHours(2),
                    Type = NotificationType.CourierRequestDelete,
                    IsActive = true,
                    IsDeleted = false
                };

                var AllAdmins = _context.ApplicationUser.Where(c => c.IsAdmin == true).Select(c => c.Id);
                foreach (var item in AllAdmins)
                {
                    var usernotification = new UserNotification
                    {
                        UserId = item,
                        Notification = notification,
                        IsRead = false
                    };
                    await _context.UserNotifications.AddAsync(usernotification);
                }
                _context.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Error occurred.");
            }

            return Ok();

        }

        [HttpGet("[action]")]
        //This For Get All Requests
        public async Task<IActionResult> GetCourier()
        {
            List<CourierRequest> Items = await _context.CourierRequests.Include(c=>c.CourierDescription).Include(c=>c.CourierDestination).Include(q=>q.CourierSendType).Where(c=>c.IsActive==true && c.IsDeleted==false).ToListAsync();

            
            return Json(new { data = Items });
        }

        [HttpPost("[action]")]
        //This For Insert Or Update Request
        public async Task<IActionResult> Update([FromForm]CourierRequest payload)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                if (payload.Id > 0)
                {
                    var courierrequestindb = _context.CourierRequests.AsNoTracking().SingleOrDefault(q => q.Id == (int)payload.Id);
                    ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                    CourierRequest Courierrequest = payload;
                    Courierrequest.ModifiedBy = AppUser.Id;
                    Courierrequest.ModifiedDate = DateTime.UtcNow.AddHours(2);
                    Courierrequest.IsDeleted = false;
                    Courierrequest.IsActive = true;
                    Courierrequest.UserId = courierrequestindb.UserId;
                    Courierrequest.CreatedBy = courierrequestindb.CreatedBy;
                    Courierrequest.CreatedDate = courierrequestindb.CreatedDate;
                    //Courierrequest.UserId = AppUser.Id;
                    _context.CourierRequests.Update(Courierrequest);
                    _context.SaveChanges();


                    var notification = new Notification
                    {
                        DateTime = DateTime.UtcNow.AddHours(2),
                        CourierRequestId = Convert.ToInt32(payload.Id),
                        CreatedBy = AppUser.Id,
                        CreatedDate = DateTime.UtcNow.AddHours(2),
                        Type = NotificationType.CourierRequestUpdate,
                        IsActive = true,
                        IsDeleted = false
                    };
                    var AllAdmins = _context.ApplicationUser.Where(c => c.IsAdmin == true).Select(c => c.Id);
                    foreach (var item in AllAdmins)
                    {
                        var usernotification = new UserNotification
                        {
                            UserId = item,
                            Notification = notification,
                            IsRead = false
                        };
                        await _context.UserNotifications.AddAsync(usernotification);
                    }
                    _context.SaveChanges();

                }
                else
                {

                    ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                    CourierRequest Courierrequest = payload;
                    if (_context.CourierRequests.Max(c => c.CourierNumber) == null)
                    {
                        Courierrequest.CourierNumber = 1;
                    }
                    else
                    {
                        Courierrequest.CourierNumber = _context.CourierRequests.Max(c => c.CourierNumber) + 1;
                    }

                    Courierrequest.CreatedDate = DateTime.UtcNow.AddHours(2);
                    Courierrequest.CreatedBy = AppUser.Id;
                    Courierrequest.UserId = AppUser.Id;
                    Courierrequest.IsActive = true;
                    Courierrequest.IsDeleted = false;

                    await _context.CourierRequests.AddAsync(Courierrequest);
                    await _context.SaveChangesAsync();

                    var CourierRequuestId = Courierrequest.Id;
                    var notification = new Notification
                    {
                        DateTime = DateTime.UtcNow.AddHours(2),
                        CourierRequestId = CourierRequuestId,
                        CreatedBy = AppUser.Id,
                        CreatedDate = DateTime.UtcNow.AddHours(2),
                        Type = NotificationType.CourierRequestAdd,
                        IsActive = true,
                        IsDeleted = false
                    };
                    var AllAdmins = _context.ApplicationUser.Where(c => c.IsAdmin == true).Select(c => c.Id);
                    foreach (var item in AllAdmins)
                    {
                        var usernotification = new UserNotification
                        {
                            UserId = item,
                            Notification = notification,
                            IsRead = false
                        };
                        await _context.UserNotifications.AddAsync(usernotification);
                    }

                    _context.SaveChanges();

                }
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine("Error occurred.");
            }
            return Ok();
        }

        
    }
}