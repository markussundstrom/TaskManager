namespace TaskMasterApi.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual IList<Todo> Todos { get; set; } = new List<Todo>();
    }
}
