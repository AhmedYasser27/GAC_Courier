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

namespace coderush.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Delivery")]
    public class DeliveryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DeliveryController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetCourier()
        {
            List<Delivery> Items = await _context.Deliverys.Where(c=>c.IsActive==true && c.IsDeleted==false).ToListAsync();

            int Count = Items.Count();
            return Ok(new { Items, Count });
        }



        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromBody]CrudViewModel<Delivery> payload)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {

                ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                Delivery delivery = payload.value;

                delivery.CreatedBy = AppUser.Id;
                delivery.IsActive = true;
                delivery.IsDeleted = false;
                delivery.UserId = AppUser.Id;
                await _context.Deliverys.AddAsync(delivery);
                await _context.SaveChangesAsync();

                var DeliveryId = delivery.Id;
                var notification = new Notification
                {
                    DateTime = DateTime.UtcNow.AddHours(2),
                    DeliveryId = DeliveryId,
                    CreatedBy = AppUser.Id,
                    CreatedDate = DateTime.UtcNow.AddHours(2),
                    Type = NotificationType.DeliveryAdd,
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Update([FromBody]CrudViewModel<Delivery> payload)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                Delivery Delivery = payload.value;
                var deliveryindb = _context.Deliverys.AsNoTracking().SingleOrDefault(c => c.Id == (int)payload.key);
                Delivery.ModifiedBy = AppUser.Id;
                Delivery.IsDeleted = false;
                Delivery.IsActive = true;
                Delivery.UserId = deliveryindb.UserId;
                Delivery.ModifiedDate = DateTime.UtcNow.AddHours(2);
                _context.Deliverys.Update(Delivery);
                _context.SaveChanges();


                var notification = new Notification
                {
                    DateTime = DateTime.UtcNow.AddHours(2),
                    DeliveryId = Convert.ToInt32(payload.key),
                    CreatedBy = AppUser.Id,
                    CreatedDate = DateTime.UtcNow.AddHours(2),
                    Type = NotificationType.DeleiveryUpdate,
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

        [HttpPost("[action]")]
        public async Task<IActionResult> Remove([FromBody]CrudViewModel<Delivery> payload)
        {

            var transaction = _context.Database.BeginTransaction();
            try
            {
                ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                Delivery Delivery = _context.Deliverys.Single(c => c.Id == (int)payload.key);
                Delivery.DeletedBy = AppUser.Id;
                Delivery.DeletedDate = DateTime.UtcNow.AddHours(2);
                Delivery.IsActive = false;
                Delivery.IsDeleted = true;
                _context.Deliverys.Update(Delivery);
                _context.SaveChanges();

                var notification = new Notification
                {
                    DateTime = DateTime.UtcNow.AddHours(2),
                    DeliveryId = Convert.ToInt32(payload.key),
                    CreatedBy = AppUser.Id,
                    CreatedDate = DateTime.UtcNow.AddHours(2),
                    Type = NotificationType.DeleiveryDelete,
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
    }
}