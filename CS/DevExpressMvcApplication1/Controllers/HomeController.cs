using System;
using System.Web.Mvc;
using DevExpress.Web.Mvc;
using DevExpressMvcApplication1.Models;

namespace DevExpressMvcApplication1.Controllers {
    public class HomeController: Controller {
        public ActionResult Index() {
            return View(Repository.Persons);
        }
        public ActionResult InlineEditingPartial() {
            return PartialView("GridView", Repository.Persons);
        }
        public ActionResult HtmlEditorPartial(string personID) {
            int persID = Int32.Parse(personID);
            Person pers = persID > 0
                ? pers = Repository.GetPersonByID(persID)
                : new Person();

            return PartialView("HtmlEditor", pers);
        }
        [HttpPost]
        public ActionResult InlineEditingAddNewPartial(Person person) {
            if (ModelState.IsValid) {
                try {
                    Repository.InsertPerson(person);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Please, correct all errors.";
                ViewData["DescriptionValue"] = person.Description;
            }
            return PartialView("GridView", Repository.Persons);
        }
        [HttpPost]
        public ActionResult InlineEditingUpdatePartial(Person person) {
            if (ModelState.IsValid) {
                try {
                    Repository.UpdatePerson(person);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            } else {
                ViewData["EditError"] = "Please, correct all errors.";
                ViewData["DescriptionValue"] = person.Description;
            }

            return PartialView("GridView", Repository.Persons);
        }
        [HttpPost]
        public ActionResult InlineEditingDeletePartial(int id) {
            if (id > 0) {
                try {
                    Repository.RemovePersonByID(id);
                } catch (Exception e) {
                    ViewData["EditError"] = e.Message;
                }
            }
            return PartialView("GridView", Repository.Persons);
        }
    }
}