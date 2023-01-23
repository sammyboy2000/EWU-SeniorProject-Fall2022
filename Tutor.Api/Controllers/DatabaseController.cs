using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tutor.api.Services;
using Tutor.Api.Models;

namespace Tutor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly DatabaseService _service;

        public DatabaseController(ILogger<DatabaseController> logger,
           DatabaseService databaseService)
        {
            _logger = logger;
            _service= databaseService;
         }

        [HttpGet("getClasses")]
        [Authorize]
        public IEnumerable<String> getClasses(String? searchString)
        {
            return _service.getClasses(searchString);
        }





    }
}
