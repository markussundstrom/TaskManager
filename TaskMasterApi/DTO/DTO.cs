using TaskMasterApi.Models;

namespace TaskMasterApi.DTO
{
    public class TodoListOverviewDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }

    public class TodoListDTO
    {
        public int Id {get; set; }
        public string Title {get; set; }
        public List<TodoDTO> Todos {get; set;}

        public TodoListDTO()
        {
            Todos = new List<TodoDTO>();
        }
    }

    public class TodoDTO
    {
        public int Id {get; set; }
        public string Title {get; set; }
        public string Description {get; set; }
        public TodoPriority Priority { get; set; }
        public bool Completed { get; set; }
    }

    public class CreateTodoListDTO
    {
        public string Title {get; set;}
    }

    public class CreateTodoDTO
    {
        public string Title {get; set; }
        public string Description {get; set; } = "";
        public TodoPriority Priority {get; set; } = TodoPriority.Low;
    }

    public class EditTodoDTO
    {
        public string Title {get; set; }
        public string Description {get; set; } = "";
        public TodoPriority Priority {get; set; } = 0;
        public bool? Completed {get; set; }
    }
}
