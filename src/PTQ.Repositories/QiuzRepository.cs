using System.Data;
using Microsoft.Data.SqlClient;
using PTQ.Models;
using PTQ.Repositories.Interfaces;

namespace PTQ.Repositories
{
    public class QuizRepository : IQuizRepository
    {
        private readonly string _connectionString;

        public QuizRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IEnumerable<Quiz>> GetAllAsync()
        {
            var list = new List<Quiz>();

            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT Id, Name FROM Quiz", conn);
            var reader = await cmd.ExecuteReaderAsync();
    
            while (await reader.ReadAsync())
            {
                list.Add(new Quiz
                {
                    Id = reader.GetInt32(0),
                    Name = reader.GetString(1)
                });
            }

            return list;
        }


        public async Task<Quiz?> GetByIdAsync(int id)
        {
            using var conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand("SELECT q.Id, q.Name, q.PathFile, t.Id, t.Name FROM Quiz q JOIN PotatoTeacher t ON q.PotatoTeacherId = t.Id WHERE q.Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            return new Quiz
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                PathFile = reader.GetString(2),
                PotatoTeacher = new PotatoTeacher
                {
                    Id = reader.GetInt32(3),
                    Name = reader.GetString(4)
                }
            };
        }

        public async Task AddAsync(Quiz quiz, SqlConnection conn, SqlTransaction transaction)
        {
            var cmd = new SqlCommand("INSERT INTO Quiz (Name, PathFile, PotatoTeacherId) OUTPUT INSERTED.Id VALUES (@Name, @PathFile, @PotatoTeacherId)", conn, transaction);
            cmd.Parameters.AddWithValue("@Name", quiz.Name);
            cmd.Parameters.AddWithValue("@PathFile", quiz.PathFile);
            cmd.Parameters.AddWithValue("@PotatoTeacherId", quiz.PotatoTeacherId);

            quiz.Id = (int)await cmd.ExecuteScalarAsync();
        }
    }
}
