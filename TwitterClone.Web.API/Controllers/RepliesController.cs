using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterCloneBackend.Entities;
using TwitterCloneBackend.Entities.Models;

namespace TwitterClone.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RepliesController : ControllerBase
    {
        private readonly DataContext _context;

        public RepliesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Replies
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Replies>>> GetReplies()
        {
            return await _context.Replies.ToListAsync();
        }

        // GET: api/Replies/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Replies>> GetReplies(Guid id)
        {
            var replies = await _context.Replies.FindAsync(id);

            if (replies == null)
            {
                return NotFound();
            }

            return replies;
        }

        // PUT: api/Replies/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReplies(Guid id, Replies replies)
        {
            if (id != replies.Id)
            {
                return BadRequest();
            }

            _context.Entry(replies).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RepliesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Replies
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Replies>> PostReplies(Replies replies)
        {
            _context.Replies.Add(replies);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReplies", new { id = replies.Id }, replies);
        }

        // DELETE: api/Replies/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReplies(Guid id)
        {
            var replies = await _context.Replies.FindAsync(id);
            if (replies == null)
            {
                return NotFound();
            }

            _context.Replies.Remove(replies);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool RepliesExists(Guid id)
        {
            return _context.Replies.Any(e => e.Id == id);
        }
    }
}
