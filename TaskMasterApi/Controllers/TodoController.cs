using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
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
        public async Task<ActionResult<IEnumerable<object>>> GetTodoLists()
        {
            List<object> listobjects = new List<object>();

            await foreach (TodoList list in _context.TodoLists)
            {
                listobjects.Add(TodoListToObject(list));
            }
            return Ok(listobjects);
        }

        [HttpGet]
        [Route("todolist/{id}")]
        public async Task<ActionResult<object>> GetSingleTodoList(int id)
        {
            TodoList? todolist = await _context.TodoLists.SingleOrDefaultAsync(l => l.Id == id);
            if (todolist == null)
            {
                return NotFound();
            }
            return Ok(TodoListToObject(todolist));
        }

        [HttpGet]
        [Route("todo/{id}")]
        public async Task<ActionResult<object>> GetSingleTodo(int id)
        {
            Todo? todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            return Ok(TodoToObject(todo));
        }

        [HttpPost]
        [Route("tasklists")]
        public async Task<ActionResult<object>> PostTaskList([FromBody] JsonElement listObject)
        {
            var newList = listObject.GetProperty("Title");
            if(!((Type)newList.GetType()).GetProperties().Any(p => p.Name.Equals("Title")))
            {
                return BadRequest();
            }
            string title = (string)newList.GetType().GetProperty("Title").GetValue(newList, null);
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest();
            }
            TodoList newTodoList = new TodoList(title);
            _context.TodoLists.Add(newTodoList);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSingleTodoList), new {Id = newTodoList.Id });
        }

        [HttpPost]
        [Route("tasklist/{id}/todos")]
        public async Task<ActionResult<object>> PostTodo(int id, [FromBody] object newTodo)
        {
            if(!((Type)newTodo.GetType()).GetProperties().Any(p => p.Name.Equals("Title")))
            {
                return BadRequest();
            }
            string title = (string)newTodo.GetType().GetProperty("Title").GetValue(newTodo, null);
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest();
            }
            TodoList? list = await _context.TodoLists.SingleOrDefaultAsync(t => t.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            string? description = (string)newTodo.GetType().GetProperty("Description").GetValue(newTodo, null);
            byte? priority = (byte)newTodo.GetType().GetProperty("Priority").GetValue(newTodo, null);
            Todo todo = new Todo(title, priority, description);
            list.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetSingleTodo), new {Id = todo.Id });
        }

        [HttpPut]
        [Route("todolist/{id}")]
        public async Task<ActionResult<object>> PutTodoList(int id, [FromBody] object listObject)
        {
            if(!((Type)listObject.GetType()).GetProperties().Any(p => p.Name.Equals("Title")))
            {
                return BadRequest();
            }
            string? title = (string)listObject.GetType().GetProperty("Title").GetValue(listObject, null);
            if (string.IsNullOrEmpty(title))
            {
                return BadRequest();
            }
            TodoList? list = await _context.TodoLists.SingleOrDefaultAsync(t => t.Id == id);
            if (list == null)
            {
                return NotFound();
            }
            list.Title = title;
            await _context.SaveChangesAsync();
            return Ok(TodoListToObject(list));
        }

        [HttpPut]
        [Route("todo/{id}")]
        public async Task<ActionResult<object>> PutTodo(int id, [FromBody] object todoObject)
        {
            Todo todo = await _context.Todos.SingleOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                return NotFound();
            }
            todo.Title = (string?)todoObject.GetType().GetProperty("Title").GetValue(todoObject, null) ?? todo.Title;
            todo.Description = (string?)todoObject.GetType().GetProperty("Description").GetValue(todoObject, null) ?? todo.Description;
            short? priority = (short?)todoObject.GetType().GetProperty("Priority").GetValue(todoObject, null);
            if (priority >= 1 && priority <= 3)
            {
                todo.Priority = (Priority)priority;
            }
            todo.Complete = (bool?)todoObject.GetType().GetProperty("Complete").GetValue(todoObject, null) ?? todo.Complete;
            await _context.SaveChangesAsync();
            return Ok(TodoToObject(todo));
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
                

        private object TodoListToObject(TodoList todolist)
        {
            List<object> Todos = new List<object>();
            foreach (Todo todo in todolist.Todos)
            {
                Todos.Add(TodoToObject(todo));
            }

            return new {Id = todolist.Id, Title = todolist.Title,
                        Todos = Todos};
        }

        private object TodoToObject(Todo todo)
        {
            return new {Id = todo.Id, Title = todo.Title,
                        Description = todo.Description, 
                        Complete = todo.Complete, Priority = todo.Priority};
        }
    }
}
