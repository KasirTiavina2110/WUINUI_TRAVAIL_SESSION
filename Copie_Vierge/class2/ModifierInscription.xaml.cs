using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

namespace class2
{
    public sealed partial class ModifierInscription : Page
    {
        private ObservableCollection<Inscription> inscriptions;

        public ModifierInscription()
        {
            this.InitializeComponent();
            ChargerInscriptions();
        }

        // Charger les inscriptions depuis la base de donn�es
        private void ChargerInscriptions()
        {
            inscriptions = new ObservableCollection<Inscription>(Singleton.getInstance().GetListeInscriptions());
            listViewInscriptions.ItemsSource = inscriptions;
        }

        // Gestionnaire pour le bouton Modifier
        private async void ModifierButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var inscription = button?.CommandParameter as Inscription;

            if (inscription == null) return;

            // Charger les adh�rents et s�ances depuis la base de donn�es
            var adherents = Singleton.getInstance().GetListeAdherents();
            var seances = Singleton.getInstance().getListeSeance();

            // Ouvrir le modal pour modifier l'inscription
            var modal = new ModalModifInscription(adherents, seances, inscription);

            // Associer explicitement le XamlRoot pour �viter le conflit
            modal.XamlRoot = this.XamlRoot;

            var result = await modal.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                // Mettre � jour les donn�es de l'inscription
                inscription.Numero_adherent = modal.NumeroAdherent;
                inscription.Id_seance = modal.IdSeance;
                inscription.Date_inscription = modal.DateInscription;

                // Mise � jour dans la base de donn�es
                if (Singleton.getInstance().ModifierInscription(inscription))
                {
                    infoBar.Message = "Inscription modifi�e avec succ�s.";
                    infoBar.Severity = InfoBarSeverity.Success;
                    infoBar.IsOpen = true;

                    ChargerInscriptions(); // Rafra�chir la liste des inscriptions
                }
                else
                {
                    infoBar.Message = "Erreur lors de la modification de l'inscription.";
                    infoBar.Severity = InfoBarSeverity.Error;
                    infoBar.IsOpen = true;
                }
            }
        }


        // Gestionnaire pour le bouton Supprimer
        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var inscription = button?.CommandParameter as Inscription;

            if (inscription == null) return;

            // Supprimer l'inscription
            if (Singleton.getInstance().SupprimerInscription(inscription.Id_inscription))
            {
                infoBar.Message = "Inscription supprim�e avec succ�s.";
                infoBar.Severity = InfoBarSeverity.Success;
                infoBar.IsOpen = true;

                inscriptions.Remove(inscription); // Retirer de la liste
            }
            else
            {
                infoBar.Message = "Erreur lors de la suppression de l'inscription.";
                infoBar.Severity = InfoBarSeverity.Error;
                infoBar.IsOpen = true;
            }
        }

        // Gestionnaire pour le clic sur un �l�ment de la liste
        private void ListViewInscriptions_ItemClick(object sender, ItemClickEventArgs e)
        {
            var inscription = e.ClickedItem as Inscription;

            if (inscription != null)
            {
                infoBar.Message = $"Inscription s�lectionn�e : Adh�rent {inscription.Numero_adherent}, S�ance {inscription.Id_seance}.";
                infoBar.Severity = InfoBarSeverity.Informational;
                infoBar.IsOpen = true;
            }
        }
    }
}
