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
    [Route("api/CourierSendType")]
    public class CourierSendTypeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourierSendTypeController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetCourierSendType()
        {
            List<CourierSendType> Items = await _context.CourierSendTypes.ToListAsync();

            int Count = Items.Count();
            return Ok(new { Items, Count });
        }
    }
}