@ModelType Employee 

@Html.DevExpress().HtmlEditor(Sub(settings) 
	settings.Name = "heFeatures"
	settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "HtmlEditorPartial", .employeeID = Model.EmployeeID}
	settings.Width = 550
End Sub).Bind(Model.Notes).GetHtml()
