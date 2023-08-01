using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;


namespace TodoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoController(TodoContext context)
        {
            _context = context;
        }


        // GET: api/<TodoController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Todo>>> GetAll()
        {
            var response = await _context.Todos.ToListAsync();

            return Ok(response);
        }

        // GET api/<TodoController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Todo>> GetById(int id)
        {
            var response = await _context.Todos.FindAsync(id);

            if (response == null) { return NotFound(); }

            return Ok(response);
        }

        // POST api/<TodoController>
        [HttpPost]
        public async Task<ActionResult<Todo>> AddTodo(Todo todo)
        {
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetById", new { id = todo.Id }, todo);
        }

        // PUT api/<TodoController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Todo>> CompletedTodo(int id, Todo todo)
        {
            if (id != todo.Id) { return BadRequest(); }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }

                throw;
            }

            return Ok(todo);
        }

        // DELETE api/<TodoController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            if (todo == null) { return NotFound(); }

            _context.Remove(todo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(x => x.Id == id);
        }
    }
}
