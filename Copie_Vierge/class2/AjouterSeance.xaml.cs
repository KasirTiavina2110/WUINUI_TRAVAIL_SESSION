using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace class2
{
    public sealed partial class AjouterSeance : Page
    {
        public AjouterSeance()
        {
            this.InitializeComponent();
            ChargerNumeroActivites();
        }

        // Charger la liste des numero_activite dans le ComboBox
        private void ChargerNumeroActivites()
        {
            try
            {
                var activites = Singleton.getInstance().getListeActivite();
                comboNumeroActivite.ItemsSource = activites;
                comboNumeroActivite.DisplayMemberPath = "Id"; // Affiche l'id_activite dans le ComboBox
                comboNumeroActivite.SelectedValuePath = "Id"; // Valeur sélectionnée
            }
            catch (Exception ex)
            {
                infoBar.Message = $"Erreur lors du chargement des activités : {ex.Message}";
                infoBar.IsOpen = true;
            }
        }

        // Bouton Ajouter
        private void btnAjouter_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderChamps())
            {
                infoBar.IsOpen = true;
                return;
            }

            // Créer une nouvelle séance à partir des données saisies
            Seance nouvelleSeance = new Seance
            {
                Numero_activite = comboNumeroActivite.SelectedValue.ToString(),
                Date = datePicker.Date.DateTime,
                Heure = timePicker.Time,
                Place_dispo = int.Parse(txtPlaceDispo.Text),
                Place_prise = int.Parse(txtPlacePrise.Text),
                Place_max = int.Parse(txtPlaceMax.Text)
            };

            // Vérifier si la séance existe déjà
            if (Singleton.getInstance().VerifierSeanceExiste(nouvelleSeance))
            {
                infoBar.Message = "Une séance avec ces informations existe déjà.";
                infoBar.IsOpen = true;
                return;
            }

            // Ajouter la séance dans la base de données
            if (Singleton.getInstance().AjouterSeance(nouvelleSeance))
            {
                // Rediriger vers la liste des séances après insertion réussie
                Frame.Navigate(typeof(AffichageSeance));
            }
            else
            {
                infoBar.Message = "Erreur lors de l'insertion de la séance.";
                infoBar.IsOpen = true;
            }
        }

        // Validation des champs
        private bool ValiderChamps()
        {
            infoBar.IsOpen = false;

            if (comboNumeroActivite.SelectedValue == null)
            {
                infoBar.Message = "Veuillez sélectionner une activité.";
                return false;
            }
            if (datePicker.Date == default)
            {
                infoBar.Message = "Veuillez sélectionner une date.";
                return false;
            }
            if (datePicker.Date.DateTime < DateTime.Now.Date)
            {
                infoBar.Message = "La date de la séance ne peut pas être antérieure à la date actuelle.";
                return false;
            }
            if (timePicker.Time == default)
            {
                infoBar.Message = "Veuillez sélectionner une heure.";
                return false;
            }
            if (!int.TryParse(txtPlaceDispo.Text, out int placeDispo))
            {
                infoBar.Message = "Le nombre de places disponibles est invalide.";
                return false;
            }
            if (!int.TryParse(txtPlacePrise.Text, out int placePrise))
            {
                infoBar.Message = "Le nombre de places prises est invalide.";
                return false;
            }
            if (!int.TryParse(txtPlaceMax.Text, out int placeMax) || placeMax <= 0)
            {
                infoBar.Message = "Le nombre de places maximales doit être un entier positif.";
                return false;
            }

            // Vérifier que Place_dispo est égal à Place_max
            if (placeDispo != placeMax)
            {
                infoBar.Message = "Le nombre de places disponibles doit être égal au nombre de places maximales.";
                return false;
            }

            // Vérifier que Place_prise est inférieur ou égal à Place_max
            if (placePrise > placeMax)
            {
                infoBar.Message = "Le nombre de places prises ne peut pas être supérieur au nombre de places maximales.";
                return false;
            }

            return true;
        }
    }
}
