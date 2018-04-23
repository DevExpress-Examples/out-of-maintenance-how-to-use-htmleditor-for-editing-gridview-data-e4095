@Imports DevExpressMvcApplication1.Models
@ModelType Person
    
@Code
    If ViewData("DescriptionValue") IsNot Nothing Then
        Model.Description = ViewData("DescriptionValue").ToString()
    End If
End Code

@Html.DevExpress().HtmlEditor( _
    Sub(settings)
            settings.Name = "Description"
            settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "HtmlEditorPartial", .personID = Model.ID}
            settings.Width = 550
    End Sub).Bind(Model.Description).GetHtml()