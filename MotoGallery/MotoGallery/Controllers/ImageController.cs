using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MotoGallery.Models;
using ORM.Entity;
using ORM;


namespace MotoGallery.Controllers
{
    public class ImageController : Controller
    {

        private EntityModel db = new EntityModel();

        public ImageController()
        {
            var t = db.Images.Where(a => true).ToList();
            var u = db.Albums.Where(a => true).ToList();
        }

        //public JsonResult AddImageAjax(string fileName, string data/*, string extension=".jpeg", int albumId=0*/)
        //{
        //    var dataIndex = data.IndexOf("base64", StringComparison.Ordinal) + 7;
        //    var cleareData = data.Substring(dataIndex);
        //    var fileData = Convert.FromBase64String(cleareData);
        //    var bytes = fileData.ToArray();

        //    var path = GetPathToImg(fileName);
        //    using (var fileStream = System.IO.File.Create(path))
        //    {
        //        fileStream.Write(bytes, 0, bytes.Length);
        //        fileStream.Close();
        //    }

        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult AddImageAjax(string fileName, string description, string data, string albumName, string extension)
        {
            if (data == null || fileName == null || albumName == null || extension == null) return Json(false, JsonRequestBehavior.AllowGet);

            var dataIndex = data.IndexOf("base64", StringComparison.Ordinal) + 7;
            var cleareData = data.Substring(dataIndex);
            var fileData = Convert.FromBase64String(cleareData);
            var bytes = fileData.ToArray();

            var path = GetPathToImg($"{fileName}.{extension}");

            if (System.IO.File.Exists(path)) throw new FileLoadException("File alredy exist", $"{fileName}.{extension}");

            using (var fileStream = System.IO.File.Create(path))
            {
                fileStream.Write(bytes, 0, bytes.Length);
                fileStream.Close();
            }

            var album = db.Albums.FirstOrDefault(a => a.Name == albumName);
            if (album == null)
            {
                album = new Album()
                {
                    Name = albumName,
                    User = db.Users.FirstOrDefault() //Must be current user
                };
                db.Albums.Add(album);
                db.SaveChanges();
                album = db.Albums.FirstOrDefault(a => a.Name == albumName);
            }

            var tempImg = new Image()
            {
                Album = album,
                AlbumId = album.Id,
                CreationDate = DateTime.Now,
                Desc = description,
                Path = Path.Combine("Content", "img", $"{fileName}.{extension}"),
                UserId = db.Users.FirstOrDefault().Id,
                Name = fileName
            };
            db.Images.Add(tempImg);
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        //[HttpPost]
        //public JsonResult RemoveImage(string url)
        //{
        //    var imgName = Path.GetFileName(url);
        //    System.IO.File.Delete(GetPathToImg(imgName));
        //    return Json(true, JsonRequestBehavior.AllowGet);
        //}

        [HttpPost]
        public JsonResult RemoveImage(int id)
        {
            var image = db.Images.FirstOrDefault(i => i.Id == id);
            db.Images.Remove(image);
            System.IO.File.Delete(Path.Combine(Server.MapPath("~"), image.Path));
            db.SaveChanges();
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAlbums()
        {
            var albums = db.Albums.Where(a => true).ToList();

            List<Image> allImages = new List<Image>();
            foreach (Album album in albums)
            {
                if (album.Images != null)
                allImages.AddRange(album.Images);
            }

            albums.Add(new Album() { Id = 0, Name = "All Images", Images = allImages });

            var jsonAlbums = albums.Select(delegate (Album a)
              {
                  var ext = GetAlbumExtentions(a);

                  return new
                  {
                      Id = a.Id,
                      Name = a.Name,
                      Extensions = ext
                  };
              }).OrderBy(a=>a.Id).ToList();

     
           
            return Json(jsonAlbums, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetImages()
        {
            var imageFiles = db.Images.Where(z => true).ToList();
            var imges = imageFiles.Select(BuildImage);
            return Json(imges, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetImagesFromAlbum(int id)
        {
            if (id == 0)
            {
                return GetImages();
            }


            var alb = db.Albums.FirstOrDefault(a => a.Id == id);
            var imageFiles = alb.Images;
            if (imageFiles == null) return GetImages();
            var imges = imageFiles.Select(BuildImage);
            return Json(imges, JsonRequestBehavior.AllowGet);
        }



        private string GetPathToImg(string fileName)
        {
            var serverPath = Server.MapPath("~");
            return Path.Combine(serverPath, "Content", "img", fileName);
        }

        private ImageModel BuildImage(Image image)
        {
            var serverPath = Server.MapPath("~");
            ;
            var imageTemp = new ImageModel()
            {
                Id = image.Id,
                Url = Url.Content(Path.Combine("~", image.Path)),
                Name = image.Name,
                Extension = Path.GetExtension(image.Path),
                Date = image.CreationDate.Value.ToShortDateString(),
                Description= image.Desc
            };
            return imageTemp;
        }

        private List<string> GetAlbumExtentions(Album a)
        {
            List<string> temp = new List<string>(); ;

            if (a.Images != null) { 
                foreach (Image img in a.Images)
            {
                var ext = Path.GetExtension(img.Path);
                if (!temp.Contains(ext)) { temp.Add(ext); }
            };
            };
            return temp;
        }
    }

}