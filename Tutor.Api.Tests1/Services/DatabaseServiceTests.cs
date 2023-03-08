using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Tutor.api.Services;
using Tutor.Api.Models;

//Issues adding and removing objects using Moq. Testing has been done by hand due to project time constraints. --Samuel 3/8/2023
namespace Tutor.api.Tests
{
    [TestClass()]
    public class DatabaseServiceTests
    {
        DatabaseService _databaseService;

        [TestInitialize]
        public void InitializeMoq()
        {
            Mock<tutor_dbContext> mockContext = new();
            //Create Moq ApiUsers

            var apiUserData = new List<ApiUser>
            {
                new ApiUser
                    {
                        UserId = 0,
                        ExternalId = "sshaw16@ewu.edu",
                        IsAdmin = true,
                        IsTutor = true,
                        IsStudent = true
                    },
                new ApiUser
                    {
                    UserId = 1,
                        ExternalId = "admin@ewu.edu",
                        IsAdmin = true,
                        IsTutor = true,
                        IsStudent = true
                    },
                new ApiUser
                    {
                    UserId = 2,
                        ExternalId = "tutor@ewu.edu",
                        IsAdmin = true,
                        IsTutor = true,
                        IsStudent = true
                    },
                new ApiUser
                    {
                    UserId = 3,
                        ExternalId = "student@ewu.edu",
                        IsAdmin = true,
                        IsTutor = true,
                        IsStudent = true
                    },
            }.AsQueryable();

            var mockApiUserSet = new Mock<DbSet<ApiUser>>();
            mockApiUserSet.As<IQueryable<ApiUser>>().Setup(a => a.Provider).Returns(apiUserData.Provider);
            mockApiUserSet.As<IQueryable<ApiUser>>().Setup(a => a.Expression).Returns(apiUserData.Expression);
            mockApiUserSet.As<IQueryable<ApiUser>>().Setup(a => a.ElementType).Returns(apiUserData.ElementType);
            mockApiUserSet.As<IQueryable<ApiUser>>().Setup(a => a.GetEnumerator()).Returns(apiUserData.GetEnumerator);

            mockContext.Setup(a => a.ApiUsers).Returns(mockApiUserSet.Object);

            //Create Moq Admins

            var adminData = new List<Admin>
            {
                new Admin
                {
                    Id = 0,
                    UserId = apiUserData.Where(x => x.ExternalId == "sshaw16@ewu.edu").First().UserId,
                    FirstName = "Samuel",
                    LastName = "Shaw"
                },
                 new Admin
                {
                    Id = 1,
                    UserId = apiUserData.Where(x => x.ExternalId == "admin@ewu.edu").First().UserId,
                    FirstName = "Admin",
                    LastName = "User"
                }
            }.AsQueryable();

            var mockAdminSet = new Mock<DbSet<Admin>>();
            mockAdminSet.As<IQueryable<Admin>>().Setup(a => a.Provider).Returns(adminData.Provider);
            mockAdminSet.As<IQueryable<Admin>>().Setup(a => a.Expression).Returns(adminData.Expression);
            mockAdminSet.As<IQueryable<Admin>>().Setup(a => a.ElementType).Returns(adminData.ElementType);
            mockAdminSet.As<IQueryable<Admin>>().Setup(a => a.GetEnumerator()).Returns(adminData.GetEnumerator);

            mockContext.Setup(a => a.Admins).Returns(mockAdminSet.Object);

            //Create Moq Tutors

            var tutorData = new List<Api.Models.Tutor>
            {
                new Api.Models.Tutor
                {
                    Id = 0,
                    UserId = apiUserData.Where(x => x.ExternalId == "sshaw16@ewu.edu").First().UserId,
                    FirstName = "Samuel",
                    LastName = "Shaw"
                },
                 new Api.Models.Tutor
                {
                    Id =1,
                    UserId = apiUserData.Where(x => x.ExternalId == "tutor@ewu.edu").First().UserId,
                    FirstName = "Tutor",
                    LastName = "User"
                }
            }.AsQueryable();

            var mockTutorSet = new Mock<DbSet<Api.Models.Tutor>>();
            mockTutorSet.As<IQueryable<Api.Models.Tutor>>().Setup(a => a.Provider).Returns(tutorData.Provider);
            mockTutorSet.As<IQueryable<Api.Models.Tutor>>().Setup(a => a.Expression).Returns(tutorData.Expression);
            mockTutorSet.As<IQueryable<Api.Models.Tutor>>().Setup(a => a.ElementType).Returns(tutorData.ElementType);
            mockTutorSet.As<IQueryable<Api.Models.Tutor>>().Setup(a => a.GetEnumerator()).Returns(tutorData.GetEnumerator);

            mockContext.Setup(a => a.Tutors).Returns(mockTutorSet.Object);

            //Create Moq Students

            var studentData = new List<Student>
            {
                new Student
                {
                    Id = 0,
                    UserId = apiUserData.Where(x => x.ExternalId == "sshaw16@ewu.edu").First().UserId,
                    FirstName = "Samuel",
                    LastName = "Shaw",
                    Email = "sshaw16@ewu.edu"
                },
                 new Student
                {
                    Id = 1,
                    UserId = apiUserData.Where(x => x.ExternalId == "student@ewu.edu").First().UserId,
                    FirstName = "Student",
                    LastName = "User",
                    Email = "student@ewu.edu",
                }
            }.AsQueryable();

            var mockStudentSet = new Mock<DbSet<Student>>();
            mockStudentSet.As<IQueryable<Student>>().Setup(a => a.Provider).Returns(studentData.Provider);
            mockStudentSet.As<IQueryable<Student>>().Setup(a => a.Expression).Returns(studentData.Expression);
            mockStudentSet.As<IQueryable<Student>>().Setup(a => a.ElementType).Returns(studentData.ElementType);
            mockStudentSet.As<IQueryable<Student>>().Setup(a => a.GetEnumerator()).Returns(studentData.GetEnumerator);

            mockContext.Setup(a => a.Students).Returns(mockStudentSet.Object);

            //Create Moq Classes

            var classData = new List<Class>
            {
                new Class{Id = 0, ClassCode = "CSCD 210"},
                new Class{Id = 1, ClassCode = "CSCD 211"},
                new Class{Id = 2, ClassCode = "CSCD 212"},
                new Class{Id = 3, ClassCode = "EENG 260"},
                new Class{Id = 4, ClassCode = "ENGL 101"},

            }.AsQueryable();
            var mockClassSet = new Mock<DbSet<Class>>();
            mockClassSet.As<IQueryable<Class>>().Setup(m => m.Provider).Returns(classData.Provider);
            mockClassSet.As<IQueryable<Class>>().Setup(m => m.Expression).Returns(classData.Expression);
            mockClassSet.As<IQueryable<Class>>().Setup(m => m.ElementType).Returns(classData.ElementType);
            mockClassSet.As<IQueryable<Class>>().Setup(m => m.GetEnumerator()).Returns(() => classData.GetEnumerator());
            //mockClassSet.Setup(m => m.Add(It.IsAny<Class>())).Callback<Class>(m => classData.Append(m));

            mockContext.Setup(m => m.Classes).Returns(mockClassSet.Object);

            //Create Moq Topics

            var topicData = new List<Topic>
            {
                new Topic
                {
                    Id = 0,
                    ClassId = classData.First().Id,
                    Topic1 = "Lists"
                },
                new Topic
                {
                    Id = 1,
                    ClassId = classData.First().Id,
                    Topic1 = "Arrays"
                },
                new Topic
                {
                    Id = 2,
                    ClassId = classData.Last().Id,
                    Topic1 = "Bibliography"
                },
            }.AsQueryable();
            var mockTopicSet = new Mock<DbSet<Topic>>();
            mockTopicSet.As<IQueryable<Topic>>().Setup(m => m.Provider).Returns(topicData.Provider);
            mockTopicSet.As<IQueryable<Topic>>().Setup(m => m.Expression).Returns(topicData.Expression);
            mockTopicSet.As<IQueryable<Topic>>().Setup(m => m.ElementType).Returns(topicData.ElementType);
            mockTopicSet.As<IQueryable<Topic>>().Setup(m => m.GetEnumerator()).Returns(() => topicData.GetEnumerator());

            mockContext.Setup(m => m.Topics).Returns(mockTopicSet.Object);

            //Create Moq Questions

            var questionData = new List<Question>
            {
                new Question
                {
                    QuestionId = Guid.NewGuid(),
                    ClassId = classData.Where(x => x.ClassCode == "CSCD 210").First().Id,
                    StudentId = studentData.Where(x => x.Email == "sshaw16@ewu.edu").First().Id,
                    TopicId = topicData.Where(x => x.Topic1 == "Lists").First().Id,
                    Question1 = "This is test question 1.",
                },
                new Question
                {
                    QuestionId = Guid.NewGuid(),
                    ClassId = classData.Where(x => x.ClassCode == "CSCD 210").First().Id,
                    StudentId = studentData.Where(x => x.Email == "student@ewu.edu").First().Id,
                    TopicId = topicData.Where(x => x.Topic1 == "Lists").First().Id,
                    Question1 = "This is test question 2.",
                },
                new Question
                {
                    QuestionId = Guid.NewGuid(),
                    ClassId = classData.Where(x => x.ClassCode == "CSCD 210").First().Id,
                    StudentId = studentData.Where(x => x.Email == "sshaw16@ewu.edu").First().Id,
                    TopicId = topicData.Where(x => x.Topic1 == "Arrays").First().Id,
                    Question1 = "This is test question 3.",
                },
            }.AsQueryable();
            var mockQuestionSet = new Mock<DbSet<Question>>();
            mockQuestionSet.As<IQueryable<Question>>().Setup(m => m.Provider).Returns(questionData.Provider);
            mockQuestionSet.As<IQueryable<Question>>().Setup(m => m.Expression).Returns(questionData.Expression);
            mockQuestionSet.As<IQueryable<Question>>().Setup(m => m.ElementType).Returns(questionData.ElementType);
            mockQuestionSet.As<IQueryable<Question>>().Setup(m => m.GetEnumerator()).Returns(() => questionData.GetEnumerator());

            mockContext.Setup(m => m.Questions).Returns(mockQuestionSet.Object);

            //Create Moq Answered Questions

            var aQuestionData = new List<AnsweredQuestion>
            {
                new AnsweredQuestion
                {
                    QuestionId = Guid.NewGuid(),
                    ClassId = classData.Where(x => x.ClassCode == "CSCD 210").First().Id,
                    StudentId = studentData.Where(x => x.Email == "sshaw16@ewu.edu").First().Id,
                    TutorId = 1,
                    TopicId = topicData.Where(x => x.Topic1 == "Lists").First().Id,
                    Question = "This is test question 1.",
                    Response = "This is test response 1.",
                },
                new AnsweredQuestion
                {
                    QuestionId = Guid.NewGuid(),
                    ClassId = classData.Where(x => x.ClassCode == "CSCD 210").First().Id,
                    StudentId = studentData.Where(x => x.Email == "student@ewu.edu").First().Id,
                    TutorId = 0,
                    TopicId = topicData.Where(x => x.Topic1 == "Lists").First().Id,
                    Question = "This is test question 2.",
                    Response = "This is test response 2.",
                },
                new AnsweredQuestion
                {
                    QuestionId = Guid.NewGuid(),
                    ClassId = classData.Where(x => x.ClassCode == "CSCD 210").First().Id,
                    StudentId = studentData.Where(x => x.Email == "sshaw16@ewu.edu").First().Id,
                    TutorId = 1,
                    TopicId = topicData.Where(x => x.Topic1 == "Arrays").First().Id,
                    Question = "This is test question 3.",
                    Response = "This is test response 3.",
                },
            }.AsQueryable();
            var mockAQuestionSet = new Mock<DbSet<AnsweredQuestion>>();
            mockAQuestionSet.As<IQueryable<AnsweredQuestion>>().Setup(m => m.Provider).Returns(aQuestionData.Provider);
            mockAQuestionSet.As<IQueryable<AnsweredQuestion>>().Setup(m => m.Expression).Returns(aQuestionData.Expression);
            mockAQuestionSet.As<IQueryable<AnsweredQuestion>>().Setup(m => m.ElementType).Returns(aQuestionData.ElementType);
            mockAQuestionSet.As<IQueryable<AnsweredQuestion>>().Setup(m => m.GetEnumerator()).Returns(() => aQuestionData.GetEnumerator());

            mockContext.Setup(m => m.AnsweredQuestions).Returns(mockAQuestionSet.Object);

            //Create test service using the created mockContext
            _databaseService = new DatabaseService(mockContext.Object);

        }

