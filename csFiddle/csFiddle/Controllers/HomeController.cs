using csFiddle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace csFiddle.Controllers
{
    public class HomeController : Controller
    {
        csFiddlerEntities context = new csFiddlerEntities();

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string userCode)
        {
            var code = new Code();
            code.id = Guid.NewGuid();
            code.code1 = userCode;
            code.version = 1;
            code.CreateDate = DateTime.Now;

            context.Codes.Add(code);
            context.SaveChanges();

            return RedirectToAction("Edit", "Home", new {id = code.id});
        }

        public ActionResult EditVersion(Guid id, int version)
        {
            var result = context.Codes.SingleOrDefault(c => c.id == id && c.version == version);
            if (result == null)
            {
                
                string errorMessage = string.Format("Code not found for id {0} and version {1}", id, version);                
                ViewBag.Message = errorMessage;
                return RedirectToAction("Error", "Home");
            }
            else
            {
                return RedirectToAction("Edit", "Home", new { id = result.id, version = result.version });   
            }  
        }

        public ActionResult Edit(Guid id)
        {
            var result = context.Codes.SingleOrDefault(c => c.id == id);
            if (result == null)
            {                
                result = new Code();
                result.code1 = string.Format("Code not found for id {0}", id);
                return View(result);
            }
            else
            {
                return View(result);    
            }            
        }

        [HttpPost]
        public ActionResult Update(Guid id, string userCode)
        {
            var result = context.Codes.SingleOrDefault(c => c.id == id);
            if (result == null)
            {                                
                string errorMessage = string.Format("Code not found for id {0}", id);
                ViewBag.Message = errorMessage;
                return RedirectToAction("Error", "Home");
            }
            else
            {
                result.version++;
                result.code1 = userCode;
                context.SaveChanges();
                return RedirectToAction("Edit", "Home", new { id = result.id, version = result.version });   
            }              
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
