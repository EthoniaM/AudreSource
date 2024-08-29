using AudreSource.Model;
using AudreySource.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AudreSource.Pages
{
    public class KanbanBoardModel : PageModel
    {
        private readonly ApplicationDbContext _context;
      

        public KanbanBoardModel(ApplicationDbContext context)
        {
       
            _context = context;
            
        }

        public IList<Kanban> KanbanTasks { get; set; }
        public Audit CurrentAudit { get; set; }
        public int AuditID => CurrentAudit?.AuditID ?? 0;

        public string GetPriorityClass(string priority)
        {
            return priority switch
            {
                "High" => "badge-high",
                "Medium" => "badge-medium",
                "Low" => "badge-low",
                _ => "badge-secondary"
            };
        }

        public async Task<IActionResult> OnGetAsync(int? auditId)
        {
            if (auditId.HasValue)
            {
                CurrentAudit = await _context.Audits.FindAsync(auditId.Value);
                if (CurrentAudit == null)
                {
                    return NotFound();
                }

                KanbanTasks = await _context.Kanbans
                    .Where(k => k.AuditID == auditId.Value)
                    .ToListAsync();

                // Update only the time part of the StartDate and EndDate
                await UpdateTimesAsync(KanbanTasks);
            }
            else
            {
                CurrentAudit = null;
                KanbanTasks = await _context.Kanbans.ToListAsync();
            }

            return Page();
        }

        private async Task UpdateTimesAsync(IList<Kanban> tasks)
        {
            var currentTime = DateTime.Now.TimeOfDay;

            foreach (var task in tasks)
            {
                if (task.StartDate.HasValue)
                {
                    task.StartDate = task.StartDate.Value.Date.Add(currentTime);
                }
                if (task.EndDate.HasValue)
                {
                    task.EndDate = task.EndDate.Value.Date.Add(currentTime);
                }
            }

            _context.Kanbans.UpdateRange(tasks);
            await _context.SaveChangesAsync();
        }

        public async Task<IActionResult> OnPostCreateTaskAsync(Kanban newTask)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Kanbans.Add(newTask);
            await _context.SaveChangesAsync();

            

            return RedirectToPage();
        }

        public class UpdateStatusRequest
        {
            public int Id { get; set; }
            public string? Status { get; set; }
        }

        public async Task<IActionResult> OnPostUpdateStatusAsync()
        {
            try
            {
                // Read and deserialize request body
                var jsonData = await new StreamReader(Request.Body).ReadToEndAsync();
                var data = JsonSerializer.Deserialize<UpdateStatusRequest>(jsonData);

                if (data == null)
                {
                    return BadRequest("Invalid data.");
                }

                // Retrieve the task from the database
                var task = await _context.Kanbans.FindAsync(data.Id);
                if (task == null)
                {
                    return NotFound("Task not found.");
                }

                // Update the task status
                task.Status = data.Status;
                _context.Kanbans.Update(task);
                await _context.SaveChangesAsync();

                // Return a success response
                return new JsonResult(new { success = true });
            }
            catch (Exception)
            {
                // Log the exception (you might use a logging framework like Serilog or NLog)
                // Log.Error(ex, "An error occurred while updating task status.");

                // Return a server error response
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
    }
}