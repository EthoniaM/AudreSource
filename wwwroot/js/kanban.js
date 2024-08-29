document.addEventListener('DOMContentLoaded', () => {
    let draggedTask = null;

    window.toggleDropdown = function (event, element) {
        event.stopPropagation();
        const dropdownMenu = element.nextElementSibling;
        dropdownMenu.classList.toggle('show');
    };

    document.addEventListener('click', function (event) {
        const dropdowns = document.querySelectorAll('.dropdown-menu');
        dropdowns.forEach(function (dropdown) {
            if (!dropdown.contains(event.target) && !dropdown.previousElementSibling.contains(event.target)) {
                dropdown.classList.remove('show');
            }
        });
    });

    const columns = document.querySelectorAll('.kanban-column');

    columns.forEach(column => {
        column.addEventListener('dragover', (e) => {
            e.preventDefault();
            column.classList.add('drag-over');
        });

        column.addEventListener('dragleave', () => {
            column.classList.remove('drag-over');
        });

        column.addEventListener('drop', async (e) => {
            e.preventDefault();
            column.classList.remove('drag-over');

            if (draggedTask) {
                const taskId = draggedTask.getAttribute('data-task-id');
                const newStatus = column.getAttribute('data-status');

                // Only update on the UI for now
                draggedTask.parentElement.removeChild(draggedTask);
                column.querySelector('.kanban-tasks').appendChild(draggedTask);
                updateColumnCounts();

                // Optional: Log to console for debug
                console.log(`Task ${taskId} moved to ${newStatus}`);

                draggedTask = null;
            }
        });
    });

    document.querySelectorAll('.kanban-task').forEach(task => {
        task.addEventListener('dragstart', (e) => {
            draggedTask = e.target;
            e.dataTransfer.setData('text/plain', e.target.getAttribute('data-task-id'));
        });
    });

    function updateColumnCounts() {
        const notStartedCount = document.querySelectorAll('.kanban-column[data-status="Not Started"] .kanban-task').length;
        const inProgressCount = document.querySelectorAll('.kanban-column[data-status="In Progress"] .kanban-task').length;
        const completedCount = document.querySelectorAll('.kanban-column[data-status="Completed"] .kanban-task').length;

        document.querySelector('.kanban-column[data-status="Not Started"] .status-header').textContent = `Not Started (${notStartedCount})`;
        document.querySelector('.kanban-column[data-status="In Progress"] .status-header').textContent = `In Progress (${inProgressCount})`;
        document.querySelector('.kanban-column[data-status="Completed"] .status-header').textContent = `Completed (${completedCount})`;
    }
});
