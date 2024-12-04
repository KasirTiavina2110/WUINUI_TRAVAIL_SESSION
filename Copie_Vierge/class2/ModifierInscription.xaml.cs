using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace class2
{
    public sealed partial class ModifierInscription : Page
    {
        private Inscription inscriptionActuelle;

        public ModifierInscription()
        {
            this.InitializeComponent();
        }

        // Charger la liste des numero_identification et id_seance dans les ComboBox
        private void ChargerAdherentsEtSeances()
        {
            try
            {
                var adherents = Singleton.getInstance().GetListeAdherents();
                comboNumeroIdentification.ItemsSource = adherents;
                comboNumeroIdentification.DisplayMemberPath = "NumeroIdentification";
                comboNumeroIdentification.SelectedValuePath = "NumeroIdentification";

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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Inscription inscription)
            {
                inscriptionActuelle = inscription;
                ChargerAdherentsEtSeances();
                RemplirChamps();
            }
        }

        // Remplir les champs avec les informations actuelles de l'inscription
        private void RemplirChamps()
        {
            if (inscriptionActuelle != null)
            {
                comboNumeroIdentification.SelectedValue = inscriptionActuelle.Numero_adherent;
                comboIdSeance.SelectedValue = inscriptionActuelle.Id_seance;
            }
        }

        // Bouton Modifier
        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (!ValiderChamps())
            {
                infoBar.IsOpen = true;
                return;
            }

            string numeroAdherent = comboNumeroIdentification.SelectedValue.ToString();
            int idSeance = (int)comboIdSeance.SelectedValue;

            // Vérifier si l'inscription existe déjà (autre que l'actuelle)
            if (Singleton.getInstance().VerifierInscriptionExiste(numeroAdherent, idSeance) &&
                (numeroAdherent != inscriptionActuelle.Numero_adherent || idSeance != inscriptionActuelle.Id_seance))
            {
                infoBar.Message = "Une inscription avec ces informations existe déjà.";
                infoBar.IsOpen = true;
                return;
            }

            // Modifier l'inscription dans la base de données
            if (Singleton.getInstance().ModifierInscription(numeroAdherent, idSeance))
            {
                Frame.Navigate(typeof(AffichageInscriptions)); // Rediriger vers la liste des inscriptions
            }
            else
            {
                infoBar.Message = "Erreur lors de la modification de l'inscription.";
                infoBar.IsOpen = true;
            }
        }

        // Bouton Supprimer
        private void btnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (inscriptionActuelle != null)
            {
                if (Singleton.getInstance().SupprimerInscription(inscriptionActuelle.Numero_adherent, inscriptionActuelle.Id_seance))
                {
                    Frame.Navigate(typeof(AffichageInscriptions)); // Rediriger après suppression
                }
                else
                {
                    infoBar.Message = "Erreur lors de la suppression de l'inscription.";
                    infoBar.IsOpen = true;
                }
            }
            else
            {
                infoBar.Message = "Impossible de supprimer l'inscription actuelle.";
                infoBar.IsOpen = true;
            }
        }

        // Bouton Annuler
        private void btnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack(); // Retourner à la page précédente
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
