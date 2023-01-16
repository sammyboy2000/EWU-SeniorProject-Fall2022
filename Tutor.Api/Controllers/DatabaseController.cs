using Microsoft.AspNetCore.Mvc;
using Tutor.api.Services;
using Tutor.Api.Services;

namespace Tutor.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DatabaseController
    {
        private readonly ILogger<DatabaseController> _logger;
        private readonly DatabaseService _databaseService;

        public DatabaseController(ILogger<DatabaseController> logger,
            DatabaseService databaseService)
        {
            _logger = logger;
            _databaseService = databaseService;
        }

        [HttpGet("~/GetClasses")]
        public IEnumerable<Class> GetClasses() { 
            _logger.LogInformation("DatabaseController.Get()");
            return _databaseService.GetClasses();
        }
        [HttpGet("~/GetSchema")]
        public IEnumerable<String> GetSchema()
        {
            _logger.LogInformation("DatabaseController.Get()");
            return _databaseService.GetSchema();
        }



    }
}
