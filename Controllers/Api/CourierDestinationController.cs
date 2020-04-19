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
    [Route("api/CourierDestination")]
    public class CourierDestinationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourierDestinationController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetCourierDestination()
        {
            List<CourierDestination> Items = await _context.CourierDestinations.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
           // return Ok();
        }
    }
}