 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DEVTRACKR.API.Entities;
using DEVTRACKR.API.Models;
using DEVTRACKR.API.Persistence;
using Microsoft.AspNetCore.Mvc;

namespace DEVTRACKR.API.Controllers
{
    [ApiController]
    [Route("api/packages")]
    public class PackagesController : ControllerBase
    {
        private readonly DevTrackRContext _context;
        public PackagesController(DevTrackRContext context)
        {
            _context =  context; 
        }

        [HttpGet]
        public IActionResult GetAll(){
            // var packages = new List<Package> {
            //     new Package("Pack 1", 1.3M),
            //     new Package("Pack 2", 0.2M)
            // };
            var packages = _context.Packages;

            return Ok(packages);
        }

        [HttpGet("{code}")]
        public IActionResult GetByCode(string code){ 
        //    var package = new Package("Pack 2", 0.2M);
        var package = _context.Packages.SingleOrDefault(p => p.Code == code);
        if (package == null)
            return NotFound();
        return Ok(package);
        }

        [HttpPost]
        public IActionResult Post(AddPackageInputModel model){
            if(model.Title.Length < 10){
                return BadRequest("Title lenght must be at least 10 characters long.");
            }
            var package = new Package(model.Title, model.Weight);
            _context.Packages.Add(package);
            return CreatedAtAction("GetByCode", new { code = package.Code }, package);
        }

        // [HttpPut("{code}")]
        // public IActionResult Put(string code){ 
        //     return Ok();
        // }

        [HttpPost("{code}/updates")]
        public IActionResult PostUpdate(string code, AddPackageUpdateInputModel model){
            // var package = new Package("Pacote 1", 1.2M);
            var package = _context.Packages.SingleOrDefault(p => p.Code == code);
            if (package == null)
                return NotFound();

            package.AddUpdate(model.Status, model.Delivered);
            return Ok();
        }
    }   
}