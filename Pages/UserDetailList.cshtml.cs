using AudreSource.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace AudreSource.Pages
{
    public class UserDetailsListModel : PageModel
    {
        private readonly string _connectionString;

        public UserDetailsListModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }
    

        public List<UserDetail> UserDetails { get; set; }

        public async Task OnGetAsync()
        {
            UserDetails = new List<UserDetail>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT UserDetailsID, Address, Country, Nationality FROM UserDetails", connection);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        UserDetails.Add(new UserDetail
                        {
                            UserDetailsID = reader.GetInt32(0),
                            Address = reader.GetString(1),
                            Country = reader.GetString(2),
                            Nationality = reader.GetString(3)
                        });
                    }
                }
            }
        }
    }
}
