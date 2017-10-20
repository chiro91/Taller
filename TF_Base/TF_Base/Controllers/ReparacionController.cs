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
    public class ReparacionController : Controller
    {
        private TallerEntities db = new TallerEntities();

 

        //
        // GET: /Reparacion/Create
        [Authorize(Roles = "Empleado")]
        public ActionResult Create()
        {
            ViewBag.idOrden = new SelectList(db.Orden.Where(a => a.total == null), "idOrden", "codigo");
            ViewBag.idRepuesto = new SelectList(db.Repuesto, "idRepuesto", "nombre");
            ViewBag.idUsuario = new SelectList(db.Usuario, "idUsuario", "NombreApellido");
            return View();
        }

        //
        // POST: /Reparacion/Create
        [Authorize(Roles = "Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Reparacion reparacion)
        {
            if (ModelState.IsValid)
            {
                reparacion.Repuesto = db.Repuesto.Find(reparacion.idRepuesto);
                reparacion.subtotal = (reparacion.Repuesto.costo * reparacion.cantRepuesto);
                db.Reparacion.Add(reparacion);
                db.SaveChanges();
                return RedirectToAction("Index","Orden");
            }

            ViewBag.idOrden = new SelectList(db.Orden.Where(a => a.total == null), "idOrden", "codigo", reparacion.idOrden);
            ViewBag.idRepuesto = new SelectList(db.Repuesto, "idRepuesto", "nombre", reparacion.idRepuesto);
            ViewBag.idUsuario = new SelectList(db.Usuario, "idUsuario", "NombreApellido", reparacion.idUsuario);
            return View(reparacion);
        }

        //
        // GET: /Reparacion/Edit/5
        [Authorize(Roles = "Supervisor")]
        public ActionResult Edit(int id = 0)
        {
            Reparacion reparacion = db.Reparacion.Find(id);
            if (reparacion == null)
            {
                return HttpNotFound();
            }
            ViewBag.idOrden = new SelectList(db.Orden, "idOrden", "codigo", reparacion.idOrden);
            ViewBag.idRepuesto = new SelectList(db.Repuesto, "idRepuesto", "nombre", reparacion.idRepuesto);
            ViewBag.idUsuario = new SelectList(db.Usuario.Except(db.Usuario.Where(a => a.idUsuario != 1)).ToList(), "idUsuario", "NombreApelido", reparacion.idUsuario);
            return View(reparacion);

        }

        //
        // POST: /Reparacion/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Reparacion reparacion)
        {
            if (ModelState.IsValid)
            {
                reparacion.Repuesto = db.Repuesto.Find(reparacion.idRepuesto);
                reparacion.subtotal = (reparacion.Repuesto.costo * reparacion.cantRepuesto);
                db.Entry(reparacion).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","Orden");
            }
            ViewBag.idOrden = new SelectList(db.Orden, "idOrden", "codigo", reparacion.idOrden);
            ViewBag.idRepuesto = new SelectList(db.Repuesto, "idRepuesto", "nombre", reparacion.idRepuesto);
            ViewBag.idUsuario = new SelectList(db.Usuario, "idUsuario", "NombreApellido", reparacion.idUsuario);
            return View(reparacion);
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}