using System.Data;
using Microsoft.Data.SqlClient;
using PTQ.Models;

namespace PTQ.Repositories.Interfaces
{
    public interface IQuizRepository
    {
        Task<IEnumerable<Quiz>> GetAllAsync();
        Task<Quiz?> GetByIdAsync(int id);
        Task AddAsync(Quiz quiz, SqlConnection conn, SqlTransaction transaction);
    }
}