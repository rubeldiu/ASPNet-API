﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParkyWeb.Models;
using ParkyWeb.Models.ViewModel;
using ParkyWeb.Repository.IRepository;

namespace ParkyWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;

        public HomeController(ILogger<HomeController> logger, INationalParkRepository npRepo, ITrailRepository trailRepo)
        {
            _logger = logger;
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }

        public async Task< IActionResult> Index()
        {
            IndexVM listOfParkAndTrails = new IndexVM()
            {
                NationalParklist = await _npRepo.GetAllAsync(SD.NationalParkAPIPath),
                TrailList = await _trailRepo.GetAllAsync(SD.TrailAPIParh)
            };
            return View(listOfParkAndTrails);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
