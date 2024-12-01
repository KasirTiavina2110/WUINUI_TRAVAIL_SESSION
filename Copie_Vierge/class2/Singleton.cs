using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;
using System.Configuration;
using Windows.System;



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

        //-------------------------- GESTION DES ACTIVITÉS -------------------------------------------

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

        public int supprimerActivite(string identifiant)
        {
            string id_activite = identifiant;

            //Compter si des séances existent
            int reponseRequete = compterSeance(id_activite);

            if(reponseRequete > 0)
            {
                return -1;
            }

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "DELETE FROM activite WHERE id_activite = @activite";
            commande.Parameters.AddWithValue("@activite", id_activite);

            con.Open();
            commande.Prepare();
            int i = commande.ExecuteNonQuery();
            con.Close();

            return i;


        }
        //Supprimer les séances en rapport avec l'acitivité supprimée
        public int compterSeance(string identifiant)
        {
            string numero_activite = identifiant;

            MySqlCommand commande = new MySqlCommand();
            commande.Connection = con;
            commande.CommandText = "SELECT COUNT(*) FROM seance WHERE numero_activite= @activite";
            commande.Parameters.AddWithValue("@activite", numero_activite);

            con.Open();
            commande.Prepare();
            int i = Convert.ToInt32(commande.ExecuteScalar());
            con.Close();

            return i;


        }

        //Vérifier si une activité existe, return 1 ou 0
        public int verifierSiActiviteExiste(string identifiant)
        {
            string id_activite = identifiant;

            MySqlCommand commande = new MySqlCommand();

            commande.Connection = con;
            commande.CommandText = "SELECT COUNT(*) FROM activite WHERE id_activite = @activite";
            commande.Parameters.AddWithValue("@activite", id_activite);

            con.Open();
            commande.Prepare();
            //Permet de retourner la valeur du COUNT(*)
            int i = Convert.ToInt32(commande.ExecuteScalar());

            con.Close();

            return i;

        }

        //Aller chercher les informations d'une activité
        public Activite informationUneActivite(string nom_activite)
        {
            Activite activite = null;

            string id_activite = nom_activite;

            MySqlCommand commande = new MySqlCommand();

            commande.Connection = con;
            commande.CommandText = "SELECT * FROM activite WHERE id_activite = @id";
            commande.Parameters.AddWithValue("@id", id_activite);

            con.Open();

            MySqlDataReader reader = commande.ExecuteReader();

            //Retourner une activite pour avoir accès aux infos selon ce qu'on veut afficher
            while (reader.Read())
            {
                activite = new Activite()
                {
                    Nom = reader["nom"].ToString(),
                    Annee = reader["annee"].ToString(),
                    Cout_Organisation = Convert.ToDouble(reader["cout_organisation"]),
                    Vente_Client = Convert.ToDouble(reader["vente_client"]),
                    Type = reader["type"].ToString(),
                    Pochette = reader["pochette"].ToString(),

                };


            }

            reader.Close();
            con.Close();

            return activite;

        }

        //Ajouter une activité
        public void ajouterActivite(Activite activite)
        {
            try
            {
                string id_activite = activite.Id;
                string nom = activite.Nom;
                string annee = activite.Annee;
                double coutOrganisation = activite.Cout_Organisation;
                double prixVente = activite.Vente_Client;
                string type = activite.Type;
                string pochette = activite.Pochette;

                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "INSERT INTO activite (id_activite, nom, annee, cout_organisation, vente_client, type, pochette) " +
                    "VALUES(@identifiant, @nom, @annee, @coutOrganisation, @prixVente, @type, @pochette)";
                commande.Parameters.AddWithValue("@identifiant", id_activite);
                commande.Parameters.AddWithValue("@nom", nom);
                commande.Parameters.AddWithValue("@annee", annee);
                commande.Parameters.AddWithValue("@coutOrganisation", coutOrganisation);
                commande.Parameters.AddWithValue("@prixVente", prixVente);
                commande.Parameters.AddWithValue("@type", type);
                commande.Parameters.AddWithValue("@pochette", pochette);

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



        // ------------------------- GESTION USAGERS ---------------------------------------------------------

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
            commande.Prepare();
            int i = commande.ExecuteNonQuery();

            con.Close();

            return i;

        }

        //Vérifier si un matricule return 1 ou 0
        public int verifierSiExiste(string matricule)
        {
            string numero_identification = matricule;

            MySqlCommand commande = new MySqlCommand();

            commande.Connection = con;
            commande.CommandText = "SELECT COUNT(*) FROM adherent WHERE numero_identification = @matricule";
            commande.Parameters.AddWithValue("@matricule", numero_identification);

            con.Open();
            commande.Prepare();
            //Permet de retourner la valeur du COUNT(*)
            int i = Convert.ToInt32(commande.ExecuteScalar());

            con.Close();

            return i;

        }


        //Aller chercher les informations d'un usager (page modification)
        public Usager informationUnUsager(string matricule)
        {

            Usager utilisateur = null;

            string numero_identification = matricule;

            MySqlCommand commande = new MySqlCommand();

            commande.Connection = con;
            commande.CommandText = "SELECT * FROM adherent WHERE numero_identification = @matricule";
            commande.Parameters.AddWithValue("@matricule", numero_identification);

            con.Open();

            MySqlDataReader reader = commande.ExecuteReader();

            //Retourner un usager pour avoir accès aux infos selon ce qu'on veut afficher
            while (reader.Read())
            {
                utilisateur = new Usager()
                {
                    Nom = reader["nom"].ToString(),
                    Prenom = reader["prenom"].ToString(),
                    Adresse = reader["adresse"].ToString(),
                    DateNaissance = Convert.ToDateTime(reader["date_naissance"]),
                    Age = Convert.ToInt32(reader["age"]),
                    Role = reader["role"].ToString(),
                    MotDePasse = reader["mot_de_passe"].ToString()
                };


            }

            reader.Close();
            con.Close();

            return utilisateur;

        }

        //Pour modifier un usager
        public bool modifierUsager(Usager usager)
        {

            MySqlCommand commande = new MySqlCommand();

            commande.Connection = con; 
            commande.CommandText = "UPDATE adherent " +
                "SET nom = @nom, prenom = @prenom, adresse = @adresse, date_naissance = @dateNaissance, age = @age, role = @role " +
                " WHERE numero_identification = @matricule";
            commande.Parameters.AddWithValue("@matricule", usager.NumeroIdentification);
            commande.Parameters.AddWithValue("@nom", usager.Nom);
            commande.Parameters.AddWithValue("@prenom", usager.Prenom);
            commande.Parameters.AddWithValue("@adresse", usager.Adresse);
            commande.Parameters.AddWithValue("@dateNaissance", usager.DateNaissance2);
            commande.Parameters.AddWithValue("@age", usager.Age);
            commande.Parameters.AddWithValue("@role", usager.Role);


            con.Open();
            commande.Prepare();
            int i = commande.ExecuteNonQuery();
            con.Close();

            return i > 0;

        }



    }
}
