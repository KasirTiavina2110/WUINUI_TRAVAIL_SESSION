using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Diagnostics;

namespace class2
{
    public sealed partial class AjouterInscription : Page
    {
        public AjouterInscription()
        {
            this.InitializeComponent();
            ChargerAdherentsEtSeances();
            InitialiserDateEtHeureInscription();
        }

        // Charger la liste des numero_identification et id_seance dans les ComboBox
        private void ChargerAdherentsEtSeances()
        {
            try
            {
                // Charger les adhérents
                var adherents = Singleton.getInstance().GetListeAdherents();

                if (adherents == null || adherents.Count == 0)
                {
                    infoBar.Message = "Aucun adhérent trouvé.";
                    infoBar.IsOpen = true;
                }
                else
                {
                    comboNumeroIdentification.ItemsSource = adherents;
                    Debug.WriteLine($"Nombre d'adhérents récupérés : {adherents.Count}");
                    comboNumeroIdentification.DisplayMemberPath = "NumeroIdentification";
                    comboNumeroIdentification.SelectedValuePath = "NumeroIdentification";
                }

                // Charger les séances
                var seances = Singleton.getInstance().getListeSeance();
                comboIdSeance.ItemsSource = seances;
                comboIdSeance.DisplayMemberPath = "Id_seance";
                comboIdSeance.SelectedValuePath = "Id_seance";
            }
            catch (Exception ex)
            {
                infoBar.Message = $"Erreur lors du chargement des adhérents et des séances : {ex.Message}";
                infoBar.IsOpen = true;
            }
        }


        // Initialiser la date et l'heure d'inscription
        private void InitialiserDateEtHeureInscription()
        {
            datePicker.Date = DateTimeOffset.Now; // Date actuelle
            txtHeureInscription.Text = DateTime.Now.ToString("HH:mm:ss"); // Heure actuelle
        }

        // Bouton Ajouter
        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderChamps())
            {
                infoBar.IsOpen = true;
                return;
            }

            string numeroAdherent = comboNumeroIdentification.SelectedValue?.ToString();
            int idSeance = int.Parse(comboIdSeance.SelectedValue.ToString());
            DateTime dateInscription = datePicker.Date.DateTime.Add(TimeSpan.Parse(txtHeureInscription.Text)); // Combinaison de la date et l'heure

            // Vérifier si l'inscription existe déjà
            if (Singleton.getInstance().VerifierInscriptionExiste(numeroAdherent, idSeance))
            {
                infoBar.Message = "Cette inscription existe déjà.";
                infoBar.IsOpen = true;
                return;
            }

            // Créer une nouvelle inscription
            Inscription nouvelleInscription = new Inscription
            {
                Numero_adherent = numeroAdherent,
                Id_seance = idSeance,
                Date_inscription = dateInscription
            };

            // Ajouter l'inscription
            if (Singleton.getInstance().AjouterInscription(nouvelleInscription))
            {
                Frame.Navigate(typeof(AffichageInscriptions));
            }
            else
            {
                infoBar.Message = "Erreur lors de l'insertion de l'inscription.";
                infoBar.IsOpen = true;
            }
        }

        // Validation des champs
        private bool ValiderChamps()
        {
            infoBar.IsOpen = false;

            if (comboNumeroIdentification.SelectedValue == null)
            {
                infoBar.Message = "Veuillez sélectionner un adhérent.";
                return false;
            }

            if (comboIdSeance.SelectedValue == null)
            {
                infoBar.Message = "Veuillez sélectionner une séance.";
                return false;
            }

            return true;
        }
    }
}
