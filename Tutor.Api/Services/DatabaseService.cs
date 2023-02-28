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

        //Added to get topics to populate dropdown --Jesse 2/17/2023
        internal IEnumerable<String> getTopics(string? searchString)
        { 
            if(searchString == null) 
            {
                //return _context.Classes.Select(x => x.ClassCode);
                return _context.Topics.Select(x => x.Topic1);
            }
            return _context.Topics.Select(x => x.Topic1)
                .Where(x => x.Contains(searchString));
        }

        internal void AddClass(string classCode)
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

        //Added to get questions to populate dropdown --Jesse 2/17/2023
        internal IEnumerable<String> GetAllQuestions(string searchString)
        { 
            if(searchString == null) 
            {
                return _context.Questions.Select(x => x.Question1);
                    
            }
            return _context.Questions.Select(x => x.Question1)
                .Where(x => x.Contains(searchString));
        }

        //Added to get questions to populate dropdown --Jesse 2/24/2023
        internal IEnumerable<Question> GetStudentsQuestions(int studentId)
        { 
            if(studentId == null) 
            {
                return _context.Questions;
                    
            }
            return _context.Questions.Where(x => x.StudentId == studentId);
        }

        //Added to get questions to populate dropdown --Jesse 2/27/2023
        internal IEnumerable<AnsweredQuestion> GetAnsweredQuestions(int studentId)
        { 
            if(studentId == null) 
            {
                return _context.AnsweredQuestions;
                    
            }
            return _context.AnsweredQuestions.Where(x => x.StudentId == studentId);
        }

        //Added to remove selected question from unanswered questions --Jesse 2/28/2023
        internal void StudentRemoveQuestion(Guid questionId)
        {
            Question q = _context.Questions.Where(x => x.QuestionId.Equals(questionId)).First();
            _context.Questions.Remove(q);
            _context.SaveChanges();
        }

        //Added to modify an existing question --Jesse 2/28/2023
        internal void StudentModifyQuestion(Question q)
        {
            _context.Questions.Update(q);
            _context.SaveChanges();
        }

        internal int GetStudentId(string studentUsername)
        {
            return _context.Students.Where(x => x.Email.Contains(studentUsername)).Select(x => x.Id).First();
        }

        internal int? GetClassId(string classCode)
        {
            try
            {
                return _context.Classes.Where(x => x.ClassCode.Contains(classCode)).Select(x => x.Id).First();
            }
            catch { return null; }
        }

        internal int GetTopicId(string topic)
        {
            return _context.Topics.Where(x => x.Topic1.Contains(topic)).Select(x => x.Id).First();
        }

        internal void AddQuestion(Question q)
        {
            _context.Questions.Add(q);
            _context.SaveChanges();
        }

        internal Question GetQuestion(Guid questionId)
        {
            return _context.Questions.Where(x => x.QuestionId == questionId).First();
        }

        internal IEnumerable<Question>? GetQuestions(String? classCode, String? topic)
        {
            if (classCode.IsNullOrEmpty() && topic.IsNullOrEmpty()) 
            {
                return _context.Questions;

            }
            else if (!classCode.IsNullOrEmpty() && topic.IsNullOrEmpty()) 
            {
                int? checkId = GetClassId(classCode);
                if (!checkId.HasValue) { return null; }
                int classId = checkId.Value;
                return _context.Questions.Where(x => x.ClassId == classId); 
            }
            else
            {
                int? checkId = GetClassId(classCode);
                if (!checkId.HasValue) { return null; }
                int classId = (int)checkId;
                int topicId = GetTopicId(topic);

               return _context.Questions.Where(x => x.ClassId == classId && x.TopicId == topicId);
            }


        }
        internal int GetTutorId(string tutorUsername)
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
       
        internal IEnumerable<TopicAggregate>? GetQuestionStatistics(string className, string topic)
        {
            List<TopicAggregate> topicAggregate = new();
            try
            {
                int classId = _context.Classes.Where(x => x.ClassCode == className).First().Id;
                int topicId = _context.Topics.Where(x => x.Topic1 == topic).First().Id;
                var questions = _context.Questions.Where(x => x.ClassId == classId && x.TopicId == topicId);
                foreach (var q in questions)
                {
                    if (!topicAggregate.Exists(x => x.TopicId == q.TopicId))
                    {
                        topicAggregate.Add(new TopicAggregate
                        {
                            ClassCode = className,
                            ClassId = classId,
                            Topic = topic,
                            TopicId = topicId,
                            Occurences = _context.Questions.Where(x => x.TopicId == q.TopicId).Count(),
                        });
                    }
                }
                return topicAggregate;
            }
            catch { return null; }
        }
        internal IEnumerable<TopicAggregate>? GetQuestionStatistics(string className)
        {
            List<TopicAggregate> topicAggregate = new();
            try
            {
                int classId = _context.Classes.Where(x => x.ClassCode == className).First().Id;
                var questions = _context.Questions.Where(x => x.ClassId == classId);
            foreach (var q in questions)
            {
                if (!topicAggregate.Exists(x => x.TopicId == q.TopicId))
                {
                    topicAggregate.Add(new TopicAggregate
                    {
                        ClassCode = className,
                        ClassId = classId,
                        Topic = _context.Topics.Where(x => x.Id == q.TopicId).First().Topic1,
                        TopicId = q.TopicId,
                        Occurences = _context.Questions.Where(x => x.TopicId == q.TopicId).Count(),
                    });
                }
            }
            return topicAggregate;
            }
            catch { return null; }
        }
        internal IEnumerable<TopicAggregate> GetQuestionStatistics()
        {
            List<TopicAggregate> topicAggregate = new();
            var questions = _context.Questions;
            foreach (var q in questions) 
            {
                if(!topicAggregate.Exists(x => x.TopicId==q.TopicId)){
                    topicAggregate.Add(new TopicAggregate
                    {
                        ClassCode = _context.Classes.Where(x => x.Id==q.ClassId).First().ClassCode,
                        ClassId = q.ClassId,
                        Topic = _context.Topics.Where(x => x.Id == q.TopicId).First().Topic1,
                        TopicId = q.TopicId,
                        Occurences = _context.Questions.Where(x => x.TopicId==q.TopicId).Count(),
                    });
                }
            }
            return topicAggregate;
        }
    }
}   