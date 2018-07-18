using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FashionableFitandFlawless.Models;
using PagedList;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace FashionableFitandFlawless.Controllers
{
   
    public class ItemsController : Controller
    {
        private FashionDb db = new FashionDb();

        // GET: Items

        [Authorize(Roles ="President")]
        public ActionResult Index()
        {
            return View(db.item.ToList());
        }

        public ActionResult DisplayItem(string product, int? page)
        {
            var products = db.item.AsQueryable();

            var strings = Request.QueryString.AllKeys.ToString();

            ViewBag.Item = product;

            var pageNumber = page ?? 1;
            var pageSize = 60;


            if (!string.IsNullOrEmpty(product))
            {

                ViewBag.Item = product.ToUpper() ?? strings;

                products = products.OrderByDescending(f => f.ItemId).Where(f => f.Category.Contains(product)
          || f.Season.Contains(product) || f.SubCategory.Contains(product) || f.Style.Contains(product) || f.Style2.Contains(product));
            }
            else
            {
                products = products.OrderByDescending(f => f.ItemId).Where(f => f.Category.Contains(product)
            || f.Season.Contains(product) || f.SubCategory.Contains(product) || f.Style.Contains(product) || f.Style2.Contains(product));
            }
            
            return View("_MainView", products.ToPagedList(pageNumber, pageSize));
        }


        public JsonResult GetProduct(string term)
        {
            List<string> product;

            product = db.subCategory.Where(x => x.SubName.Contains(term)).Select(y => y.SubName).Distinct().ToList();

            // var furniture = db.furnitures.AsQueryable();

            return Json(product, JsonRequestBehavior.AllowGet);
        }


        // GET: Items/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.item.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // GET: Items/Create
        [Authorize(Roles = "President")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "President")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ItemId,Name,ImagePath,ImageByte,Url,Style,Style2,Price,Category,Season,SubCategory,Shipping,Note")] Items items, HttpPostedFileBase images)
        {
            if (!ModelState.IsValid)
            {
                //db.item.Add(items);
                //db.SaveChanges();
                return View("Create");
            }
            else
            {
                if (images != null)
                {
                    items.ImagePath = images.ContentType;

                    var fileExtension = Path.GetExtension(images.FileName).ToLower();
                    var smallImage = Resize(images.InputStream, 250, 250, fileExtension);

                    items.ImageByte = new byte[smallImage.Length];
                    smallImage.Read(items.ImageByte, 0, (int)smallImage.Length);
                }

                db.item.Add(items);
                db.SaveChanges();
                ViewBag.message = "item Successfully saved!";

                ModelState.Clear();
                return View("Create");
            }

        }

        public Stream Resize(Stream input, int width, int height, string fileExtension)
        {
            var pic = new Bitmap(input);
            var smallPic = new Bitmap(width, height);

            //smallPic.SetResolution(pic.HorizontalResolution, pic.VerticalResolution);

            ImageFormat savedFileFormat = default(ImageFormat);

            switch (fileExtension)
            {
                case ".jpg":
                case ".jpeg":
                    savedFileFormat = ImageFormat.Jpeg;
                    break;
                case ".jxr":
                    savedFileFormat = ImageFormat.Jpeg;
                    break;
                case ".png":
                    savedFileFormat = ImageFormat.Png;
                    break;
                default:
                    savedFileFormat = ImageFormat.Jpeg;
                    break;
            }

            MemoryStream output = new MemoryStream();
            using (Graphics g = Graphics.FromImage(smallPic))
            {
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.SmoothingMode = SmoothingMode.HighSpeed;

                // Figure out the ratio
                double ratioX = (double)width / (double)pic.Width;
                double ratioY = (double)height / (double)pic.Height;
                // use whichever multiplier is smaller
                double ratio = ratioX < ratioY ? ratioX : ratioY;

                // now we can get the new height and width
                int newHeight = Convert.ToInt32(pic.Height * ratio);
                int newWidth = Convert.ToInt32(pic.Width * ratio);

                // Now calculate the X,Y position of the upper-left corner 
                // (one of these will always be zero)
                int posX = Convert.ToInt32((width - (pic.Width * ratio)) / 2);
                int posY = Convert.ToInt32((height - (pic.Height * ratio)) / 2);

                g.Clear(Color.White); // white padding
                g.DrawImage(pic, posX, posY, newWidth, newHeight);

                smallPic.Save(output, savedFileFormat);

                output.Position = 0;
            }

            return output;
        }

        public FileContentResult GetImage(int id)
        {
            Items furniture = db.item.Find(id);

            if (furniture != null)
            {
                return File(furniture.ImageByte, furniture.ImagePath);
            }
            else
            {
                return null;
            }
        }

        // GET: Items/Edit/5
        [Authorize(Roles = "President")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.item.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "President")]
        public ActionResult Edit([Bind(Include = "ItemId,Name,ImagePath,ImageByte,Url,Style,Style2,Price,Category,Season,SubCategory,Shipping,Note")] Items items, HttpPostedFileBase images)
        {
            if (!ModelState.IsValid)
            {
                return View(items);
                //db.Entry(items).State = EntityState.Modified;
                //db.SaveChanges();
                //return RedirectToAction("Index");
            }
            else
            {
                var oldEntry = db.item.Find(items.ItemId);
                oldEntry.UpdateFromExisting(items);

                if (images != null)
                {
                    oldEntry.ImagePath = images.ContentType;
                    

                    var fileExtension = Path.GetExtension(images.FileName).ToLower();
                    var smallImage = Resize(images.InputStream, 250, 250, fileExtension);
                    oldEntry.ImageByte = new byte[smallImage.Length];
                    
                    smallImage.Read(oldEntry.ImageByte, 0, (int)smallImage.Length);
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
        }

        // GET: Items/Delete/5
        [Authorize(Roles = "President")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Items items = db.item.Find(id);
            if (items == null)
            {
                return HttpNotFound();
            }
            return View(items);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Items items = db.item.Find(id);
            db.item.Remove(items);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
