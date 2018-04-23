<!DOCTYPE HTML>
<html>
<head>
    <title>@ViewBag.Title</title>
    @Html.DevExpress().GetStyleSheets(
        New StyleSheet With {.ExtensionSuite = ExtensionSuite.NavigationAndLayout},
        New StyleSheet With {.ExtensionSuite = ExtensionSuite.Editors},
        New StyleSheet With {.ExtensionSuite = ExtensionSuite.HtmlEditor},
        New StyleSheet With {.ExtensionSuite = ExtensionSuite.GridView}
     ) 
    @Html.DevExpress().GetScripts(
        New Script With {.ExtensionSuite = ExtensionSuite.NavigationAndLayout},
        New Script With {.ExtensionSuite = ExtensionSuite.HtmlEditor},
        New Script With {.ExtensionSuite = ExtensionSuite.GridView},
        New Script With {.ExtensionSuite = ExtensionSuite.Editors}
    ) 
</head>
<body>
    @RenderBody()
</body>
</html>