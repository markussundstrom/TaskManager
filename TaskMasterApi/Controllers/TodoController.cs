using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TaskMasterApi.DTO;
using TaskMasterApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TaskMasterApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }
        /// <summary>
        /// View overview of todolists
        /// </summary>
        /// <returns>A list of todolists</returns>
        [HttpGet]
        [Route("todolists")]
        public async Task<ActionResult<IEnumerable<TodoListOverviewDTO>>> GetTodoLists()
        {
            List<object> listobjects = new List<object>();

            await foreach (TodoList list in _context.TodoLists)
            {
                listobjects.Add(TodoListToTodoListOverviewDTO(list));
            }
            return Ok(listobjects);
        }

        [HttpGet]
        [Route("todolist/{id}")]
        public async Task<ActionResult<TodoListDTO>> GetSingleTodoList(int id)
        {
            TodoList? todolist = await _context.TodoLists.Include("Todos").SingleOrDefaultAsync(l => l.Id == id);
            if (todolist == null)
            {
                return NotFound();
            }
            return Ok(TodoListToTodoListDTO(todolist));
        }

        [HttpGet]
        [Route("todo/{id}")]
        public async Task<ActionResult<TodoDTO>> GetSingleTodo(int id)
        {
            Todo? todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(TodoToTodoDTO(todo));
        }

        [HttpPost]
        [Route("todolists")]
        public async Task<ActionResult<TodoListDTO>> PostTaskList(CreateTodoListDTO todoListDto)
        {
            if (string.IsNullOrEmpty(todoListDto.Title))
            {
                return BadRequest();
            }
            TodoList newTodoList = new TodoList{Title = todoListDto.Title };
            _context.TodoLists.Add(newTodoList);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSingleTodoList", new {id = newTodoList.Id }, TodoListToTodoListDTO(newTodoList));
        }

        [HttpPost]
        [Route("todolist/{id}/todos")]
        public async Task<ActionResult<TodoDTO>> PostTodo(int id, CreateTodoDTO todoDto)
        {
            TodoList? list = await _context.TodoLists.Include("Todos").SingleOrDefaultAsync(t => t.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(todoDto.Title))
            {
                return BadRequest();
            }

            Todo todo = new Todo();
            todo.Title = todoDto.Title;
            todo.Priority = TodoPriority.IsDefined(typeof(TodoPriority), todoDto.Priority) ? todoDto.Priority : TodoPriority.Low;
            todo.Description = todoDto.Description;
            list.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetSingleTodo", new {id = todo.Id }, TodoToTodoDTO(todo));
        }

        [HttpPut]
        [Route("todolist/{id}")]
        public async Task<ActionResult<TodoListDTO>> PutTodoList(int id, CreateTodoListDTO todoListDto)
        {
            TodoList? list = await _context.TodoLists.SingleOrDefaultAsync(t => t.Id == id);
            if (list == null)
            {
                return NotFound();
            }

            if (string.IsNullOrEmpty(todoListDto.Title))
            {
                return BadRequest();
            }

            list.Title = todoListDto.Title;
            await _context.SaveChangesAsync();
            return Ok(TodoListToTodoListDTO(list));
        }

        [HttpPut]
        [Route("todo/{id}")]
        public async Task<ActionResult<TodoDTO>> PutTodo(int id, EditTodoDTO todoDto)
        {
            Todo? todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            todo.Title = todoDto.Title ?? todo.Title;
            todo.Description = todoDto.Description ?? todo.Description;

            if ((byte)todoDto.Priority >= 1 && (byte)todoDto.Priority <= 3)
            {
                todo.Priority = todoDto.Priority;
            }
            todo.Completed = todoDto.Completed ?? todo.Completed;
            await _context.SaveChangesAsync();
            return Ok(TodoToTodoDTO(todo));
        }


        [HttpDelete]
        [Route("todolist/{id}")]
        public async Task<ActionResult> DeleteTodoList(int id)
        {
            TodoList? todoList = await _context.TodoLists.SingleOrDefaultAsync(t => t.Id == id);
            if (todoList == null)
            {
                return NotFound();
            }
            _context.TodoLists.Remove(todoList);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete]
        [Route("todo/{id}")]
        public async Task<ActionResult> DeleteTodo(int id)
        {
            Todo? todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();
            return Ok();
        }

        private TodoListOverviewDTO TodoListToTodoListOverviewDTO (TodoList todolist)
        {
            return new TodoListOverviewDTO{Title = todolist.Title, Id = todolist.Id };
        }

        private TodoListDTO TodoListToTodoListDTO (TodoList todolist)
        {
            TodoListDTO todoListDto = new TodoListDTO{Title = todolist.Title, Id = todolist.Id};
            foreach (Todo todo in todolist.Todos)
            {
                todoListDto.Todos.Add(TodoToTodoDTO(todo));
            }
            return todoListDto;
        }

        private TodoDTO TodoToTodoDTO (Todo todo)
        {
            return new TodoDTO {Id = todo.Id, Title = todo.Title, 
                                Completed = todo.Completed, Priority = todo.Priority,
                                Description = todo.Description };
        }
    }
}
