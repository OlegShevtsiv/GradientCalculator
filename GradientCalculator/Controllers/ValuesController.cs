using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using GradientCalculator.Configs;
using GradientCalculator.Models;
using GradientCalculator.ViewModels;
using ImageController;
using Microsoft.AspNetCore.Mvc;

namespace GradientCalculator.Controllers
{
    public class ValuesController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangeCulture(ChangeLangVM model)
        {

            if (model.newCulture.ToLower().Equals("uk"))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(co.DefaultLang_UA);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(co.DefaultLang_UA);
            }
            else if (model.newCulture.ToLower().Equals("en"))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(co.Lang_EN);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(co.Lang_EN);
            }

            return LocalRedirect(RewrightUrlToNewLang(model.url, model.newCulture));
        }

        [NonAction]
        public static List<GCLanguage> GetAvLang(string current)
        {
            List<GCLanguage> langs = new List<GCLanguage>
                     {
                         new GCLanguage("uk", "UK"),
                         new GCLanguage("en", "EN")
                     };

            langs.RemoveAll(l => l.nativeLang.Equals(current.ToLower()));
            return langs;
        }

        [NonAction]
        public static string RewrightUrlToNewLang(string url, string newCulture)
        {
            if (!url.StartsWith("/"))
            {
                url = "/" + url;
            }

            if (url.StartsWith("/uk"))
            {
                url = url.Replace("/uk", "");
            }
            else if (url.StartsWith("/ua"))
            {
                url = url.Replace("/ua", "");
            }
            else if (url.StartsWith("/en"))
            {
                url = url.Replace("/en", "");
            }

            if (url.Contains("//"))
            {
                url = url.Replace("//", "/");
            }

            if (!url.StartsWith("/"))
            {
                url = "/" + url;
            }

            url = "/" + newCulture + url;

            return url;
        }


        //[Route("/t")]
        //public object test() 
        //{
        //    FileInfo fileInfo = new FileInfo(@"C:\Users\User\Desktop\Проги\GradientCalculator\GradientCalculator\wwwroot\img\flags\en.png");

        //    byte[] data = new byte[fileInfo.Length];

        //    using (FileStream fs = fileInfo.OpenRead())
        //    {
        //        fs.Read(data, 0, data.Length);
        //    }

        //    var res = Methods.ResizeImage(50, 20, data);

        //    using (var ms = new MemoryStream(res))
        //    {
        //        Image.FromStream(ms).Save(@"C:\Users\User\Desktop\Проги\GradientCalculator\GradientCalculator\wwwroot\img\flags\en1.png");
        //    }


        //    fileInfo = new FileInfo(@"C:\Users\User\Desktop\Проги\GradientCalculator\GradientCalculator\wwwroot\img\flags\uk.png");

        //    data = new byte[fileInfo.Length];

        //    using (FileStream fs = fileInfo.OpenRead())
        //    {
        //        fs.Read(data, 0, data.Length);
        //    }

        //    res = Methods.ResizeImage(50, 20, data);

        //    using (var ms = new MemoryStream(res))
        //    {
        //        Image.FromStream(ms).Save(@"C:\Users\User\Desktop\Проги\GradientCalculator\GradientCalculator\wwwroot\img\flags\uk1.png");
        //    }



        //    return null;
        //}
    }
}