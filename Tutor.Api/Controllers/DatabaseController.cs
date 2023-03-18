using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
            _service = databaseService;
        }

        [HttpGet("getClasses")]
        [Authorize]
        public IEnumerable<String> GetClasses(String? searchString)
        {
            return _service.GetClasses(searchString);
        }

        [HttpGet("getClassesAdmin")]
        [Authorize]
        public IEnumerable<Class> GetClassesAdmin()
        {
            return _service.GetClassesAdmin();
        }

        //Added to get topics to populate dropdown --Jesse 2/17/2023
        [HttpGet("getTopics")]
        [Authorize]
        public IEnumerable<String> GetTopics(String? classCode)
        {
            if (classCode == null)
            {
                return null!;
            }
            else
            {
                int? checkId = _service.GetClassId(classCode);
                int classId = (int)checkId!;
                return _service.GetClassTopics(classId);
            }
        }

        [HttpGet("getTopicName")]
        [Authorize]
        public String GetTopicName(int? i)
        {
            return _service.GetTopicName(i);
        }

        [HttpGet("getTopicNameAdmin")]
        [Authorize(Roles = Roles.Admin)]
        public IEnumerable<Topic> GetTopicNamesAdmin()
        {
            return _service.GetTopicNames();
        }

        //Added to get questions to populate dropdown --Jesse 2/17/2023
        [HttpGet("GetAllQuestions")]
        [Authorize(Roles = Roles.Tutor)]
        public IEnumerable<String> GetAllQuestions(String? searchString)
        {
            return _service.GetAllQuestions(searchString!);
        }

        [HttpGet("GetQuestionData")]
        [Authorize(Roles = Roles.Admin)]
        public IEnumerable<TopicAggregate>? GetQuestionData(String? className, String? topicName)
        {
            if (className == null && topicName == null)
            {
                return _service.GetQuestionStatistics();
            }
            if (className != null && topicName == null)
            {
                return _service.GetQuestionStatistics(className);
            }
            else
            {
                return _service.GetQuestionStatistics(className!, topicName!);
            }
        }


// Admin User Methods
        [HttpGet("GetUser")]
        [Authorize(Roles = Roles.Admin)]
        public String GetUser(String? username)
        {
            return _service.GetUserEmail(username!);
        }

        [HttpGet("GetUserRoles")]
        [Authorize(Roles = Roles.Admin)]
        public IEnumerable<bool> GetUserRoles(String? username)
        {
            return _service.GetUserRoles(username!);
        }

        [HttpPost("ModifyUserRoles")]
        [Authorize(Roles = Roles.Admin)]
        public bool ModifyUserRoles(String? username, bool? isStudent, bool? isTutor, bool? isAdmin)
        {
            return _service.ModifyUserRoles(username!, (bool)isStudent!, (bool)isTutor!, (bool)isAdmin!);
        }

        [HttpPost("RemoveUser")]
        [Authorize(Roles = Roles.Admin)]
        public bool RemoveUser(String? username)
        {
            return _service.RemoveUser(username!);
        }

// Class Methods
        [HttpPost("AddClass")]
        [Authorize]
        public String AddClass(string classCode, string? className)
        {
            bool result = _service.AddClass(classCode, className!);
            if (result) { return "Success"; }
            else { return "Failed to add class."; }
        }

        [HttpPost("UpdateClass")]
        [Authorize(Roles = "Admin")]
        public String UpdateClass(string? classCode, string? className) {
            if (classCode.IsNullOrEmpty()) { return "Class Code cannot be empty"; }
            int? classId = _service.GetClassId(classCode!);
            if (className.IsNullOrEmpty()) { return "Class Name cannot be empty."; }
            bool result = _service.UpdateClass(classId!, classCode!, className!);
            if (result) { return "Success"; }
            else { return "Failed to modify class."; }
        }

        [HttpPost("RemoveClass")]
        [Authorize(Roles = "Admin")]
        public String RemoveClass(int? classId)
        {
            if (classId == null) { return "Class Id cannot be empty"; }
            bool result = _service.RemoveClass(classId!);
            if (result) { return "Success"; }
            else { return "Failed to remove class."; }
        }

// Topic Methods
        [HttpPost("AddTopic")]
        [Authorize]
        public String AddTopic(string? classCode, string? topic)
        {
            if (classCode.IsNullOrEmpty()) { return "Class Code cannot be empty"; }
            if (topic.IsNullOrEmpty()) { return "Topic cannot be empty."; }
            bool result = _service.AddTopic(classCode!, topic!);
            if (result) { return "Success"; }
            else { return "Failed to add topic."; }
        }

        [HttpPost("RemoveTopic")]
        [Authorize(Roles = "Admin, Tutor")]
        public String RemoveTopic(string? classCode, string? topic)
        {
            if (classCode.IsNullOrEmpty()) { return "Class Code cannot be empty"; }
            if (topic.IsNullOrEmpty()) { return "Topic cannot be empty."; }
            bool result = _service.RemoveTopic(classCode!, topic!);
            if (result) { return "Success"; }
            else { return "Failed to Remove topic."; }
        }

        [HttpPost("ModifyTopic")]
        [Authorize(Roles = "Admin, Tutor")]
        public String ModifyTopic(string? classCode, string? topic, string? newTopic)
        {
            if (classCode.IsNullOrEmpty()) { return "Class Code cannot be empty"; }
            if (topic.IsNullOrEmpty()) { return "Topic cannot be empty."; }
            if (newTopic.IsNullOrEmpty()) { return "New Topic cannot be empty."; }
            bool result = _service.ModifyTopic(classCode!, topic!, newTopic!);
            if (result) { return "Success"; }
            else { return "Failed to Modify topic."; }
        }
    }
}
