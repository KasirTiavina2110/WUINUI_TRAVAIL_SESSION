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
        string numeroIdentification;
        string nom;
        string prenom;
        string adresse;
        DateTime dateNaissance;
        int age;
        string role;
        string motDePasse;
        string dateNaissance2;



        public Usager() { }


        //Constructeur pour ajout d'un usager
        public Usager(string nom, string prenom, string adresse, string datenaissance2, int age, string role, string motDePasse)
        {

            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.dateNaissance2 = datenaissance2;
            this.age = age;
            this.role = role;
            this.motDePasse = motDePasse;
        }

        //Constructeur pour modification d'un Usager
        public Usager(string numeroIdentification, string nom, string prenom, string adresse, string datenaissance2, int age, string role)
        {
            this.numeroIdentification = numeroIdentification;
            this.nom = nom;
            this.prenom = prenom;
            this.adresse = adresse;
            this.dateNaissance2 = datenaissance2;
            this.age = age;
            this.role = role;
        }





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

        public string DateNaissance2
        {
            get => dateNaissance2;
            set
            {
                dateNaissance2 = value;
                OnPropertyChanged(nameof(DateNaissance2));
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