using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tutor.api.Services;
using Tutor.Api.Identity;
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
        [HttpGet("GetAllQuestions")]
        [Authorize(Roles = Roles.Tutor)]
        public IEnumerable<String> GetAllQuestions(String? searchString)
        {
            return _service.GetAllQuestions(searchString);
        }

        [HttpGet("GetQuestionData")]
        [Authorize(Roles = Roles.Admin)]
        public IEnumerable<TopicAggregate>? GetQuestionData(String? className, String? topicName) 
        { 
            if(className == null && topicName == null) 
            {
                return _service.GetQuestionStatistics();
            }
            if(className != null && topicName == null)
            {
                return _service.GetQuestionStatistics(className);
            }
            else
            {
                return _service.GetQuestionStatistics(className, topicName);
            }
        }

    }
}