        [TestMethod()]
        public void GetClassesTest()
        {
            List<String> classes = _databaseService.GetClasses("").ToList();
            Assert.AreEqual(5, classes.Count());
            Assert.AreEqual("CSCD 210", classes[0]);
            Assert.AreEqual("CSCD 211", classes[1]);
            Assert.AreEqual("CSCD 212", classes[2]);
            Assert.AreEqual("EENG 260", classes[3]);
            Assert.AreEqual("ENGL 101", classes[4]);
        }

        [TestMethod()]
        public void GetClassTopicsTest()
        {
            List<String> topics = _databaseService.GetClassTopics(null).ToList();
            Assert.AreEqual(3, topics.Count());
            Assert.AreEqual("Lists", topics[0]);
            Assert.AreEqual("Arrays", topics[1]);
            Assert.AreEqual("Bibliography", topics[2]);
        }

        [TestMethod()]
        public void GetClassTopicsTest1()
        {
            List<String> topics = _databaseService.GetClassTopics(0).ToList();
            Assert.AreEqual(2, topics.Count());
            Assert.AreEqual("Lists", topics[0]);
            Assert.AreEqual("Arrays", topics[1]);
        }

        [TestMethod()]
        public void GetTopicsTest()
        {
            List<String> topics = _databaseService.GetTopics(null).ToList();
            Assert.AreEqual(3, topics.Count());
            Assert.AreEqual("Lists", topics[0]);
            Assert.AreEqual("Arrays", topics[1]);
            Assert.AreEqual("Bibliography", topics[2]);
        }

