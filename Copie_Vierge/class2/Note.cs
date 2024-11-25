using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace class2
{
    internal class Note : INotifyPropertyChanged
    {







        //-------NE PAS OUBLIER CA POUR RENDRE L'AFFICHAGE DYNAMIQUE---------------

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
