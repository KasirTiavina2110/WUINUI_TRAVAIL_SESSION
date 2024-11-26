﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace class2
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            // Page par défaut affichée
            mainFrame.Navigate(typeof(AffichageSeance));

            // Masque les options administratives au démarrage
            adminSection.Visibility = Visibility.Collapsed;
        }

        private async void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            var item = args.SelectedItem as NavigationViewItem;

            switch (item.Name)
            {
                case "voirActivite":
                    mainFrame.Navigate(typeof(AffichageSeance));
                    break;
                case "ajouterUsager":
                    mainFrame.Navigate(typeof(AjouterUsager));
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
                    DeconnecterUsager();
                    break;
                default:
                    break;
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
                mainFrame.Navigate(typeof(AffichageSeance));
                adminSection.Visibility = Visibility.Collapsed; // Cache les options admin
            }
        }

        private void DeconnecterUsager()
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
                _ = dialog.ShowAsync();
                return;
            }

            // Déconnecte l'utilisateur
            SessionManager.Instance.DeconnecterUsager();

            // Retourne à la page d'accueil
            mainFrame.Navigate(typeof(AffichageSeance));

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
            _ = successDialog.ShowAsync();
        }

    }
}
