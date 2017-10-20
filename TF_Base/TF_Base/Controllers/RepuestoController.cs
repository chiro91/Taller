using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TF_Base.Models;

namespace TF_Base.Controllers
{
    public class RepuestoController : Controller
    {
        private TallerEntities db = new TallerEntities();

        //
        // GET: /Repuesto/
        [Authorize(Roles = "Supervisor")]
        public ActionResult Index()
        {
            return View(db.Repuesto.ToList());
        }


        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        public ActionResult Index(FormCollection fm)
        {
            string nombre = fm["busqueda"];

            return View(db.Repuesto.Where(a => a.nombre.Contains(nombre)).ToList());
 
        }


        //
        // GET: /Repuesto/Details/5
        [Authorize(Roles = "Supervisor")]
        public ActionResult Details(int id = 0)
        {
            Repuesto repuesto = db.Repuesto.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuesto);
        }

        //
        // GET: /Repuesto/Create
        [Authorize(Roles="Supervisor")]
        public ActionResult Create()
        {
            
            return View();
        }

        //
        // POST: /Repuesto/Create
        [Authorize(Roles = "Supervisor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                if (db.Repuesto.Where(a => a.nombre == repuesto.nombre).SingleOrDefault() == null)
                {
                    db.Repuesto.Add(repuesto);
                    db.SaveChanges();
                    return RedirectToAction("Index","Repuesto");
                }
                else
                {
                    ModelState.AddModelError("", "El repuesto ya existe");
                    return View();
                }

            }

            return View(repuesto);
        }

        //
        // GET: /Repuesto/Edit/5
        [Authorize(Roles ="Supervisor")]
        public ActionResult Edit(int id = 0)
        {
            Repuesto repuesto = db.Repuesto.Find(id);
            if (repuesto == null)
            {
                return HttpNotFound();
            }
            return View(repuesto);
        }

        //
        // POST: /Repuesto/Edit/5
        [Authorize(Roles = "Supervisor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Repuesto repuesto)
        {
            if (ModelState.IsValid)
            {
                db.Entry(repuesto).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(repuesto);
        }

     

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}