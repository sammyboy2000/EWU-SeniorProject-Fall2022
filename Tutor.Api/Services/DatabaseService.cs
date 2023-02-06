using Microsoft.EntityFrameworkCore;
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

        internal int getClassId(string classCode)
        {
            return _context.Classes.Where(x => x.ClassCode.Contains(classCode)).Select(x => x.Id).First();
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
    }
}   