using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MARK6.Models;

namespace MARK6.Controllers
{
    public class ImageController : Controller
    {
        // GET: Image
        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        // GET: Image
        [HttpPost]
        public ActionResult Add(Image image)
        {
            string fileName = Path.GetFileNameWithoutExtension(image.ImageFile.FileName);
            string extension = Path.GetExtension(image.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymssfff") + extension;
            image.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image"), fileName);
            image.ImageFile.SaveAs(fileName);

            using (DBModels dBModels = new DBModels())
            {
                try
                {
                    dBModels.Images.Add(image);
                    dBModels.SaveChanges();
                }
                catch (Exception ex)
                {
                    string errorMessage = "Error occurred: " + ex.Message;
                    if (ex.InnerException != null)
                    {
                        errorMessage += "\nInner Exception: " + ex.InnerException.Message;
                    }
                    errorMessage += "\nStack Trace: " + ex.StackTrace;

                    // Log or display the errorMessage for debugging purposes
                    Console.WriteLine(errorMessage);
                }

            }
            ModelState.Clear();
            return View();
        }
    }
}