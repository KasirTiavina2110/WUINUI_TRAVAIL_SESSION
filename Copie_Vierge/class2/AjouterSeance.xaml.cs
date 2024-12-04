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
                comboNumeroActivite.SelectedValuePath = "Id"; // Valeur s�lectionn�e
            }
            catch (Exception ex)
            {
                infoBar.Message = $"Erreur lors du chargement des activit�s : {ex.Message}";
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

            // Cr�er une nouvelle s�ance � partir des donn�es saisies
            Seance nouvelleSeance = new Seance
            {
                Numero_activite = comboNumeroActivite.SelectedValue.ToString(),
                Date = datePicker.Date.DateTime,
                Heure = timePicker.Time,
                Place_dispo = int.Parse(txtPlaceDispo.Text),
                Place_prise = int.Parse(txtPlacePrise.Text),
                Place_max = int.Parse(txtPlaceMax.Text)
            };

            // V�rifier si la s�ance existe d�j�
            if (Singleton.getInstance().VerifierSeanceExiste(nouvelleSeance))
            {
                infoBar.Message = "Une s�ance avec ces informations existe d�j�.";
                infoBar.IsOpen = true;
                return;
            }

            // Ajouter la s�ance dans la base de donn�es
            if (Singleton.getInstance().AjouterSeance(nouvelleSeance))
            {
                // Rediriger vers la liste des s�ances apr�s insertion r�ussie
                Frame.Navigate(typeof(AffichageSeance));
            }
            else
            {
                infoBar.Message = "Erreur lors de l'insertion de la s�ance.";
                infoBar.IsOpen = true;
            }
        }

        // Validation des champs
        private bool ValiderChamps()
        {
            infoBar.IsOpen = false;

            if (comboNumeroActivite.SelectedValue == null)
            {
                infoBar.Message = "Veuillez s�lectionner une activit�.";
                return false;
            }
            if (datePicker.Date == default)
            {
                infoBar.Message = "Veuillez s�lectionner une date.";
                return false;
            }
            if (datePicker.Date.DateTime < DateTime.Now.Date)
            {
                infoBar.Message = "La date de la s�ance ne peut pas �tre ant�rieure � la date actuelle.";
                return false;
            }
            if (timePicker.Time == default)
            {
                infoBar.Message = "Veuillez s�lectionner une heure.";
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
                infoBar.Message = "Le nombre de places maximales doit �tre un entier positif.";
                return false;
            }

            // V�rifier que Place_dispo est �gal � Place_max
            if (placeDispo != placeMax)
            {
                infoBar.Message = "Le nombre de places disponibles doit �tre �gal au nombre de places maximales.";
                return false;
            }

            // V�rifier que Place_prise est inf�rieur ou �gal � Place_max
            if (placePrise > placeMax)
            {
                infoBar.Message = "Le nombre de places prises ne peut pas �tre sup�rieur au nombre de places maximales.";
                return false;
            }

            return true;
        }
    }
}
