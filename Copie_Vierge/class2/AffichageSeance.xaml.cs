using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
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
    public sealed partial class AffichageSeance : Page, INotifyPropertyChanged
    {
        public AffichageSeance()
        {
            this.InitializeComponent();

            voirListe.ItemsSource = Singleton.getInstance().getListeActivite();

        }


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            Activite activite = button.DataContext as Activite;

            Singleton.getInstance().supprimerActivite(activite);


        }











        //----------------- Ne pas oublier pour rendre le changement dynamique-------------------------
        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName) /*Important de ne pas oublier ca */
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
