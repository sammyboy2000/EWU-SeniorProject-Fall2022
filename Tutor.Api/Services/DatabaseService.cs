using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using Tutor.Api.Models;

namespace Tutor.api.Services
{
    public class DatabaseService
    {
        private readonly tutor_dbContext _context;

        public DatabaseService(tutor_dbContext context)
        {
            _context = context;
        }
        internal IEnumerable<String> getClasses(string? searchString)
        { 
            if(searchString == null) 
            {
                return _context.Classes.Select(x => x.ClassCode);
                    
            }
            return _context.Classes.Select(x => x.ClassCode)
                .Where(x => x.Contains(searchString));
        }
        internal void addClass(string classCode)
        {
            var classes = _context.Classes.ToDictionary(f => f.ClassCode);

                if (!classes.ContainsKey(classCode))
                {
                    _context.Classes.Add(new Class { ClassCode = classCode });
                }
            
            _context.SaveChanges();

        }

        internal IEnumerable<Question> GetQuestions(string searchString)
        {
            if (searchString == null)
            {
                return _context.Questions;

            }
            return _context.Questions
                .Where(x => x.QuestionId.ToString().Contains(searchString));
        }

        internal int getStudentId(string studentUsername)
        {
            return _context.Students.Where(x => x.Email.Contains(studentUsername)).Select(x => x.Id).First();
        }

        internal int? getClassId(string classCode)
        {
            try
            {
                return _context.Classes.Where(x => x.ClassCode.Contains(classCode)).Select(x => x.Id).First();
            }
            catch { return null; }
        }

        internal int getTopicId(string topic)
        {
            return _context.Topics.Where(x => x.Topic1.Contains(topic)).Select(x => x.Id).First();
        }

        internal void addQuestion(Question q)
        {
            _context.Questions.Add(q);
            _context.SaveChanges();
        }

        internal Question GetQuestion(Guid questionId)
        {
            return _context.Questions.Where(x => x.QuestionId == questionId).First();
        }

        internal IEnumerable<Question>? GetQuestions(String classCode, String topic)
        {
            if (classCode.IsNullOrEmpty() && topic.IsNullOrEmpty()) 
            {
                return _context.Questions;

            }
            else if (!classCode.IsNullOrEmpty() && topic.IsNullOrEmpty()) 
            {
                int? checkId = getClassId(classCode);
                if (!checkId.HasValue) { return null; }
                int classId = checkId.Value;
                return _context.Questions.Where(x => x.ClassId == classId); 
            }
            else
            {
                int? checkId = getClassId(classCode);
                if (!checkId.HasValue) { return null; }
                int classId = (int)checkId;
                int topicId = getTopicId(topic);

               return _context.Questions.Where(x => x.ClassId == classId && x.TopicId == topicId);
            }


        }
        internal int getTutorId(string tutorUsername)
        {
            int internalId = _context.ApiUsers.Where(x => x.ExternalId.Contains(tutorUsername)).Select(x => x.UserId).First();
            return _context.Tutors.Where(x => x.UserId == internalId).Select(x => x.Id).First();
        }

        internal Boolean AnswerQuestion(AnsweredQuestion a)
        {
            Guid questionId = a.QuestionId;
            try
            {
                _context.AnsweredQuestions.Add(a);
                _context.SaveChanges();
                return RemoveQuestion(questionId);
            } 
            catch { return false; }
        }
        private bool RemoveQuestion(Guid questionId)
        {
            Question q = _context.Questions.Where(x => x.QuestionId.Equals(questionId)).First();
            if (q == null) { return false; }
            return RemoveQuestion(q);
        }
        private bool RemoveQuestion(Question q)
        {
            try
            {
                _context.Questions.Remove(q);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }
       
    }
}   