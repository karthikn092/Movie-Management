using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieManagement.Models;

namespace MovieManagement.Controllers
{
    public class ProducersController : Controller
    {
        private MovieDbContext db = new MovieDbContext();

        public ProducersController()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
        }


        // GET: Producers
        public ActionResult Index()
        {
            try
            {
                return View(db.Producer.ToList());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Producers/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer producer = db.Producer.Find(id);
                if (producer == null)
                {
                    return HttpNotFound();
                }
                return View(producer);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Producers/Create
        public ActionResult Create()
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: Producers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProducerId,ProducerName,Dob,Bio")] Producer producer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Producer.Add(producer);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(producer);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Producers/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer producer = db.Producer.Find(id);
                if (producer == null)
                {
                    return HttpNotFound();
                }
                return View(producer);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: Producers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProducerId,ProducerName,Dob,Bio")] Producer producer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(producer).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(producer);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Producers/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {

                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Producer producer = db.Producer.Find(id);
                if (producer == null)
                {
                    return HttpNotFound();
                }
                return View(producer);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: Producers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Producer producer = db.Producer.Include(y => y.Movies).FirstOrDefault(x => x.ProducerId == id);
                if (producer.Movies != null && producer.Movies.Count > 0)
                {
                    ViewBag.MovieNames = producer.Movies.Select(x => x.MovieName).ToList();
                    ViewBag.Delete = "Producers";
                    return View("ErrorPage");
                }
                db.Producer.Remove(producer);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult CreatePartial()
        {
            try
            {
                return PartialView("CreatePartial");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult CreatePartial([Bind(Include = "ProducerId,ProducerName,Dob,Bio")] Producer producer)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Producer.Add(producer);
                    db.SaveChanges();
                    return Json(producer);
                }
                return PartialView("CreatePartial", producer);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
