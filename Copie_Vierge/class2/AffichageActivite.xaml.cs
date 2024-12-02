using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace class2
{
    public sealed partial class AffichageActivite : Page, INotifyPropertyChanged
    {
        private ObservableCollection<Activite> _activitiesList;
        public ObservableCollection<Activite> ActivitiesList
        {
            get { return _activitiesList; }
            set
            {
                _activitiesList = value;
                OnPropertyChanged(nameof(ActivitiesList));
            }
        }

        public AffichageActivite()
        {
            this.InitializeComponent();
            ActivitiesList = Singleton.getInstance().getListeActivite();
        }

        private void ActivitiesGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var selectedActivity = e.ClickedItem as Activite;
            var currentUser = SessionManager.Instance.UsagerConnecte;

            if (currentUser != null)
            {
                if (currentUser.Role == "admin")
                {
                    // Naviguer vers la page de modification de l'activité
                    this.Frame.Navigate(typeof(ModifierActivitePage), selectedActivity.Id);
                }
                else if (currentUser.Role == "adherent")
                {
                    // Naviguer vers la page d'inscription à une séance
                    this.Frame.Navigate(typeof(VoirSeance), selectedActivity.Id);
                }
            }
            else
            {
                // Aucun utilisateur connecté, rediriger vers la page de connexion ou afficher un message
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

        private void OnPropertyChanged(string propertyName) /*Important de ne pas oublier ça*/
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
