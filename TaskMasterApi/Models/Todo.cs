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
        public Priority Priority { get; set; }
        public bool Complete {get; set; }

        public Todo(){ }

        public Todo(string title, byte? priority, string description = "")
        {
            Title = title;
            Description = description;
            if (priority >= 1 && 3 >= priority)
            {
                Priority = (Priority)priority;
            }
            else
            {
                Priority = Priority.Low;
            }
        } 

    }

    public enum Priority : byte
    {
        High = 1,
        Medium = 2,
        Low = 3,
    }
}
