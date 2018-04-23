Imports System
Imports System.Web.Mvc
Imports DevExpress.Web.Mvc
Imports DevExpressMvcApplication1.Models

Namespace DevExpressMvcApplication1.Controllers
	Public Class HomeController
		Inherits Controller

		Public Function Index() As ActionResult
			Return View(Repository.Persons)
		End Function
		Public Function InlineEditingPartial() As ActionResult
			Return PartialView("GridView", Repository.Persons)
		End Function
		Public Function HtmlEditorPartial(ByVal personID As String) As ActionResult
			Dim persID As Integer = Int32.Parse(personID)
			Dim pers As Person
			If persID > 0 Then
				pers = Repository.GetPersonByID(persID)
			Else
				pers = New Person()
			End If

			Return PartialView("HtmlEditor", pers)
		End Function
		<HttpPost>
		Public Function InlineEditingAddNewPartial(ByVal person As Person) As ActionResult
			If ModelState.IsValid Then
				Try
					Repository.InsertPerson(person)
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			Else
                ViewData("EditError") = "Please, correct all errors."
                ViewData("DescriptionValue") = person.Description
            End If
			Return PartialView("GridView", Repository.Persons)
		End Function
		<HttpPost>
		Public Function InlineEditingUpdatePartial(ByVal person As Person) As ActionResult
			If ModelState.IsValid Then
				Try
					Repository.UpdatePerson(person)
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			Else
                ViewData("EditError") = "Please, correct all errors."
                ViewData("DescriptionValue") = person.Description
            End If

			Return PartialView("GridView", Repository.Persons)
		End Function
		<HttpPost>
		Public Function InlineEditingDeletePartial(ByVal id As Integer) As ActionResult
			If id > 0 Then
				Try
					Repository.RemovePersonByID(id)
				Catch e As Exception
					ViewData("EditError") = e.Message
				End Try
			End If
			Return PartialView("GridView", Repository.Persons)
		End Function
	End Class
End Namespace