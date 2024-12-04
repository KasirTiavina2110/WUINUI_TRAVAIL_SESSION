using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace class2
{
    public sealed partial class AjouterInscription : Page
    {
        public AjouterInscription()
        {
            this.InitializeComponent();
            ChargerAdherentsEtSeances();
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
                infoBar.Message = $"Erreur lors du chargement des adh�rents et des s�ances : {ex.Message}";
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

            string numeroAdherent = comboNumeroIdentification.SelectedValue.ToString();
            int idSeance = (int)comboIdSeance.SelectedValue;

            // V�rifier si l'inscription existe d�j�
            if (Singleton.getInstance().VerifierInscriptionExiste(numeroAdherent, idSeance))
            {
                infoBar.Message = "Cette inscription existe d�j�.";
                infoBar.IsOpen = true;
                return;
            }

            // Ajouter l'inscription dans la base de donn�es
            if (Singleton.getInstance().AjouterInscription(numeroAdherent, idSeance))
            {
                // Rediriger vers la liste des inscriptions apr�s insertion r�ussie
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
