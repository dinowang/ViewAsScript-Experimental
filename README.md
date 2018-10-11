# Problem and design

Project you ASP.NET Core views to other site with zero effort.

(TBC)

# Usage

## Add MiddlewareFilter attribute with ViewAsJavaScriptPipeline to your targeted action

HomeController.cs

`[MiddlewareFilter(typeof(ViewAsJavaScriptPipeline))]`

```csharp
public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [MiddlewareFilter(typeof(ViewAsJavaScriptPipeline))]
    public IActionResult About()
    {
        ViewData["Message"] = "Your application description page.";

        return View();
    }
}
```

## Render view with &lt;script&gt; tag and targeted action url

For example, include `<script type="text/javascript" src="/Home/About"></script>` in Home\Index.cshtml

```html
@{
    ViewData["Title"] = "Home Page";
}

<div id="myCarousel" class="carousel slide" data-ride="carousel" data-interval="6000">
    ...
</div>

...

<div class="row">
    <div class="col-md-12">
        <script type="text/javascript" src="/Home/About"></script>
    </div>    
</div>
```

## Result

Home/About will render as JavaScript

```js
document.write("<!DOCTYPE html>\n<html>\n<head>\n    <meta charset=\"utf-8\" />\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\" />\n    <title>About - ViewAsScript</title>\n\n    \n        <link rel=\"stylesheet\" href=\"/lib/bootstrap/dist/css/bootstrap.css\" />\n        <link rel=\"stylesheet\" href=\"/css/site.css\" />\n    \n    \n</head>\n<body>\n    <nav class=\"navbar navbar-inverse navbar-fixed-top\">\n        <div class=\"container\">\n            <div class=\"navbar-header\">\n                <button type=\"button\" class=\"navbar-toggle\" data-toggle=\"collapse\" data-target=\".navbar-collapse\">\n                    <span class=\"sr-only\">Toggle navigation</span>\n                    <span class=\"icon-bar\"></span>\n                    <span class=\"icon-bar\"></span>\n                    <span class=\"icon-bar\"></span>\n                </button>\n                <a class=\"navbar-brand\" href=\"/\">ViewAsScript</a>\n            </div>\n            <div class=\"navbar-collapse collapse\">\n                <ul class=\"nav navbar-nav\">\n                    <li><a href=\"/\">Home</a></li>\n                    <li><a href=\"/Home/About\">About</a></li>\n                    <li><a href=\"/Home/Contact\">Contact</a></li>\n                </ul>\n            </div>\n        </div>\n    </nav>\n\n    \n\n\n\n    <div class=\"container body-content\">\n        <h2>About</h2>\n<h3>Your application description page.</h3>\n\n<p>Use this area to provide additional information.</p>\n\n        <hr />\n        <footer>\n            <p>&copy; 2018 - ViewAsScript</p>\n        </footer>\n    </div>\n\n    \n        <script src=\"/lib/jquery/dist/jquery.js\"></script>\n        <script src=\"/lib/bootstrap/dist/js/bootstrap.js\"></script>\n        <script src=\"/js/site.js?v=4q1jwFhaPaZgr8WAUSrux6hAuh0XDg9kPS3xIVq36I0\"></script>\n    \n    \n\n    \n</body>\n</html>\n");
```