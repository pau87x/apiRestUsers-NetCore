using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Models;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserContext _context;

        public UserController(UserContext context)
        {
            _context = context;

            if (_context.UserItems.Count() == 0)
            {
                // Create a new UserItem if collection is empty,
                // which means you can't delete all UserItems.
                _context.UserItems.Add(new UserItem { Name = "Item1", Username = "Item1",Email = "p@p.com" });
                _context.SaveChanges();
            }
        }

                // GET: api/User
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserItem>>> GetUserItems()
        {
            return await _context.UserItems.ToListAsync();
        }

        // GET: api/User/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserItem>> GetUserItem(long id)
        {
            var user = await _context.UserItems.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/User
        [HttpPost]
        public async Task<ActionResult<UserItem>> PostUserItem(UserItem item)
        {
            _context.UserItems.Add(item);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUserItem), new { id = item.Id }, item);
        }

        // PUT: api/User/1
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserItem(long id, UserItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }

            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/User/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserItem(long id)
        {
            var userItem = await _context.UserItems.FindAsync(id);

            if (userItem == null)
            {
                return NotFound();
            }

            _context.UserItems.Remove(userItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}