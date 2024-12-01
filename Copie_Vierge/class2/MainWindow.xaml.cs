using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace class2
{
    public sealed partial class MainWindow // Retirer ': Window' ici
    {
        public MainWindow()
        {
            this.InitializeComponent();
            // Page par défaut affichée
            mainFrame.Navigate(typeof(AffichageActivite));

            // Masque les options administratives au démarrage
            adminSection.Visibility = Visibility.Collapsed;
        }

        private async void navView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            var invokedItem = args.InvokedItemContainer as NavigationViewItem;

            if (invokedItem != null)
            {
                switch (invokedItem.Name)
                {
                    case "voirActivite":
                        mainFrame.Navigate(typeof(AffichageActivite));
                        break;
                    case "voirSeance":
                        mainFrame.Navigate(typeof(AffichageSeance));
                        break;
                    case "ajouterUsager":
                        mainFrame.Navigate(typeof(AjouterUsager));
                        break;
                    case "supprimerUsager":
                        mainFrame.Navigate(typeof(SupprimerUsager));
                        break;
                    case "modifierUsager":
                        mainFrame.Navigate(typeof(ModifierUsager));
                        break;
                    case "ajouterActivite":
                        mainFrame.Navigate(typeof(AjouterActivite));
                        break;
                    case "supprimerActivite":
                        mainFrame.Navigate(typeof(SupprimerActivite));
                        break;
                    case "modifierActivite":
                        mainFrame.Navigate(typeof(ModifierActivite));
                        break;
                    case "ajouterSeance":
                        mainFrame.Navigate(typeof(AjouterSeance));
                        break;
                    case "supprimerSeance":
                        mainFrame.Navigate(typeof(SupprimerSeance));
                        break;
                    case "modifierSeance":
                        mainFrame.Navigate(typeof(ModifierSeance));
                        break;
                    case "voirStatistique":
                        mainFrame.Navigate(typeof(VoirStatistique));
                        break;
                    case "connexion":
                        var modalConnexion = new ModalConnexion();
                        modalConnexion.XamlRoot = navView.XamlRoot;
                        await modalConnexion.ShowAsync();

                        // Redirection après connexion
                        if (SessionManager.Instance.UsagerConnecte != null)
                        {
                            RedirigerSelonRole();
                        }
                        break;
                    case "deconnexion":
                        await DeconnecterUsager();
                        break;
                    default:
                        break;
                }
            }
        }

        private void RedirigerSelonRole()
        {
            var usager = SessionManager.Instance.UsagerConnecte;

            if (usager.Role == "admin")
            {
                mainFrame.Navigate(typeof(ModifierUsager));
                adminSection.Visibility = Visibility.Visible; // Montre les options admin
            }
            else if (usager.Role == "adherent")
            {
                mainFrame.Navigate(typeof(AffichageActivite));
                adminSection.Visibility = Visibility.Collapsed; // Cache les options admin
            }
        }

        private async System.Threading.Tasks.Task DeconnecterUsager()
        {
            // Vérifie si aucun utilisateur n'est connecté
            if (SessionManager.Instance.UsagerConnecte == null)
            {
                // Affiche une notification indiquant qu'il n'y a pas d'utilisateur connecté
                var dialog = new ContentDialog
                {
                    Title = "Déconnexion",
                    Content = "Vous n'êtes pas connecté.",
                    CloseButtonText = "OK",
                    XamlRoot = navView.XamlRoot
                };
                await dialog.ShowAsync();
                return;
            }

            // Déconnecte l'utilisateur
            SessionManager.Instance.DeconnecterUsager();

            // Retourne à la page d'accueil
            mainFrame.Navigate(typeof(AffichageActivite));

            // Cache les options administratives
            adminSection.Visibility = Visibility.Collapsed;

            // Affiche une notification de déconnexion réussie
            var successDialog = new ContentDialog
            {
                Title = "Déconnexion",
                Content = "Vous avez été déconnecté avec succès.",
                CloseButtonText = "OK",
                XamlRoot = navView.XamlRoot
            };
            await successDialog.ShowAsync();
        }
    }
}
