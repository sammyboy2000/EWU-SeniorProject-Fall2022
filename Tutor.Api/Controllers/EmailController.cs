using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tutor.Api.Identity;
using Tutor.Api.Models;

namespace Tutor.Api.Controllers;
[Route("Email")]
[ApiController]

public class EmailController : Controller
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

