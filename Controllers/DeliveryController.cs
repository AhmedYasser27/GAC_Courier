﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace coderush.Controllers
{
    [Authorize(Roles = Pages.MainMenu.Delivery.RoleName)]
    public class DeliveryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}