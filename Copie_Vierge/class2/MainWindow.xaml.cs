using @class2;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace class2
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
  

    public sealed partial class MainWindow : Window, INotifyPropertyChanged
    {
       


        public event PropertyChangedEventHandler PropertyChanged;

        public MainWindow()
        {
            this.InitializeComponent();
            mainFrame.Navigate(typeof(AffichageSeance));

        }
        private async void navView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            //On vient chercher les éléments de Type NavigationViewItem (comme la balise)
            var item = args.SelectedItem as NavigationViewItem;

            //On fait un switch pour aller chercher le x:Name
            switch (item.Name)
            {
                case "voirActivite":
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
                case "ajouterSeance":
                    mainFrame.Navigate(typeof(AjouterSeance));
                    break;
                case "voirStatistique":
                    mainFrame.Navigate(typeof(VoirStatistique));
                    break;
                case "connexion":

                    ModalConnexion modalConnexion = new ModalConnexion();

                    modalConnexion.XamlRoot = navView.XamlRoot;

                    modalConnexion.PrimaryButtonText = "Se connecter";
                    modalConnexion.Title = "Informations de connexion";
                    modalConnexion.CloseButtonText = "Annuler";

                    var result = await modalConnexion.ShowAsync();

                    break;
                default:
                    break;
            }
        }

    }
}
