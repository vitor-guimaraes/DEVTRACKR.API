 using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DEVTRACKR.API.Entities;
using DEVTRACKR.API.Models;
using DEVTRACKR.API.Persistence;
using DEVTRACKR.API.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace DEVTRACKR.API.Controllers
{
    [ApiController]
    [Route("api/packages")]
    public class PackagesController : ControllerBase
    {
        // private readonly DevTrackRContext _context;
        // public PackagesController(DevTrackRContext context)
        // {
        //     _context =  context; 
        // }

        private readonly IPackageRepository _repository;

        private readonly ISendGridClient _client;

        // public PackagesController(IPackageRepository repository)
        // {
        //     _repository =  repository; 
        // }

        public PackagesController(IPackageRepository repository, ISendGridClient client)
        {
            _repository =  repository;
            _client = client;
        }

        [HttpGet]
        public IActionResult GetAll(){
            // var packages = new List<Package> {
            //     new Package("Pack 1", 1.3M),
            //     new Package("Pack 2", 0.2M)
            // };
            // var packages = _context.Packages;

            var packages = _repository.GetAll();

            return Ok(packages);
        }

        [HttpGet("{code}")]
        public IActionResult GetByCode(string code){ 
        //    var package = new Package("Pack 2", 0.2M);
        // var package = _context.Packages.Include(p => p.Updates).SingleOrDefault(p => p.Code == code);
        // if (package == null)
        //     return NotFound();

        var package = _repository.GetByCode(code);
        return Ok(package);
        }

        [HttpPost]
        /// <summary>
        /// CADASTRO DE UM PACOTE
        /// </summary>
        /// <param name="model">Dados do pacote</param>
        /// <returns>Objeto recém criado</returns>
        public async Task<IActionResult> Post(AddPackageInputModel model){
            if(model.Title.Length < 10){
                return BadRequest("Title lenght must be at least 10 characters long.");
            }
            var package = new Package(model.Title, model.Weight);
            // _context.Packages.Add(package);
            // _context.SaveChanges();

            _repository.Add(package);

            var message = new SendGridMessage {
                From = new EmailAddress("email", "nickname"),
                Subject = "Your package was dispatched.",
                PlainTextContent = $"Your Package with codw {package.Code} was dispatched."
            };

            message.AddTo(model.SenderEmail, model.SenderName);

            await _client.SendEmailAsync(message);

            return CreatedAtAction("GetByCode", new { code = package.Code }, package);
        }

        // [HttpPut("{code}")]
        // public IActionResult Put(string code){ 
        //     return Ok();
        // }

        [HttpPost("{code}/updates")]
        public async Task<IActionResult> PostUpdate(string code, AddPackageUpdateInputModel model){
            // var package = new Package("Pacote 1", 1.2M);

            // var package = _context.Packages.SingleOrDefault(p => p.Code == code);
            // if (package == null)
            //     return NotFound();

            // package.AddUpdate(model.Status, model.Delivered);
            // _context.SaveChanges();

            var package = _repository.GetByCode(code);

            if (package == null)
                return NotFound();
            
            package.AddUpdate(model.Status, model.Delivered);

            var message = new SendGridMessage {
                From = new EmailAddress("email", "nickname"),
                Subject = "Your package has been updated." ,
                PlainTextContent = $"{package.Code} has been updated"
            };

            message.AddTo(model.SenderEmail, model.SenderName);

            await _client.SendEmailAsync(message);

            _repository.Update(package);

            return Ok();
        }
    }   
}