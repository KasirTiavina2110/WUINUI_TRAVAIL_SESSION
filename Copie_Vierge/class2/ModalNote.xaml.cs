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
    public sealed partial class ModalNote : ContentDialog
    {


        bool valide;
        public ModalNote()
        {
            this.InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            valide = true;
            erreur_note.Text = string.Empty;

            if (string.IsNullOrWhiteSpace(note.Text))
            {
                erreur_note.Text = "La note est obligatoire";
                valide = false;
            }
            else
            {
                double noteFinale;
                if (double.TryParse(note.Text, out noteFinale))
                {
                    if (noteFinale < 0 || noteFinale > 5)
                    {
                        erreur_note.Text = "La note doit être entre 0 et 5";
                        valide = false;
                    }
                }
                else
                {
                    erreur_note.Text = "Veuillez entrer une valeur numérique valide.";
                    valide = false;
                }
            }

            if (!valide)
            {
                args.Cancel = true;
            }

            if (valide)
            {
                //Ajouter la note dans la BD, modification test
            }
        }


        private void ContentDialog_Closing(ContentDialog sender, ContentDialogClosingEventArgs args)
        {
            if (args.Result == ContentDialogResult.Primary && !valide)
            {
                args.Cancel = true; // Empêche la fermeture si invalide
            }

        }
    }
}
