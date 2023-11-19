using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MARK6.Models;
using PagedList;

namespace MARK6.Controllers
{
    public class MediaGalleryController : Controller
    {
        private MARK6DBEntities db = new MARK6DBEntities();

        // GET: MediaGallery
        public ActionResult Index(string mediaType, int? page)
        {
            ViewBag.MediaType = mediaType;
            List<MediaGallery> mediaGalleries;
            mediaGalleries = db.MediaGalleries.Where(s => s.MediaType == mediaType).ToList();

            // Set the page size
            int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["pageSize"]);

            // Get the page number from the query string or default to 1
            int pageNumber = (page ?? 1);

            // Create a paginated list
            var pagedData = mediaGalleries.ToPagedList(pageNumber, pageSize);
            return View(pagedData);
        }

        // GET: MediaGallery/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaGallery mediaGallery = db.MediaGalleries.Find(id);
            if (mediaGallery == null)
            {
                return HttpNotFound();
            }
            return View(mediaGallery);
        }

        // GET: MediaGallery/Create
        public ActionResult Create(string mediaType)
        {
            ViewBag.MediaType = mediaType;
            return PartialView();
        }

        // POST: MediaGallery/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create( MediaGallery mediaGallery)
        {
            if (ModelState.IsValid)
            {
                string fileName = Path.GetFileNameWithoutExtension(mediaGallery.MediaFile.FileName.Replace(" ",""));
                string extension = Path.GetExtension(mediaGallery.MediaFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymssfff") + extension;
                mediaGallery.MediaType = GetMediaType(extension).ToString();
                mediaGallery.MediaPath = "~/MediaGallery/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/MediaGallery"), fileName);
                mediaGallery.MediaFile.SaveAs(fileName);

                db.MediaGalleries.Add(mediaGallery);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(mediaGallery);  
        }

        // GET: MediaGallery/Edit/5

        [Route("MediaGallery/Edit/{id}")]
        public ActionResult Edit(int? id,string mediaType)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaGallery mediaGallery = db.MediaGalleries.Find(id);
            if (mediaGallery == null)
            {
                return HttpNotFound();
            }
            ViewBag.MediaType = mediaType;
            return View(mediaGallery);
        }

        // POST: MediaGallery/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MediaGallery mediaGallery)
        {
            if (ModelState.IsValid)
            {

                MediaGallery existingMediaFile = db.MediaGalleries.AsNoTracking().Where(s => s.MediaId == mediaGallery.MediaId).FirstOrDefault();

                //Delete Existing File
                string absoultePath = Server.MapPath(existingMediaFile.MediaPath);
                System.IO.File.Delete(absoultePath);

                //Add New File
                string fileName = Path.GetFileNameWithoutExtension(mediaGallery.MediaFile.FileName.Replace(" ", ""));
                string extension = Path.GetExtension(mediaGallery.MediaFile.FileName);
                fileName = fileName + DateTime.Now.ToString("yymssfff") + extension;
                mediaGallery.MediaType = GetMediaType(extension).ToString();
                mediaGallery.MediaPath = "~/MediaGallery/" + fileName;
                fileName = Path.Combine(Server.MapPath("~/MediaGallery"), fileName);
                mediaGallery.MediaFile.SaveAs(fileName);

                db.Entry(mediaGallery).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return PartialView(mediaGallery);
        }

        // GET: MediaGallery/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MediaGallery mediaGallery = db.MediaGalleries.Find(id);
            if (mediaGallery == null)
            {
                return HttpNotFound();
            }
            return PartialView(mediaGallery);
        }

        // POST: MediaGallery/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            

            MediaGallery mediaGallery = db.MediaGalleries.Find(id);

            //Delete Existing File
            string absoultePath = Server.MapPath(mediaGallery.MediaPath);
            System.IO.File.Delete(absoultePath);

            db.MediaGalleries.Remove(mediaGallery);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Route("MediaGallery/Download/{id}")]
        public ActionResult Download(int? id)
        {
            MediaGallery existingMediaFile = db.MediaGalleries.AsNoTracking().Where(s => s.MediaId == id).FirstOrDefault();

            //Delete Existing File
            string absoultePath = Server.MapPath(existingMediaFile.MediaPath);
            // Assuming you have an "Images" folder in your project

            // Check if the file exists
            if (System.IO.File.Exists(absoultePath))
            {
                var filename = Path.GetFileName(absoultePath);
                // Set the Content-Disposition header to force the browser to download the file
                return File(absoultePath, "image/jpeg", filename);
            }
            else
            {
                // If the file doesn't exist, return a 404 Not Found response
                return HttpNotFound();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Private Methods
        private FileType GetMediaType(string fileExtension)
		{
            string[] imageExtensions = ConfigurationManager.AppSettings["imageExtensions"]
                                            .Split(',');
            string[] videoExtensions = ConfigurationManager.AppSettings["videoExtensions"]
                                            .Split(',');

            if (imageExtensions.Contains(fileExtension.ToLower()))
            {
                return FileType.Photo;
            }
            else if (videoExtensions.Contains(fileExtension.ToLower()))
            {
                return FileType.Video;
            }
            else
            {
                return FileType.Unknown;
            }
        }

        private enum FileType
        {
            Photo,
            Video,
            Unknown
        }
        #endregion
    }


}
