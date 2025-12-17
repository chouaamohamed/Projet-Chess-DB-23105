using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChessDB.Models
{
    public class Joueur : INotifyPropertyChanged
    {   
        public int ID { get; set; } //numéro unique pour identifier le joueur
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Email { get; set; }
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

        //constructeur = méthode spéciale appelée quand on crée un nouveau joueur
        public Joueur(int id, string nom, string prenom, string email, double elo)
        {
            ID = id;
            Nom = nom;
            Prenom = prenom;
            Email = email;
            Elo = elo;
        }

        //fonctions nécessaires pour implémenter les "notifications" au changement d'elo
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}