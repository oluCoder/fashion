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

namespace FashionableFitandFlawless.Controllers
{
   
    public class SubCategoriesController : Controller
    {
        private FashionDb db = new FashionDb();

        // GET: SubCategories
        [Authorize(Roles = "President")]
        public ActionResult Index()
        {
            var subCategory = db.subCategory.Include(s => s.category);
            return View(subCategory.ToList());
        }

        public ActionResult Sub(string subs, int? page)
        {
            //var pageNumber = page ?? 1;
            //var pageSize = 60;

            var subcat = db.subCategory.Where(f => f.CategoryId.ToString().Trim().Contains(subs));

            ViewBag.Item = subs;
            return PartialView("Sub", subcat.ToList());
        }

        public ActionResult Subs(string subs, int? page)
        {
            //var pageNumber = page ?? 1;
            //var pageSize = 60;

            var subcat = db.subCategory.Where(f => f.Categories.ToString().Trim().Contains(subs));

            ViewBag.Item = subs;
            return PartialView("Sub", subcat.ToList());
        }

        // GET: SubCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.subCategory.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: SubCategories/Create
        [Authorize(Roles = "President")]
        public ActionResult Create()
        {
            ViewBag.CategoryId = new SelectList(db.category, "CategoryId", "Section");
            return View();
        }

        // POST: SubCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "President")]
        public ActionResult Create([Bind(Include = "SubId,CategoryId,Categories,SubName")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.subCategory.Add(subCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CategoryId = new SelectList(db.category, "CategoryId", "Section", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Edit/5
        [Authorize(Roles = "President")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.subCategory.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            ViewBag.CategoryId = new SelectList(db.category, "CategoryId", "Section", subCategory.CategoryId);
            return View(subCategory);
        }

        // POST: SubCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "President")]
        public ActionResult Edit([Bind(Include = "SubId,CategoryId,Categories,SubName")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryId = new SelectList(db.category, "CategoryId", "Section", subCategory.CategoryId);
            return View(subCategory);
        }

        // GET: SubCategories/Delete/5
        [Authorize(Roles = "President")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCategory subCategory = db.subCategory.Find(id);
            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // POST: SubCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "President")]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCategory subCategory = db.subCategory.Find(id);
            db.subCategory.Remove(subCategory);
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
