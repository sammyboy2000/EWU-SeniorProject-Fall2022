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

        //Added to get topics to populate dropdown --Jesse 2/17/2023
        [HttpGet("getTopics")]
        [Authorize]
        public IEnumerable<String> getTopics(String? searchString)
        {
            return _service.getTopics(searchString);
        }

        //Added to get questions to populate dropdown --Jesse 2/17/2023
        [HttpGet("GetQuestions")]
        [Authorize]
        public IEnumerable<Question> GetQuestions(String? searchString)
        {
            return _service.GetQuestions(searchString);
        }

    }
}
