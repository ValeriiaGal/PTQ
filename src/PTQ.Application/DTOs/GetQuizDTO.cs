namespace PTQ.Application.DTOs
{
    public class GetQuizDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string PathFile { get; set; } = null!;
        public string PotatoTeacherName { get; set; } = null!;
    }
}