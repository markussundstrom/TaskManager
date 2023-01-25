using TaskMasterUi.Models;

namespace TaskMasterUi.Services
{
    public class TaskMasterService
    {
        private readonly HttpClient _httpClient;

        public TaskMasterService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<TodoListOverview>>? GetTodoLists()
        {
            List<TodoListOverview> todolists = new List<TodoListOverview>();
            var response = await _httpClient.GetFromJsonAsync<List<TodoListOverview>>("api/todolists");
            todolists.AddRange(response.ToList());
            return todolists;
        }
    }
}
