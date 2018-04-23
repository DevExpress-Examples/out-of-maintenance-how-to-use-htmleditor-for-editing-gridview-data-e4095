@Code
	ViewBag.Title = "This example demonstrates how to use HtmlEditor to edit GridView row data"
End Code

@ModelType System.Collections.IEnumerable
@Html.Partial("GridView", Model)