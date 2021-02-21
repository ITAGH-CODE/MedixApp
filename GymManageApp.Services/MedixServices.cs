using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Web;
using GymManageApp.DAL;

namespace GymManageApp.Services
{
    public class MedixServices : IMedixServices
    {
        private readonly MedixTesttEntities _db;
        public MedixServices(MedixTesttEntities db)
        {
            _db = db;
        }

        public List<WorkOrders> GetWorkOrders()
        {
            return _db.WorkOrders.Include(c => c.Clients).Include(c => c.States).Include(c => c.Status).ToList();
        }

        public List<Clients> GetClients()
        {
            return _db.Clients.ToList();
        }
        public List<States> GetStates()
        {
            return _db.States.ToList();
        }
        public List<Status> GetStatus()
        {
            return _db.Status.ToList();
        }
        public WorkOrders AddWorkOrder(WorkOrders wo)
        {
            var worder = _db.WorkOrders.Where(w => w.WorkOrderNumber == wo.WorkOrderNumber).FirstOrDefault();
            if (worder == null)
            {
                _db.WorkOrders.Add(wo);
                _db.SaveChanges();
                return wo;
            }
            else
                return null;
        }

        public WorkOrders GetWorkOrderById(int Id)
        {
            return _db.WorkOrders.Find(Id);
        }
        public void EditWorkOrder(WorkOrders wo)
        {
            WorkOrders worder = GetWorkOrderById(wo.WorkOrderId);
            if(worder != null)
            {
                worder.ClientId = wo.ClientId;
                worder.StateId = wo.StateId;
                worder.StatusId = wo.StatusId;
                _db.SaveChanges();
            }
        }

        public void DeleteWorkOrder(int Id)
        {
            var worder = GetWorkOrderById(Id);
            if(worder != null)
            {
                _db.WorkOrders.Remove(worder);
                _db.SaveChanges();
            }
        }
    }
}