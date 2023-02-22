namespace Tutor.Api.Models
{
    public class TopicAggregate
    {
        public string ClassCode { get; set;}
        public int ClassId { get; set;}
        public string Topic { get; set;}
        public int TopicId { get; set;}   
        public int Occurences { get; set;}
    }
}
