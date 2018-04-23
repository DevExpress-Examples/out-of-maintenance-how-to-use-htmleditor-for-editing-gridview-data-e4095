Imports Microsoft.VisualBasic
Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Data
Imports System.Data.Linq
Imports System.Linq
Imports System.Web
Imports System.Web.UI


	Public NotInheritable Class NorthwindDataProvider
		Private Const NorthwindDataContextKey As String = "DXNorthwindDataContext"

		Private Sub New()
		End Sub
		Public Shared ReadOnly Property DB() As NorthwindDataContext
			Get
				If HttpContext.Current.Items(NorthwindDataContextKey) Is Nothing Then
					HttpContext.Current.Items(NorthwindDataContextKey) = New NorthwindDataContext()
				End If
				Return CType(HttpContext.Current.Items(NorthwindDataContextKey), NorthwindDataContext)
			End Get
		End Property

		Private Shared Function CalculateAveragePrice(ByVal categoryID As Integer) As Double
			Return CDbl(( _
						From product In DB.Products _
						Where product.CategoryID.Equals(categoryID) _
						Select product).Average(Function(s) s.UnitPrice))
		End Function
		Public Shared Function GetCategories() As IEnumerable
			Return _
				From category In DB.Categories _
				Select category
		End Function
		Public Shared Function GetCategoriesNames() As IEnumerable
			Return _
				From category In DB.Categories _
				Select category.CategoryName
		End Function
		Public Shared Function GetCategoriesAverage() As IEnumerable
			Return _
				From category In DB.Categories _
				Select New With {Key category.CategoryName, Key .AvgPrice = CalculateAveragePrice(category.CategoryID)}
		End Function
		Public Shared Function GetCustomers() As IEnumerable
			Return _
				From customer In DB.Customers _
				Select customer
		End Function
		Public Shared Function GetProducts() As IEnumerable
			Return _
				From product In DB.Products _
				Select product
		End Function
		Public Shared Function GetProducts(ByVal categoryName As String) As IEnumerable
			Return _
				From product In DB.Products _
				Join category In DB.Categories On product.CategoryID Equals category.CategoryID _
				Where category.CategoryName = categoryName _
				Select product
		End Function
		Public Shared Function GetEmployees() As IEnumerable
			Return _
				From employee In DB.Employees _
				Select employee
		End Function
		Public Shared Function GetEmployeePhoto(ByVal employeeId As Integer) As Binary
			Return ( _
					From employee In DB.Employees _
					Where employee.EmployeeID.Equals(employeeId) _
					Select employee.Photo).SingleOrDefault()
		End Function
		Public Shared Function GetEmployeeNotes(ByVal employeeId As Integer) As String
			Return ( _
					From employee In DB.Employees _
					Where employee.EmployeeID.Equals(employeeId) _
					Select employee.Notes).Single()
		End Function
		Public Shared Function GetOrders() As IEnumerable
			Return _
				From order In DB.Orders _
				Select order
		End Function
		Public Shared Function GetInvoices() As IEnumerable
			Return _
				From invoice In DB.Invoices _
				Join customer In DB.Customers On invoice.CustomerID Equals customer.CustomerID _
				Select New With {Key customer.CompanyName, Key invoice.City, Key invoice.Region, Key invoice.Country, Key invoice.UnitPrice, Key invoice.Quantity}
		End Function
		Public Shared Function GetFullInvoices() As IEnumerable
			Return _
				From invoice In DB.Invoices _
				Join customer In DB.Customers On invoice.CustomerID Equals customer.CustomerID _
				Join order In DB.Orders On invoice.OrderID Equals order.OrderID _
				Select New With {Key .SalesPerson = order.Employee.FirstName & " " & order.Employee.LastName, Key customer.CompanyName, Key invoice.Country, Key invoice.Region, Key invoice.OrderDate, Key invoice.ProductName, Key invoice.UnitPrice, Key invoice.Quantity}
		End Function
		Public Shared Function GetInvoices(ByVal customerID As String) As IEnumerable
			Return _
				From invoice In DB.Invoices _
				Where invoice.CustomerID = customerID _
				Select invoice
		End Function

		Public Shared Function GetEditableProducts() As IList(Of EditableProduct)
			Dim products As IList(Of EditableProduct) = CType(HttpContext.Current.Session("Products"), IList(Of EditableProduct))

			If products Is Nothing Then
				products = ( _
						From product In DB.Products _
						Select New EditableProduct With {.ProductID = product.ProductID, .ProductName = product.ProductName, .CategoryID = product.CategoryID, .QuantityPerUnit = product.QuantityPerUnit, .UnitPrice = product.UnitPrice, .UnitsInStock = product.UnitsInStock, .Discontinued = product.Discontinued}).ToList()
				HttpContext.Current.Session("Products") = products
			End If
			Return products
		End Function
		Public Shared Function GetEditableProduct(ByVal productID As Integer) As EditableProduct
			Return ( _
					From product In GetEditableProducts() _
					Where product.ProductID = productID _
					Select product).FirstOrDefault()
		End Function
		Public Shared Function GetNewEditableProductID() As Integer
			Dim lastProduct As EditableProduct = ( _
					From product In GetEditableProducts() _
					Select product).Last()
			Return If((lastProduct IsNot Nothing), lastProduct.ProductID + 1, 0)
		End Function
		Public Shared Sub DeleteProduct(ByVal productID As Integer)
			Dim product As EditableProduct = GetEditableProduct(productID)
			If product IsNot Nothing Then
				GetEditableProducts().Remove(product)
			End If
		End Sub
		Public Shared Sub InsertProduct(ByVal product As EditableProduct)
			Dim editProduct As New EditableProduct()
			editProduct.ProductID = GetNewEditableProductID()
			editProduct.ProductName = product.ProductName
			editProduct.CategoryID = product.CategoryID
			editProduct.QuantityPerUnit = product.QuantityPerUnit
			editProduct.UnitPrice = product.UnitPrice
			editProduct.UnitsInStock = product.UnitsInStock
			editProduct.Discontinued = product.Discontinued
			GetEditableProducts().Add(editProduct)
		End Sub
		Public Shared Sub UpdateProduct(ByVal product As EditableProduct)
			Dim editProduct As EditableProduct = GetEditableProduct(product.ProductID)
			If editProduct IsNot Nothing Then
				editProduct.ProductName = product.ProductName
				editProduct.CategoryID = product.CategoryID
				editProduct.QuantityPerUnit = product.QuantityPerUnit
				editProduct.UnitPrice = product.UnitPrice
				editProduct.UnitsInStock = product.UnitsInStock
				editProduct.Discontinued = product.Discontinued
			End If
		End Sub

		Public Shared Function GetEmployeesList() As IEnumerable
			Return _
				From employee In DB.Employees _
				Select New With {Key .ID = employee.EmployeeID, Key .Name = employee.LastName & " " & employee.FirstName}
		End Function
		Public Shared Function GetFirstEmployeeID() As Integer
			Return ( _
					From employee In DB.Employees _
					Select employee.EmployeeID).First()
		End Function
		Public Shared Function GetEmployee(ByVal employeeId As Integer) As Employee
			Return ( _
					From employee In DB.Employees _
					Where employeeId.Equals(employee.EmployeeID) _
					Select employee).Single()
		End Function
		Public Shared Function GetOrders(ByVal employeeID As Integer) As IEnumerable
			Return _
				From order In DB.Orders _
				Where order.EmployeeID.Equals(employeeID) _
				Join order_detail In DB.Order_Details On order.OrderID Equals order_detail.OrderID _
				Join customer In DB.Customers On order.CustomerID Equals customer.CustomerID _
				Select New With {Key order.OrderID, Key order.ShipName, Key order_detail.Quantity, Key order_detail.UnitPrice, Key customer.ContactName, Key customer.CompanyName, Key customer.City, Key customer.Address, Key customer.Phone, Key customer.Fax}
		End Function
	End Class

	Public Class EditableProduct
		Private privateProductID As Integer
		Public Property ProductID() As Integer
			Get
				Return privateProductID
			End Get
			Set(ByVal value As Integer)
				privateProductID = value
			End Set
		End Property

		Private privateProductName As String
		<Required(ErrorMessage := "Product Name is required"), StringLength(50, ErrorMessage := "Must be under 50 characters")> _
		Public Property ProductName() As String
			Get
				Return privateProductName
			End Get
			Set(ByVal value As String)
				privateProductName = value
			End Set
		End Property

		Private privateCategoryID? As Integer
		<Required(ErrorMessage := "Category is required")> _
		Public Property CategoryID() As Integer?
			Get
				Return privateCategoryID
			End Get
			Set(ByVal value? As Integer)
				privateCategoryID = value
			End Set
		End Property

		Private privateQuantityPerUnit As String
		<StringLength(100, ErrorMessage := "Must be under 100 characters")> _
		Public Property QuantityPerUnit() As String
			Get
				Return privateQuantityPerUnit
			End Get
			Set(ByVal value As String)
				privateQuantityPerUnit = value
			End Set
		End Property

		Private privateUnitPrice? As Decimal
		<Range(0, 10000, ErrorMessage := "Must be between 0 and 10000$")> _
		Public Property UnitPrice() As Decimal?
			Get
				Return privateUnitPrice
			End Get
			Set(ByVal value? As Decimal)
				privateUnitPrice = value
			End Set
		End Property

		Private privateUnitsInStock? As Short
		<Range(0, 1000, ErrorMessage := "Must be between 0 and 1000")> _
		Public Property UnitsInStock() As Short?
			Get
				Return privateUnitsInStock
			End Get
			Set(ByVal value? As Short)
				privateUnitsInStock = value
			End Set
		End Property

		Private discontinued_Renamed? As Boolean
		Public Property Discontinued() As Boolean?
			Get
				Return discontinued_Renamed
			End Get
			Set(ByVal value? As Boolean)
				discontinued_Renamed = If(value Is Nothing, False, value)
			End Set
		End Property
	End Class

