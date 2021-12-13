using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Tabim_Proje.Data;
using Tabim_Proje.Models;

namespace Tabim_Proje.Controllers
{
    [Authorize]
    public class RequestsController : Controller
    {
        private readonly IWebHostEnvironment _hostingEnv;
        private readonly ApplicationDbContext _db;

        public RequestsController(IWebHostEnvironment hostingEnv, ApplicationDbContext db)
        {
            _hostingEnv = hostingEnv;
            _db = db;
        }

        public IActionResult NewRequest()
        {
            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult NewRequest(RequestVM requestVM)
        {


            if (requestVM.File != null)
            {
                var fileName = Path.GetFileName(requestVM.File.FileName);
                //Control if it is pdf file
                string ext = Path.GetExtension(requestVM.File.FileName);
                if (ext.ToLower() != ".pdf")
                {
                    return View();
                }

                var filePath = Path.Combine(_hostingEnv.WebRootPath, "documents", fileName);

                using (var fileSteam = new FileStream(filePath, FileMode.Create))
                {
                    requestVM.File.CopyToAsync(fileSteam);
                }

                // To save filePath to database
                UserRequest userRequest = new();
                userRequest.ApplicationUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                userRequest.UserName = requestVM.UserName;
                userRequest.UserLastName = requestVM.UserLastName;
                userRequest.Explanation = requestVM.Explanation;
                userRequest.Document = filePath;
                userRequest.DocumentName = fileName;
                _db.UserRequests.Add(userRequest);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
            return View();
        }


    }
}
