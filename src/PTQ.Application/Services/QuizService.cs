using System.Data;
using Microsoft.Data.SqlClient;
using PTQ.Application.DTOs;
using PTQ.Application.Interfaces;
using PTQ.Models;
using PTQ.Repositories.Interfaces;

namespace PTQ.Application.Services
{
    public class QuizService : IQuizService
    {
        private readonly IQuizRepository _quizRepo;
        private readonly IPotatoTeacherRepository _teacherRepo;
        private readonly string _connectionString;

        public QuizService(IQuizRepository quizRepo, IPotatoTeacherRepository teacherRepo, string connectionString)
        {
            _quizRepo = quizRepo;
            _teacherRepo = teacherRepo;
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<GetQuizDTO>> GetAllQuizzesAsync()
        {
            var quizzes = await _quizRepo.GetAllAsync();
            return quizzes.Select(q => new GetQuizDTO
            {
                Id = q.Id,
                Name = q.Name,
            });
        }

        public async Task<GetQuizDTO?> GetQuizByIdAsync(int id)
        {
            var quiz = await _quizRepo.GetByIdAsync(id);
            if (quiz == null) return null;

            return new GetQuizDTO
            {
                Id = quiz.Id,
                Name = quiz.Name,
                PathFile = quiz.PathFile,
                PotatoTeacherName = quiz.PotatoTeacher?.Name ?? "Unknown"
            };
        }

        public async Task<GetQuizDTO> CreateQuizAsync(CreateQuizDTO dto)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            using var transaction = conn.BeginTransaction();

            try
            {
                var teacher = await _teacherRepo.GetByNameAsync(dto.PotatoTeacherName, conn, transaction);

                if (teacher == null)
                {
                    teacher = new PotatoTeacher { Name = dto.PotatoTeacherName };
                    await _teacherRepo.AddAsync(teacher, conn, transaction);
                }

                var quiz = new Quiz
                {
                    Name = dto.Name,
                    PathFile = dto.PathFile,
                    PotatoTeacherId = teacher.Id
                };

                await _quizRepo.AddAsync(quiz, conn, transaction);
                transaction.Commit();

                return new GetQuizDTO
                {
                    Id = quiz.Id,
                    Name = quiz.Name,
                    PathFile = quiz.PathFile,
                    PotatoTeacherName = teacher.Name
                };
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }
    }
}
