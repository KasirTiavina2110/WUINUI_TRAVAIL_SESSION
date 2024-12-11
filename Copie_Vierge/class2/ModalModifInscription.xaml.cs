using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.ObjectModel;

namespace class2
{
    public sealed partial class ModalModifInscription : ContentDialog
    {
        public string NumeroAdherent { get; private set; }
        public int IdSeance { get; private set; }
        public DateTime DateInscription { get; private set; }
        private readonly Singleton singleton;

        public ModalModifInscription(ObservableCollection<Usager> adherents, ObservableCollection<Seance> seances, Inscription inscription = null)
        {
            this.InitializeComponent();
            singleton = Singleton.getInstance();

            // Charger les adhérents
            ComboAdherent.ItemsSource = adherents;
            ComboAdherent.DisplayMemberPath = "Nom";
            ComboAdherent.SelectedValuePath = "NumeroIdentification";

            // Charger les séances
            ComboSeance.ItemsSource = seances;
            ComboSeance.DisplayMemberPath = "Numero_activite";
            ComboSeance.SelectedValuePath = "Id_seance";

            // Initialiser les champs
            if (inscription != null)
            {
                ComboAdherent.SelectedValue = inscription.Numero_adherent;
                ComboSeance.SelectedValue = inscription.Id_seance;
                DatePickerInscription.Date = new DateTimeOffset(inscription.Date_inscription.Date);
                TxtHeureInscription.Text = inscription.Date_inscription.ToString("HH:mm:ss");
            }
            else
            {
                InitialiserDateEtHeureInscription();
            }
        }

        // Initialiser la date et l'heure d'inscription
        private void InitialiserDateEtHeureInscription()
        {
            DatePickerInscription.Date = DateTimeOffset.Now; // Date actuelle
            TxtHeureInscription.Text = DateTime.Now.ToString("HH:mm:ss"); // Heure actuelle
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Vérifier si le champ de l'heure est vide
            if (string.IsNullOrWhiteSpace(TxtHeureInscription.Text))
            {
                args.Cancel = true;
                AfficherErreur("Veuillez remplir le champ de l'heure (format HH:mm:ss).");
                return;
            }

            try
            {
                DateTime date = DatePickerInscription.Date.DateTime; // Récupère la date
                TimeSpan heure = TimeSpan.Parse(TxtHeureInscription.Text); // Récupère l'heure
                DateInscription = date.Add(heure); // Combine la date et l'heure

                // Récupérer le `NumeroAdherent`
                if (ComboAdherent.SelectedValue != null)
                {
                    NumeroAdherent = ComboAdherent.SelectedValue.ToString();
                }
                else
                {
                    args.Cancel = true;
                    AfficherErreur("Veuillez sélectionner un adhérent.");
                    return;
                }

                // Récupérer le `IdSeance`
                if (ComboSeance.SelectedValue != null && int.TryParse(ComboSeance.SelectedValue.ToString(), out int idSeance))
                {
                    IdSeance = idSeance;
                }
                else
                {
                    args.Cancel = true;
                    AfficherErreur("Veuillez sélectionner une séance.");
                    return;
                }
            }
            catch (Exception ex)
            {
                args.Cancel = true;
                AfficherErreur($"Erreur lors de la validation des données : {ex.Message}");
            }
        }




        // Afficher un message d'erreur
        private void AfficherErreur(string message)
        {
            ContentDialog erreurDialog = new ContentDialog
            {
                Title = "Erreur",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };
            _ = erreurDialog.ShowAsync();
        }
    }
}
