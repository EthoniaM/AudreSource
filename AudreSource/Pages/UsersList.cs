using AudreSource.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AudreSource.Pages
{
    public class UsersList : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public UsersList(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        public List<User> Users { get; set; } = new List<User>();

        public async Task OnGetAsync()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT UsersID, FirstNames, Surname, MobileNumber, Position, EmailAddress FROM Users", connection);
                await connection.OpenAsync();
                var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    Users.Add(new User
                    {
                        UsersID = reader.GetInt32(0),
                        FirstNames = reader.GetString(1),
                        Surname = reader.GetString(2),
                        MobileNumber = reader.GetString(3),
                        Position = reader.GetString(4),
                        EmailAddress = reader.GetString(5),
                    });
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Users WHERE UsersID = @UsersID", connection);
                command.Parameters.AddWithValue("@UsersID", id);
                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToPage("/UsersList");
        }
    }
}
