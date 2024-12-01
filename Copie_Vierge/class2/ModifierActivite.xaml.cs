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
    }
}
