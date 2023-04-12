﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;

namespace MDP.AspNetCore.Authentication.Liff.Lab
{
    public class HomeController : Controller
    {
        // Methods
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
