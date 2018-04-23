Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Linq
Imports System.Web.UI

Public MustInherit Class ItemsData
	Implements IHierarchicalEnumerable, IEnumerable
	Private data As IEnumerable

	Public Sub New()
		Me.data = GetData()
	End Sub

	Public Function GetEnumerator() As IEnumerator Implements System.Collections.IEnumerable.GetEnumerator
		Return Me.data.GetEnumerator()
	End Function
	Public Function GetHierarchyData(ByVal enumeratedItem As Object) As IHierarchyData Implements IHierarchicalEnumerable.GetHierarchyData
		Return CType(enumeratedItem, IHierarchyData)
	End Function

	Public MustOverride Function GetData() As IEnumerable
End Class

Public Class ItemData
	Implements IHierarchyData
	Private privateText As String
	Public Property Text() As String
		Get
			Return privateText
		End Get
		Protected Set(ByVal value As String)
			privateText = value
		End Set
	End Property
	Private privateNavigateUrl As String
	Public Property NavigateUrl() As String
		Get
			Return privateNavigateUrl
		End Get
		Protected Set(ByVal value As String)
			privateNavigateUrl = value
		End Set
	End Property

	Public Sub New(ByVal text As String, ByVal navigateUrl As String)
		Text = text
		NavigateUrl = navigateUrl
	End Sub

	' IHierarchyData
	Private ReadOnly Property IHierarchyData_HasChildren() As Boolean Implements IHierarchyData.HasChildren
		Get
			Return HasChildren()
		End Get
	End Property
	Private ReadOnly Property IHierarchyData_Item() As Object Implements IHierarchyData.Item
		Get
			Return Me
		End Get
	End Property
	Private ReadOnly Property Path() As String Implements IHierarchyData.Path
		Get
			Return NavigateUrl
		End Get
	End Property
	Private ReadOnly Property Type() As String Implements IHierarchyData.Type
		Get
			Return Me.GetType().ToString()
		End Get
	End Property
	Private Function GetChildren() As IHierarchicalEnumerable Implements IHierarchyData.GetChildren
		Return CreateChildren()
	End Function
	Private Function GetParent() As IHierarchyData Implements IHierarchyData.GetParent
		Return Nothing
	End Function

	Protected Overridable Function HasChildren() As Boolean
		Return False
	End Function
	Protected Overridable Function CreateChildren() As IHierarchicalEnumerable
		Return Nothing
	End Function
End Class


Public Class CategoriesData
	Inherits ItemsData
	Public Overrides Function GetData() As IEnumerable
		Return _
			From category In NorthwindDataProvider.DB.Categories _
			Select New CategoryData(category)
	End Function
End Class

Public Class CategoryData
	Inherits ItemData
	Private privateCategory As Category
	Public Property Category() As Category
		Get
			Return privateCategory
		End Get
		Protected Set(ByVal value As Category)
			privateCategory = value
		End Set
	End Property

	Public Sub New(ByVal category As Category)
		MyBase.New(category.CategoryName, "?CategoryID=" & category.CategoryID)
		Category = category
	End Sub

	Protected Overrides Overloads Function HasChildren() As Boolean
		Return True
	End Function
	Protected Overrides Function CreateChildren() As IHierarchicalEnumerable
		Return New ProductsData(Category.CategoryID)
	End Function
End Class

Public Class ProductsData
	Inherits ItemsData
	Private privateCategoryID As Integer
	Public Property CategoryID() As Integer
		Get
			Return privateCategoryID
		End Get
		Protected Set(ByVal value As Integer)
			privateCategoryID = value
		End Set
	End Property

	Public Sub New(ByVal categoryID As Integer)
		MyBase.New()
		CategoryID = categoryID
	End Sub

	Public Overrides Function GetData() As IEnumerable
		Return _
			From product In NorthwindDataProvider.DB.Products _
			Where product.CategoryID.Equals(CategoryID) _
			Select New ProductData(product)
	End Function
End Class

Public Class ProductData
	Inherits ItemData
	Public Sub New(ByVal product As Product)
		MyBase.New(product.ProductName, "?CategoryID=" & product.CategoryID & "&ProductID=" & product.ProductID)
	End Sub
End Class