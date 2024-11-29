using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Configuration;



namespace class2
{
    internal class Singleton
    {

        MySqlConnection con;
        ObservableCollection<Activite> listeActivite;
   
        public static Singleton instance = null;


        public Singleton()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420-345-ri_eq15;Uid=1748697;Pwd=1748697;");
            listeActivite = new ObservableCollection<Activite>();


        }


        public static Singleton getInstance()
        {
            if (instance == null)
            {
                instance = new Singleton();
            }
            return instance;
        }


        //Recevoir la liste des activités
        public ObservableCollection<Activite> getListeActivite()
        {

            afficherActivite();
            return listeActivite;
        }


        //Aller chercher la liste des activités

        public void afficherActivite()
        {
            try
            {
                listeActivite.Clear(); //Afin d'éviter l'accumulation des fausses données

                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "SELECT * FROM activite";

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    string id = reader[0].ToString();
                    string nom = reader[1].ToString();
                    string annee = reader[2].ToString();
                    double cout_organisation = Double.Parse(reader[3].ToString());
                    double vente_client = Double.Parse(reader[4].ToString());
                    string type = reader[5].ToString();
                    string pochette = reader[6].ToString();

                    Activite activite = new Activite(id,nom,annee,cout_organisation,vente_client,type,pochette);
                    listeActivite.Add(activite);

                }
                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
            }

        }

        //Supprimer une activité, RESTE VÉRIFIER POUR ÊTRE SÛR QUE CA FONCTIONNE
        //J'attend l'ajout d'activité pour tester

        public void supprimerActivite(Activite activite)
        {

            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "DELETE FROM activite WHERE id_activite = @activite";
                commande.Parameters.AddWithValue("@activite", activite.Id);

                con.Open();
                int i = commande.ExecuteNonQuery();
                con.Close();

                if (i > 0)
                {
                    // Suppression réussie dans la base de données, maintenant on enlève de la collection
                    listeActivite.Remove(activite);
                }
            }
            catch
            {
                if (con.State == System.Data.ConnectionState.Open) //Pour vérifier que la connexion est ouverte, sinon ca va planter
                {
                    con.Close();
                }
            }

        }


        //Ajouter un usager
        public void ajouterUsager(Usager usager)
        {
            try
            {
                string nom = usager.Nom;
                string prenom = usager.Prenom;
                string adresse = usager.Adresse;
                string dateNaissance = usager.DateNaissance2;
                int age = usager.Age;
                string role = usager.Role.ToLower();
                string motDePasse = usager.MotDePasse;


                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "INSERT INTO adherent (nom, prenom, adresse, date_naissance, age, role, mot_de_passe) VALUES(@nom,@prenom,@adresse,@dateNaissance,@age,@role,@motDePasse)";
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@prenom", prenom);
                commande.Parameters.AddWithValue("@adresse", adresse);
                commande.Parameters.AddWithValue("@dateNaissance", dateNaissance);
                commande.Parameters.AddWithValue("@age", age);
                commande.Parameters.AddWithValue("@role", role);
                commande.Parameters.AddWithValue("@motDePasse", motDePasse);

                con.Open();
                commande.Prepare();
                commande.ExecuteNonQuery();

                con.Close();

            }
            catch (Exception ex)
            {
                if (con.State == System.Data.ConnectionState.Open) //Pour vérifier que la connexion est ouverte, sinon ca va planter
                {
                    con.Close();
                }


            }

        }

        //Supprimer un usager
        public int supprimerUsager(string matricule)
        {

            string numero_identifiaction = matricule;

            MySqlCommand commande = new MySqlCommand();

            commande.Connection = con;
            commande.CommandText = "DELETE FROM adherent WHERE numero_identification = @matricule";
            commande.Parameters.AddWithValue("@matricule", numero_identifiaction);

            con.Open();

            int i = commande.ExecuteNonQuery();

            con.Close();

            return i;

        }



    }
}
