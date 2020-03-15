using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManageApp.DAL;

namespace GymManageApp.Services
{
    public interface IGymServices
    {
        List<Gym_Client> GetClients();
        void AjouterClient(Gym_Client newClient);
        bool isAlreadyExistClientForAdding(Gym_Client newClient);
        bool isAlreadyExistClientForEditing(Gym_Client newClient);
        Gym_Payement GetPayement(int? idPayement);
        Gym_Client GetClient(int IdClient);
        bool AddNewPayement(int idClient);
        bool DeleteClient(int idClient);
    }
}
