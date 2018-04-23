<!DOCTYPE HTML>
<html>
<head>
	<title>@ViewBag.Title</title>
	@Html.DevExpress().GetStyleSheets(New StyleSheet With {.ExtensionSuite = ExtensionSuite.NavigationAndLayout}, _
                                New StyleSheet With {.ExtensionSuite = ExtensionSuite.GridView}, _
								New StyleSheet With {.ExtensionSuite = ExtensionSuite.HtmlEditor}, _
                                New StyleSheet With {.ExtensionSuite = ExtensionSuite.Editors} _
                                )
	<link href="@Url.Content("~/Content/Site.css")" rel="stylesheet" type="text/css" />
	<script src="@Url.Content("~/Scripts/jquery-1.4.4.min.js")" type="text/javascript"></script>
	@Html.DevExpress().GetScripts(New Script With {.ExtensionSuite = ExtensionSuite.NavigationAndLayout}, _
                                New Script With {.ExtensionSuite = ExtensionSuite.GridView}, _
								New Script With {.ExtensionSuite = ExtensionSuite.HtmlEditor}, _
                                New Script With {.ExtensionSuite = ExtensionSuite.Editors} _
                                )
</head>
<body>
	@RenderBody()
</body>
</html>
