using PTQ.Application.DTOs;

namespace PTQ.Application.Interfaces
{
    public interface IQuizService
    {
        Task<IEnumerable<GetQuizDTO>> GetAllQuizzesAsync();
        Task<GetQuizDTO?> GetQuizByIdAsync(int id);
        Task<GetQuizDTO> CreateQuizAsync(CreateQuizDTO dto);
    }
}