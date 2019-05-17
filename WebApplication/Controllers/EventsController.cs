using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Application;
using Application.Entities;
using System.Diagnostics;

namespace WebApplication.Controllers
{
    
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static int id1;

        
        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events

        public async Task<IActionResult> Index(int id)
        {
            id1 = id;
            return View(await _context.Events.ToListAsync());
        }

        [HttpGet]
        /*IEnumerable<Event> GetEvents()
        {
            List<Event> events = _context.Events.ToList();
            return events;
        }*/
        public ActionResult ShowEvents(int id)
        {
            id1 = id;
            return View();
        }

        public IActionResult GetEvents()
        {
            if (id1 == 0) { id1 = 1; }
            var events = _context.Events.ToList();
            var filtered_events = events.Where(x => x.UserId == id1).ToList();

            return new JsonResult(filtered_events);

        }
        public IActionResult GetEventsForShow()
        {
            if (id1 == 0) { id1 = 1; }
            var events = _context.Events.ToList();
            var filtered_events = events.Where(x => x.UserId == id1).ToList();

            return new JsonResult(filtered_events);

        }
        [HttpPost]
        public JsonResult DeleteEvent(int eventId)
        {
            Console.WriteLine(eventId);
            var status = false;
            var events = _context.Events.ToList();
            var v = events.Where(a => a.EventId == eventId).FirstOrDefault();
                if (v != null)
                {
                    _context.Events.Remove(v);
                    _context.SaveChanges();
                    status = true;
                }
           
            return new JsonResult(status);
        }
        [HttpPost]
        public JsonResult SaveEvent(Event e)
        {
            var status = false;
            int userId = 1;

                if (e.EventId > 0)
                {
                Console.WriteLine("viduje1");
                    //Update the event
                    var v = _context.Events.Where(a => a.EventId == e.EventId).FirstOrDefault();
                    if (v != null)
                    {
                        Console.WriteLine("viduje2");
                        v.Subject = e.Subject;
                        v.Start = e.Start;
                        v.End = e.End;
                        v.Description = e.Description;
                        v.IsFullDay = e.IsFullDay;
                        v.ThemeColor = e.ThemeColor;
                        
                    }
                }
                else
                {
                Console.WriteLine("else 1");
                if (id1 > 0)
                {
                    e.UserId = id1;
                }
                else
                {
                    e.UserId = userId;
                }
                 _context.Events.Add(e);
                }

            _context.SaveChanges();
            status = true;

            
            return new JsonResult(status);
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("EventId,Subject,Description,Start,End,ThemeColor,IsFullDay,UserId")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }
            return View(@event);
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("EventId,Subject,Description,Start,End,ThemeColor,IsFullDay,UserId")] Event @event)
        {
            if (id != @event.EventId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.EventId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
                .FirstOrDefaultAsync(m => m.EventId == id);
            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.EventId == id);
        }
    }
}
