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
        internal IEnumerable<String> getClasses(string searchString)
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

        internal IEnumerable<string> getTopics(string? searchString)
        {
            throw new NotImplementedException();
        }
        /*
            if (searchString == null)
            {
                return _context.Topics.Select(x => x.Topic1);

            }
            return _context.Topics.Select(x => x.Topic1)
                .Where(x => x.Contains(searchString));
        */
    }
}