using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace class2
{
    public sealed partial class AffichageInscriptions : Page
    {
        public AffichageInscriptions()
        {
            this.InitializeComponent();
            ChargerInscriptions();
        }

        // Charger les inscriptions depuis le Singleton
        private void ChargerInscriptions()
        {
            try
            {
                var inscriptions = Singleton.getInstance().ObtenirListeInscriptions();
                listViewInscriptions.ItemsSource = inscriptions;
            }
            catch (Exception ex)
            {
                infoBar.Message = $"Erreur lors du chargement des inscriptions : {ex.Message}";
                infoBar.IsOpen = true;
            }
        }

        // Gestion de l'événement ItemClick pour ListView
        private void listViewInscriptions_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Inscription selectedInscription)
            {
                Frame.Navigate(typeof(ModifierInscription), selectedInscription);
            }
            else
            {
                infoBar.Message = "Veuillez sélectionner une inscription à modifier.";
                infoBar.IsOpen = true;
            }
        }

        // Bouton Modifier
        private void btnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (listViewInscriptions.SelectedItem is Inscription selectedInscription)
            {
                // Naviguer vers ModifierInscription en passant l'inscription sélectionnée
                Frame.Navigate(typeof(ModifierInscription), selectedInscription);
            }
            else
            {
                infoBar.Message = "Veuillez sélectionner une inscription à modifier.";
                infoBar.IsOpen = true;
            }
        }
    }
}
