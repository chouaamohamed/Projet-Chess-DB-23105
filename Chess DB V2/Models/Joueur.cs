using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChessDB.Models
{
    public class Joueur : INotifyPropertyChanged //INotifyPropertyChanged = méthode qui notifie le système d'un chngmt (ici les caractéristiques du joueur)
    {   
        public int ID { get; set; } //numéro unique pour identifier le joueur
        private string _email = ""; //le = "" c'est pour initaliser avec une chaine vide (au cas où VSC a peur qu'il y a rien comme valeur --> parano)
        private string _nom = "";
        private string _prenom = "";
        private double _elo;
        public double Elo //fonction mettant à jour l'Elo pour l'affichage
        {
            get { return _elo; }
            set 
            { 
                if (_elo != value) //si la valeur est différente de ce qu'on a déjà
                {
                    _elo = value; //on enregistre la nouvelle valeur
                    OnPropertyChanged(); //+ on lance l'alerte
                }
            }
        }

        public string Nom //fonction mettant à jour le nom pour l'affichage (en cas de modif)
        {
            get { return _nom; }
            set
            {
                if (_nom != value)
                {
                    _nom = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Prenom //en cas de modif
        {
            get { return _prenom; }
            set
            {
                if (_prenom != value)
                {
                    _prenom = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Email //en cas de modif
        {
            get { return _email; }
            set
            {
                if (_email != value)
                {
                    _email = value;
                    OnPropertyChanged();
                }
            }
        }

        //constructeur = méthode spéciale appelée quand on crée un nouveau joueur
        public Joueur(int id, string nom, string prenom, string email, double elo)
        {
            ID = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Elo = elo;
        }

        //fonctions nécessaires pour implémenter les "notifications" à qqconque modif du joueur
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}