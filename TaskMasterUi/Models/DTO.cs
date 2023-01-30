
namespace TaskMasterUi.Models
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
        public TodoPriority Priority {get; set; }
    }

    public class EditTodoDTO
    {
        public string Title {get; set; }
        public string Description {get; set; } = "";
        public TodoPriority Priority {get; set; }
        public bool Completed {get; set; }
    }

    public enum TodoPriority : byte
    {
        High = 1,
        Medium = 2,
        Low = 3,
    }
}
