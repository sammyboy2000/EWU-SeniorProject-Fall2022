using Microsoft.AspNetCore.Mvc;
using Tutor.Api.Models;
using Tutor.api;
using NETCore.MailKit.Core;
using Tutor.api.Services;

namespace Tutor.Api.Controllers;
    [Route("Email")]
    [ApiController]

    public class EmailController  : Controller
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly Services.EmailService _service;

    public EmailController(ILogger<DatabaseController> logger,
           Services.EmailService emailService)
    {
        _logger = logger;
        _service = emailService;
    }
    [HttpGet("testEmail(tutoring.ewu@gmail.com)")]
        public Boolean TestSend()
        {
            var rng = new Random();
            var message = new Message(new string[] { "tutoring.ewu@gmail.com" }, "Test email", "This is a test email.");
            _service.SendEmail(message);
            return true; 
        }
    }

