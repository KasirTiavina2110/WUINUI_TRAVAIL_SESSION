using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace class2
{
    public sealed partial class ModifierSeance : Page
    {
        private Seance currentSeance;

        public ModifierSeance()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // R�cup�rer la s�ance s�lectionn�e pass�e via la navigation
            if (e.Parameter is Seance seance)
            {
                currentSeance = seance;
                RemplirChamps();
            }

            // Charger la liste des activit�s pour le ComboBox
            numero_activite.ItemsSource = Singleton.getInstance().ObtenirNumeroActivites();
        }

        private void RemplirChamps()
        {
            // Remplir les champs avec les donn�es de la s�ance
            if (currentSeance != null)
            {
                id_seance.Text = currentSeance.Id_seance.ToString();
                numero_activite.SelectedItem = currentSeance.Numero_activite;
                date.Date = currentSeance.Date;
                heure.Time = currentSeance.Heure;
                place_dispo.Text = currentSeance.Place_dispo?.ToString();
                place_prise.Text = currentSeance.Place_prise?.ToString();
                place_max.Text = currentSeance.Place_max.ToString();
            }
        }

        private void btn_modifier_Click(object sender, RoutedEventArgs e)
        {
            // Valider et mettre � jour la s�ance
            operation_reussie.Text = string.Empty;

            if (ValiderChamps())
            {
                currentSeance.Numero_activite = numero_activite.SelectedItem?.ToString();
                currentSeance.Date = date.Date.DateTime;
                currentSeance.Heure = heure.Time;
                currentSeance.Place_dispo = string.IsNullOrWhiteSpace(place_dispo.Text) ? null : int.Parse(place_dispo.Text);
                currentSeance.Place_prise = string.IsNullOrWhiteSpace(place_prise.Text) ? null : int.Parse(place_prise.Text);
                currentSeance.Place_max = int.Parse(place_max.Text);

                bool reussi = Singleton.getInstance().ModifierSeance(currentSeance);
                operation_reussie.Text = reussi ? "Modification r�ussie" : "Erreur lors de la modification";
            }
        }

        private bool ValiderChamps()
        {
            bool valide = true;

            // R�initialiser les erreurs
            erreur_numero_activite.Text = erreur_date.Text = erreur_heure.Text =
                erreur_place_dispo.Text = erreur_place_prise.Text = erreur_place_max.Text = string.Empty;

            // Validation des champs
            if (numero_activite.SelectedIndex == -1)
            {
                erreur_numero_activite.Text = "L'activit� est obligatoire.";
                valide = false;
            }

            // V�rifier si une date valide est s�lectionn�e
            if (date.Date == default)
            {
                erreur_date.Text = "La date est obligatoire.";
                valide = false;
            }

            // V�rifier si une heure valide est s�lectionn�e
            if (!heure.SelectedTime.HasValue)
            {
                erreur_heure.Text = "L'heure est obligatoire.";
                valide = false;
            }

            // V�rifier si le nombre maximal de places est valide
            if (!int.TryParse(place_max.Text, out _) || int.Parse(place_max.Text) <= 0)
            {
                erreur_place_max.Text = "Le nombre maximal de places doit �tre un entier positif.";
                valide = false;
            }

            return valide;
        }



        private void btn_annuler_Click(object sender, RoutedEventArgs e)
        {
            // R�initialiser les champs
            RemplirChamps();
        }

        private void btn_supprimer_Click(object sender, RoutedEventArgs e)
        {
            // Supprimer la s�ance
            bool reussi = Singleton.getInstance().SupprimerSeance(currentSeance.Id_seance);

            if (reussi)
            {
                operation_reussie.Text = "S�ance supprim�e avec succ�s.";
                this.Frame.GoBack();
            }
            else
            {
                operation_reussie.Text = "Erreur lors de la suppression.";
            }
        }
    }
}
