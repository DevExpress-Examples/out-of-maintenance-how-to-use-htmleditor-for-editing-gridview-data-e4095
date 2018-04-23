Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Web.Mvc
Imports DevExpress.Web.Mvc

Namespace DevExpressMvcApplication1.Controllers
	Public Class HomeController
		Inherits Controller
		Public Function Index() As ActionResult
			ViewBag.Message = "Welcome to DevExpress Extensions for ASP.NET MVC!"

			Return View(NorthwindDataProvider.GetEmployees())
		End Function
		Public Function InlineEditingPartial() As ActionResult
			Return PartialView("GridView", NorthwindDataProvider.GetEmployees())
		End Function
		Public Function HtmlEditorPartial(ByVal employeeID As String) As ActionResult
			Dim empId As Integer = Int32.Parse(employeeID)
			Dim emp As Employee = If(empId > 0, NorthwindDataProvider.GetEmployee(empId), New Employee())

			Return PartialView("HtmlEditor", emp)
		End Function
		<HttpPost> _
		Public Function InlineEditingAddNewPartial(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal employee As Employee) As ActionResult
			employee.Notes = HtmlEditorExtension.GetHtml("heFeatures")
			If ModelState.IsValid Then
				Try
					'update a database
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			Else
				ViewData("EditError") = "Please, correct all errors."
			End If
			Return PartialView("GridView", NorthwindDataProvider.GetEmployees())
		End Function
		<HttpPost> _
		Public Function InlineEditingUpdatePartial(<ModelBinder(GetType(DevExpressEditorsBinder))> ByVal employee As Employee) As ActionResult
			employee.Notes = HtmlEditorExtension.GetHtml("heFeatures")
			If ModelState.IsValid Then
				Try
					'update a database
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			Else
				ViewData("EditError") = "Please, correct all errors."
			End If

			Return PartialView("GridView", NorthwindDataProvider.GetEmployees())
		End Function
		<HttpPost> _
		Public Function InlineEditingDeletePartial(ByVal employeeID As Integer) As ActionResult
			If employeeID > 0 Then
				Try
					'update a database
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			End If
			Return PartialView("GridView", NorthwindDataProvider.GetEmployees())
		End Function
	End Class
End Namespace
