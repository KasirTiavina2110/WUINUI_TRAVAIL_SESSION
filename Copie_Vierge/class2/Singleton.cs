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
using System.Data;
using System.Diagnostics;



namespace class2
{
    internal class Singleton
    {

        MySqlConnection con;
        ObservableCollection<Activite> listeActivite;
        ObservableCollection<Usager> listeUsager;
        private ObservableCollection<Seance> listeSeance;
        ObservableCollection<Inscription> listeInscription;


        public static Singleton instance = null;


        public Singleton()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420-345-ri_eq15;Uid=1748697;Pwd=1748697;");
            listeActivite = new ObservableCollection<Activite>();
            listeSeance = new ObservableCollection<Seance>();

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

        //Recevoir la liste des utilisateurs
        public ObservableCollection<Usager> GetListeAdherents()
        {
            ObservableCollection<Usager> listeAdherents = new ObservableCollection<Usager>();

            try
            {
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "SELECT * FROM adherent WHERE role = 'adherent'";

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    Usager usager = new Usager
                    {
                        NumeroIdentification = reader["numero_identification"].ToString(),
                        Nom = reader["Nom"].ToString(),
                        Prenom = reader["Prenom"].ToString(),
                        Age = Convert.ToInt32(reader["age"])
                    };

                    listeAdherents.Add(usager);
                }

                reader.Close();
                con.Close();
            }
            catch (Exception ex)
            {
                // Déboguer l'exception
                Debug.WriteLine($"Erreur lors de la récupération des adhérents : {ex.Message}");
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return listeAdherents;
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

                    Activite activite = new Activite(id, nom, annee, cout_organisation, vente_client, type, pochette);
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

            if (reponseRequete > 0)
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

        //Modifier une activite 
        public bool modifierActivite(Activite activite)
        {

            try
            {

                MySqlCommand commande = new MySqlCommand();

                commande.Connection = con;
                commande.CommandText = "UPDATE activite " +
                    "SET nom = @nom, annee = @annee, cout_organisation = @coutOrganisation, vente_client = @venteClient, type = @type, pochette = @pochette " +
                    " WHERE id_activite = @idModifier";
                commande.Parameters.AddWithValue("@nom", activite.Nom);
                commande.Parameters.AddWithValue("@annee", activite.Annee);
                commande.Parameters.AddWithValue("@coutOrganisation", activite.Cout_Organisation);
                commande.Parameters.AddWithValue("@venteClient", activite.Vente_Client);
                commande.Parameters.AddWithValue("@type", activite.Type);
                commande.Parameters.AddWithValue("@pochette", activite.Pochette);
                commande.Parameters.AddWithValue("idModifier", activite.Id);



                con.Open();
                commande.Prepare();
                int i = commande.ExecuteNonQuery();
                con.Close();

                return i > 0;

            }
            catch (Exception ex)
            {

                if (con.State == System.Data.ConnectionState.Open)
                {
                    con.Close();
                }


                return false;
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

            string numero_identification = matricule;

            MySqlCommand commande = new MySqlCommand();

            commande.Connection = con;
            commande.CommandText = "DELETE FROM adherent WHERE numero_identification = @matricule";
            commande.Parameters.AddWithValue("@matricule", numero_identification);

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
                " WHERE numero_identifiaction = @matricule";
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

        /*Seance */
        public ObservableCollection<Seance> getListeSeance()
        {
            affichageSeance();
            return listeSeance;
        }

        public ObservableCollection<Inscription> GetListeMesSeance()
        {
            afficherMesSeances();
            return listeInscription;
        }


        private void affichageSeance()
        {
            try
            {
                if (listeSeance == null) // Vérifie si la liste est initialisée
                {
                    listeSeance = new ObservableCollection<Seance>();
                }

                listeSeance.Clear(); // Assurez-vous qu'elle est prête à être utilisée
                MySqlCommand commande = new MySqlCommand("SELECT * FROM seance", con);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    Seance seance = new Seance
                    {
                        Id_seance = Convert.ToInt32(reader["id_seance"]),
                        Numero_activite = reader["numero_activite"].ToString(),
                        Date = Convert.ToDateTime(reader["date"]),
                        Heure = TimeSpan.Parse(reader["heure"].ToString()),
                        Place_dispo = reader["place_dispo"] != DBNull.Value ? (int?)Convert.ToInt32(reader["place_dispo"]) : null,
                        Place_prise = Convert.ToInt32(reader["place_prise"]),
                        Place_max = Convert.ToInt32(reader["place_max"])
                    };
                    listeSeance.Add(seance);
                }
                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
                Debug.WriteLine("Erreur lors de l'affichage des séances : " + ex.Message);
            }
        }


        private void afficherMesSeances()
        {

            try
            {
                if (listeInscription == null) // Vérifie si la liste est initialisée
                {
                    listeInscription = new ObservableCollection<Inscription>();
                }

                var currentUser = SessionManager.Instance.UsagerConnecte;

                var numero = currentUser.NumeroIdentification;

                listeInscription.Clear(); // Assurez-vous qu'elle est prête à être utilisée
                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "SELECT * FROM inscription WHERE numero_adherent = @numero";
                commande.Parameters.AddWithValue("@numero", numero);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    Inscription inscription = new Inscription
                    {

                        Id_inscription = Convert.ToInt32(reader["id_inscription"]),
                        Numero_adherent = reader["numero_adherent"].ToString(),
                        Id_seance = Convert.ToInt32(reader["id_seance"]),
                        Date_inscription = Convert.ToDateTime(reader["date_inscription"])
                       
                    };

                    listeInscription.Add(inscription);

                }
                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
                Debug.WriteLine("Erreur lors de l'affichage des séances : " + ex.Message);
            }

        }

        public void ajouterNote(int note)
        {

            try
            {
                int noteFinale = note;

                MySqlCommand commande = new MySqlCommand();
                commande.Connection = con;
                commande.CommandText = "INSERT INTO note (numero_adherent, numero_activite, note) (@numeroAdherent, @numeroActivite, @note)";
                commande.Parameters.AddWithValue("@numeroAdherent", SessionManager.Instance.UsagerConnecte.NumeroIdentification);
                commande.Parameters.AddWithValue("@note", noteFinale);

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


        public ObservableCollection<Seance> GetSeancesByActivity(string activityCode)
        {
            ObservableCollection<Seance> seancesByActivity = new ObservableCollection<Seance>();
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
                    SELECT s.* 
                    FROM seance s
                    INNER JOIN activite a ON s.numero_activite = a.id_activite
                    WHERE a.id_activite = @activityCode", con);
                commande.Parameters.AddWithValue("@activityCode", activityCode);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    Seance seance = new Seance
                    {
                        Id_seance = Convert.ToInt32(reader["id_seance"]),
                        Numero_activite = reader["numero_activite"].ToString(),
                        Date = Convert.ToDateTime(reader["date"]),
                        Heure = TimeSpan.Parse(reader["heure"].ToString()),
                        Place_dispo = reader["place_dispo"] != DBNull.Value ? (int?)Convert.ToInt32(reader["place_dispo"]) : null,
                        Place_prise = Convert.ToInt32(reader["place_prise"]),
                        Place_max = Convert.ToInt32(reader["place_max"])
                    };
                    seancesByActivity.Add(seance);
                }
                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
                Debug.WriteLine("Erreur lors de la récupération des séances par activité : " + ex.Message);
            }
            return seancesByActivity;
        }

        /*Inscrption*/

        public bool InscriptionSeance(int idSeance, string numeroAdherent)
        {
            try
            {
                // Récupérer la date et l'heure de la séance
                DateTime dateSeance;
                TimeSpan heureSeance;

                using (var cmd = new MySqlCommand("SELECT date, heure FROM seance WHERE id_seance = @idSeance", con))
                {
                    cmd.Parameters.AddWithValue("@idSeance", idSeance);
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (!reader.Read())
                        {
                            throw new Exception("Séance introuvable.");
                        }
                        dateSeance = Convert.ToDateTime(reader["date"]);
                        heureSeance = TimeSpan.Parse(reader["heure"].ToString());
                    }
                    con.Close();
                }

                // Vérification des conflits d'horaires
                if (!VerifierConflitHoraire(numeroAdherent, dateSeance, heureSeance))
                {
                    Debug.WriteLine("Conflit d'horaire détecté pour l'utilisateur : " + numeroAdherent);
                    return false;
                }

                // Ajouter l'inscription
                using (var cmd = new MySqlCommand("INSERT INTO inscription (id_seance, numero_adherent) VALUES (@idSeance, @numeroAdherent)", con))
                {
                    cmd.Parameters.AddWithValue("@idSeance", idSeance);
                    cmd.Parameters.AddWithValue("@numeroAdherent", numeroAdherent);
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                    con.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    con.Close();

                    return rowsAffected > 0;
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Erreur SQL : " + ex.Message);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return false;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erreur : " + ex.Message);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return false;
            }
        }


