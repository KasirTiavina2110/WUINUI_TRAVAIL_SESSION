using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace class2
{
    internal class Activite : INotifyPropertyChanged
    {

        string id, nom, annee, type;
        double cout_organisation, vente_client;


        public Activite(string id, string nom, string annee, string type, double cout_organisation, double vente_client)
        {
            this.id = id;
            this.nom = nom;
            this.annee = annee;
            this.type = type;
            this.cout_organisation = cout_organisation;
            this.vente_client = vente_client;
        }

        public string Id
        {
            get { return id; }
            set
            {
                if (id != value)
                {
                    id = value;
                    OnPropertyChanged(nameof(Id));
                }
            }

        }

        public string Nom
        {
            get { return nom; }
            set
            {
                if (nom != value)
                {
                    nom = value;
                    OnPropertyChanged(nameof(Nom));
                }
            }

        }

        public string Annee
        {
            get { return annee; }
            set
            {
                if (annee != value)
                {
                    annee = value;
                    OnPropertyChanged(nameof(Annee));
                }
            }

        }

        public string Type
        {
            get { return type; }
            set
            {
                if (type != value)
                {
                    type = value;
                    OnPropertyChanged(nameof(Type));
                }
            }

        }

        public double Cout_Organisation
        {
            get { return cout_organisation; }
            set
            {
                if (cout_organisation != value)
                {
                    cout_organisation = value;
                    OnPropertyChanged(nameof(Cout_Organisation));
                }
            }

        }

        public double Vente_Client
        {
            get { return vente_client; }
            set
            {
                if (vente_client != value)
                {
                    vente_client = value;
                    OnPropertyChanged(nameof(Vente_Client));
                }
            }

        }

        public override string ToString()
        {
            return base.ToString();
        }


        //-------NE PAS OUBLIER CA POUR RENDRE L'AFFICHAGE DYNAMIQUE---------------

        public event PropertyChangedEventHandler PropertyChanged;


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
