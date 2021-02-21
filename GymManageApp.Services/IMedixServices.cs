using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GymManageApp.DAL;

namespace GymManageApp.Services
{
    public interface IMedixServices
    {
        List<WorkOrders> GetWorkOrders();

        List<Clients> GetClients();
        List<States> GetStates();
        List<Status> GetStatus();
        WorkOrders AddWorkOrder(WorkOrders wo);
        WorkOrders GetWorkOrderById(int Id);
        void EditWorkOrder(WorkOrders wo);
        void DeleteWorkOrder(int Id);

    }
}