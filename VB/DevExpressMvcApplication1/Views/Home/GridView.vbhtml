@Imports DevExpressMvcApplication1.Models
@ModelType System.Collections.IEnumerable
@Code
    Dim grid As GridViewExtension = Html.DevExpress().GridView( _
      Sub(settings)
              settings.Name = "gv"
              settings.KeyFieldName = "ID"
              settings.CallbackRouteValues = New With {.Controller = "Home", .Action = "InlineEditingPartial"}
              settings.SettingsEditing.AddNewRowRouteValues = New With {.Controller = "Home", .Action = "InlineEditingAddNewPartial"}
              settings.SettingsEditing.UpdateRowRouteValues = New With {.Controller = "Home", .Action = "InlineEditingUpdatePartial"}
              settings.SettingsEditing.DeleteRowRouteValues = New With {.Controller = "Home", .Action = "InlineEditingDeletePartial"}
              settings.SettingsEditing.Mode = GridViewEditingMode.EditFormAndDisplayRow
              settings.SettingsBehavior.ConfirmDelete = True

              settings.CommandColumn.Visible = True
              settings.CommandColumn.ShowNewButton = True
              settings.CommandColumn.ShowEditButton = True
              settings.CommandColumn.ShowDeleteButton = True

              settings.Columns.Add("FirstName")
              settings.Columns.Add("SecondName")
				
              settings.Columns.Add( _
                  Sub(column)
                          column.FieldName = "Description"
                          column.Caption = "NoDescriptiontes"
                          column.EditFormSettings.ColumnSpan = 2
                          column.EditFormSettings.CaptionLocation = ASPxColumnCaptionLocation.Top
                          
                          column.ColumnType = MVCxGridViewColumnType.Memo
                          CType(column.PropertiesEdit, MemoProperties).EncodeHtml = False
                          
                          column.SetEditItemTemplateContent( _
                              Sub(c)
                                      Dim dataObject As Person = If(c.Grid.IsNewRowEditing, New Person(), c.Grid.GetRow(c.VisibleIndex))
                                      Html.RenderPartial("HtmlEditor", dataObject)
                              End Sub)
                  End Sub)

              settings.PreRender = _
                              Sub(sender, e)
                                      CType(sender, MVCxGridView).StartEdit(1)
                              End Sub
      End Sub)

    If ViewData("EditError") IsNot Nothing Then
        grid.SetEditErrorText(CStr(ViewData("EditError")))
    End If
End Code

@grid.Bind(Model).GetHtml()
