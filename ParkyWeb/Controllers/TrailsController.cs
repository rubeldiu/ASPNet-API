using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using ParkyWeb.Models;
using ParkyWeb.Repository.IRepository;
using Microsoft.AspNetCore.Mvc.Rendering;
using ParkyWeb.Models.ViewModel;

namespace ParkyWeb.Controllers
{
    public class TrailsController : Controller
    {
        private readonly INationalParkRepository _npRepo;
        private readonly ITrailRepository _trailRepo;
        public TrailsController(INationalParkRepository npRepo,ITrailRepository trailRepo)
        {
            _npRepo = npRepo;
            _trailRepo = trailRepo;
        }
        public IActionResult Index()
        {
            return View( new Trail() { });
        }
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
            TrailsVM objVM = new TrailsVM()
            {
                NationalParkList = npList.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                }),
                Trails = new Trail()
            };

          
            if (id == null)
            {
                //this will be insert
                return View(objVM) ;
            }
            //this will be update
            objVM.Trails = await _trailRepo.GetAsync(SD.TrailAPIParh, id.GetValueOrDefault());
            if (objVM.Trails == null)
            {
                return NotFound();
            }
            return View(objVM);
            
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult > Upsert(TrailsVM obj)
        {
            if (ModelState.IsValid) { 
                if (obj.Trails.Id == 0)
                {
                    await _trailRepo.CreateAsync(SD.TrailAPIParh, obj.Trails);
                }
                else
                {
                    await _trailRepo.UpdateAsync(SD.TrailAPIParh + obj.Trails.Id, obj.Trails);
                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
                IEnumerable<NationalPark> npList = await _npRepo.GetAllAsync(SD.NationalParkAPIPath);
                TrailsVM objVM = new TrailsVM()
                {
                    NationalParkList = npList.Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    }),
                    Trails = obj.Trails
                };

                return View(objVM);
            }
        }

        public async Task <IActionResult > GetAllTrail()
        {
            return Json(new { data = await _trailRepo.GetAllAsync(SD.TrailAPIParh) });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _trailRepo.DeleteAsync(SD.TrailAPIParh, id);
            if (status )
            {
                return Json(new { success=true, message="Delete Successful"});
            }
            return Json(new { success = false, message = "Delete Not Successful" });
        }
    }
}