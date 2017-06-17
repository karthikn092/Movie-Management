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
    public class ActorsController : Controller
    {
        private MovieDbContext db = new MovieDbContext();

        public ActorsController()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
        }

        // GET: Actors
        public ActionResult Index()
        {
            try
            {
                return View(db.Actor.ToList());
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Actors/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Actor actor = db.Actor.Find(id);
                if (actor == null)
                {
                    return HttpNotFound();
                }
                return View(actor);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Actors/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Actors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActorId,ActorName,Dob,Bio")] Actor actor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Actor.Add(actor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(actor);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Actors/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Actor actor = db.Actor.Find(id);
                if (actor == null)
                {
                    return HttpNotFound();
                }
                return View(actor);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: Actors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActorId,ActorName,Dob,Bio")] Actor actor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Entry(actor).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(actor);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Actors/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Actor actor = db.Actor.Find(id);
                if (actor == null)
                {
                    return HttpNotFound();
                }
                return View(actor);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: Actors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                Actor actor = db.Actor.Include(y => y.Movies).FirstOrDefault(x => x.ActorId == id);
                if (actor.Movies != null && actor.Movies.Count > 0)
                {
                    ViewBag.MovieNames = actor.Movies.Select(x => x.MovieName).ToList();
                    ViewBag.Delete = "Actors";
                    return View("ErrorPage");
                }

                db.Actor.Remove(actor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        public ActionResult CreateActorPartial()
        {
            try
            {
                return PartialView("CreateActorPartial");
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult CreateActorPartial([Bind(Include = "ActorId,ActorName,Dob,Bio")] Actor actor)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    db.Actor.Add(actor);
                    db.SaveChanges();
                    Movie movie = new Movie();
                    movie.ActorsCollection = GetActorsCollection();
                    return PartialView("DropDownPartial", movie);

                }
                return PartialView("CreateActorPartial", actor);
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

        private List<CheckBoxModel> GetActorsCollection()
        {
            List<Actor> actors = db.Actor.ToList();
            List<CheckBoxModel> actorCollection = null;

            if (actors != null)
            {
                actorCollection = new List<CheckBoxModel>();
                actors.ForEach(
                    item => actorCollection.Add(new CheckBoxModel() { Name = item.ActorName, ID = item.ActorId })
                  );
            }
            return actorCollection;
        }
    }
}
