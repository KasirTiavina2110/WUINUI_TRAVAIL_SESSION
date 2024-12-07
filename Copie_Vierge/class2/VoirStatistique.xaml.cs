using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Text;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace class2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VoirStatistique : Page
    {
        public VoirStatistique()
        {
            this.InitializeComponent();
            AfficherNombreAdherent();
            AfficherNombreActivite();
            AdherentParActivite();
            MoyenneEvaluationParActivite();
            NombreInscriptionParType();
            AgeMoyenAdherent();
            ActivitePlusAdherent();
        }

        public void AfficherNombreAdherent()
        {

            int nombreAdherent = SingletonStatistique.GetInstance().NombreAdherent();
            nombreTotalAdherents.Text += nombreAdherent.ToString();


        }

        public void AfficherNombreActivite()
        {

            int nombreActivite = SingletonStatistique.GetInstance().NombreActivite();
            nombreTotalActivite.Text += nombreActivite.ToString();

        }

        public void AdherentParActivite()
        {
            var informations = SingletonStatistique.GetInstance().AdherentParActivite();

            stk_parent.Children.Clear(); // Vider le StackPanel avant d'ajouter les nouveaux éléments

            //Entête
            TextBlock header = new TextBlock 
            { 
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0),
                FontWeight = FontWeights.Bold,
                TextDecorations = TextDecorations.Underline,
                FontSize = 25, Text = "Le nombre d'adhérent par séance"
            }; 
            stk_parent.Children.Add(header);

            foreach (var info in informations)
            {
                StackPanel seanceStackPanel = new StackPanel
                {
                    Margin = new Thickness(15),
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                TextBlock seanceText = new TextBlock
                {
                    Text = $"Activité {info.Key} : {info.Value} adhérents"
                };

                seanceStackPanel.Children.Add(seanceText);
                stk_parent.Children.Add(seanceStackPanel);
            }
          
        }

        public void MoyenneEvaluationParActivite()
        {

            var informations = SingletonStatistique.GetInstance().NoteMoyenneActivite();

            stk_note_parent.Children.Clear();

            //Entête
            TextBlock header = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0),
                FontWeight = FontWeights.Bold,
                TextDecorations = TextDecorations.Underline,
                FontSize = 25,
                Text = "La note moyenne"
            };

            stk_note_parent.Children.Add(header);

            foreach (var info in informations)
            {
                StackPanel seanceStackPanel = new StackPanel
                {
                    Margin = new Thickness(15),
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                TextBlock seanceText = new TextBlock
                {
                    Text = $"Activité {info.Key} : ",
                    Margin = new Thickness(5, 5, 0, 0)

                };

                RatingControl rating = new RatingControl
                {
                    Value = info.Value,
                    IsReadOnly = true,
                    Margin = new Thickness(5, 0, 0, 0)
                };

                seanceStackPanel.Children.Add(seanceText);
                seanceStackPanel.Children.Add(rating);
                stk_note_parent.Children.Add(seanceStackPanel);
            }
        }


        //Voir le nombre d'inscription par type
        public void NombreInscriptionParType()
        {
            var informations = SingletonStatistique.GetInstance().NombreInscriptionParType();

            stk_type_parent.Children.Clear();

            //Entête
            TextBlock header = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 0),
                FontWeight = FontWeights.Bold,
                TextDecorations = TextDecorations.Underline,
                FontSize = 25,
                Text = "Le nombre d'inscription par type"
            };

            stk_type_parent.Children.Add(header);

            foreach (var info in informations)
            {
                StackPanel seanceStackPanel = new StackPanel
                {
                    Margin = new Thickness(15),
                    Orientation = Orientation.Horizontal,
                    HorizontalAlignment = HorizontalAlignment.Center
                };

                TextBlock seanceText = new TextBlock
                {
                    Text = $"{info.Key} : {info.Value} inscriptions",

                };

                seanceStackPanel.Children.Add(seanceText);
                stk_type_parent.Children.Add(seanceStackPanel);
            }
        }

        public void AgeMoyenAdherent()
        {
            var informations = SingletonStatistique.GetInstance().AgeMoyenAdherent();
            AgeMoyen.Text += informations.ToString();

        }

        public void ActivitePlusAdherent()
        {
            var informations = SingletonStatistique.GetInstance().ActivitePlusAdherent();

            foreach(var info in informations)
            {
                activitePlusPopulaire.Text = $"{info.Key} : {info.Value}";
            }

        }

    }
}
