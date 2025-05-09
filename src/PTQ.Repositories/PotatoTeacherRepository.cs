using System.Data;
using Microsoft.Data.SqlClient;
using PTQ.Models;
using PTQ.Repositories.Interfaces;

namespace PTQ.Repositories
{
    public class PotatoTeacherRepository : IPotatoTeacherRepository
    {
        private readonly string _connectionString;

        public PotatoTeacherRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<PotatoTeacher?> GetByNameAsync(string name, SqlConnection conn, SqlTransaction transaction)
        {
            var cmd = new SqlCommand("SELECT Id, Name FROM PotatoTeacher WHERE Name = @Name", conn, transaction);
            cmd.Parameters.AddWithValue("@Name", name);

            var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return null;

            var teacher = new PotatoTeacher
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1)
            };
            await reader.CloseAsync();
            return teacher;
        }

        public async Task AddAsync(PotatoTeacher teacher, SqlConnection conn, SqlTransaction transaction)
        {
            var cmd = new SqlCommand("INSERT INTO PotatoTeacher (Name) OUTPUT INSERTED.Id VALUES (@Name)", conn, transaction);
            cmd.Parameters.AddWithValue("@Name", teacher.Name);
            teacher.Id = (int)await cmd.ExecuteScalarAsync();
        }
    }
}