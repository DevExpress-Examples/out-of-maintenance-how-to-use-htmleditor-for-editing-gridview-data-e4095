@ModelType System.Collections.IEnumerable

@Code		 
	Dim grid = Html.DevExpress().GridView(Sub(settings) 
			settings.Name = "gvEditing"
	settings.KeyFieldName = "EmployeeID"
	settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "InlineEditingPartial"}
	settings.SettingsEditing.AddNewRowRouteValues = New With {.Controller = "Home", .Action = "InlineEditingAddNewPartial"}
	settings.SettingsEditing.UpdateRowRouteValues = New With {.Controller = "Home", .Action = "InlineEditingUpdatePartial"}
	settings.SettingsEditing.DeleteRowRouteValues = New With {.Controller = "Home", .Action = "InlineEditingDeletePartial"}
	settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow
	settings.SettingsBehavior.ConfirmDelete = True
	settings.CommandColumn.Visible = True
	settings.CommandColumn.NewButton.Visible = True
	settings.CommandColumn.DeleteButton.Visible = True
	settings.CommandColumn.EditButton.Visible = True
	settings.Columns.Add("LastName")
	settings.Columns.Add("FirstName")
	settings.Columns.Add("Title")
	settings.Columns.Add("BirthDate")
	settings.Columns.Add("HireDate")
	settings.Columns.Add("Address")
	settings.Columns.Add("HomePhone")
	settings.Columns.Add(Sub(column) 
		column.FieldName = "Notes"
		column.Caption = "Notes"
		column.EditFormSettings.ColumnSpan = 2
		column.EditFormSettings.CaptionLocation = ASPxColumnCaptionLocation.Top
		column.ColumnType = MVCxGridViewColumnType.Memo
		column.SetEditItemTemplateContent(Sub(c) 
			Dim dataObject = If(c.Grid.IsNewRowEditing, New Employee(), c.Grid.GetRow(c.VisibleIndex))
			Html.RenderPartial("HtmlEditor", dataObject)
			End Sub)
		End Sub)
	settings.PreRender = Sub(sender, e) 
			CType(sender, MVCxGridView).StartEdit(1)
		End Sub
	End Sub)
	
	

	If ViewData("EditError") IsNot Nothing Then
		grid.SetEditErrorText(CStr(ViewData("EditError")))
	End If
End Code
@grid.Bind(Model).GetHtml()
