using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AudreSource.Pages
{
    public class CreateRoleModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CreateRoleModel(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
            RoleInfo = new RoleInfo();
            _connectionString = _configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        [BindProperty]
        public RoleInfo RoleInfo { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("INSERT INTO Roles (Role, RoleDescription) VALUES (@Role, @RoleDescription)", connection);
                command.Parameters.AddWithValue("@Role", RoleInfo.Role);
                command.Parameters.AddWithValue("@RoleDescription", RoleInfo.RoleDescription);
                

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToPage("/Success", new { message = "Roles has been created successfully!" });
        }
    }

}


