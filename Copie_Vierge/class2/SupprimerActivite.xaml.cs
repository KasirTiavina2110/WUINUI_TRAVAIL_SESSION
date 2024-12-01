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
    public sealed partial class SupprimerActivite : Page
    {

        bool valide;
        public SupprimerActivite()
        {
            this.InitializeComponent();
        }

        private void bouton_supprimer_Click(object sender, RoutedEventArgs e)
        {

            valide = true;

            erreur_id_activite.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(id_activite.Text))
            {
                erreur_id_activite.Text = "L'id de l'activité est obligatoire";
                valide = false;
            }
            if (valide)
            {
                string identifiant = id_activite.Text.ToUpper();

                int reponseRequete = Singleton.getInstance().supprimerActivite(identifiant);

                if (reponseRequete > 0)
                {

                    suppression_reussi.Text = "Activité supprimée avec succès";
                    id_activite.Text = string.Empty;

                }
                else if(reponseRequete == -1)
                {
                    erreur_id_activite.Text = "L'activité contient au moins 1 séance. Veuillez supprimer la ou les séances avant";
                }
                else
                {
                    erreur_id_activite.Text = "Le numéro d'activité n'existe pas dans le système";

                }


            }

        }
    }
}
