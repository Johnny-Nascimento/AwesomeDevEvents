using AwesomeDevEvents.API.Entities;
using AwesomeDevEvents.API.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AwesomeDevEvents.API.Controllers
{
    [Route("api/dev-events")]
    [ApiController]
    public class DevEventsController : ControllerBase
    {
        private readonly DevEventsDbContext _context;

        public DevEventsController(DevEventsDbContext context)
        {
            _context = context;
        }

        // api/dev-events GET
        [HttpGet]
        public IActionResult GetAll()
        {
            var devEvents = _context.DevEvent.Where(d => !d.IsDeleted).ToList();

            return Ok(devEvents);
        }

        // api/dev-events/id GET
        [HttpGet("{id}")]
        public IActionResult GetById(Guid id)
        {
            var devEvent = _context.DevEvent
                .Include(d => d.Speakers)
                .SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
                return NotFound(); 

            return Ok(devEvent);
        }

        // api/dev-events POST
        [HttpPost]
        public IActionResult Post(DevEvent devEvent)
        {
            _context.DevEvent.Add(devEvent);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetById), new { id = devEvent.Id }, devEvent);
        }

        // api/dev-events/id PUT
        [HttpPut("{id}")]
        public IActionResult Update(Guid id, DevEvent input)
        {
            var devEvent = _context.DevEvent.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
                return NotFound();

            devEvent.Update(input.Title, input.Description, input.StartDate, input.EndDate);

            _context.DevEvent.Update(devEvent);
            _context.SaveChanges();

            return NoContent();
        }

        // api/dev-events/id DELETE
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            var devEvent = _context.DevEvent.SingleOrDefault(d => d.Id == id);

            if (devEvent == null)
                return NotFound();

            devEvent.Delete();
            _context.SaveChanges();

            return NoContent();
        }

        // api/dev-events/id/speakers POST
        [HttpPost("{id}/speakers")]
        public IActionResult PostSpeaker(Guid id, DevEventSpeaker devEventSpeaker)
        {
            devEventSpeaker.Id = id;

            var devEvent = _context.DevEvent.Any(d => d.Id == id);

            if (!devEvent)
                return NotFound();

            _context.DevEventSpeaker.Add(devEventSpeaker);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
