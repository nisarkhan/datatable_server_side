using System.Data;
using System.Linq;
using System.Web.Mvc;
using DataTablesMvc4.Models;
using System.Web.Optimization;
using System.Collections.Generic;
using System;

namespace DataTablesMVC4.Controllers
{
    public class DataTablesMvc4Controller : Controller
    {
        private salt_dbEntities db = new salt_dbEntities();

        public class jQueryDataTableParamModel
        {
            /// <summary>
            /// Request sequence number sent by DataTable,
            /// same value must be returned in response
            /// </summary>       
            public string sEcho { get; set; }

            /// <summary>
            /// Text used for filtering
            /// </summary>
            public string sSearch { get; set; }

            /// <summary>
            /// Number of records that should be shown in table
            /// </summary>
            public int iDisplayLength { get; set; }

            /// <summary>
            /// First record that should be shown(used for paging)
            /// </summary>
            public int iDisplayStart { get; set; }

            /// <summary>
            /// Number of columns in table
            /// </summary>
            public int iColumns { get; set; }

            /// <summary>
            /// Number of columns that are used in sorting
            /// </summary>
            public int iSortingCols { get; set; }

            /// <summary>
            /// Comma separated list of column names
            /// </summary>
            public string sColumns { get; set; }

            /// <summary>
            /// Integer (0 based) of which column was clicked to order by
            /// </summary>
            public int? iSortCol_0 { get; set; }

            /// <summary>
            /// String to denote which way to order column
            /// </summary>
            public string sSortDir_0 { get; set; }

            ///// 
            ///// Request sequence number sent by DataTable,
            ///// same value must be returned in response
            /////        
            //public string sEcho { get; set; }

            ///// 
            ///// Text used for filtering
            ///// 
            //public string sSearch { get; set; }

            ///// 
            ///// Number of records that should be shown in table
            ///// 
            //public int iDisplayLength { get; set; }

            ///// 
            ///// First record that should be shown(used for paging)
            ///// 
            //public int iDisplayStart { get; set; }

            ///// 
            ///// Number of columns in table
            ///// 
            //public int iColumns { get; set; }

            ///// 
            ///// Number of columns that are used in sorting
            ///// 
            //public int iSortingCols { get; set; }

            ///// 
            ///// Comma separated list of column names
            ///// 
            //public string sColumns { get; set; }
        }


    //     var allCompanies = DataRepository.GetCompanies();
    //IEnumerable<Company> filteredCompanies = allCompanies;
            
    //var displayedCompanies = filteredCompanies
    //                    .Skip(param.iDisplayStart)
    //                    .Take(param.iDisplayLength); 
 
    //var result = from c in displayedCompanies 
    //            select new[] { Convert.ToString(c.ID), c.Name,
    //                      c.Address, c.Town };
    //return Json(new{  sEcho = param.sEcho,
    //                          iTotalRecords = allCompanies.Count(),
    //                          iTotalDisplayRecords = filteredCompanies.Count(),
    //                          aaData = result},
    //                    JsonRequestBehavior.AllowGet);

        //http://www.codeproject.com/Articles/155422/jQuery-DataTables-and-ASP-NET-MVC-Integration-Part

        public ActionResult AjaxHand(jQueryDataTableParamModel param)
        {
            var allCompanies = db.aspnet_Users;//.ToList();
            IEnumerable<aspnet_Users> filteredCompanies;//= allCompanies;

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filteredCompanies = allCompanies.Where(c => c.FirstName.Contains(param.sSearch) || c.LastName.Contains(param.sSearch) || c.UserName.Contains(param.sSearch));
            }
            else
            {
                filteredCompanies = allCompanies;
            }

