namespace PTQ.Models
{
    public class PotatoTeacher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Quiz> Quizzes { get; set; }
    }
}