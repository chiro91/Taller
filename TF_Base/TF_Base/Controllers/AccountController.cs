using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TF_Base.Models;
using WebMatrix.WebData;

namespace mvcStore.Controllers
{
    public class AccountController : Controller
    {
        TallerEntities db = new TallerEntities();
        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login login)
        {
            if (ModelState.IsValid)
            {
                bool res = WebSecurity.Login(login.UserName, login.Password, login.RememberMe);
                if (res)
                {
                    return RedirectToAction("Redirect");
                }
            }

            ModelState.AddModelError("", "Error al logearse");
            return View(login);
        }


        //Metodo para redirigir al usuario dependiendo del rol
        public ActionResult Redirect()
        {
            if (User.IsInRole("Supervisor"))
            {
                return RedirectToAction("VistaSupervisor", "Account");
            }
            else
            {
                return RedirectToAction("VistaEmpleado", "Account"); 
            }
        }
        //
        // GET: /Account/Logout
        public ActionResult Logout()
        {
            WebSecurity.Logout();
            return RedirectToAction("Login", "Account");
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Supervisor")]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Supervisor")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Registro registro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    WebSecurity.CreateUserAndAccount(registro.UserName, registro.Password, new {nombre = registro.Nombre, apellido = registro.Apellido, dni = registro.Dni, mail = registro.mail });
                    Roles.AddUserToRole(registro.UserName ,"Empleado");
                    WebSecurity.Login(registro.UserName, registro.Password);
                    return RedirectToAction("VistaSupervisor","Account");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", e.StatusCode.ToString());
                }
            }
            return View();
        }

        //Interfaz del supervisor GET
        [Authorize(Roles = "Supervisor")]
        public ActionResult VistaSupervisor()
        {
            return View();


            //interfaz del empleado GET

        }
        [Authorize(Roles = "Empleado")]
        public ActionResult VistaEmpleado()
        {
            return View();
        }


        //Interfaz intermedia para verificar si un propietario esta en el sistema
        //GET
        [Authorize(Roles="Empleado")]
        public ActionResult Orden()
        {
            return View();
        }

        //POST
        [Authorize(Roles = "Empleado")]
        [HttpPost]
        public ActionResult Orden(FormCollection fm)
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
            if (db.Propietario.Where(x => x.dni == dni).SingleOrDefault() == null)
            {
                return RedirectToAction("Create", "Propietario");
            }
            else
                return RedirectToAction("Create", "Orden");
 
        }
    }
}
