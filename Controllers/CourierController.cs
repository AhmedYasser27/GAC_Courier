using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using coderush.Controllers.Report;
using DevExpress.XtraPrinting;
using System.IO;

namespace coderush.Controllers
{
    
    public class CourierController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _hostingEnvironment;
      
        public CourierController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment; 
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Save(int? id)
        {
            if(id == null)
            {
                id = 0;
            }
           var v = _context.CourierRequests.Include(c => c.CourierDescription).Include(c => c.CourierDestination).Include(q => q.CourierSendType).Where(a => a.Id == id && a.IsActive == true && a.IsDeleted == false).FirstOrDefault();

            ViewBag.GetAllDescription = _context.CourierDescriptions.ToList();
            ViewBag.GetAllDestinations = _context.CourierDestinations.ToList();
            ViewBag.GetAllSendTyps = _context.CourierSendTypes.ToList();
                return PartialView(v);

        }

        [HttpGet]
        public IActionResult Print(int id)
        {
            List<CourierRequest> AllRequestList = new List<CourierRequest>();
            CourierRequest CourierRequest = _context.CourierRequests
                .Include(z => z.CourierDescription)
                .Include(a => a.CourierDestination)
                .Include(c => c.CourierSendType)
                .SingleOrDefault(q => q.Id == id);
            
            RequestReport NewReport = new RequestReport();
            AllRequestList.Add(CourierRequest);
            NewReport.DataSource = AllRequestList;




            
            using (MemoryStream ms = new MemoryStream())
            {
                NewReport.ExportToPdf(ms, new PdfExportOptions() { ShowPrintDialogOnOpen = true });
                return ExportDocument(ms.ToArray(), "pdf", "Report.pdf", true);
            }

        }

        private FileResult ExportDocument(byte[] document, string format, string fileName, bool isInline)
        {
            string contentType;
            string disposition = (isInline) ? "inline" : "attachment";

            switch (format.ToLower())
            {
                case "docx":
                    contentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                    break;
                case "xls":
                    contentType = "application/vnd.ms-excel";
                    break;
                case "xlsx":
                    contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    break;
                case "mht":
                    contentType = "message/rfc822";
                    break;
                case "html":
                    contentType = "text/html";
                    break;
                case "txt":
                case "csv":
                    contentType = "text/plain";
                    break;
                case "png":
                    contentType = "image/png";
                    break;
                default:
                    contentType = String.Format("application/{0}", format);
                    break;
            }

            Response.Headers.Add("Content-Disposition", String.Format("{0}; filename={1}", disposition, fileName));
            return File(document, contentType);
        }

    }
}