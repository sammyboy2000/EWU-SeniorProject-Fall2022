using Microsoft.IdentityModel.Tokens;
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
        public IEnumerable<String> GetClasses(string? searchString)
        {
            if (searchString == null)
            {
                return _context.Classes.OrderBy(x => x.ClassCode).Select(x => x.ClassCode);

            }
            return _context.Classes.OrderBy(x => x.ClassCode).Select(x => x.ClassCode)
                .Where(x => x.Contains(searchString));
        }

        public IEnumerable<Class> GetClassesAdmin()
        {
            return _context.Classes.OrderBy(x => x.ClassCode);
        }

        //Added to get topics to populate dropdown --Jesse 2/28/2023
        public IEnumerable<String> GetClassTopics(int? classId)
        {
            if (classId == null)
            {
                return _context.Topics.Select(x => x.Topic1);
            }
            return _context.Topics.Where(x => x.ClassId == classId).Select(x => x.Topic1);
        }

        //Added to get topics to populate dropdown --Jesse 2/17/2023 %% No longer needed in student %%
        public IEnumerable<String> GetTopics(string? searchString)
        {
            if (searchString.IsNullOrEmpty())
            {
                //return _context.Classes.Select(x => x.ClassCode);
                return _context.Topics.Select(x => x.Topic1);
            }
            return _context.Topics.Select(x => x.Topic1).Where(x => x.Contains(searchString!));
        }

        public String GetTopicName(int? searchString)
        {
            if (searchString == null)
            {
                return "";
            }
            return _context.Topics.Where(x => x.Id == searchString).First().Topic1;
        }

        public IEnumerable<Topic> GetTopicNames()
        {
            return _context.Topics;
        }

        public bool AddClass(string classCode)
        {
            var classes = _context.Classes.ToDictionary(f => f.ClassCode);

            if (!classes.ContainsKey(classCode))
            {
                _context.Classes.Add(new Class { ClassCode = classCode });
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool AddClass(string classCode, string? className)
        {
            if (className.IsNullOrEmpty())
            {
                return AddClass(classCode);
            }
            var classes = _context.Classes.ToDictionary(f => f.ClassCode);

            if (!classes.ContainsKey(classCode))
            {
                _context.Classes.Add(new Class
                {
                    ClassCode = classCode,
                    ClassName = className,
                });
                _context.SaveChanges();
                return true;
            }
            else
            {
                return false;
            }
        }

        //Checks via question's GUID
        public IEnumerable<Question> GetQuestions(string searchString)
        {
            if (searchString.IsNullOrEmpty())
            {
                return _context.Questions.OrderBy(x => x.CreatedTime);
            }
            return _context.Questions
                .Where(x => x.QuestionId.ToString().Contains(searchString)).OrderBy(x => x.CreatedTime);
        }

        //Added to get questions to populate dropdown --Jesse 2/17/2023
        public IEnumerable<String> GetAllQuestions(string searchString)
        {
            if (searchString == null)
            {
                return _context.Questions.OrderBy(x => x.CreatedTime).Select(x => x.Question1);
            }
            return _context.Questions.OrderBy(x => x.CreatedTime).Select(x => x.Question1)
                .Where(x => x.Contains(searchString));
        }

        //Added to get questions to populate dropdown --Jesse 2/24/2023
        //Tutor side --Samuel
        public IEnumerable<Question> GetStudentsQuestions(int? studentId)
        {
            if (studentId == null)
            {
                return _context.Questions.OrderBy(x => x.CreatedTime);
            }
            return _context.Questions.Where(x => x.StudentId == studentId).OrderBy(x => x.CreatedTime);
        }

        //Added to get questions to populate student answers --Jesse 2/27/2023
        //Student side --Samuel
        public IEnumerable<AnsweredQuestion> GetAnsweredQuestions(int? studentId)
        {
            if (studentId == null)
            {
                return null!;

            }
            return _context.AnsweredQuestions.Where(x => x.StudentId == studentId).OrderBy(x => x.CreatedTime);
        }

        //Added to get questions to populate tutor answers --Jesse 2/28/2023
        public IEnumerable<AnsweredQuestion> GetTutorAnsweredQuestions(int? tutorId)
        {
            if (tutorId == null)
            {
                return null!;

            }
            return _context.AnsweredQuestions.Where(x => x.TutorId == tutorId).OrderBy(x => x.CreatedTime);
        }

        //Added to remove selected question from unanswered questions --Jesse 2/28/2023
        public void StudentRemoveQuestion(Guid questionId)
        {
            Question q = _context.Questions.Where(x => x.QuestionId.Equals(questionId)).First();
            _context.Questions.Remove(q);
            _context.SaveChanges();
        }

        //Added to modify an existing question --Jesse 2/28/2023
        public void StudentModifyQuestion(Question q)
        {
            _context.Questions.Update(q);
            _context.SaveChanges();
        }

        public int? GetStudentId(string studentUsername)
        {
            if (studentUsername.IsNullOrEmpty())
            {
                return null;
            }
            try
            {
                int? studentId = _context.Students.Where(x => x.Email.Contains(studentUsername)).Select(x => x.Id).First();
                return studentId;
            }
            catch (InvalidOperationException e)
            {
                return null;
            }
        }

        public int? GetClassId(string classCode)
        {
            try
            {
                return _context.Classes.Where(x => x.ClassCode.Contains(classCode)).Select(x => x.Id).First();
            }
            catch { return null; }
        }

        public int? GetTopicId(string topic)
        {
            try
            {
                return _context.Topics.Where(x => x.Topic1.Contains(topic)).Select(x => x.Id).First();
            }
            catch { return null; }
        }

        public void AddQuestion(Question q)
        {
            _context.Questions.Add(q);
            _context.SaveChanges();
        }

        public Question GetQuestion(Guid questionId)
        {
            return _context.Questions.Where(x => x.QuestionId == questionId).First();
        }

        public IEnumerable<Question>? GetQuestions(String? classCode, String? topic)
        {
            if (classCode.IsNullOrEmpty() && topic.IsNullOrEmpty())
            {
                return _context.Questions.OrderBy(x => x.CreatedTime);

            }
            else if (!classCode.IsNullOrEmpty() && topic.IsNullOrEmpty())
            {
                int? checkId = GetClassId(classCode!);
                if (!checkId.HasValue) { return null; }
                int classId = checkId.Value;
                return _context.Questions.Where(x => x.ClassId == classId).OrderBy(x => x.CreatedTime);
            }
            else
            {
                int? checkId = GetClassId(classCode!);
                if (!checkId.HasValue) { return null; }
                int classId = (int)checkId;
                int topicId = (int)GetTopicId(topic!)!;

                return _context.Questions.Where(x => x.ClassId == classId && x.TopicId == topicId).OrderBy(x => x.CreatedTime);
            }


        }
        public int GetTutorId(string tutorUsername)
        {
            int internalId = _context.ApiUsers.Where(x => x.ExternalId.Contains(tutorUsername)).Select(x => x.UserId).First();
            return _context.Tutors.Where(x => x.UserId == internalId).Select(x => x.Id).First();
        }

        public Boolean AnswerQuestion(AnsweredQuestion a)
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

        public bool RemoveQuestion(Guid questionId)
        {
            Question q = _context.Questions.Where(x => x.QuestionId.Equals(questionId)).First();
            if (q == null) { return false; }
            return RemoveQuestion(q);
        }
        public bool RemoveQuestion(Question q)
        {
            try
            {
                _context.Questions.Remove(q);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        public IEnumerable<TopicAggregate>? GetQuestionStatistics(string classCode, string topic)
        {
            List<TopicAggregate> topicAggregate = new();
            try
            {
                int classId = _context.Classes.Where(x => x.ClassCode == classCode).First().Id;
                int topicId = _context.Topics.Where(x => x.Topic1 == topic).First().Id;
                try
                {
                    var questions = _context.Questions.Where(x => x.ClassId == classId && x.TopicId == topicId);
                    foreach (var q in questions)
                    {
                        if (!topicAggregate.Exists(x => x.TopicId == q.TopicId))
                        {
                            topicAggregate.Add(new TopicAggregate
                            {
                                ClassCode = classCode,
                                ClassId = classId,
                                Topic = topic,
                                TopicId = topicId,
                                Occurences = _context.Questions.Where(x => x.TopicId == q.TopicId).Count() + _context.AnsweredQuestions.Where(x => x.TopicId == q.TopicId).Count(),
                            });
                        }
                    }
                }
                catch { }
                try
                {
                    var aquestions = _context.AnsweredQuestions.Where(x => x.ClassId == classId && x.TopicId == topicId);

                    foreach (var q in aquestions)
                    {
                        if (!topicAggregate.Exists(x => x.TopicId == q.TopicId))
                        {
                            topicAggregate.Add(new TopicAggregate
                            {
                                ClassCode = classCode,
                                ClassId = classId,
                                Topic = topic,
                                TopicId = topicId,
                                Occurences = _context.Questions.Where(x => x.TopicId == q.TopicId).Count() + _context.AnsweredQuestions.Where(x => x.TopicId == q.TopicId).Count(),
                            });
                        }
                    }
                }
                catch { }
                return topicAggregate;
            }
            catch { return null; }
        }
        public IEnumerable<TopicAggregate>? GetQuestionStatistics(string className)
        {
            List<TopicAggregate> topicAggregate = new();
            try
            {
                int classId = _context.Classes.Where(x => x.ClassCode == className).First().Id;
                try
                {
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
                                Occurences = _context.Questions.Where(x => x.TopicId == q.TopicId).Count() + _context.AnsweredQuestions.Where(x => x.TopicId == q.TopicId).Count(),
                            });
                        }
                    }
                }
                catch { }
                try
                {
                    var aquestions = _context.AnsweredQuestions.Where(x => x.ClassId == classId);
                    foreach (var q in aquestions)
                    {
                        if (!topicAggregate.Exists(x => x.TopicId == q.TopicId))
                        {
                            topicAggregate.Add(new TopicAggregate
                            {
                                ClassCode = className,
                                ClassId = classId,
                                Topic = _context.Topics.Where(x => x.Id == q.TopicId).First().Topic1,
                                TopicId = q.TopicId,
                                Occurences = _context.Questions.Where(x => x.TopicId == q.TopicId).Count() + _context.AnsweredQuestions.Where(x => x.TopicId == q.TopicId).Count(),
                            });
                        }
                    }
                }
                catch { }
                return topicAggregate;
            }
            catch { return null; }
        }
        public IEnumerable<TopicAggregate> GetQuestionStatistics()
        {
            List<TopicAggregate> topicAggregate = new();
            try
            {
                var questions = _context.Questions;
                foreach (var q in questions)
                {
                    if (!topicAggregate.Exists(x => x.TopicId == q.TopicId))
                    {
                        topicAggregate.Add(new TopicAggregate
                        {
                            ClassCode = _context.Classes.Where(x => x.Id == q.ClassId).First().ClassCode,
                            ClassId = q.ClassId,
                            Topic = _context.Topics.Where(x => x.Id == q.TopicId).First().Topic1,
                            TopicId = q.TopicId,
                            Occurences = _context.Questions.Where(x => x.TopicId == q.TopicId).Count() + _context.AnsweredQuestions.Where(x => x.TopicId == q.TopicId).Count(),
                        });
                    }
                }
            }
            catch { }
            try
            {
                var aquestions = _context.AnsweredQuestions;
                foreach (var q in aquestions)
                {
                    if (!topicAggregate.Exists(x => x.TopicId == q.TopicId))
                    {
                        topicAggregate.Add(new TopicAggregate
                        {
                            ClassCode = _context.Classes.Where(x => x.Id == q.ClassId).First().ClassCode,
                            ClassId = q.ClassId,
                            Topic = _context.Topics.Where(x => x.Id == q.TopicId).First().Topic1,
                            TopicId = q.TopicId,
                            Occurences = _context.Questions.Where(x => x.TopicId == q.TopicId).Count() + _context.AnsweredQuestions.Where(x => x.TopicId == q.TopicId).Count(),
                        });
                    }
                }
            }
            catch { }
            return topicAggregate;
        }

        public string GetStudentUsername(int studentId)
        {
            Student s = _context.Students.Where(x => x.Id == studentId).First();
            return s.Email;
        }

        public string GetTutorUsername(int tutorId)
        {
            return _context.ApiUsers.Where(x => x.UserId == tutorId).Select(x => x.ExternalId).First();
        }

        public bool AddTopic(string classCode, string topic)
        {
            Topic t = new Topic();
            t.ClassId = _context.Classes.Where(x => x.ClassCode == classCode).First().Id;
            t.Topic1 = topic;
            try
            {
                _context.Topics.Add(t);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        public bool RemoveTopic(string classCode, string topic)
        {
            int topicId = (int)GetTopicId(topic!)!;
            var questions = _context.Questions.Where(x => x.TopicId == topicId);
            foreach (var question in questions)
            {
                RemoveQuestion(question);
            }

            Topic t = _context.Topics.Where(x => x.Topic1 == topic).First();
            try
            {
                _context.Topics.Remove(t);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        public bool ModifyTopic(string classCode, string topic, string newTopic)
        {
            Topic t = _context.Topics.Where(x => x.Topic1 == topic).First();
            t.Topic1 = newTopic;
            try
            {
                _context.Topics.Update(t);
                //_context.Questions.Where(x => x.TopicId == t.Id).ToList().ForEach(x => x.TopicId = t.Id);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        public bool UpdateClass(int? classId, string newClassCode, string newClassDesc)
        {
            Class c = _context.Classes.Where(x => x.Id == classId).First();
            c.ClassCode = newClassCode;
            c.ClassName = newClassDesc;
            try
            {
                _context.Classes.Update(c);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        public bool RemoveClass(int? classId)
        {
            Class c = _context.Classes.Where(x => x.Id == classId).First();
            try
            {
                var topics = _context.Topics.Where(x => x.ClassId == c.Id);
                var questions = _context.Questions.Where(x => x.ClassId == c.Id);
                foreach (var question in questions)
                {
                    RemoveQuestion(question);
                }
                foreach (var topic in topics)
                {
                    _context.Topics.Remove(topic);
                }
            }
            catch { return false; }
            try
            {
                _context.Classes.Remove(c);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        // Admin User Methods
        public IEnumerable<AnsweredQuestion> GetAdminAnsweredQuestions()
        {
            return _context.AnsweredQuestions;
        }

        public String GetUserEmail(string username)
        {
            try
            {
                return _context.ApiUsers.Where(x => x.ExternalId == username).Select(x => x.ExternalId).First();
            }
            catch (InvalidOperationException e) { return "No user found"; }
        }

        public IEnumerable<bool> GetUserRoles(string username)
        {
            bool[] userRoles = new bool[3];
            try
            {
                userRoles[0] = _context.ApiUsers.Where(x => x.ExternalId == username).Select(x => x.IsStudent).First();
                userRoles[1] = _context.ApiUsers.Where(x => x.ExternalId == username).Select(x => x.IsTutor).First();
                userRoles[2] = _context.ApiUsers.Where(x => x.ExternalId == username).Select(x => x.IsAdmin).First();
                return userRoles;
            }
            catch (InvalidOperationException e) { return null; }
        }

        public bool ModifyUserRoles(string username, bool isStudent, bool isTutor, bool isAdmin)
        {
            ApiUser user = _context.ApiUsers.Where(x => x.ExternalId == username).First();
            user.IsStudent = isStudent;
            user.IsTutor = isTutor;
            user.IsAdmin = isAdmin;
            try
            {
                _context.ApiUsers.Update(user);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }

        public bool RemoveUser(string username)
        {
            ApiUser user = _context.ApiUsers.Where(x => x.ExternalId == username).First();
            try
            {
                _context.ApiUsers.Remove(user);
                _context.SaveChanges();
            }
            catch { return false; }
            return true;
        }
    }
}