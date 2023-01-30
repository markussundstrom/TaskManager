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

        public async Task<List<TodoListOverviewDTO>>? GetTodoLists()
        {
            List<TodoListOverviewDTO> todolists = new List<TodoListOverviewDTO>();
            var response = await _httpClient.GetFromJsonAsync<List<TodoListOverviewDTO>>("api/todolists");
            todolists.AddRange(response.ToList());
            return todolists;
        }

        public async Task<TodoListDTO>? GetSingleTodoList(int id)
        {
            return await _httpClient.GetFromJsonAsync<TodoListDTO>("api/todolist/" + id);
        }

        public async Task<TodoDTO>? GetSingleTodo(int id)
        {
            return await _httpClient.GetFromJsonAsync<TodoDTO>("api/todo/" + id);
        }

        public async Task PutTodo(int id, EditTodoDTO todo)
        {
            await _httpClient.PutAsJsonAsync<EditTodoDTO>("api/todo/" + id, todo);
        }
    }
}
