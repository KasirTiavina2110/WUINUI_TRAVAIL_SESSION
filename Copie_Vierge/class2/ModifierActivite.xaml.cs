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
    public sealed partial class ModifierActivite : Page
    {

        bool valide;
        public ModifierActivite()
        {
            this.InitializeComponent();

        }

        public void cacherFormulaire()
        {
            secondeSection.Visibility = Visibility.Collapsed;

        }

        public void afficherFormulaire()
        {
            secondeSection.Visibility = Visibility.Visible;

        }

        private void btn_chercher_activite_Click(object sender, RoutedEventArgs e)
        {
            valide = true;

            erreur_id_activite.Text = string.Empty;
            modification_reussie.Text = string.Empty;

            cacherFormulaire();

            if (string.IsNullOrWhiteSpace(id_activite.Text))
            {
                erreur_id_activite.Text = "L'id est obligatoire";
                valide = false;
            }

            if (valide)
            {
                string identifiant = id_activite.Text.ToUpper();
                
                int reponseRequete = Singleton.getInstance().verifierSiActiviteExiste(identifiant);

                if(reponseRequete > 0)
                {
                    afficherFormulaire();

                    //Aller chercher les informations de l'activité
                    Activite activite = Singleton.getInstance().informationUneActivite(identifiant);

                    if (activite != null)
                    {

                        nom.Text = activite.Nom;
                        cout_organisation.Text = activite.Cout_Organisation.ToString();
                        prix_vente.Text = activite.Vente_Client.ToString();
                        image.Text = activite.Pochette;

                        string anneActivite = activite.Annee.ToString();
                        if (choix_annee.Items.Contains(anneActivite))
                        {
                            choix_annee.SelectedItem = anneActivite;
                        }

                        string type = activite.Type.ToString();
                        if (choix_type.Items.Contains(type))
                        {
                            choix_type.SelectedItem = type;
                        }



                    }
                }
                else
                {
                    erreur_id_activite.Text = "L'id de l'activité n'existe pas";
                }
            }

        }

        private void btn_modifier_activite_Click(object sender, RoutedEventArgs e)
        {
            valide = true;

            erreur_id_activite.Text = string.Empty;
            erreur_nom.Text = string.Empty;
            erreur_cout_organisation.Text = string.Empty;
            erreur_prix_vente.Text = string.Empty;
            erreur_image.Text = string.Empty;
            erreur_choix_type.Text = string.Empty;
            erreur_choix_annee.Text = string.Empty;
            modification_reussie.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(id_activite.Text))
            {
                erreur_id_activite.Text = "Le matricule est obligatoire";
                valide = false;

            }
            if (string.IsNullOrWhiteSpace(nom.Text))
            {
                erreur_nom.Text = "Le nom est obligatoire";
                valide = false;
            }
            if (string.IsNullOrWhiteSpace(cout_organisation.Text))
            {
                erreur_cout_organisation.Text = "Le cout de l'organisation est obligatoire";
                valide = false;
            }
            if (string.IsNullOrWhiteSpace(prix_vente.Text))
            {
                erreur_prix_vente.Text = "Le prix de vente aux adhérents est obligatoire";
                valide = false;

            }
            if (string.IsNullOrWhiteSpace(image.Text))
            {
                erreur_image.Text = "Le lien de l'image est obligatoire";
                valide = false;
            }
            if(choix_type.SelectedIndex == -1)
            {
                erreur_choix_type.Text = "Le choix du type est obligatoire";
                valide = false;
            }
            if(choix_annee.SelectedIndex == -1)
            {
                erreur_choix_annee.Text = "Le choix de l'année est obligatoire";
                valide = false;
            }

            if (valide)
            {
                string identifiant = id_activite.Text;

                string nomActivite = nom.Text;
                double coutOrganisationActivite = Double.Parse(cout_organisation.Text);
                double prixVenteActivite = Double.Parse(prix_vente.Text);
                string imageActivite = image.Text;
                string typeActivite = choix_type.SelectedValue.ToString();
                string anneActivite = choix_annee.SelectedValue.ToString();

                Activite activite = new Activite(identifiant, nomActivite, anneActivite, coutOrganisationActivite, prixVenteActivite, typeActivite, imageActivite);

                bool reussi = Singleton.getInstance().modifierActivite(activite);

                if (reussi)
                {
                    id_activite.Text = string.Empty;
                    cacherFormulaire();
                    modification_reussie.Text = "Modification réussie";

                }
                else
                {
                    erreur_id_activite.Text = "L'id n'existe pas";
                }
            }

        }
    }
}
