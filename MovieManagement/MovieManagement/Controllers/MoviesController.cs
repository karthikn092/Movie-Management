using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieManagement.Models;
using System.IO;
using System.Collections;

namespace MovieManagement.Controllers
{
    public class MoviesController : Controller
    {
        private MovieDbContext db = new MovieDbContext();

        public MoviesController()
        {
            db.Configuration.ValidateOnSaveEnabled = false;
        }

        // GET: Movies
        public ActionResult Index()
        {
            try
            {
                List<Movie> movie = db.Movie.Include(x => x.Actors).Include(y => y.Producer).ToList();
                movie.ForEach(item =>
                {
                    item.ProducerString = item.Producer.ProducerName;
                }
                    );
                return View(movie);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Movies/Details/5
        public ActionResult Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movie movie = db.Movie.Include(y => y.Producer).Include(y => y.Actors).FirstOrDefault(x => x.MovieId == id);

                if (movie == null)
                {
                    return HttpNotFound();
                }
                movie.ProducerString = movie.Producer.ProducerName;
                return View(movie);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Movies/Create
        public ActionResult Create()
        {
            try
            {
                Movie movie = new Movie();
                movie.ActorsCollection = GetActorsCollection();
                movie.ProducerCollection = GetProducersList();
                movie.YearCollection = GetYearCollection();
                return View(movie);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieId,MovieName,YearOfRelease,Plot,Poster,ProducerString,ActorsCollection,ImageData")] Movie movie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    HttpPostedFileBase file = Request.Files["ImageData"];
                    movie.Poster = ConvertToBytes(file);

                    var producer = db.Producer.FirstOrDefault(x => x.ProducerName == movie.ProducerString);
                    movie.Producer = producer;
                    if (producer.Movies == null)
                        producer.Movies = new List<Movie>();
                    producer.Movies.Add(movie);

                    var selectedItem = movie.ActorsCollection.Where(x => x.IsSelected);
                    foreach (var item in selectedItem)
                    {
                        var actor = db.Actor.FirstOrDefault(x => x.ActorName == item.Name);
                        movie.Actors.Add(actor);
                        actor.Movies.Add(movie);
                    }
                    db.Movie.Add(movie);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View(movie);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Movies/Edit/5
        public ActionResult Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movie movie = db.Movie.Include(z => z.Producer).Include(z => z.Actors).FirstOrDefault(x => x.MovieId == id);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                movie.ProducerCollection = GetProducersList();
                movie.YearCollection = GetYearCollection();
                movie.ProducerString = movie.Producer.ProducerName;

                List<Actor> actorCollection = new List<Actor>(movie.Actors);
                foreach (var each in actorCollection)
                {
                    movie.ActorsCollection.Add(new CheckBoxModel() { Name = each.ActorName, ID = each.ActorId, IsSelected = true });
                }
                db.Actor.ToList().Except(movie.Actors).ToList().ForEach(item => movie.ActorsCollection.Add(new CheckBoxModel() { Name = item.ActorName, ID = item.ActorId }));

                return View(movie);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieId,MovieName,YearOfRelease,Plot,Poster,,ProducerString,ActorsCollection,ImageData")] Movie movie)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var existingMovie = db.Movie.Where(s => s.MovieId == movie.MovieId).Include(y => y.Producer).Include(y => y.Actors);
                    var mov = existingMovie.FirstOrDefault();
                    mov.MovieName = movie.MovieName;
                    mov.Plot = movie.Plot;
                    mov.YearOfRelease = movie.YearOfRelease;

                    var producer = db.Producer.FirstOrDefault(x => x.ProducerName == movie.ProducerString);
                    mov.Producer = producer;

                    mov.Actors.Clear();
                    var selectedItem = movie.ActorsCollection.Where(x => x.IsSelected);
                    foreach (var item in selectedItem)
                    {
                        var actor = db.Actor.FirstOrDefault(x => x.ActorName == item.Name);
                        mov.Actors.Add(actor);
                    }
                    HttpPostedFileBase file = Request.Files["ImageData"];
                    if (file != null && file.ContentLength > 0)
                    {
                        mov.Poster = ConvertToBytes(file);
                    }

                    db.Entry(mov).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                return View(movie);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // GET: Movies/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Movie movie = db.Movie.Include(y => y.Actors).Include(y => y.Producer).FirstOrDefault(x => x.MovieId == id);
                if (movie == null)
                {
                    return HttpNotFound();
                }
                movie.ProducerString = movie.Producer.ProducerName;
                return View(movie);
            }
            catch (Exception)
            {
                return View("Error");
            }
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {
                Movie movie = db.Movie.Include(y => y.Actors).Include(y => y.Producer).FirstOrDefault(x => x.MovieId == id);
                db.Movie.Remove(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
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

        private List<object> GetProducersList()
        {
            List<Producer> actors = db.Producer.ToList();
            List<object> producerCollection = null;

            if (actors != null)
            {
                producerCollection = new List<object>();
                actors.ForEach(
                    item => producerCollection.Add(new SelectListItem() { Text = item.ProducerName, Value = item.ProducerName })
                  );
            }
            return producerCollection;
        }

        private List<object> GetYearCollection()
        {
            List<int> years = Enumerable.Range(1900, 118).OrderByDescending(i=>i).ToList();
            List<object> producerCollection = null;

            if (years != null)
            {
                producerCollection = new List<object>();
                years.ForEach(
                    item => producerCollection.Add(new SelectListItem() { Text = item.ToString(), Value = item.ToString() })
                  );
            }
            return producerCollection;

        }

        private byte[] ConvertToBytes(HttpPostedFileBase image)
        {
            byte[] imageBytes = null;
            BinaryReader reader = new BinaryReader(image.InputStream);
            imageBytes = reader.ReadBytes((int)image.ContentLength);
            return imageBytes;
        }
    }
}