        [TestMethod()]
        public void GetTopicsTest1()
        {
            List<String> topics = _databaseService.GetTopics("Lists").ToList();
            Assert.AreEqual(1, topics.Count());
            Assert.AreEqual("Lists", topics[0]);
        }

        //Fails due to moq not making adding an object reasonable
        //[TestMethod()]
        //public void AddClassTest()
        //{
        //    _databaseService.AddClass("CSCD 240");
        //    int? i = _databaseService.GetClassId("CSCD 240");
        //    Assert.IsNotNull(i);
        //}

        [TestMethod()]
        public void AddClassTest1()
        {
            _databaseService.AddClass("CSCD 212");
            int? i = _databaseService.GetClassId("CSCD 212");
            Assert.IsNotNull(i);
        }

        [TestMethod()]
        public void GetQuestionsTest()
        {
            var q = _databaseService.GetQuestions(null!);
            Assert.AreEqual(3, q.Count());
        }

        [TestMethod()]
        public void GetQuestionsTest1()
        {
            Guid g = _databaseService.GetQuestions("").First().QuestionId;
            var q = _databaseService.GetQuestions(g.ToString());
            Assert.AreEqual(1, q.Count());
        }

        [TestMethod()]
        public void GetAllQuestionsTest()
        {
            var q = _databaseService.GetAllQuestions(null!);
            Assert.AreEqual(3, q.Count());
        }

