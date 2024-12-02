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
    public sealed partial class ModifierUsager : Page
    {

        bool valide;
        public ModifierUsager()
        {
            this.InitializeComponent();
            SetDatePicker(); //Pour personnalisé le DatePicker
        }

        private void SetDatePicker()
        {
            DateTime aujourdhui = DateTime.Now;
            DateTime anneeMax = aujourdhui.AddYears(-18);

            annee_naissance.MaxYear = anneeMax;
        }


        //Chercher l'usager pour voir s'il existe et affiche formulaire si vrai
        private void btn_chercher_usager_Click(object sender, RoutedEventArgs e)
        {
            valide = true;

            erreur_matricule.Text = string.Empty;

            cacherFormulaireModifier();

            if (string.IsNullOrWhiteSpace(matricule.Text))
            {
                erreur_matricule.Text = "Le matricule est obligatoire";
                valide = false;
            }

            if (valide)
            {
                string identifiant = matricule.Text.ToUpper();

                int reponseRequete = Singleton.getInstance().verifierSiExiste(identifiant);

                if(reponseRequete > 0)
                {
                    //Afficher le formulaire avec les informations
                    section_nom.Visibility = Visibility.Visible;
                    section_dateNaissance.Visibility = Visibility.Visible;
                    section_adresse.Visibility = Visibility.Visible;
                    section_role.Visibility = Visibility.Visible;
                    section_btn_modification.Visibility = Visibility.Visible;


                    //Aller chercher les informations de l'usager
                    Usager usager = Singleton.getInstance().informationUnUsager(identifiant);

                    //Afficher les informations
                    if(usager != null)
                    {
                        nom.Text = usager.Nom;
                        prenom.Text = usager.Prenom;
                        adresse_usager.Text = usager.Adresse;
                        annee_naissance.Date = usager.DateNaissance;

                        //Afficher le rôle
                        if(usager.Role == "adhérent")
                        {
                            role.SelectedIndex = 1;
                        }
                        else
                        {
                            role.SelectedIndex = 0;
                        }
                    }
                }
                else
                {
                    erreur_matricule.Text = "Le matricule n'existe pas";
                }

            }

        }

        public void cacherFormulaireModifier()
        {
            section_nom.Visibility = Visibility.Collapsed;
            section_dateNaissance.Visibility = Visibility.Collapsed;
            section_adresse.Visibility = Visibility.Collapsed;
            section_role.Visibility = Visibility.Collapsed;
            section_btn_modification.Visibility = Visibility.Collapsed;

        }

        private void bouton_modifier_usager_Click(object sender, RoutedEventArgs e)
        {
            valide = true;

            //Vider les champs d'erreurs
            erreur_matricule.Text = string.Empty;
            erreur_nom.Text = string.Empty;
            erreur_prenom.Text = string.Empty;
            erreur_annee_naissance.Text = string.Empty;
            erreur_adresse_usager.Text = string.Empty;
            erreur_role.Text = string.Empty;

            //Vérifier si les champs sont vides ou non
            if (string.IsNullOrWhiteSpace(matricule.Text))
            {
                erreur_matricule.Text = "Le matricule est obligatoire";
                valide = false;

            }
            if (string.IsNullOrWhiteSpace(nom.Text))
            {
                erreur_nom.Text = "Le nom est obligatoire";
                valide = false;

            }
            if (string.IsNullOrWhiteSpace(prenom.Text))
            {
                erreur_prenom.Text = "Le prénom est obligatoire";
                valide = false;
            }
            if (string.IsNullOrWhiteSpace(adresse_usager.Text))
            {
                erreur_adresse_usager.Text = "L'adresse est obligatoire";
                valide = false;
            }
            if (role.SelectedIndex == -1)
            {
                erreur_role.Text = "Le rôle est obligatoire";
                valide = false;
            }
            if (annee_naissance.SelectedDate == null)
            {
                erreur_annee_naissance.Text = "La date de naissance est obligatoire";
                valide = false;
            }

            if (valide)
            {

                string identifiant = matricule.Text;

                //Assigner les valeurs des champs
                string nomUsager = nom.Text;
                string prenomUsager = prenom.Text;
                string adresseUsager = adresse_usager.Text;
                var dateNaissanceUsager = annee_naissance.SelectedDate.Value;
                string dateFinale = dateNaissanceUsager.ToString("yyyy-MM-dd");
                int ageUsager = DateTime.Now.Year - dateNaissanceUsager.Year;

                //Calcul si la fête est passé pour décrémenter si pas passé
                if (DateTime.Now.DayOfYear < dateNaissanceUsager.DayOfYear)
                {
                    ageUsager--;
                }

                string roleUsager = role.SelectedItem as string;

                Usager usager = new Usager(identifiant, nomUsager, prenomUsager, adresseUsager, dateFinale, ageUsager, roleUsager);

                bool reussi = Singleton.getInstance().modifierUsager(usager);

                if (reussi)
                {
                    cacherFormulaireModifier();
                    matricule.Text = string.Empty;
                    modification_reussie.Text = "Modification de l'usager effectuée";

                }
                else
                {
                    erreur_matricule.Text = "Le matricule n'existe pas";
                }
            }


        }
    }
}
