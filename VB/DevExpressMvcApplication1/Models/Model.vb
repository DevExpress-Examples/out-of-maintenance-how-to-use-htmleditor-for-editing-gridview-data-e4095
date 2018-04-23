Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.Web

Namespace DevExpressMvcApplication1.Models
	Public Class Repository
		Public Shared ReadOnly Property Persons() As IList(Of Person)
			Get
				If HttpContext.Current.Session("Persons") Is Nothing Then
					HttpContext.Current.Session("Persons") = GeneratePersons()
				End If
				Return DirectCast(HttpContext.Current.Session("Persons"), IList(Of Person))
			End Get
		End Property
		Public Shared Function GetPersonByID(ByVal id As Integer) As Person
			Return (
			    From person In Persons
			    Where person.ID = id
			    Select person).SingleOrDefault()
		End Function
		Public Shared Function GeneratePersonID() As Integer
			Return If(Persons.Count > 0, Persons.Last().ID + 1, 0)
		End Function
		Public Shared Sub InsertPerson(ByVal person As Person)
			If person IsNot Nothing Then
				person.ID = GeneratePersonID()
				Persons.Add(person)
			End If
		End Sub
		Public Shared Sub UpdatePerson(ByVal person As Person)
			Dim editablePerson As Person = GetPersonByID(person.ID)
			If editablePerson IsNot Nothing Then
				editablePerson.ID = person.ID
				editablePerson.FirstName = person.FirstName
				editablePerson.SecondName = person.SecondName
				editablePerson.Description = person.Description
			End If
		End Sub
		Public Shared Sub RemovePersonByID(ByVal id As Integer)
			Dim editablePerson As Person = GetPersonByID(id)
			If editablePerson IsNot Nothing Then
				Persons.Remove(editablePerson)
			End If
		End Sub
		Private Shared Function GeneratePersons() As IList(Of Person)
			Return New List(Of Person) From {
				New Person() With {.ID = 0, .FirstName = "Nick", .SecondName = "F", .Description = "<b>Admin</b>"},
				New Person() With {.ID = 1, .FirstName = "Jain", .SecondName = "K", .Description = "<i>User</i>"},
				New Person() With {.ID = 2, .FirstName = "Loren", .SecondName = "F", .Description = "<u>Guest</u>"},
				New Person() With {.ID = 3, .FirstName = "Mike", .SecondName = "N", .Description = "Undefined"}
			}
		End Function
	End Class

	Public Class Person
		Public Property ID() As Integer

		<Required(ErrorMessage := "First Name is required")>
		Public Property FirstName() As String
		Public Property SecondName() As String
		Public Property Description() As String
	End Class
End Namespace