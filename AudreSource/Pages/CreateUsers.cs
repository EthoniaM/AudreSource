using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class CreateUsers : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CreateUsers(ApplicationDbContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _context = context;
            User = new User();
            _connectionString = _configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        [BindProperty]
        public new User User { get; set; }
        public List<User> Users { get; set; }

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
                var command = new SqlCommand("INSERT INTO Users (FirstNames, Surname, MobileNumber, Position, EmailAddress, Password) VALUES (@FirstNames, @Surname, @MobileNumber, @Position, @EmailAddress, @Password)", connection);
                command.Parameters.AddWithValue("@FirstNames", User.FirstNames);
                command.Parameters.AddWithValue("@Surname", User.Surname);
                command.Parameters.AddWithValue("@MobileNumber", User.MobileNumber);
                command.Parameters.AddWithValue("@Position", User.Position);
                command.Parameters.AddWithValue("@EmailAddress", User.EmailAddress);
                command.Parameters.AddWithValue("@Password", User.Password);

                await connection.OpenAsync();
                await command.ExecuteNonQueryAsync();
            }

            return RedirectToPage("/Success", new { message = "User created successfully!" });
        }
    }
}