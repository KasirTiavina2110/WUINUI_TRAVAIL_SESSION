using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace class2
{
    public class Usager : INotifyPropertyChanged
    {
        private string numeroIdentification;
        private string nom;
        private string prenom;
        private string adresse;
        private DateTime dateNaissance;
        private int age;
        private string role;
        private string motDePasse;

        public string NumeroIdentification
        {
            get => numeroIdentification;
            set
            {
                numeroIdentification = value;
                OnPropertyChanged(nameof(NumeroIdentification));
            }
        }

        public string Nom
        {
            get => nom;
            set
            {
                nom = value;
                OnPropertyChanged(nameof(Nom));
            }
        }

        public string Prenom
        {
            get => prenom;
            set
            {
                prenom = value;
                OnPropertyChanged(nameof(Prenom));
            }
        }

        public string Adresse
        {
            get => adresse;
            set
            {
                adresse = value;
                OnPropertyChanged(nameof(Adresse));
            }
        }

        public DateTime DateNaissance
        {
            get => dateNaissance;
            set
            {
                dateNaissance = value;
                OnPropertyChanged(nameof(DateNaissance));
            }
        }

        public int Age
        {
            get => age;
            set
            {
                age = value;
                OnPropertyChanged(nameof(Age));
            }
        }

        public string Role
        {
            get => role;
            set
            {
                role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public string MotDePasse
        {
            get => motDePasse;
            set
            {
                motDePasse = value;
                OnPropertyChanged(nameof(MotDePasse));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}