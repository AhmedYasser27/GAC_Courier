using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using coderush.Models.CourierViewModel;
using coderush.Models.SyncfusionViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/WayBill")]
    public class WayBillController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public WayBillController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        // GET: api/Customer
        [HttpGet("[action]")]
        public async Task<IActionResult> GetWayBill()
        {


            List<WayBillViewModel> Items = await _context.WayBills.Where(c => c.IsActive == true && c.IsDeleted == false).Include(C => C.CourierRequest)
          .Select(c => new WayBillViewModel { Id = c.Id, ByWayBillId = c.ByWayBillId, CourierDestinationId = c.CourierDestinationId, IsFully = c.IsFully, WaybillNumber = c.WaybillNumber, Date = c.Date, CourierId = (c.CourierRequest.Where(a => a.CourierRequest.IsActive == true && a.CourierRequest.IsDeleted == false).Select(q => q.CourierRequestId.ToString())).ToArray() })
          .ToListAsync();
            //List<WayBillViewModel> Items = new List<WayBillViewModel>();
                int Count = Items.Count();
                return Ok(new { Items, Count });
            
          
            // var ItemVm = Items.Select(c => new WayBillViewModel { Id = c.Id, Date = c.Date });
            
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> ExpectedRequests()
        {

        //    List<CourierRequestViewModel> GetAllRequests = await _context.CourierRequests.Include(c => c.WayBill)
        //.Where(c => c.WayBill.Where(q => q.IsActive == true && q.IsDeleted == false && q.CourierRequestId == c.Id).Select(a => a.CourierRequest)/*.Select(q => q.CourierRequestId).Contains(c.Id)*/)
        //.Include(c => c.CourierDestination)
        //.Select(c => new CourierRequestViewModel { CourierId = c.Id, CourierNumber = (c.CourierNumber.ToString() + "-" + c.CourierDestination.Name) }).ToListAsync();



            List<CourierRequestViewModel> GetAllRequests = await   _context.CourierRequests.Include(c => c.WayBill)
        .Where(c => !c.WayBill.Where(v=>v.IsActive==true&& v.IsDeleted==false).Select(q => q.CourierRequestId).Contains(c.Id)).Where(a=>a.IsActive==true&&a.IsDeleted==false)
        .Include(c => c.CourierDestination)
        .Select(c => new CourierRequestViewModel { CourierId = c.Id, CourierNumber = (c.CourierNumber.ToString() + "-" + c.CourierDestination.Name) }).ToListAsync();
           

           
            return Ok(GetAllRequests);


            // var ItemVm = Items.Select(c => new WayBillViewModel { Id = c.Id, Date = c.Date });

        }


        [HttpPost("[action]")]
        public async Task<IActionResult> Insert([FromBody]CrudViewModel<WayBillViewModel> payload)
        {
            var transaction = _context.Database.BeginTransaction();
            try
            {

                ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                WayBillViewModel WayBill = payload.value;
                var StringLists = WayBill.CourierId.ToList();
                WayBill waybillsave = new WayBill
                {
                    ByWayBillId = WayBill.ByWayBillId,
                    Id = WayBill.Id,
                    IsFully = WayBill.IsFully,
                    CourierDestinationId = WayBill.CourierDestinationId,
                    Date = WayBill.Date,
                    WaybillNumber = WayBill.WaybillNumber,
                    IsActive = true,
                    IsDeleted = false,
                    CreatedBy = AppUser.Id,
                    CreatedDate = DateTime.UtcNow.AddHours(2),
                    UserId = AppUser.Id
                   
            };
                foreach (var item in StringLists)
                {
                    waybillsave.CourierRequest.Add(new CourierRequestWayBill { CourierRequestId = int.Parse(item) , IsActive=true,IsDeleted=false});
                    
                }
               await   _context.WayBills.AddAsync(waybillsave);

               await _context.SaveChangesAsync();
                
              

                var WayBillId = waybillsave.Id;
                var notification = new Notification
                {
                    DateTime = DateTime.UtcNow.AddHours(2),
                    WayBillId = WayBillId,
                    CreatedBy = AppUser.Id,
                    CreatedDate = DateTime.UtcNow.AddHours(2),
                    Type = NotificationType.WayBillAdd,
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
           public async Task<IActionResult> Update([FromBody]CrudViewModel<WayBillViewModel> payload)
           {
            var transaction = _context.Database.BeginTransaction();
            try
            {
                ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                var IntegarLists = (payload.value.CourierId.ToList()).ConvertAll(int.Parse);

                var WayBillInDb = _context.WayBills.Include(c => c.CourierRequest).Include(c=>c.User).FirstOrDefault(c => c.Id == payload.value.Id);
                WayBillInDb.ModifiedBy = AppUser.Id;
                WayBillInDb.ModifiedDate = DateTime.UtcNow.AddHours(2);
               
                var RemovedRequests = WayBillInDb.CourierRequest.Where(f => !IntegarLists.Contains(f.CourierRequestId)).ToList();
                foreach (var f in RemovedRequests)
                {
                    WayBillInDb.CourierRequest.Remove(f);
                }
                //Add new features
                var AddedRequests = IntegarLists.Where(CourierId => !WayBillInDb.CourierRequest.Any(f => f.CourierRequestId == CourierId));
                foreach (var id in AddedRequests)
                    WayBillInDb.CourierRequest.Add(new CourierRequestWayBill { CourierRequestId = id ,IsActive=true,IsDeleted=false});


                await _context.SaveChangesAsync();

                var notification = new Notification
                {
                    DateTime = DateTime.UtcNow.AddHours(2),
                    WayBillId = Convert.ToInt32(payload.key),
                    CreatedBy = AppUser.Id,
                    CreatedDate = DateTime.UtcNow.AddHours(2),
                    Type = NotificationType.WayBillUpdate,
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
        public async Task<IActionResult> Remove([FromBody]CrudViewModel<WayBillViewModel> payload)
        {
            //WayBill WayBill = _context.WayBills.Include(c =>c.CourierRequest)
            //    .Where(x => x.Id == (int)payload.key)
            //    .FirstOrDefault();
            //_context.WayBills.Remove(WayBill);
            //_context.SaveChanges();
          

            var transaction = _context.Database.BeginTransaction();
            try
            {
               
                ApplicationUser AppUser = await _userManager.GetUserAsync(User);
                WayBill WayBill = _context.WayBills.Include(c=>c.CourierRequest).Single(c => c.Id == (int)payload.key);

               
                foreach (var f in WayBill.CourierRequest)
                {
                    f.IsActive = false;
                    f.IsDeleted = true;
                }
                WayBill.DeletedBy = AppUser.Id;
                WayBill.DeletedDate = DateTime.UtcNow.AddHours(2);
                WayBill.IsActive = false;
                WayBill.IsDeleted = true;
                _context.WayBills.Update(WayBill);
                _context.SaveChanges();

                var notification = new Notification
                {
                    DateTime = DateTime.UtcNow.AddHours(2),
                    WayBillId = Convert.ToInt32(payload.key),
                    CreatedBy = AppUser.Id,
                    CreatedDate = DateTime.UtcNow.AddHours(2),
                    Type = NotificationType.WayBillDelete,
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