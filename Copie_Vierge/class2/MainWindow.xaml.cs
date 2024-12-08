using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

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
                    case "voirStatistique":
                        mainFrame.Navigate(typeof(VoirStatistique));
                        break;
                    case "exporterCsvAdherent":
                        await ExporterCsvAdherent();
                        break;
                    case "exporterCsvActivite":
                        await ExporterCsvActivite();
                        break;
                    case "mesSeances":

                        var currentUser = SessionManager.Instance.UsagerConnecte;

                        if (currentUser != null)
                        {
                            mainFrame.Navigate(typeof(VoirMesSeances));
                        }
                        else
                        {
                            var modalConnexion2 = new ModalConnexion();
                            modalConnexion2.XamlRoot = navView.XamlRoot;
                            await modalConnexion2.ShowAsync();

                            // Redirection après connexion
                            if (SessionManager.Instance.UsagerConnecte != null)
                            {
                                RedirigerSelonRole();
                            }

                        }
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

                    // Gestion des inscriptions
                    case "ajouterInscription":
                        mainFrame.Navigate(typeof(AjouterInscription));
                        break;
                    case "modifierInscription":
                        mainFrame.Navigate(typeof(ModifierInscription));
                        break;
                    case "obtenirListeInscription":
                        mainFrame.Navigate(typeof(AffichageInscriptions));
                        break;
                    default:
                        break;
                }
            }
        }

        // Gestion du clic sur le bouton "Retour"
        private void NavView_BackRequested(NavigationView sender, NavigationViewBackRequestedEventArgs args)
        {
            if (mainFrame.CanGoBack)
            {
                mainFrame.GoBack();
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

        //Exporter un CSV
        public async Task ExporterCsvAdherent()
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "Liste des adhérents";
            picker.FileTypeChoices.Add("Liste des adhérents", new List<string>() { ".csv" });

            //crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            if (monFichier != null)
            {
                //Aller chercher la liste
                var liste = Singleton.getInstance().GetListeAdherents();

                if(liste != null)
                {
                    //Convertir en List pour pouvoir ConvertAll
                    List<Usager> listeUsager = liste.ToList();

                    if (liste != null && liste.Count > 0)
                    {
                        await Windows.Storage.FileIO.WriteLinesAsync(monFichier, listeUsager.ConvertAll(x => x.ExportationCSV), Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    }
                    else
                    {
                        await Windows.Storage.FileIO.WriteTextAsync(monFichier, "Aucune donnée à exporter", Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    }
                }
                else
                {
                    await Windows.Storage.FileIO.WriteTextAsync(monFichier, "Erreur : La liste des usagers est null.", Windows.Storage.Streams.UnicodeEncoding.Utf8);
                }
            }

        }

        public async Task ExporterCsvActivite()
        {
            var picker = new Windows.Storage.Pickers.FileSavePicker();

            var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            WinRT.Interop.InitializeWithWindow.Initialize(picker, hWnd);

            picker.SuggestedFileName = "Liste des activités";
            picker.FileTypeChoices.Add("Liste des activités", new List<string>() { ".csv" });

            //crée le fichier
            Windows.Storage.StorageFile monFichier = await picker.PickSaveFileAsync();

            if (monFichier != null)
            {
                //Aller chercher la liste
                var liste = Singleton.getInstance().getListeActivite();

                if (liste != null)
                {
                    //Convertir en List pour pouvoir ConvertAll
                    List<Activite> listeActivite = liste.ToList();

                    if (liste != null && liste.Count > 0)
                    {
                        await Windows.Storage.FileIO.WriteLinesAsync(monFichier, listeActivite.ConvertAll(x => x.ExportationCSV), Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    }
                    else
                    {
                        await Windows.Storage.FileIO.WriteTextAsync(monFichier, "Aucune donnée à exporter", Windows.Storage.Streams.UnicodeEncoding.Utf8);
                    }
                }
                else
                {
                    await Windows.Storage.FileIO.WriteTextAsync(monFichier, "Erreur : La liste des activités est null.", Windows.Storage.Streams.UnicodeEncoding.Utf8);
                }
            }
        }
    }
}
