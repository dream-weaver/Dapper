using Dapper.Entity;
using Dapper.Services;
using Dapper.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dapper.Web.Controllers
{
    public class ContactsController : Controller
    {
        private readonly IContactContribRepository repository;

        public ContactsController(IContactContribRepository repository)
        {
            this.repository = repository;
        }

        // GET: Contacts
        public ActionResult Index()
        {
            var contacts = repository.GetAll();
            return View(contacts); 
        }

        // GET: Contacts/Details/5
        public ActionResult Details(int id)
        {
            var vm = repository.Find(id);
            if (vm!=null)
            {
                return View(vm);
            }
            return View();
        }

        // GET: Contacts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contacts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection, Contact model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (repository.Add(model))
                    {
                        TempData["Message"] = "Contact Saved Successfully!";
                        return RedirectToAction("Index", "Contacts");
                    }
                    return View();
                }
                return View();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }                
        }

        // GET: Contacts/Edit/5
        public ActionResult Edit(int id)
        {
            var vm = repository.Find(id);
            if (vm != null)
            {
                return View(vm);
            }
            return View();
        }

        // POST: Contacts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection, Contact contact)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (repository.Update(contact))
                    {
                        TempData["Message"] = "Contact Updated Successfully!";
                        return RedirectToAction("Index", "Contacts");
                    }
                    return View();
                }
                return View();
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        // GET: Contacts/Delete/5
        public ActionResult Delete(int id)
        {
            var vm = repository.Find(id);
            if (vm != null)
            {
                return View(vm);
            }
            return View();
        }

        // POST: Contacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                repository.Remove(id);
                TempData["Message"] = "Contact Deleted Successfully!";
                return RedirectToAction("Index", "Contacts");
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
    }
}