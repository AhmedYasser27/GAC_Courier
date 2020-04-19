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
    [Route("api/CourierDescription")]
    public class CourierDescriptionController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CourierDescriptionController(ApplicationDbContext context)
        {
            _context = context;
        }


        // GET: api/Customer
        [HttpGet]
        public async Task<IActionResult> GetCourierDescription()
        {
            List<CourierDescription> Items = await _context.CourierDescriptions.ToListAsync();
            int Count = Items.Count();
            return Ok(new { Items, Count });
        }
    }
}