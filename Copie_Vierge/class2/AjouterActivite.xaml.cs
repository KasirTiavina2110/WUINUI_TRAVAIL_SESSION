using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
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
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AjouterActivite : Page
    {
        bool valide;
        public AjouterActivite()
        {
            this.InitializeComponent();
        }

        private void btn_ajout_activite_Click(object sender, RoutedEventArgs e)
        {
            valide = true;

            erreur_id.Text = string.Empty;
            erreur_nom.Text = string.Empty;
            erreur_cout_organisation.Text = string.Empty;
            erreur_prix_vente.Text = string.Empty;
            erreur_choix_type.Text = string.Empty;
            erreur_choix_annee.Text = string.Empty;
            erreur_image.Text = string.Empty;
            ajout_reussi.Text = string.Empty;


            if (string.IsNullOrWhiteSpace(id_activite.Text))
            {
                erreur_id.Text = "L'id de l'activité est obligatoire";
                valide = false;
            }
            if (string.IsNullOrWhiteSpace(nom.Text))
            {
                erreur_nom.Text = "Le nom est obligatoire";
                valide = false;
            }
            if (string.IsNullOrWhiteSpace(cout_organisation.Text))
            {
                erreur_cout_organisation.Text = "Le cout d'organisation est obligatoire";
                valide = false;
            }
            if (string.IsNullOrWhiteSpace(prix_vente.Text))
            {
                erreur_prix_vente.Text = "Le prix de vente est obligatoire";
                valide = false;
            }
            if(choix_type.SelectedIndex == -1)
            {
                erreur_choix_type.Text = "Le type d'activité est obligatoire";
                valide = false;
            }
            if(choix_annee.SelectedIndex == -1)
            {
                erreur_choix_annee.Text = "L'année est obligatoire";
                valide=false;
            }
            if (string.IsNullOrWhiteSpace(image.Text))
            {
                erreur_image.Text = "Le lien est obligatoire";
                valide = false;
            }

            double prixVente;
            bool prixVenteValide = Double.TryParse(prix_vente.Text, out prixVente);
            if (!prixVenteValide)
            {
                erreur_prix_vente.Text = "Le prix doit être un nombre valide";
                valide = false;
            }

            double coutOrganisation;
            bool coutOrganisationValide = Double.TryParse(cout_organisation.Text, out coutOrganisation);
            if (!coutOrganisationValide)
            {
                erreur_cout_organisation.Text = "Le cout doit être un nombre valide";
                valide = false;
            }

            if (valide)
            {
                string identifiant = id_activite.Text;
                string nomActivite = nom.Text;

                string typeActivite = choix_type.SelectedItem.ToString();
                string anneeActivite = choix_annee.SelectedItem.ToString();
                string imageActivite = image.Text;

                    Activite activite = new Activite(identifiant, nomActivite, anneeActivite, coutOrganisation, prixVente, typeActivite, imageActivite);

                    Singleton.getInstance().ajouterActivite(activite);

                    id_activite.Text = string.Empty;
                    nom.Text = string.Empty;
                    cout_organisation.Text = string.Empty;
                    prix_vente.Text = string.Empty;
                    choix_type.SelectedIndex = -1;
                    choix_annee.SelectedIndex = -1;
                    image.Text = string.Empty;

                    ajout_reussi.Text = "L'ajout est effectué avec succès";
          

            }

        }
    }
}
