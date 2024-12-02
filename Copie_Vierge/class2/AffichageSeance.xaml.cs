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

        // Propri�t� pour le r�le de l'utilisateur
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
            // D�finir le r�le de l'utilisateur actuel
            CurrentUserRole = SessionManager.Instance.UsagerConnecte?.Role;
            this.DataContext = this; // D�finir le DataContext
        }

        private void SeanceListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedSeance = e.ClickedItem as Seance;
            var currentUser = SessionManager.Instance.UsagerConnecte;

            if (currentUser != null)
            {
                if (currentUser.Role == "admin")
                {
                    // Naviguer vers la page de modification de la s�ance
                    this.Frame.Navigate(typeof(ModifierSeance), selectedSeance);
                }
                else if (currentUser.Role == "adherent")
                {
                    // Naviguer vers la page d'inscription � la s�ance
                    this.Frame.Navigate(typeof(Inscription), selectedSeance);
                }
            }
            else
            {
                // Aucun utilisateur connect�, afficher un message
                ContentDialog noUserDialog = new ContentDialog()
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Utilisateur non connect�",
                    Content = "Veuillez vous connecter pour continuer.",
                    CloseButtonText = "OK"
                };
                _ = noUserDialog.ShowAsync();
            }
        }

        // Impl�mentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
