using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using MySql.Data.MySqlClient; // Assurez-vous que ceci est inclus

namespace class2
{
    public sealed partial class AffichageSeance : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Seance> _seanceList;
        public ObservableCollection<Seance> SeanceList
        {
            get { return _seanceList; }
            set
            {
                _seanceList = value;
                OnPropertyChanged(nameof(SeanceList));
            }
        }

        // Propriété pour le rôle de l'utilisateur
        private string _currentUserRole;
        public string CurrentUserRole
        {
            get { return _currentUserRole; }
            set
            {
                _currentUserRole = value;
                OnPropertyChanged(nameof(CurrentUserRole));
            }
        }

        public AffichageSeance()
        {
            this.InitializeComponent();
            SeanceList = Singleton.getInstance().getListeSeance();
            // Définir le rôle de l'utilisateur actuel
            CurrentUserRole = SessionManager.Instance.UsagerConnecte?.Role;
            this.DataContext = this; // Définir le DataContext
        }

        private void SeanceListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedSeance = e.ClickedItem as Seance;
            var currentUser = SessionManager.Instance.UsagerConnecte;

            if (currentUser != null)
            {
                if (currentUser.Role == "admin")
                {
                    // Naviguer vers la page de modification de la séance
                    this.Frame.Navigate(typeof(ModifierSeance), selectedSeance);
                }
                else if (currentUser.Role == "adherent")
                {
                    // Naviguer vers la page d'inscription à la séance
                    this.Frame.Navigate(typeof(Inscription), selectedSeance);
                }
            }
            else
            {
                // Aucun utilisateur connecté, afficher un message
                ContentDialog noUserDialog = new ContentDialog()
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Utilisateur non connecté",
                    Content = "Veuillez vous connecter pour continuer.",
                    CloseButtonText = "OK"
                };
                _ = noUserDialog.ShowAsync();
            }
        }

        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
