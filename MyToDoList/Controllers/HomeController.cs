using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyToDoList.Models;

namespace MyToDoList.Controllers
{
    public class HomeController : Controller
    {
        private readonly ToDoDBContext context;

        public HomeController(ToDoDBContext ctx) => context = ctx;

        public IActionResult Index(string subject, string due, string status)
        {
            var filters = new Filter(subject, due, status);
            ViewBag.Filters = filters;

            ViewBag.Subject = context.Subjects.ToList();
            ViewBag.Status = context.Status.ToList();
            ViewBag.DueFilters = Filter.DueFilterValues;

            IQueryable<ToDo> query = context.ToDos
                .Include(t => t.subject)
                .Include(t => t.Status);

            if (filters.HasSubject)
            {
                query = query.Where(t => t.SubjectId == filters.SubjectId);
            }
            if (filters.HasStatus)
            {
                query = query.Where(t => t.StatusId == filters.StatusId);
            }
            if (filters.HasDue)
            {
                var today = DateTime.Today;
                if (filters.IsPast)
                {
                    query = query.Where(t => t.DueDate < today);
                }
                else if (filters.IsFuture)
                {
                    query = query.Where(t => t.DueDate > today);
                }
                else if (filters.IsToday)
                {
                    query = query.Where(t => t.DueDate == today);
                }
            }

            var tasks = query.OrderBy(t => t.DueDate).ToList();
            return View(tasks);
        }

        [HttpGet]
        public IActionResult Add()
        {
            ViewBag.Subject = context.Subjects.ToList();
            ViewBag.Status = context.Status.ToList();

            var task = new ToDo { StatusId = "open" };
            return View(task);
        }

        [HttpPost]
        public IActionResult Add(ToDo task)
        {
            if (ModelState.IsValid)
            {
                context.ToDos.Add(task);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Subject = context.Subjects.ToList();
                ViewBag.Status = context.Status.ToList();
                return View(task);
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var task = context.ToDos.Find(id);
            if (task == null)
            {
                return NotFound();
            }

            ViewBag.Subject = context.Subjects.ToList();
            ViewBag.Status = context.Status.ToList();
            return View(task);
        }

        [HttpPost]
        public IActionResult Edit(ToDo task)
        {
            if (ModelState.IsValid)
            {
                context.ToDos.Update(task);
                context.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Subject = context.Subjects.ToList();
                ViewBag.Status = context.Status.ToList();
                return View(task);
            }
        }

        [HttpPost]
        public IActionResult Filters(string subject, string due, string status)
        {
            return RedirectToAction("Index", new { subject, due, status });
        }

        [HttpPost]
        public IActionResult MarkComplete([FromRoute] string subject, string due, string status, ToDo selected)
        {
            selected = context.ToDos.Find(selected.Id);

            if (selected != null)
            {
                selected.StatusId = "closed";
                context.SaveChanges();
            }
            return RedirectToAction("Index", new { subject, due, status });
        }

        [HttpPost]
        public IActionResult DeleteComplete(string subject, string due, string status)
        {
            var toDelete = context.ToDos
                .Where(t => t.StatusId == "closed").ToList();

            foreach (var task in toDelete)
            {
                context.ToDos.Remove(task);
            }
            context.SaveChanges();
            return RedirectToAction("Index", new { subject, due, status });
        }
    }
}
