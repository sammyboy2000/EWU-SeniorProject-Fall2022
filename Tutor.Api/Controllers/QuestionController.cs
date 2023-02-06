using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NETCore.MailKit.Core;
using Tutor.api.Services;
using Tutor.Api.Identity;
using Tutor.Api.Models;

namespace Tutor.Api.Controllers
{
    [Route("Questions")]
    [ApiController]
    public class QuestionController : Controller
    {
        private readonly ILogger<QuestionController> _logger;
        private readonly Services.EmailService _service;
        private readonly DatabaseService _database;
        public QuestionController(ILogger<QuestionController> logger, Services.EmailService service, DatabaseService database)
        {
            _logger = logger;
            _service = service;
            _database = database;   
        }
        [HttpGet]
        [Authorize(Roles = Roles.Student)]
        public String AddQuestion(String studentUsername, String classCode, String topic, String question) 
        { 
            if (string.IsNullOrWhiteSpace(studentUsername)) { return "Error, studentUsername cannot be blank."; }
            if (string.IsNullOrWhiteSpace(classCode)) { return "Error, classCode cannot be blank."; }
            if (string.IsNullOrWhiteSpace(topic)) { return "Error, topic cannot be blank."; }
            if (string.IsNullOrWhiteSpace(question)) { return "Error, question cannot be blank."; }

            int studentId = _database.getStudentId(studentUsername);
            int classId = _database.getClassId(classCode);
            int topicId = _database.getTopicId(topic);

            Question q = new();
            q.StudentId= studentId;
            q.ClassId= classId;
            q.TopicId= topicId;
            q.Question1= question;

            try
            {
                _database.addQuestion(q);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return "Error, failed to save question.";
            }

            if(_service.SendQuestionConfirmation(q.QuestionId, studentUsername))
            {
                return "Question " + q.QuestionId + " has successfully beem submitted.";
            }
            else
            {
                return "Question " + q.QuestionId + " has successfully beem submitted, but email confirmation has not been sent due to an error.";
            }
        }

        
    }

}