            //sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<aspnet_Users, string> orderingFunction = (c => sortColumnIndex == 1 ? c.FirstName :
                                                                sortColumnIndex == 2 ? c.LastName :
                                                                c.UserName);

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
            else
                filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);
            //end sorting

            var displayedCompanies = filteredCompanies
                                .Skip(param.iDisplayStart)
                                .Take(param.iDisplayLength);


            var result = from c in displayedCompanies
                            select new[] { c.FirstName, c.LastName, c.UserName };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allCompanies.Count(),
                iTotalDisplayRecords = filteredCompanies.Count(),
                aaData = result.ToList()
            }, JsonRequestBehavior.AllowGet);





                // aoColumnDefs 
                //return Json(new
                //{
                //    sEcho = param.sEcho,
                //    iTotalRecords = db.aspnet_Users.Count(),
                //    iTotalDisplayRecords = db.aspnet_Users.Count(),
                //    aaData = db.aspnet_Users.ToList().Take(param.iDisplayLength)

                //},
                //    JsonRequestBehavior.AllowGet
                //);
             
        }

        
        public ActionResult AjaxHand1(jQueryDataTableParamModel param)
        {
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = db.aspnet_Users.Count(),
                    iTotalDisplayRecords = db.aspnet_Users.Count(),
                    aaData = db.aspnet_Users.Where(x => x.FirstName.Contains(param.sSearch.Trim())).ToList()

                },
                    JsonRequestBehavior.AllowGet
                );
            }
            else
            {
                var allCompanies = db.aspnet_Users.ToList();
                IEnumerable<aspnet_Users> filteredCompanies = allCompanies;

                //sorting
                var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
                Func<aspnet_Users, string> orderingFunction = (c => sortColumnIndex == 1 ? c.FirstName :
                                                                    sortColumnIndex == 2 ? c.LastName :
                                                                    c.UserName);

                var sortDirection = Request["sSortDir_0"]; // asc or desc
                if (sortDirection == "asc")
                    filteredCompanies = filteredCompanies.OrderBy(orderingFunction);
                else
                    filteredCompanies = filteredCompanies.OrderByDescending(orderingFunction);
                //end sorting

                var displayedCompanies = filteredCompanies
                                    .Skip(param.iDisplayStart)
                                    .Take(param.iDisplayLength);


                var result = from c in displayedCompanies
                             select new[] { c.FirstName, c.LastName, c.UserName };

                return Json(new
                {
                    sEcho = param.sEcho,
                    iTotalRecords = allCompanies.Count(),
                    iTotalDisplayRecords = filteredCompanies.Count(),
                    aaData = result
                },JsonRequestBehavior.AllowGet);





               // aoColumnDefs 
                //return Json(new
                //{
                //    sEcho = param.sEcho,
                //    iTotalRecords = db.aspnet_Users.Count(),
                //    iTotalDisplayRecords = db.aspnet_Users.Count(),
                //    aaData = db.aspnet_Users.ToList().Take(param.iDisplayLength)

                //},
                //    JsonRequestBehavior.AllowGet
                //);
            }
        }

        //
        // GET: /DataTablesMvc4/

        public ActionResult Index()
        {
            return View(db.aspnet_Users.ToList());
        }

        //
        // GET: /DataTablesMvc4/Details/5

        public ActionResult Details(int id = 0)
        {
            aspnet_Users datatableswithjson = db.aspnet_Users.Find(id);
            if (datatableswithjson == null)
            {
                return HttpNotFound();
            }
            return View(datatableswithjson);
        }

        //
        // GET: /DataTablesMvc4/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DataTablesMvc4/Create

        [HttpPost]
        public ActionResult Create(aspnet_Users datatableswithjson)
        {
            if (ModelState.IsValid)
            {
                db.aspnet_Users.Add(datatableswithjson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(datatableswithjson);
        }

        //
        // GET: /DataTablesMvc4/Edit/5

        public ActionResult Edit(int id = 0)
        {
            aspnet_Users datatableswithjson = db.aspnet_Users.Find(id);
            if (datatableswithjson == null)
            {
                return HttpNotFound();
            }
            return View(datatableswithjson);
        }

        //
        // POST: /DataTablesMvc4/Edit/5

        [HttpPost]
        public ActionResult Edit(aspnet_Users datatableswithjson)
        {
            if (ModelState.IsValid)
            {
                db.Entry(datatableswithjson).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(datatableswithjson);
        }

        //
        // GET: /DataTablesMvc4/Delete/5

        public ActionResult Delete(int id = 0)
        {
            aspnet_Users datatableswithjson = db.aspnet_Users.Find(id);
            if (datatableswithjson == null)
            {
                return HttpNotFound();
            }
            return View(datatableswithjson);
        }

        //
        // POST: /DataTablesMvc4/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            aspnet_Users datatableswithjson = db.aspnet_Users.Find(id);
            db.aspnet_Users.Remove(datatableswithjson);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}