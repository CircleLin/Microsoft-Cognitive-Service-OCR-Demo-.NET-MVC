using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using textmvc.Models;
using System.IO;
using System.Threading.Tasks;

namespace textmvc.Controllers
{
    public class HomeController : Controller
    {        
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Index(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if(file.ContentType == "image/jpeg" || file.ContentType == "image/png" || file.ContentType == "image/gif")
                {                 
                    //path combine
                    var filename = Path.GetFileName(file.FileName);
                    var path = Path.Combine(Server.MapPath("~/FileUpload"), filename);

                    //check if file exist
                    if (!System.IO.File.Exists(path))
                    {
                        file.SaveAs(path);
                    }                   

                    TextHandler textHandler = new TextHandler(path);

                    //Send Request to Computer Vision API by TextHandler
                    TextJson data = await textHandler.MakeORCRequest().ConfigureAwait(false);

                    if (data == null || data.regions.Count() == 0)
                    {
                        TempData["message"] = "無法辨識出文字 \n Can not recognize text";
                        return View();
                    }

                    var lines = data.regions.FirstOrDefault().lines;
                    string texts = string.Empty;

                    foreach (var line in lines)
                    {
                        foreach (var word in line.words)
                        {
                            texts += word.text + " ";
                        }
                    }

                    ViewBag.texts = texts;
                    return View();                  
                }
                else
                {
                    TempData["message"] = "請上傳jpeg、png或gif格式的圖檔";
                }
            }
            TempData["message"] = "請上傳圖檔";
            return View();
        }
    }
}