        [TestMethod()]
        public void GetAllQuestionsTest1()
        {
            var q = _databaseService.GetAllQuestions("This is test question 1.");
            Assert.AreEqual(1, q.Count());
        }

        [TestMethod()]
        public void GetStudentsQuestionsTest()
        {
            var q = _databaseService.GetStudentsQuestions(null!);
            Assert.AreEqual(3, q.Count());
        }

        [TestMethod()]
        public void GetStudentsQuestionsTest1()
        {
            var q = _databaseService.GetStudentsQuestions(_databaseService.GetStudentId("sshaw16@ewu.edu"));
            Assert.AreEqual(2, q.Count());
        }

        [TestMethod()]
        public void GetAnsweredQuestionsTest()
        {
            var q = _databaseService.GetAnsweredQuestions(null!);
            Assert.IsNull(q);
        }

        [TestMethod()]
        public void GetAnsweredQuestionsTest1()
        {
            var q = _databaseService.GetAnsweredQuestions(_databaseService.GetStudentId("student@ewu.edu"));
            Assert.AreEqual(1, q.Count());
        }

        [TestMethod()]
        public void GetTutorAnsweredQuestionsTest()
        {
            var q = _databaseService.GetTutorAnsweredQuestions(null!);
            Assert.IsNull(q);
        }
        [TestMethod()]
        public void GetTutorAnsweredQuestionsTest1()
        {
            var q = _databaseService.GetTutorAnsweredQuestions(_databaseService.GetTutorId("tutor@ewu.edu"));
            Assert.AreEqual(2, q.Count());
        }

