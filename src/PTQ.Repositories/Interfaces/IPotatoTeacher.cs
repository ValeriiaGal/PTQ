using System.Data;
using Microsoft.Data.SqlClient;
using PTQ.Models;

namespace PTQ.Repositories.Interfaces
{
    public interface IPotatoTeacherRepository
    {
        Task<PotatoTeacher?> GetByNameAsync(string name, SqlConnection conn, SqlTransaction transaction);
        Task AddAsync(PotatoTeacher teacher, SqlConnection conn, SqlTransaction transaction);
    }
}