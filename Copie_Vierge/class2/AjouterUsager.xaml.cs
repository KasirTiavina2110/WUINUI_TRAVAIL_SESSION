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
    public sealed partial class AjouterUsager : Page
    {

        bool valide;
        public AjouterUsager()
        {
            this.InitializeComponent();

            SetDatePicker();

        }


        private void SetDatePicker()
        {
            DateTime aujourdhui = DateTime.Now;
            DateTime anneeMax = aujourdhui.AddYears(-18);
  

            annee_naissance.MaxYear = anneeMax;
        }

        private void bouton_ajout_usager_Click(object sender, RoutedEventArgs e)
        {
            valide = true;

            //Vider les champs d'erreurs
            erreur_nom.Text = string.Empty;
            erreur_prenom.Text = string.Empty;
            erreur_annee_naissance.Text = string.Empty;
            erreur_adresse_usager.Text = string.Empty;
            erreur_role.Text = string.Empty;
            erreur_mot_de_passe.Text = string.Empty;

            //Vérifier si les champs sont vides ou non
            if(string.IsNullOrWhiteSpace(nom.Text) )
            {
                erreur_nom.Text = "Le nom est obligatoire";
                valide = false;

            }
            if(string.IsNullOrWhiteSpace(prenom.Text))
            {
                erreur_prenom.Text = "Le prénom est obligatoire";
                valide = false;
            }
            if (string.IsNullOrWhiteSpace(adresse_usager.Text))
            {
                erreur_adresse_usager.Text = "L'adresse est obligatoire";
                valide = false;
            }
            if (string.IsNullOrWhiteSpace(mot_de_passe.Password))
            {
                erreur_mot_de_passe.Text = "Le mot de passe est obligatoire";
                valide = false;
            }
            if(role.SelectedIndex == -1)
            {
                erreur_role.Text = "Le rôle est obligatoire";
                valide = false ;
            }
            if (annee_naissance.SelectedDate == null)
            { 
                erreur_annee_naissance.Text = "La date de naissance est obligatoire";
                valide = false;
            }

            if (valide)
            {

                try
                {

                    string nomUsager = nom.Text;
                    string prenomUsager = prenom.Text;
                    string adresseUsager = adresse_usager.Text;
                    var dateNaissanceUsager = annee_naissance.SelectedDate.Value;
                    string dateFinale = dateNaissanceUsager.ToString("yyyy-MM-dd");


                    int ageUsager = DateTime.Now.Year - dateNaissanceUsager.Year;
                    string roleUsager = role.SelectedItem as string;
                    string motDePasseUsager = mot_de_passe.Password;

                    var usager = new Usager(nomUsager, prenomUsager, adresseUsager, dateFinale, ageUsager, roleUsager, motDePasseUsager);


                    //reste à ajouter le produit avec le Singleton

                }
                catch (Exception ex)
                {
                    erreur_annee_naissance.Text = "Fonctionne crisss";
                }
             



            }

        }
    }
}