        public bool VerifierConflitHoraire(string numeroAdherent, DateTime date, TimeSpan heure)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand
                {
                    Connection = con,
                    CommandText = "verifier_conflit_horaire"
                };
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                // Ajout des paramètres
                commande.Parameters.AddWithValue("@p_numero_adherent", numeroAdherent);
                commande.Parameters.AddWithValue("@p_date", date);
                commande.Parameters.AddWithValue("@p_heure", heure);

                con.Open();
                commande.ExecuteNonQuery();
                con.Close();

                return true; // Aucun conflit détecté
            }
            catch (MySqlException ex)
            {
                con.Close();

                // Vérifier si l'erreur vient de la procédure stockée
                if (ex.Number == 1005)
                {
                    System.Diagnostics.Debug.WriteLine("Conflit d'horaire détecté : " + ex.Message);
                    return false; // Conflit détecté
                }

                // Autres erreurs
                System.Diagnostics.Debug.WriteLine("Erreur lors de la vérification du conflit d'horaire : " + ex.Message);
                throw; // Relancer l'exception pour traitement ailleurs si nécessaire
            }
        }

        public Seance ObtenirSeanceParId(int idSeance)
        {
            Seance seance = null;
            try
            {
                MySqlCommand commande = new MySqlCommand("SELECT * FROM seance WHERE id_seance = @id", con);
                commande.Parameters.AddWithValue("@id", idSeance);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();

                if (reader.Read())
                {
                    seance = new Seance
                    {
                        Id_seance = Convert.ToInt32(reader["id_seance"]),
                        Numero_activite = reader["numero_activite"].ToString(),
                        Date = Convert.ToDateTime(reader["date"]),
                        Heure = TimeSpan.Parse(reader["heure"].ToString()),
                        Place_dispo = reader["place_dispo"] != DBNull.Value ? (int?)Convert.ToInt32(reader["place_dispo"]) : null,
                        Place_prise = Convert.ToInt32(reader["place_prise"]),
                        Place_max = Convert.ToInt32(reader["place_max"])
                    };
                }

                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
                Debug.WriteLine("Erreur lors de la récupération de la séance : " + ex.Message);
            }

            return seance;
        }

        public bool ModifierSeance(Seance seance)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
            UPDATE seance 
            SET numero_activite = @numeroActivite, date = @date, heure = @heure, 
                place_dispo = @placeDispo, place_prise = @placePrise, place_max = @placeMax
            WHERE id_seance = @id", con);

                commande.Parameters.AddWithValue("@numeroActivite", seance.Numero_activite);
                commande.Parameters.AddWithValue("@date", seance.Date);
                commande.Parameters.AddWithValue("@heure", seance.Heure);
                commande.Parameters.AddWithValue("@placeDispo", seance.Place_dispo);
                commande.Parameters.AddWithValue("@placePrise", seance.Place_prise);
                commande.Parameters.AddWithValue("@placeMax", seance.Place_max);
                commande.Parameters.AddWithValue("@id", seance.Id_seance);

                con.Open();
                int rowsAffected = commande.ExecuteNonQuery();
                con.Close();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Erreur lors de la modification de la séance : " + ex.Message);
                con.Close();
                return false;
            }
        }

      
        public ObservableCollection<Activite> ObtenirListeActivites()
        {
            try
            {
                if (listeActivite == null || listeActivite.Count == 0)
                {
                    afficherActivite(); // Charge les activités si la liste est vide
                }
                return listeActivite;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Erreur lors de l'obtention de la liste des activités : " + ex.Message);
                return new ObservableCollection<Activite>();
            }
        }

        public List<string> ObtenirNumeroActivites()
        {
            List<string> numeroActivites = new List<string>();

            try
            {
                MySqlCommand commande = new MySqlCommand("SELECT id_activite FROM activite", con);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    // Ajout du nom de l'activité à la liste
                    numeroActivites.Add(reader["id_activite"].ToString());
                }

                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Erreur lors de la récupération des noms des activités : " + ex.Message);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

            return numeroActivites;
        }

        private void SupprimerInscriptionsParSeance(int idSeance)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("DELETE FROM inscription WHERE id_seance = @idSeance", con);
                commande.Parameters.AddWithValue("@idSeance", idSeance);

                con.Open();
                commande.ExecuteNonQuery();
                con.Close();
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Erreur lors de la suppression des inscriptions : " + ex.Message);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private bool VerifierInscriptionsPourSeance(int idSeance)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand("SELECT COUNT(*) FROM inscription WHERE id_seance = @idSeance", con);
                commande.Parameters.AddWithValue("@idSeance", idSeance);

                con.Open();
                int count = Convert.ToInt32(commande.ExecuteScalar());
                con.Close();

                return count > 0;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Erreur lors de la vérification des inscriptions : " + ex.Message);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return false;
            }
        }
        public bool SupprimerSeance(int idSeance)
        {
            try
            {
                // Vérifiez si des inscriptions existent
                if (VerifierInscriptionsPourSeance(idSeance))
                {
                    // Supprimez d'abord les inscriptions associées
                    SupprimerInscriptionsParSeance(idSeance);
                }

                // Supprimez la séance
                MySqlCommand commande = new MySqlCommand("DELETE FROM seance WHERE id_seance = @id", con);
                commande.Parameters.AddWithValue("@id", idSeance);

                con.Open();
                int rowsAffected = commande.ExecuteNonQuery();
                con.Close();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Erreur lors de la suppression de la séance : " + ex.Message);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return false;
            }
        }
        public bool VerifierSeanceExiste(Seance seance)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand(
                    "SELECT COUNT(*) FROM seance WHERE numero_activite = @numeroActivite AND date = @date AND heure = @heure", con);

                commande.Parameters.AddWithValue("@numeroActivite", seance.Numero_activite);
                commande.Parameters.AddWithValue("@date", seance.Date.ToString("yyyy-MM-dd"));
                commande.Parameters.AddWithValue("@heure", seance.Heure.ToString(@"hh\:mm\:ss"));

                con.Open();
                int count = Convert.ToInt32(commande.ExecuteScalar());
                con.Close();

                // Retourne true si une séance existe déjà
                return count > 0;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Erreur lors de la vérification de la séance : " + ex.Message);
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
                return false;
            }
        }

        public bool AjouterSeance(Seance seance)
        {
            try
            {
                MySqlCommand commande = new MySqlCommand(@"
            INSERT INTO seance (numero_activite, date, heure, place_dispo, place_prise, place_max)
            VALUES (@numeroActivite, @date, @heure, @placeDispo, @placePrise, @placeMax)", con);

                commande.Parameters.AddWithValue("@numeroActivite", seance.Numero_activite);
                commande.Parameters.AddWithValue("@date", seance.Date);
                commande.Parameters.AddWithValue("@heure", seance.Heure);
                commande.Parameters.AddWithValue("@placeDispo", seance.Place_dispo);
                commande.Parameters.AddWithValue("@placePrise", seance.Place_prise);
                commande.Parameters.AddWithValue("@placeMax", seance.Place_max);

                con.Open();
                int rowsAffected = commande.ExecuteNonQuery();
                con.Close();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Erreur lors de l'ajout de la séance : " + ex.Message);
                con.Close();
                return false;
            }
        }

        // Vérifier si une inscription existe
        public bool VerifierInscriptionExiste(string numeroAdherent, int idSeance)
        {
            try
            {
                string query = @"
            SELECT COUNT(*) FROM inscription 
            WHERE numero_adherent = @numeroAdherent AND id_seance = @idSeance";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@numeroAdherent", numeroAdherent);
                cmd.Parameters.AddWithValue("@idSeance", idSeance);

                con.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                con.Close();

                return count > 0; // Retourne true si une inscription existe
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur lors de la vérification de l'inscription : {ex.Message}");
                if (con.State == System.Data.ConnectionState.Open) con.Close();
                return false;
            }
        }


        // Ajouter une inscription
        public bool AjouterInscription(Inscription inscription)
        {
            try
            {
                MySqlCommand cmd = new MySqlCommand(@"
            INSERT INTO inscription (numero_adherent, id_seance, date_inscription) 
            VALUES (@numeroAdherent, @idSeance, @dateInscription)", con);

                cmd.Parameters.AddWithValue("@numeroAdherent", inscription.Numero_adherent);
                cmd.Parameters.AddWithValue("@idSeance", inscription.Id_seance);
                cmd.Parameters.AddWithValue("@dateInscription", inscription.Date_inscription);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                return rowsAffected > 0;
            }
            catch (MySqlException ex)
            {
              //  Debug.WriteLine("Erreur lors de l'ajout de l'inscription : " + ex.Message);
                con.Close();
                return false;
            }
        }

        public bool ModifierInscription(Inscription inscription)
        {
            try
            {
                string query = @"
            UPDATE inscription 
            SET numero_adherent = @numeroAdherent, 
                id_seance = @idSeance, 
                date_inscription = @dateInscription
            WHERE id_inscription = @idInscription";

                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@numeroAdherent", inscription.Numero_adherent);
                cmd.Parameters.AddWithValue("@idSeance", inscription.Id_seance);
                cmd.Parameters.AddWithValue("@dateInscription", inscription.Date_inscription);
                cmd.Parameters.AddWithValue("@idInscription", inscription.Id_inscription);

                con.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                con.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur lors de la modification de l'inscription : {ex.Message}");
                if (con.State == System.Data.ConnectionState.Open) con.Close();
                return false;
            }
        }


        public bool SupprimerInscription(int idInscription)
        {
            try
            {
                MySqlCommand command = new MySqlCommand("DELETE FROM inscription WHERE id_inscription = @idInscription", con);
                command.Parameters.AddWithValue("@idInscription", idInscription);

                con.Open();
                int rowsAffected = command.ExecuteNonQuery();
                con.Close();

                return rowsAffected > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur lors de la suppression de l'inscription : {ex.Message}");
                return false;
            }
        }

        public ObservableCollection<Inscription> ObtenirListeInscriptions()
        {
            ObservableCollection<Inscription> listeInscriptions = new ObservableCollection<Inscription>();
            try
            {
                MySqlCommand commande = new MySqlCommand(
                    "SELECT numero_adherent, id_seance FROM inscription", con);

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();

                while (reader.Read())
                {
                    Inscription inscription = new Inscription
                    {
                        Numero_adherent = reader["numero_adherent"].ToString(),
                        Id_seance = Convert.ToInt32(reader["id_seance"])
                    };
                    listeInscriptions.Add(inscription);
                }

                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                //   Debug.WriteLine("Erreur lors de l'obtention des inscriptions : " + ex.Message);
                if (con.State == ConnectionState.Open) con.Close();
            }

            return listeInscriptions;
        }
        public ObservableCollection<Inscription> GetListeInscriptions()
        {
            ObservableCollection<Inscription> inscriptions = new ObservableCollection<Inscription>();
            try
            {
                MySqlCommand command = new MySqlCommand("SELECT * FROM inscription", con);
                con.Open();
                MySqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    inscriptions.Add(new Inscription
                    {
                        Id_inscription = reader.GetInt32("id_inscription"),
                        Numero_adherent = reader.GetString("numero_adherent"),
                        Id_seance = reader.GetInt32("id_seance"),
                        Date_inscription = reader.GetDateTime("date_inscription")
                    });
                }
                con.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur lors de la récupération des inscriptions : {ex.Message}");
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
            return inscriptions;
        }

    }


}
