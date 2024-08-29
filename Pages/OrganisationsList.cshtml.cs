using AudreSource.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace AudreSource.Pages
{
    public class OrganisationsListModel : PageModel
    {
        private readonly string _connectionString;

        public OrganisationsListModel(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }


        public List<Organisation> Organisations { get; set; }

        public async Task OnGetAsync()
        {
            Organisations = new List<Organisation>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT OrganisationID, OrganisationName, Representative, Address, ContactNumber, Domain FROM Organisations", connection);
                await connection.OpenAsync();
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Organisations.Add(new Organisation
                        {
                            OrganisationID = reader.GetInt32(0),
                            OrganisationName = reader.GetString(1),
                            Representative = reader.GetString(2),
                            Address = reader.GetString(3),
                            ContactNumber = reader.GetString(4), 
                            Domain = reader.GetString(5)          
                        });
                    }
                }
            }
        }
    }
}
