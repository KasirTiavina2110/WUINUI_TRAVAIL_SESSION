using System;
using System.ComponentModel;

namespace class2
{
    public class Seance : INotifyPropertyChanged
    {
        private int id_seance;
        public int Id_seance
        {
            get { return id_seance; }
            set
            {
                if (id_seance != value)
                {
                    id_seance = value;
                    OnPropertyChanged(nameof(Id_seance));
                }
            }
        }

        private string numero_activite;
        public string Numero_activite
        {
            get { return numero_activite; }
            set
            {
                if (numero_activite != value)
                {
                    numero_activite = value;
                    OnPropertyChanged(nameof(Numero_activite));
                }
            }
        }

        private DateTime date;
        public DateTime Date
        {
            get { return date; }
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged(nameof(Date));
                }
            }
        }

        private TimeSpan heure;
        public TimeSpan Heure
        {
            get { return heure; }
            set
            {
                if (heure != value)
                {
                    heure = value;
                    OnPropertyChanged(nameof(Heure));
                }
            }
        }

        private int? place_dispo;
        public int? Place_dispo
        {
            get { return place_dispo; }
            set
            {
                if (place_dispo != value)
                {
                    place_dispo = value;
                    OnPropertyChanged(nameof(Place_dispo));
                }
            }
        }

        private int? place_prise;
        public int? Place_prise
        {
            get { return place_prise; }
            set
            {
                if (place_prise != value)
                {
                    place_prise = value;
                    OnPropertyChanged(nameof(Place_prise));
                }
            }
        }

        private int place_max;
        public int Place_max
        {
            get { return place_max; }
            set
            {
                if (place_max != value)
                {
                    place_max = value;
                    OnPropertyChanged(nameof(Place_max));
                }
            }
        }

        //-------NE PAS OUBLIER CA POUR RENDRE L'AFFICHAGE DYNAMIQUE---------------

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
