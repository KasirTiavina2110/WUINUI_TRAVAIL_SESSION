using System;
using System.ComponentModel;

namespace class2
{
    public class Inscription : INotifyPropertyChanged
    {
        private int id_inscription;
        private string numero_adherent;
        private int id_seance;
        private DateTime date_inscription;

        public int Id_inscription
        {
            get { return id_inscription; }
            set
            {
                if (id_inscription != value)
                {
                    id_inscription = value;
                    OnPropertyChanged(nameof(Id_inscription));
                }
            }
        }

        public string Numero_adherent
        {
            get { return numero_adherent; }
            set
            {
                if (numero_adherent != value)
                {
                    numero_adherent = value;
                    OnPropertyChanged(nameof(Numero_adherent));
                }
            }
        }

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

        public DateTime Date_inscription
        {
            get { return date_inscription; }
            set
            {
                if (date_inscription != value)
                {
                    date_inscription = value;
                    OnPropertyChanged(nameof(Date_inscription));
                }
            }
        }

        // Constructeur par défaut
        public Inscription() { }

        // Constructeur avec paramètres
        public Inscription(int id_inscription, string numero_adherent, int id_seance, DateTime date_inscription)
        {
            Id_inscription = id_inscription;
            Numero_adherent = numero_adherent;
            Id_seance = id_seance;
            Date_inscription = date_inscription;
        }

        // Méthode ToString pour le débogage ou l'affichage
        public override string ToString()
        {
            return $"Inscription: {Id_inscription}, Adhérent: {Numero_adherent}, Séance: {Id_seance}, Date: {Date_inscription}";
        }

        // Implémentation de l'interface INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
