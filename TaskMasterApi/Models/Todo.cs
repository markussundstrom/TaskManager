using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaskMasterApi.Models
{
    public class Todo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public String Title { get; set; }
        public string Description { get; set; }
        public TodoPriority Priority { get; set; }
        public bool Completed {get; set; }
        public virtual TodoList TodoList { get; set; }
/*
        public Todo(){ }

        public Todo(string title, TodoPriority priority, string description = "")
        {
            Title = title;
            Description = description;
            if ((byte)priority >= 1 && 3 >= (byte)priority)
            {
                Priority = (TodoPriority)priority;
            }
            else
            {
                Priority = TodoPriority.Low;
            }
        } 

*/    }

    public enum TodoPriority : byte
    {
        High = 1,
        Medium = 2,
        Low = 3,
    }
}
