using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace class2
{
    public class SessionManager
    {
        private static SessionManager _instance;
        private readonly string _connectionString;
        public Usager UsagerConnecte { get; private set; }

        private SessionManager()
        {
            // Remplacez par vos informations de connexion MySQL
            _connectionString = "Server=cours.cegep3r.info;Database=a2024_420335ri_eq11;Uid=1748697;Pwd=1748697;";
        }

        public static SessionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SessionManager();
                }
                return _instance;
            }
        }

        public Usager AuthentifierUsager(string numeroIdentification, string motDePasse, string role)
        {
            using (var connection = new MySqlConnection(_connectionString))
            {
                connection.Open();

                string query = @"SELECT * FROM adherent 
                             WHERE numero_identification = @numero 
                             AND mot_de_passe = @motDePasse 
                             AND role = @role";

                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@numero", numeroIdentification);
                    command.Parameters.AddWithValue("@motDePasse", motDePasse);
                    command.Parameters.AddWithValue("@role", role);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usager
                            {
                                NumeroIdentification = reader["numero_identification"].ToString(),
                                Nom = reader["nom"].ToString(),
                                Prenom = reader["prenom"].ToString(),
                                Adresse = reader["adresse"].ToString(),
                                DateNaissance = DateTime.Parse(reader["date_naissance"].ToString()),
                                Age = int.Parse(reader["age"].ToString()),
                                Role = reader["role"].ToString(),
                                MotDePasse = reader["mot_de_passe"].ToString()
                            };
                        }
                    }
                }
            }

            return null; // Aucun usager trouvé
        }

        public void ConnecterUsager(Usager usager)
        {
            UsagerConnecte = usager;
        }

        public void DeconnecterUsager()
        {
            UsagerConnecte = null;
        }
    }

}
