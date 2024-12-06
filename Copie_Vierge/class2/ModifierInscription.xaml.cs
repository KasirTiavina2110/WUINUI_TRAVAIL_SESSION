using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Linq;
using System.Collections.Generic;

namespace class2
{
    public sealed partial class ModifierInscription : Page
    {
        private Inscription inscriptionActuelle;

        public ModifierInscription()
        {
            this.InitializeComponent();
        }

        // Charger la liste des inscriptions
        private void ChargerInscriptions()
        {
            try
            {
                var inscriptions = Singleton.getInstance().GetListeInscriptions();

                if (inscriptions == null || !inscriptions.Any())
                {
                    infoBar.Message = "Aucune inscription trouvée.";
                    infoBar.IsOpen = true;
                }
                else
                {
                    listViewInscriptions.ItemsSource = inscriptions;
                }
            }
            catch (Exception ex)
            {
                infoBar.Message = $"Erreur lors du chargement des inscriptions : {ex.Message}";
                infoBar.IsOpen = true;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            ChargerInscriptions();  // Charger toutes les inscriptions
        }

        // Sélectionner une inscription dans la liste
        private void ListViewInscriptions_ItemClick(object sender, ItemClickEventArgs e)
        {
            inscriptionActuelle = e.ClickedItem as Inscription;
            if (inscriptionActuelle != null)
            {
                // Afficher le formulaire avec les données de l'inscription
                formPanel.Visibility = Visibility.Visible;

                // Charger les adhérents et les séances
                ChargerAdherentsEtSeances();

                // Remplir les champs du formulaire
                comboNumeroIdentification.SelectedValue = inscriptionActuelle.Numero_adherent;
                comboIdSeance.SelectedValue = inscriptionActuelle.Id_seance;
                datePickerInscription.SelectedDate = inscriptionActuelle.Date_inscription;  // Utilisation directe de DateTime
            }
            else
            {
                // Ajout d'un message d'erreur ou d'une gestion si l'élément n'est pas valide
                infoBar.Message = "L'élément sélectionné est invalide.";
                infoBar.IsOpen = true;
            }
        }

        // Charger les adhérents et les séances
        private void ChargerAdherentsEtSeances()
        {
            var adherents = Singleton.getInstance().GetListeAdherents();
            var seances = Singleton.getInstance().getListeSeance();

            comboNumeroIdentification.ItemsSource = adherents;
            comboNumeroIdentification.DisplayMemberPath = "Nom";  // Afficher le nom dans le ComboBox
            comboNumeroIdentification.SelectedValuePath = "NumeroIdentification";

            comboIdSeance.ItemsSource = seances;
            comboIdSeance.DisplayMemberPath = "Numero_activite";  // Afficher l'activité dans le ComboBox
            comboIdSeance.SelectedValuePath = "Id_seance";
        }

        // Modifier l'inscription
        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderChamps())
            {
                infoBar.IsOpen = true;
                return;
            }

            string numeroAdherent = comboNumeroIdentification.SelectedValue.ToString();
            int idSeance = (int)comboIdSeance.SelectedValue;

            // Obtenir la date de l'inscription, ou utiliser DateTime.Now si la date est vide
            DateTime dateInscription = datePickerInscription.SelectedDate?.DateTime ?? DateTime.Now;

            // Appeler la méthode ModifierInscription avec tous les paramètres nécessaires
            if (Singleton.getInstance().ModifierInscription(inscriptionActuelle.Id_inscription, numeroAdherent, idSeance, dateInscription))
            {
                infoBar.Message = "Inscription modifiée avec succès.";
                infoBar.Severity = InfoBarSeverity.Success;
                infoBar.IsOpen = true;
                ChargerInscriptions(); // Recharger la liste des inscriptions
                formPanel.Visibility = Visibility.Collapsed; // Masquer le formulaire
            }
            else
            {
                infoBar.Message = "Erreur lors de la modification de l'inscription.";
                infoBar.IsOpen = true;
            }
        }

        // Annuler et masquer le formulaire
        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            formPanel.Visibility = Visibility.Collapsed; // Masquer le formulaire
        }

        // Supprimer l'inscription
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var inscription = button?.DataContext as Inscription;

            if (inscription != null && Singleton.getInstance().SupprimerInscription(inscription.Id_inscription))
            {
                infoBar.Message = "Inscription supprimée avec succès.";
                infoBar.Severity = InfoBarSeverity.Success;
                infoBar.IsOpen = true;
                ChargerInscriptions(); // Recharger la liste des inscriptions
            }
            else
            {
                infoBar.Message = "Erreur lors de la suppression de l'inscription.";
                infoBar.IsOpen = true;
            }
        }

        // Validation des champs
        private bool ValiderChamps()
        {
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
