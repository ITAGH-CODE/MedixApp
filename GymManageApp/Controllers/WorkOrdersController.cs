using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using GymManageApp.DAL;
using Medix.Web.Models;
using GymManageApp.Services;
using PagedList.Mvc;
using PagedList;

namespace Medix.Web.Controllers
{
    public class WorkOrdersController : Controller
    {
        private readonly IMedixServices _medixServices;

        public WorkOrdersController()
        {
            _medixServices = new MedixServices(new MedixTesttEntities());
        }


        // GET: WorkOrders
        public ActionResult Index(int? i)
        {
            var workOrders = _medixServices.GetWorkOrders().ToList().ToPagedList(i ?? 1,5);
            return View(workOrders);
        }

        public JsonResult GetWorkOrders()
        {
            var workOrders = _medixServices.GetWorkOrders();
            return Json(new { Count = workOrders.Count }, JsonRequestBehavior.AllowGet);
        }


        // GET: WorkOrders/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(_medixServices.GetClients(), "ClientId", "Name");
            ViewBag.StateId = new SelectList(_medixServices.GetStates(), "StateId", "Code");
            ViewBag.StatusId = new SelectList(_medixServices.GetStatus(), "StatusId", "Name");
            WorkOrders wo = new WorkOrders();
            return View(wo);
        }

        // POST: WorkOrders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "WorkOrderId,WorkOrderNumber,CreatedDate,ClientId,StateId,StatusId,ETA")] WorkOrders workOrders)
        {
            if (ModelState.IsValid)
            {
                DateTime etaDate = new DateTime();
                ViewBag.ClientId = new SelectList(_medixServices.GetClients(), "ClientId", "Name");
                ViewBag.StateId = new SelectList(_medixServices.GetStates(), "StateId", "Code");
                ViewBag.StatusId = new SelectList(_medixServices.GetStatus(), "StatusId", "Name");
                if (string.IsNullOrWhiteSpace(workOrders.WorkOrderNumber) || workOrders.StateId == null || workOrders.ClientId == null || workOrders.StatusId == null)
                {
                    TempData["messageNotifyError"] = "Please fill all fields";

                }
                else
                {
                    if (DateTime.TryParse(workOrders.ETA.ToString(), out etaDate))
                    {
                        workOrders.ETA = etaDate;
                    }
                    else
                        workOrders.ETA = null;
                    workOrders.CreatedDate = DateTime.Now;
                    WorkOrders mymodel = _medixServices.AddWorkOrder(workOrders);
                    ViewBag.isExist = false;
                    ViewBag.success = true;

                    if (mymodel == null)
                    {
                        TempData["messageNotifyError"] = "WorkOrder with the same Number already exist in the database";
                    }
                    else
                    {
                        TempData["messageNotifySuccess"] = "WorkOrder successfully inserted in the database";
                        return RedirectToAction("Index");
                    }
                }
                
            }
            return View("Create",workOrders);
            
        }

        // GET: WorkOrders/Edit
        public ActionResult Edit(int Id)
        {
            ViewBag.ClientId = new SelectList(_medixServices.GetClients(), "ClientId", "Name");
            ViewBag.StateId = new SelectList(_medixServices.GetStates(), "StateId", "Code");
            ViewBag.StatusId = new SelectList(_medixServices.GetStatus(), "StatusId", "Name");

            WorkOrders wo = _medixServices.GetWorkOrderById(Id);
            return View(wo);
        }

        [HttpGet]
        public ActionResult Delete(int Id)
        {
            ViewBag.ClientId = new SelectList(_medixServices.GetClients(), "ClientId", "Name");
            ViewBag.StateId = new SelectList(_medixServices.GetStates(), "StateId", "Code");
            ViewBag.StatusId = new SelectList(_medixServices.GetStatus(), "StatusId", "Name");

            WorkOrders wo = _medixServices.GetWorkOrderById(Id);
            return View(wo);
        }


        [HttpPost]
        // GET: WorkOrders/Edit
        public ActionResult Edit([Bind(Include = "WorkOrderId,WorkOrderNumber,CreatedDate,ClientId,StateId,StatusId,ETA")] WorkOrders wo)
        {
            if (ModelState.IsValid)
            {
                _medixServices.EditWorkOrder(wo);
            }
            TempData["messageNotifySuccess"] = "WorkOrder successfully updated";
            return RedirectToAction("Index");
        }


        [HttpPost]
        // GET: WorkOrders/Edit
        public ActionResult DeleteWO(int Id)
        {
            _medixServices.DeleteWorkOrder(Id);
            TempData["messageNotifySuccess"] = "WorkOrder successfully deleted";
            return RedirectToAction("Index");
        }
    }
}
