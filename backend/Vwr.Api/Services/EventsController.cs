using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Vwr.Domain.Entities;

namespace Vwr.Api.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        AppDb _db;
        public EventsController(AppDb db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetEvents() 
        {
            var xyu = await _db.Events.OrderByDescending(e => e.ReceivedAt).Take(100).ToListAsync();
            return Ok(xyu);
        }
    }
}
