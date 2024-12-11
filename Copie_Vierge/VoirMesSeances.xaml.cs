using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    public sealed partial class VoirMesSeances : Page, INotifyPropertyChanged
    {


        public VoirMesSeances()
        {
            this.InitializeComponent();
            lvInscription.ItemsSource = Singleton.getInstance().GetListeMesSeance();
        }


        // Implémentation de INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName) /*Important de ne pas oublier ça*/
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            ModalNote modalNote = new ModalNote();

            modalNote.XamlRoot = racine.XamlRoot;

            modalNote.PrimaryButtonText = "Noter";
            modalNote.Title = "Noter une séance";
            modalNote.CloseButtonText = "Annuler";

            var result = await modalNote.ShowAsync();
        }

    }
}
