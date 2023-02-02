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

        public async Task EditTodoList(int id, CreateTodoListDTO todoList)
        {
            await _httpClient.PutAsJsonAsync<CreateTodoListDTO>("api/todolist/" + id, todoList);
        }

        public async Task EditTodo(int id, EditTodoDTO todo)
        {
            await _httpClient.PutAsJsonAsync<EditTodoDTO>("api/todo/" + id, todo);
        }

        public async Task CreateTodoList(CreateTodoListDTO todoList)
        {
            await _httpClient.PostAsJsonAsync<CreateTodoListDTO>("api/todolists", todoList);
        }

        public async Task CreateTodo(int listId, CreateTodoDTO todo)
        {
            await _httpClient.PostAsJsonAsync<CreateTodoDTO>("api/todolist/" + listId + "/todos", todo);
        }

        public async Task DeleteTodoList(int id)
        {
            await _httpClient.DeleteAsync("api/todolist/" + id);
        }

        public async Task DeleteTodo(int id)
        {
            await _httpClient.DeleteAsync("api/todo/" + id);
        }
    }
}
