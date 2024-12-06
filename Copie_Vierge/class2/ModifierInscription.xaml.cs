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
                    infoBar.Message = "Aucune inscription trouv�e.";
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

        // S�lectionner une inscription dans la liste
        private void ListViewInscriptions_ItemClick(object sender, ItemClickEventArgs e)
        {
            inscriptionActuelle = e.ClickedItem as Inscription;
            if (inscriptionActuelle != null)
            {
                // Afficher le formulaire avec les donn�es de l'inscription
                formPanel.Visibility = Visibility.Visible;

                // Charger les adh�rents et les s�ances
                ChargerAdherentsEtSeances();

                // Remplir les champs du formulaire
                comboNumeroIdentification.SelectedValue = inscriptionActuelle.Numero_adherent;
                comboIdSeance.SelectedValue = inscriptionActuelle.Id_seance;
                datePickerInscription.SelectedDate = inscriptionActuelle.Date_inscription;  // Utilisation directe de DateTime
            }
            else
            {
                // Ajout d'un message d'erreur ou d'une gestion si l'�l�ment n'est pas valide
                infoBar.Message = "L'�l�ment s�lectionn� est invalide.";
                infoBar.IsOpen = true;
            }
        }

        // Charger les adh�rents et les s�ances
        private void ChargerAdherentsEtSeances()
        {
            var adherents = Singleton.getInstance().GetListeAdherents();
            var seances = Singleton.getInstance().getListeSeance();

            comboNumeroIdentification.ItemsSource = adherents;
            comboNumeroIdentification.DisplayMemberPath = "Nom";  // Afficher le nom dans le ComboBox
            comboNumeroIdentification.SelectedValuePath = "NumeroIdentification";

            comboIdSeance.ItemsSource = seances;
            comboIdSeance.DisplayMemberPath = "Numero_activite";  // Afficher l'activit� dans le ComboBox
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

            // Appeler la m�thode ModifierInscription avec tous les param�tres n�cessaires
            if (Singleton.getInstance().ModifierInscription(inscriptionActuelle.Id_inscription, numeroAdherent, idSeance, dateInscription))
            {
                infoBar.Message = "Inscription modifi�e avec succ�s.";
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
                infoBar.Message = "Inscription supprim�e avec succ�s.";
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
                infoBar.Message = "Veuillez s�lectionner un adh�rent.";
                return false;
            }

            if (comboIdSeance.SelectedValue == null)
            {
                infoBar.Message = "Veuillez s�lectionner une s�ance.";
                return false;
            }

            return true;
        }
    }
}