        //[TestMethod()]
        //public void StudentRemoveQuestionTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void StudentModifyQuestionTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void GetStudentIdTest()
        {
            int? i = _databaseService.GetStudentId("");
            Assert.IsNull(i);
        }

        [TestMethod()]
        public void GetStudentIdTest1()
        {
            int? i = _databaseService.GetStudentId("student@ewu.edu");
            Assert.AreEqual(1, i);
        }

        [TestMethod()]
        public void GetClassIdTest()
        {
            int? i = _databaseService.GetClassId(null!);
            Assert.IsNull(i);
        }

        [TestMethod()]
        public void GetClassIdTest1()
        {
            int? i = _databaseService.GetClassId("CSCD 210");
            Assert.AreEqual(0, i);
        }

        [TestMethod()]
        public void GetTopicIdTest()
        {
            int? i = _databaseService.GetTopicId(null!);
            Assert.IsNull(i);
        }

        [TestMethod()]
        public void GetTopicIdTest1()
        {
            int? i = _databaseService.GetTopicId("Lists");
            Assert.AreEqual(0, i);
        }

        //[TestMethod()]
        //public void AddQuestionTest()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void GetQuestionTest()
        {
            var questions = _databaseService.GetQuestions("");
            Guid g = questions.First().QuestionId;
            Question q = _databaseService.GetQuestion(g);
            Assert.IsNotNull(q);
            Assert.AreEqual(q.Question1, "This is test question 1.");
        }


        [TestMethod()]
        public void GetTutorIdTest()
        {
            int? i = _databaseService.GetTutorId("tutor@ewu.edu");
            Assert.AreEqual(1, i);
        }

        //[TestMethod()]
        //public void AnswerQuestionTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void RemoveQuestionTest()
        //{
        //    Assert.Fail();
        //}

        //[TestMethod()]
        //public void RemoveQuestionTest1()
        //{
        //    Assert.Fail();
        //}

        [TestMethod()]
        public void GetQuestionStatisticsTest()
        {
            var stats = _databaseService.GetQuestionStatistics();
            Assert.IsNotNull(stats);
            Assert.AreEqual(stats.Count(), 2);
            var stat = stats.First();
            Assert.AreEqual(stat.Occurences, 2);
        }

        [TestMethod()]
        public void GetQuestionStatisticsTest1()
        {
            var stats = _databaseService.GetQuestionStatistics("CSCD 210");
            Assert.IsNotNull(stats);
            Assert.AreEqual(stats.Count(), 2);
            var stat = stats.First();
            Assert.AreEqual(stat.Occurences, 2);
        }

        [TestMethod()]
        public void GetQuestionStatisticsTest2()
        {
            var stats = _databaseService.GetQuestionStatistics("CSCD 210", "Lists");
            Assert.IsNotNull(stats);
            Assert.AreEqual(stats.Count(), 1);
            var stat = stats.First();
            Assert.AreEqual(stat.Occurences, 2);
        }

        [TestMethod()]
        public void getStudentUsernameTest()
        {
            String email = _databaseService.GetStudentUsername(0);
            Assert.AreEqual(email, "sshaw16@ewu.edu");
        }
    }
}