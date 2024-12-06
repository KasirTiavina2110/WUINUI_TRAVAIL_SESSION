using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System;

namespace class2
{
    public sealed partial class MessagePage : Page
    {
        public MessagePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is Tuple<string, string> messageData)
            {
                // Afficher le titre et le message
                MessageTitle.Text = messageData.Item1; // Titre du message
                MessageContent.Text = messageData.Item2; // Contenu du message
            }
            else
            {
                // Valeur par défaut si aucun paramètre n'est passé
                MessageTitle.Text = "Message";
                MessageContent.Text = "Aucun détail disponible.";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack(); // Retourne à la page précédente
            }
        }
    }
}
