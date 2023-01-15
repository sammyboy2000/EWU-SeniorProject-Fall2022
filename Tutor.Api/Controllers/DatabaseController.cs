using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
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
        private readonly MySqlConnection _connecection;

        public DatabaseController(ILogger<DatabaseController> logger,
            DatabaseService databaseService, MySqlConnection connection)
        {
            _logger = logger;
            _databaseService = databaseService;
            _connecection = connection;
        }

        [HttpGet]
        public IEnumerable<Class> GetClasses() { 
            _logger.LogInformation("DatabaseController.Get()");
            return _databaseService.GetClasses(_connecection);
        }

        [HttpPost]
        public void Post([FromBody] GameScore score)
        {
            //_logger.LogInformation("LeaderBoardController.Post()");
            //_leaderBoardService.AddScore(score);
        }



    }
}
