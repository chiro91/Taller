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
    public class OrdenController : Controller
    {
        
        private TallerEntities db = new TallerEntities();

        //
        // GET: /Orden/
        [Authorize(Roles = "Supervisor,Empleado")]
        public ActionResult Index()
        {
            var orden = db.Orden.Where(a => a.fechaOut == null).Include(o => o.Propietario);
            return View(orden.ToList());
        }

        //POST
        [HttpPost]
        [Authorize(Roles = "Supervisor,Empleado")]
        public ActionResult Index(FormCollection fm) 
        {
            int dni;
            try
            {
                dni = int.Parse(fm["busqueda"]);
            }
            catch 
            {
                ModelState.AddModelError("errordni", "Ingrese un valor valido");
                return View();
            }
            List<Orden> listaOrden = db.Orden.Where(p => p.Propietario.dni == dni && p.fechaOut == null).Include(k => k.Propietario).ToList();
            return View(listaOrden);
        }

        //
        // GET: /Orden/Details/5

        public ActionResult Details(int id = 0)
        {
            Orden orden = db.Orden.Find(id);
            int manoDeObra = 0;
            double subTotal = 0d;
            foreach (Reparacion item in orden.Reparacion)
            {
                manoDeObra += item.cantHoras;
                subTotal += item.subtotal;
            }

            manoDeObra *= 350;
            Session["manoDeObra"] = manoDeObra;
            Session["total"] = Math.Round(manoDeObra + subTotal);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        //
        // GET: /Orden/Create
        [Authorize(Roles = "Empleado")]
        public ActionResult Create()
        {
            ViewBag.idPropietario = new SelectList(db.Propietario, "idPropietario", "dni");
            return View();
        }

        //
        // POST: /Orden/Create
        [Authorize(Roles = "Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Orden orden)
        {
            if (ModelState.IsValid)
            {
                if (db.Orden.Where(a => a.patente == orden.patente).Where(b => b.fechaOut == null).ToList() == null)
                {
                    orden.fechaIn = DateTime.Now;
                    List<Orden> codigo = db.Orden.ToList();
                    if (codigo == null)
                    {
                        orden.codigo = "10000";
                    }
                    orden.codigo = Convert.ToString(int.Parse(codigo.Last().codigo) + 1);
                    db.Orden.Add(orden);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Auto ya ingresado");
                    return View();
                }
            }

            ViewBag.idPropietario = new SelectList(db.Propietario.OrderBy(a => a.dni), "idPropietario", "dni", orden.idPropietario);
            return View(orden);
        }

        //
        // GET: /Orden/Edit/5
        [Authorize(Roles = "Empleado")]
        public ActionResult Edit(int id = 0)
        {
            Orden orden = db.Orden.Find(id);
            int manoDeObra = 0;
            double subTotal = 0d;
            foreach (Reparacion item in orden.Reparacion)
            {
                manoDeObra += item.cantHoras;
                subTotal += item.subtotal;
            }

            manoDeObra *= 350;
            Session["manoDeObra"] = manoDeObra;
            Session["total"] = Math.Round(manoDeObra + subTotal);
            if (orden == null)
            {
                return HttpNotFound();
            }
            return View(orden);
        }

        //
        // POST: /Orden/Edit/5

        [Authorize(Roles = "Empleado")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Orden orden)
        {
            orden.Propietario = db.Propietario.Find(orden.idPropietario);
         
                
                    orden.fechaOut = DateTime.Now;
                    double aux = 0;
                    int aux2 = 0;
                    foreach (Reparacion item in db.Reparacion.Where(r => r.idOrden == orden.idOrden).ToList())
                    {
                        aux += item.subtotal;
                        aux2 += item.cantHoras;
                    }
                    orden.total = Math.Round(aux + (aux2 * 350));
                    db.Entry(orden).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index","Orden");
 
            }
            
        
       
        //Vista de las ordenes que ya fueron cerradas 
        //GET
        [Authorize(Roles = "Supervisor,Empleado")]
        public ActionResult OrdenesCerradas()
        {
            return View(db.Orden.Where(a => a.fechaOut != null).ToList());
        }


        //POST
        [Authorize(Roles = "Supervisor,Empleado")]
        [HttpPost]
        public ActionResult OrdenesCerradas(FormCollection fm)
        {
            int dni;
            try
            {
                dni = int.Parse(fm["busqueda"]);
            }
            catch 
            { 
                ModelState.AddModelError("errordni","Ingrese un valor valido");
                return View();
            }
            dni = int.Parse(fm["busqueda"]);
            List<Orden> listaOrden = db.Orden.Where(p => p.Propietario.dni == dni && p.fechaOut != null).Include(k => k.Propietario).ToList();
            return View(listaOrden);
 
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}