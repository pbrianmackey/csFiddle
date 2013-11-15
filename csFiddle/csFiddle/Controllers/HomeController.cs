using csFiddle.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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
            var rootNode = new Code();
            rootNode.id = Guid.NewGuid();
            rootNode.userCode = userCode;
            rootNode.version = 0;
            rootNode.CreateDate = DateTime.Now;
            rootNode.latest = false;
            context.Codes.Add(rootNode);            

            var editNode = CreateEditNode(rootNode.id, userCode, rootNode, rootNode: true);

            Save();


            return RedirectToAction("Edit", "Home", new { parentId = editNode.parentId, version = editNode.version });
        }

        public ActionResult Edit(Guid parentId, int version)
        {
            var result = context.Codes.SingleOrDefault(c => c.parentId == parentId && c.version == version);
            if (result == null)
            {
                string errorMessage = string.Format("Code not found for parentId {0} and version {1}", parentId, version);                
                ViewBag.Message = errorMessage;
                return RedirectToAction("Error", "Home");
            }
            else
            {
                //return RedirectToAction("Edit", "Home", new { id = result.id, version = result.version });   
                return View(result);
            }  
        }

        //public ActionResult Edit(Guid id)
        //{
        //    var result = context.Codes.SingleOrDefault(c => c.id == id);
        //    if (result == null)
        //    {                
        //        result = new Code();
        //        result.userCode = string.Format("Code not found for id {0}", id);
        //        return View(result);
        //    }
        //    else
        //    {
        //        return View(result);    
        //    }            
        //}

        [HttpPost]
        public ActionResult Update(Guid id, Guid parentId, int version, string userCode)
        {
            var oldVersion = context.Codes.SingleOrDefault(c => c.id == id && c.version == version);
            if (oldVersion == null)
            {                                
                string errorMessage = string.Format("Code not found for id {0}", id);
                ViewBag.Message = errorMessage;
                return RedirectToAction("Error", "Home");
            }
            else
            {
                var result = CreateEditNode((Guid)oldVersion.parentId, userCode, oldVersion);                
                Save();
                return RedirectToAction("Edit", new {result.parentId, result.version});
            }            
        }

        public ActionResult List()
        {            
            List<Code> result = context.Codes.Where(c => c.latest).OrderByDescending(c => c.CreateDate).ToList();
            return View(result);
        }

        private Code CreateEditNode(Guid parentId, string userCode, Code currentCodeInView, bool rootNode = false)
        {
            var editNode = new Code();
            editNode.id = Guid.NewGuid();
            editNode.userCode = userCode;        
            if (!rootNode)
            {
                var latestCode = GetLatestCode(currentCodeInView);
                latestCode.latest = false;
            }
            
            editNode.version = GetLatestVersion(currentCodeInView);
            editNode.CreateDate = DateTime.Now;
            editNode.parentId = parentId;
            editNode.latest = true;                        

            context.Codes.Add(editNode);

            return editNode;
        }

        private int GetLatestVersion(Code currentCodeInView)
        {
            bool isParentCode = currentCodeInView.parentId == null;
            if (isParentCode)
                return 1;

            int newVersion = GetLatestCode(currentCodeInView).version + 1;
            return newVersion;            
        }

        private Code GetLatestCode(Code currentCodeInView)
        {
            return context.Codes.Single(c => c.parentId == currentCodeInView.parentId && c.latest);
        }

        private void Save()
        {
            try
            {
                context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
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
