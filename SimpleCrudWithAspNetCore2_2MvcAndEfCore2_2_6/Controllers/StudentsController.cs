using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6.Models;

namespace SimpleCrudWithAspNetCore2_2MvcAndEfCore2_2_6.Controllers
{
    public class StudentsController : Controller
    {
        private readonly SchoolContext db;
        public StudentsController(SchoolContext context)
        {
            db = context;
        }

        // GET: Students
        public IActionResult Index()
        {
            return View(db.Students.ToList());
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Id,FirstName,LastName")] Student student)
        {
            if (ModelState.IsValid)
            {
                db.Add(student);
                db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = db.Students.Find(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FirstName,LastName")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    db.Update(student);
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (db.Students.Any(x => x.Id == student.Id))
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
            return View(student);
        }

        // GET: Students/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var student = db.Students
                .FirstOrDefault(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var student = db.Students.Find(id);
            db.Students.Remove(student);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}