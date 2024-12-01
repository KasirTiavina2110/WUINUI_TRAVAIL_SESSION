using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace class2
{
    internal class SingletonStatistique
    {

        MySqlConnection con;
        ObservableCollection<Activite> listeActivite;

        public static SingletonStatistique instance = null;


        public SingletonStatistique()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420-345-ri_eq15;Uid=1748697;Pwd=1748697;");
            listeActivite = new ObservableCollection<Activite>();


        }


        public static SingletonStatistique GetInstance()
        {
            if (instance == null)
            {
                instance = new SingletonStatistique();
            }
            return instance;
        }

        //Trouver le nombre d'adhérents
        public int NombreAdherent()
        {
            int nombre = 0;

            try
            {

                MySqlCommand commande = new MySqlCommand("nombre_total_adherent");

                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                commande.Prepare();
                nombre = Convert.ToInt32(commande.ExecuteScalar());
                con.Close();

                return nombre;


            }
            catch(Exception ex)
            {
                con.Close();

                return nombre;
            }

        }

        //Trouver le nombre d'activite

        public int NombreActivite()
        {
            int nombre = 0;
            try
            {
               
                MySqlCommand commande = new MySqlCommand("nombre_total_activite");

                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                commande.Prepare();
                nombre = Convert.ToInt32(commande.ExecuteScalar());
                con.Close();

                return nombre;

            }
            catch(Exception ex)
            {
                con.Close();
                return nombre;
            }

        }


        public Dictionary<string, int> AdherentParActivite()
        {
            int nombre = 0;
            string nomActivite = "";
            var informations = new Dictionary<string, int>();

            try
            {

                MySqlCommand commande = new MySqlCommand("adherent_par_activite");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    nombre = Int32.Parse(reader["NombreAdherent"].ToString());
                    nomActivite = reader["numero_activite"].ToString();

                    informations[nomActivite] = nombre; //Associer le nombre et la catégorie en ensemble

                }
                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
            }
            return informations; //Retourner le dictionnaire de données



        }

        public Dictionary<string, double> NoteMoyenneActivite()
        {
            double nombre = 0;
            string nomActivite = "";
            var informations = new Dictionary<string, double>();

            try
            {

                MySqlCommand commande = new MySqlCommand("note_moyenne_par_activite");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    nombre = Double.Parse(reader["noteMoyenne"].ToString());
                    nomActivite = reader["numero_activite"].ToString();

                    informations[nomActivite] = nombre; //Associer le nombre et la catégorie en ensemble

                }
                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
            }

            return informations; //Retourner le dictionnaire de données

        }


        public Dictionary<string, int> NombreInscriptionParType()
        {

            int nombre = 0;
            string typeActivite = "";
            var informations = new Dictionary<string, int>();

            try
            {

                MySqlCommand commande = new MySqlCommand("nombre_inscription_par_type");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    nombre = Int32.Parse(reader["inscriptionParType"].ToString());
                    typeActivite = reader["type"].ToString();

                    informations[typeActivite] = nombre; //Associer le nombre et la catégorie en ensemble

                }
                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
            }
            return informations; //Retourner le dictionnaire de données

        }

        public double AgeMoyenAdherent()
        {

            double ageMoyen = 0;
            try
            {

                MySqlCommand commande = new MySqlCommand("age_moyen_adherent");

                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                commande.Prepare();
                ageMoyen = Convert.ToDouble(commande.ExecuteScalar());
                con.Close();

                return ageMoyen;

            }
            catch (Exception ex)
            {
                con.Close();
                return ageMoyen;
            }

        }


        public Dictionary<string, int> ActivitePlusAdherent()
        {

            int nombre = 0;
            string numeroActivite = "";
            var informations = new Dictionary<string, int>();

            try
            {

                MySqlCommand commande = new MySqlCommand("activite_plus_adherent");
                commande.Connection = con;
                commande.CommandType = System.Data.CommandType.StoredProcedure;

                con.Open();
                MySqlDataReader reader = commande.ExecuteReader();
                while (reader.Read())
                {
                    nombre = Int32.Parse(reader["nombreAdherent"].ToString());
                    numeroActivite = reader["numero_activite"].ToString();

                    informations[numeroActivite] = nombre; //Associer le nombre et la catégorie en ensemble

                }
                reader.Close();
                con.Close();
            }
            catch (MySqlException ex)
            {
                con.Close();
            }
            return informations; //Retourner le dictionnaire de données

        }

    }
}
