using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
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
    public sealed partial class SupprimerUsager : Page
    {

        bool valide;
        public SupprimerUsager()
        {
            this.InitializeComponent();
        }

        private void bouton_supprimer_Click(object sender, RoutedEventArgs e)
        {

            valide = true;

            //Vider le champ d'erreur
            erreur_matricule.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(matricule.Text))
            {
                erreur_matricule.Text = "Le matricule est obligatoire";
                valide = false;
            }
            if(valide)
            {

                string identifiant = matricule.Text.ToUpper();

                int reponseRequete =  Singleton.getInstance().supprimerUsager(identifiant);

                if(reponseRequete > 0)
                {
                    suppression_reussi.Text = "Utilisateur supprimé avec succès";
                    matricule.Text = string.Empty;
  
                }
                else
                {
                    erreur_matricule.Text = "Le numéro d'identification n'existe pas dans le système";

                }

                
            }


        }
    }
}
