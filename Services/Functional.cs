﻿using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace coderush.Services
{
    public class Functional : IFunctional
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoles _roles;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;

        public Functional(UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           SignInManager<ApplicationUser> signInManager,
           IRoles roles,
           IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _roles = roles;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
        }

      

        public async Task InitAppData()
        {
            try
            {
                List<CourierDescription> CourierDescriptions = new List<CourierDescription>() {
                    new CourierDescription{Name = "Envelope"},
                    new CourierDescription{Name = "Box"},
                    new CourierDescription{Name = "Furniture"},
                    new CourierDescription{Name = "Computer"},
                    new CourierDescription{Name = "Paper"},
                };
                await _context.CourierDescriptions.AddRangeAsync(CourierDescriptions);
                await _context.SaveChangesAsync();

                List<CourierDestination> CourierDomestics = new List<CourierDestination>() {
                    new CourierDestination{Name = "GAC Cairo"},
                    new CourierDestination{Name = "GAC Alex"},
                    new CourierDestination{Name = "GAC Suez"},
                    new CourierDestination{Name = "GAC PSD"},
                    new CourierDestination{Name = "GAC Alex Port"},
                    new CourierDestination{Name = "GAC Damietta"},
                    new CourierDestination{Name = "CIB Port Said"},
                    new CourierDestination{Name = "GAC Airport"},
                    new CourierDestination{Name = "GAC Safaga"},
                    new CourierDestination{Name = "GAC Ismailia"},
                    new CourierDestination{Name = "Other,Pls Specify"},
                    new CourierDestination{Name = "Ordinery Mail"},
                    new CourierDestination{Name = "Reqistered Mail"},
                    new CourierDestination{Name = "Car / Super Jet"},
                    new CourierDestination{Name = "GAC's Account"},
                    new CourierDestination{Name = "Client's Account"}
                };
                await _context.CourierDestinations.AddRangeAsync(CourierDomestics);
                await _context.SaveChangesAsync();


                List<ByWayBill> ByWayBill = new List<ByWayBill>() {
                    new ByWayBill{Name = "Aramex"},
                    new ByWayBill{Name = "DHL"},
                    new ByWayBill{Name = "GAC"},
                    new ByWayBill{Name = "Superjet"},
                    new ByWayBill{Name = "TNT"}
                };

                await _context.ByWayBills.AddRangeAsync(ByWayBill);
                await _context.SaveChangesAsync();


                List<CourierSendType> CourierSendTypes = new List<CourierSendType>() {
                    new CourierSendType{Name = "Business"},
                    new CourierSendType{Name = "Personal"}
                };
                await _context.CourierSendTypes.AddRangeAsync(CourierSendTypes);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendEmailBySendGridAsync(string apiKey, 
            string fromEmail, 
            string fromFullName, 
            string subject, 
            string message, 
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromFullName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, email));
            await client.SendEmailAsync(msg);

        }

        public async Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(fromEmail, fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = smtpUser,
                    Password = smtpPassword
                };
                smtp.Credentials = credential;
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.EnableSsl = smtpSSL;
                await smtp.SendMailAsync(message);

            }

        }

        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                await _roles.GenerateRolesFromPagesAsync();

                ApplicationUser superAdmin = new ApplicationUser();
                superAdmin.Email = _superAdminDefaultOptions.Email;
                superAdmin.UserName = superAdmin.Email;
                superAdmin.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);

                if (result.Succeeded)
                {
                    //add to user profile
                    UserProfile profile = new UserProfile();
                    profile.FirstName = "Super";
                    profile.LastName = "Admin";
                    profile.Email = superAdmin.Email;
                    profile.ApplicationUserId = superAdmin.Id;
                    await _context.UserProfile.AddAsync(profile);
                    await _context.SaveChangesAsync();

                    await _roles.AddToRoles(superAdmin.Id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> UploadFile(List<IFormFile> files, IHostingEnvironment env, string uploadFolder)
        {
            var result = "";

            var webRoot = env.WebRootPath;
            var uploads = System.IO.Path.Combine(webRoot, uploadFolder);
            var extension = "";
            var filePath = "";
            var fileName = "";


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    extension = System.IO.Path.GetExtension(formFile.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    filePath = System.IO.Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    result = fileName;

                }
            }

            return result;
        }

    }
}
