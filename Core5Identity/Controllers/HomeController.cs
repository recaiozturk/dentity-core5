﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Core5Identity.Controllers
{
    public class HomeController : Controller
    {
        //[Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
