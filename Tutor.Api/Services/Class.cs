namespace Tutor.Api.Services
{
    public class Class
    {
        public int Id { get; set; }
        public string ClassCode { get; set; }

        public Class(int id, string classCode)
        {
            Id = id;
            ClassCode = classCode;
        }
    }
}
