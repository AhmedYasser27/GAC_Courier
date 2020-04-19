using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace coderush.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/ByWayBill")]
    public class ByWayBillController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ByWayBillController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetByWayBill()
        {
            List<ByWayBill> Items = await _context.ByWayBills.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }
    }
}