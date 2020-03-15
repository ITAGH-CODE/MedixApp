using GymManageApp.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GymManageApp.Services
{
    public class GymServices : IGymServices
    {
        private readonly GymDBEntities _db;

        public GymServices(GymDBEntities db)
        {
            _db = db;
        }

        public List<Gym_Client> GetClients()
        {
            var clients = _db.Gym_Client.ToList();
            return clients;
        }
        public void AjouterClient(Gym_Client newClient)
        {
            try
            {
                Gym_Client dbClient = _db.Gym_Client.Where(c => c.IdClient == newClient.IdClient).FirstOrDefault();
                if(dbClient != null)
                {
                    dbClient.Nom = newClient.Nom;
                    dbClient.Prenom = newClient.Prenom;
                    dbClient.CIN = dbClient.CIN;
                    dbClient.Telephone = dbClient.Telephone;
                    dbClient.Email = dbClient.Email;
                    _db.SaveChanges();
                }
                else
                {
                    newClient.Actif = true;
                    _db.Gym_Client.Add(newClient);
                    _db.SaveChanges();

                    Gym_Client client = _db.Gym_Client.Where(c => c.Nom == newClient.Nom && c.Prenom == newClient.Prenom && c.CIN == newClient.CIN).FirstOrDefault();
                    if (client != null)
                    {
                        Gym_Payement payement = new Gym_Payement();
                        payement.DateDebut = DateTime.Now;
                        payement.DateFin = DateTime.Now.AddMonths(1);
                        payement.DateProchainPayement = DateTime.Now.AddMonths(1).AddDays(1);
                        payement.IdClient = client.IdClient;
                        payement.isWellOrdered = true;
                        payement.Montant = 200;
                        _db.Gym_Payement.Add(payement);
                        _db.SaveChanges();
                        payement = _db.Gym_Payement.Where(p => p.IdClient == client.IdClient && p.DateDebut.Day == DateTime.Now.Day).FirstOrDefault();
                        if (payement != null)
                        {
                            _db.Gym_Client.Find(payement.IdClient).IdPayementEnCours = payement.IdPayement;
                            _db.SaveChanges();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                // Catcher l'exception dans un fichier de log
                Console.WriteLine(ex.Message);
            }
        }

        public bool DeleteClient(int idClient)
        {
            bool isDeleted = false;
            var client = _db.Gym_Client.Find(idClient);
            if(client != null)
            {
                client.Gym_Payement.Clear();
                _db.Gym_Client.Remove(client);
                _db.SaveChanges();
                isDeleted = true;
            }
            return isDeleted;
        }

        public bool AddNewPayement(int idClient)
        {
            try
            {
                Gym_Payement paiement = new Gym_Payement();
                paiement.DateDebut = DateTime.Now;
                paiement.DateFin = DateTime.Now.AddMonths(1);
                paiement.DateProchainPayement = DateTime.Now.AddMonths(1).AddDays(1);
                paiement.IdClient = idClient;
                paiement.isWellOrdered = true;
                paiement.Montant = 200;
                _db.Gym_Payement.Add(paiement);
                _db.SaveChanges();
                var client = GetClient(idClient);
                paiement = _db.Gym_Payement.Where(p => p.IdClient == client.IdClient && p.DateDebut.Day == DateTime.Now.Day).OrderByDescending(d=>d.IdPayement).FirstOrDefault();
                if (paiement != null)
                {
                    _db.Gym_Client.Find(paiement.IdClient).IdPayementEnCours = paiement.IdPayement;
                    _db.SaveChanges();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public bool isAlreadyExistClientForAdding(Gym_Client newClient)
        {
            var client = _db.Gym_Client.Where(c => c.Nom == newClient.Nom && c.Prenom == newClient.Prenom);
            if (client != null && client.Count() > 0)
                return true;
            else
                return false;
        }
        public bool isAlreadyExistClientForEditing(Gym_Client newClient)
        {
            var client = _db.Gym_Client.Where(c => c.IdClient == newClient.IdClient);
            if (client != null && client.Count() > 0 && client.FirstOrDefault().IdClient > 0)
                return true;
            else
                return false;
        }
        public Gym_Payement GetPayement(int? idPayement)
        {
            var payement = (idPayement != null) ? _db.Gym_Payement.Where(p => p.IdPayement == idPayement).FirstOrDefault() : null;
            return payement;
        }

        public Gym_Client GetClient(int IdClient)
        {
            return _db.Gym_Client.Where(c => c.IdClient == IdClient).FirstOrDefault();
        }
    }
}