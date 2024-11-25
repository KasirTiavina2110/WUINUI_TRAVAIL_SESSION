using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Collections.ObjectModel;



namespace class2
{
    internal class Singleton
    {

        MySqlConnection con;
        ObservableCollection<Activite> listeActivite;
   
        public static Singleton instance = null;


        public Singleton()
        {
            con = new MySqlConnection("Server=cours.cegep3r.info;Database=a2024_420335ri_eq11;Uid=1748697;Pwd=1748697;");
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

                    Activite activite = new Activite(id,nom, annee, type, cout_organisation, vente_client);
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


    }
}
