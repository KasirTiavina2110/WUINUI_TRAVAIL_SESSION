using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace class2
{
    public sealed partial class ModalConnexion : ContentDialog
    {
        private bool valide;

        public ModalConnexion()
        {
            this.InitializeComponent();
            valide = false;
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string numeroIdentification = matricule.Text.Trim();
            string motDePasse = mdp.Password.Trim();
            string role = (roleUsager.SelectedItem as ComboBoxItem)?.Content.ToString();

            // R�initialiser les erreurs
            ResetErreurs();

            // Validation des champs
            bool hasError = false;

            if (string.IsNullOrEmpty(numeroIdentification))
            {
                erreur_matricule.Visibility = Visibility.Visible;
                erreur_matricule.Text = "Veuillez entrer un matricule valide.";
                hasError = true;
            }

            if (string.IsNullOrEmpty(motDePasse))
            {
                erreur_mdp.Visibility = Visibility.Visible;
                erreur_mdp.Text = "Veuillez entrer un mot de passe.";
                hasError = true;
            }

            if (string.IsNullOrEmpty(role))
            {
                erreur_roleUsager.Visibility = Visibility.Visible;
                erreur_roleUsager.Text = "Veuillez s�lectionner un r�le.";
                hasError = true;
            }

            if (hasError)
            {
                args.Cancel = true; // Emp�che la fermeture si invalide
                return;
            }

            // Authentification
            Usager usager = SessionManager.Instance.AuthentifierUsager(numeroIdentification, motDePasse, role);
            if (usager == null)
            {
                erreur_matricule.Visibility = Visibility.Visible;
                erreur_matricule.Text = "Identifiants ou r�le incorrects.";
                args.Cancel = true; // Emp�che la fermeture
                return;
            }

            // Si tout est valide
            SessionManager.Instance.ConnecterUsager(usager);
            valide = true;
            this.Hide();
        }

        private void ResetErreurs()
        {
            erreur_matricule.Visibility = Visibility.Collapsed;
            erreur_mdp.Visibility = Visibility.Collapsed;
            erreur_roleUsager.Visibility = Visibility.Collapsed;
        }

        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary && !valide)
            {
                args.Cancel = true; // Emp�che la fermeture si invalide
            }
        }
    }
}
