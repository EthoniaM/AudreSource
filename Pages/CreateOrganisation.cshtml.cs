using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;

namespace AudreSource.Pages
{
    public class CreateOrganisationModel : PageModel
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CreateOrganisationModel(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        [BindProperty]
        public Organisation Organisation { get; set; } = new Organisation();

        public IActionResult OnGet()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var query = "INSERT INTO Organisations (OrganisationName, Representative, Address, ContactNumber, Domain) " +
            "VALUES (@OrganisationName, @Representative, @Address, @ContactNumber, @Domain)";


            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@OrganisationName", Organisation.OrganisationName);
                command.Parameters.AddWithValue("@Representative", Organisation.Representative);
                command.Parameters.AddWithValue("@Address", Organisation.Address);
                command.Parameters.AddWithValue("@ContactNumber", Organisation.ContactNumber);
                command.Parameters.AddWithValue("@Domain", Organisation.Domain);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToPage("/Success", new { message = "Organisation created successfully!" });
        }
    }
}
