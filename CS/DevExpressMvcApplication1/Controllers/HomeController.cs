using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace DevExpressMvcApplication1.Controllers {
	public class HomeController : Controller {
		public ActionResult Index() {
			ViewBag.Message = "Welcome to DevExpress Extensions for ASP.NET MVC!";

			return View(NorthwindDataProvider.GetEmployees());
		}
		public ActionResult InlineEditingPartial() {
			return PartialView("GridView", NorthwindDataProvider.GetEmployees());
		}
		public ActionResult HtmlEditorPartial(string employeeID) {			
			int empId= Int32.Parse(employeeID);
			Employee emp = empId> 0 
				? emp = NorthwindDataProvider.GetEmployee(empId)
				: new Employee();
			
			return PartialView("HtmlEditor",  emp);
		}		
		[HttpPost]
		public ActionResult InlineEditingAddNewPartial([ModelBinder(typeof(DevExpressEditorsBinder))] Employee employee) {
                                                employee.Notes = HtmlEditorExtension.GetHtml("heFeatures");
			if (ModelState.IsValid) {
				try {
					//update a database
				}
				catch (Exception e) {
					ViewData["EditError"] = e.Message;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";
			return PartialView("GridView", NorthwindDataProvider.GetEmployees());
		}
		[HttpPost]
		public ActionResult InlineEditingUpdatePartial([ModelBinder(typeof(DevExpressEditorsBinder))] Employee employee) {
                                                employee.Notes = HtmlEditorExtension.GetHtml("heFeatures");
			if (ModelState.IsValid) {
				try {
					//update a database
				}
				catch (Exception e) {
					ViewData["EditError"] = e.Message;
				}
			}
			else
				ViewData["EditError"] = "Please, correct all errors.";

			return PartialView("GridView", NorthwindDataProvider.GetEmployees());
		}
		[HttpPost]
		public ActionResult InlineEditingDeletePartial(int employeeID) {
			if (employeeID > 0) {
				try {
					//update a database
				}
				catch (Exception e) {
					ViewData["EditError"] = e.Message;
				}
			}
			return PartialView("GridView", NorthwindDataProvider.GetEmployees());
		}
	}
}
