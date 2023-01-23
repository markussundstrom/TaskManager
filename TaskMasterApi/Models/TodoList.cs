namespace TaskMasterApi.Models
{
    public class TodoList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Todo> Todos { get; set; }

        public TodoList(string title)
        {
            Title = title;
            Todos = new List<Todo>();
        }
    }
}
