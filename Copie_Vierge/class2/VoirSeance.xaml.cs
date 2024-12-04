using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MySql.Data.MySqlClient;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace class2
{
    public sealed partial class VoirSeance : Page, INotifyPropertyChanged
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

        private string _activityCode;
        public string ActivityCode
        {
            get { return _activityCode; }
            set
            {
                if (_activityCode != value) // Vérifie si la valeur a changé
                {
                    _activityCode = value;
                    OnPropertyChanged(nameof(ActivityCode));
                }
            }
        }


        // Propriétés pour InfoBar
        private string _infoBarTitle;
        public string InfoBarTitle
        {
            get { return _infoBarTitle; }
            set
            {
                if (_infoBarTitle != value)
                {
                    Debug.WriteLine($"InfoBarTitle changé : {_infoBarTitle} -> {value}");
                    _infoBarTitle = value;
                    OnPropertyChanged(nameof(InfoBarTitle));
                }
            }
        }

        private string _infoBarMessage;
        public string InfoBarMessage
        {
            get { return _infoBarMessage; }
            set
            {
                if (_infoBarMessage != value)
                {
                    _infoBarMessage = value;
                    OnPropertyChanged(nameof(InfoBarMessage));
                }
            }
        }

        private bool _isInfoBarVisible;
        public bool IsInfoBarVisible
        {
            get { return _isInfoBarVisible; }
            set
            {
                if (_isInfoBarVisible != value)
                {
                    _isInfoBarVisible = value;
                    OnPropertyChanged(nameof(IsInfoBarVisible));
                }
            }
        }


        private InfoBarSeverity _infoBarSeverity;
        public InfoBarSeverity InfoBarSeverity
        {
            get { return _infoBarSeverity; }
            set
            {
                _infoBarSeverity = value;
                OnPropertyChanged(nameof(InfoBarSeverity));
            }
        }

        public VoirSeance()
        {
            this.InitializeComponent();
            this.DataContext = this;
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is string activityCode && !string.IsNullOrEmpty(activityCode))
            {
                Debug.WriteLine($"ActivityCode reçu : {activityCode}");
                ActivityCode = activityCode;
                LoadSeances();
            }
            else
            {
                Debug.WriteLine("ActivityCode invalide ou manquant");
                ShowErrorInfoBar("Paramètre Invalide", "Le code d'activité fourni est invalide.", InfoBarSeverity.Error);
            }
        }

        private void LoadSeances()
        {
            try
            {
                if (!string.IsNullOrEmpty(ActivityCode))
                {
                    Debug.WriteLine($"ActivityCode dans LoadSeances : {ActivityCode}");
                    var seances = Singleton.getInstance().GetSeancesByActivity(ActivityCode);

                    if (SeanceList != seances) // Évite de réaffecter la même collection
                    {
                        SeanceList = seances;

                        if (SeanceList.Count == 0)
                        {
                            ShowErrorInfoBar("Aucune séance trouvée", $"Aucune séance n'a été trouvée pour l'activité : {ActivityCode}", InfoBarSeverity.Warning);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine($"ActivityCode est vide ou null : '{ActivityCode}'");
                    ShowErrorInfoBar("Paramètre Invalide", "Le code d'activité fourni est invalide.", InfoBarSeverity.Error);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Erreur dans LoadSeances : {ex.Message}");
                ShowErrorInfoBar("Erreur", "Une erreur s'est produite lors du chargement des séances.", InfoBarSeverity.Error);
            }
        }



        private void SeanceListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is Seance selectedSeance)
            {
                var currentUser = SessionManager.Instance.UsagerConnecte;

                if (currentUser != null && currentUser.Role.Equals("admin", StringComparison.OrdinalIgnoreCase))
                {
                    // Naviguer vers la page de modification de la séance si admin
                    this.Frame.Navigate(typeof(ModifierSeance), selectedSeance);
                }
                else
                {
                    // Si non-admin, ne fait rien (non-cliquable)
                    Debug.WriteLine("L'élément n'est pas cliquable pour ce rôle.");
                }
            }
        }
        public bool IsAdmin
        {
            get
            {
                var currentUser = SessionManager.Instance.UsagerConnecte;
                return currentUser != null && currentUser.Role.Equals("admin", StringComparison.OrdinalIgnoreCase);
            }
        }





        // Gestionnaire d'événement pour fermer l'InfoBar
        private void ErrorInfoBar_Closed(InfoBar sender, InfoBarClosedEventArgs args)
        {
            IsInfoBarVisible = false;
        }
        private void InscriptionButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int idSeance)
            {
                var currentUser = SessionManager.Instance.UsagerConnecte;

                if (currentUser != null && currentUser.Role.Equals("adherent", StringComparison.OrdinalIgnoreCase))
                {
                    string numeroAdherent = currentUser.NumeroIdentification;

                    bool isInscriptionSuccessful = Singleton.getInstance().InscriptionSeance(idSeance, numeroAdherent);

                    if (isInscriptionSuccessful)
                    {
                        ShowErrorInfoBar("Succès", "Vous êtes inscrit à la séance avec succès.", InfoBarSeverity.Success);
                    }
                    else
                    {
                        ShowErrorInfoBar("Conflit d'Horaire", "Vous êtes déjà inscrit à une activité à ce créneau horaire.", InfoBarSeverity.Warning);
                    }
                }
                else
                {
                    ShowErrorInfoBar("Accès Non Autorisé", "Vous devez être connecté en tant qu'adhérent pour vous inscrire.", InfoBarSeverity.Warning);
                }
            }
        }


        // Gestion des erreurs SQL
        private void HandleSqlException(MySqlException ex)
        {
            switch (ex.Number)
            {
                case 1002: // Conflit d'inscription
                    ShowErrorInfoBar("Conflit d'Inscription", "Vous êtes déjà inscrit à cette séance.", InfoBarSeverity.Warning);
                    break;
                case 1003: // Places complètes
                    ShowErrorInfoBar("Places Complètes", "Toutes les places pour cette séance sont prises.", InfoBarSeverity.Warning);
                    break;
                case 1005: // Conflit horaire
                    ShowErrorInfoBar("Conflit d'Horaires", "Vous êtes déjà inscrit à une activité à ce créneau horaire.", InfoBarSeverity.Warning);
                    break;
                default:
                    ShowErrorInfoBar("Erreur Inconnue", ex.Message, InfoBarSeverity.Error);
                    break;
            }
        }


        // Afficher l'InfoBar
        private void ShowErrorInfoBar(string title, string message, InfoBarSeverity severity)
        {
            if (InfoBarTitle != title || InfoBarMessage != message || InfoBarSeverity != severity)
            {
                InfoBarTitle = title;
                InfoBarMessage = message;
                InfoBarSeverity = severity;
                IsInfoBarVisible = true;
            }
        }





        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) /*Important de ne pas oublier ça*/
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            Debug.WriteLine($"OnPropertyChanged déclenché pour : {propertyName}");
        }
    }
}
