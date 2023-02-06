using Microsoft.AspNetCore.Mvc;
using Tutor.Api.Models;
using Tutor.api;
using NETCore.MailKit.Core;
using Tutor.api.Services;
using Microsoft.AspNetCore.Authorization;
using Tutor.Api.Identity;

namespace Tutor.Api.Controllers;
    [Route("Email")]
    [ApiController]

    public class EmailController  : Controller
    {
        private readonly ILogger<EmailController> _logger;
        private readonly Services.EmailService _service;

    public EmailController(ILogger<EmailController> logger,
           Services.EmailService emailService)
    {
        _logger = logger;
        _service = emailService;
    }
    [HttpGet("testEmail(tutoring.ewu@gmail.com)")]
    [Authorize(Roles = Roles.Admin)]
        public Boolean TestSend()
        {
            var message = new Message(new string[] { "tutoring.ewu@gmail.com" }, "Test email", "This is a test email.");
            _service.SendEmail(message);
            return true; 
        }
    }

