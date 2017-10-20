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
    public class PropietarioController : Controller
    {
        private TallerEntities db = new TallerEntities();

        //
        // GET: /Propietario/
        [Authorize(Roles = "Supervisor")]
        public ActionResult Index()
        {
            return View(db.Propietario.ToList());
        }


        //POST: /PROPIETARIO
        //

        [Authorize(Roles = "Supervisor")]
        [HttpPost]
        public ActionResult Index(FormCollection fm)
        {
            int dni;
            try
            {
                dni = int.Parse(fm["busqueda"]);
            }
            catch
            {
                ModelState.AddModelError("", "Ingrese un valor valido");
                return View();
            }
            return View(db.Propietario.Where(p => p.dni == dni).ToList());
            
 
        }
        // GET: /Propietario/Details/5
        [Authorize(Roles = "Empleado,Supervisor")]
        public ActionResult Details(int id = 0)
        {
            Propietario propietario = db.Propietario.Find(id);
            if (propietario == null)
            {
                return HttpNotFound();
            }
            return View(propietario);
        }

        //
        // GET: /Propietario/Create
        [Authorize(Roles ="Empleado,Supervisor")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Propietario/Create
        [Authorize(Roles = "Empleado,Supervisor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario, FormCollection fm)
        {
            
            if (ModelState.IsValid)
            {
                int dni =int.Parse( fm["dni"]);
                if (db.Propietario.Where(x => x.dni == dni).SingleOrDefault() == null)
                {
                    db.Propietario.Add(propietario);
                    db.SaveChanges();
                    return RedirectToAction("Index","Orden");
                }
                else
                {
                    ModelState.AddModelError("", "El propietario ya se encuentra en el sistema");
                    return View();
                }
            }

            return View(propietario);
        }

        //
        // GET: /Propietario/Edit/5
        [Authorize(Roles = "Supervisor")]
        public ActionResult Edit(int id = 0)
        {
            Propietario propietario = db.Propietario.Find(id);
            if (propietario == null)
            {
                return HttpNotFound();
            }
            return View(propietario);
        }

        //
        // POST: /Propietario/Edit/5
        [Authorize(Roles = "Supervisor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Propietario propietario)
        {
            if (ModelState.IsValid)
            {
                db.Entry(propietario).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(propietario);
        }

        //
        // GET: /Propietario/Delete/5
        [Authorize(Roles = "Supervisor")]
        public ActionResult Delete(int id = 0)
        {
            Propietario propietario = db.Propietario.Find(id);
            if (propietario == null)
            {
                return HttpNotFound();
            }
            return View(propietario);
        }

        //
        // POST: /Propietario/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Propietario propietario = db.Propietario.Find(id);
            db.Propietario.Remove(propietario);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}