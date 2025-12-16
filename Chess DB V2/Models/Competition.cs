using System.Collections.Generic; //pour utiliser les "List"
using System.Collections.ObjectModel; //pour des listes qui se mettent à jour

namespace ChessDB.Models
{
    public class Competition
    {
        public int ID { get; set; }
        public string Nom { get; set; }
        
        //liste des joueurs inscrits spécifiquement à CE tournoi
        public ObservableCollection<Joueur> JoueursInscrits { get; set; } //observablecollection sert à mettre à jour l'écran dès que y a un chgmt
        
        //liste des matchs qui ont lieu durant ce tournoi
        public List<Match> Matchs { get; set; }

        public Competition(int id, string nom)
        {
            ID = id;
            Nom = nom;
            //on initialise les listes vides pour pouvoir ajouter des choses dedans plus tard
            JoueursInscrits = new ObservableCollection<Joueur>();
            Matchs = new List<Match>();
        }
    }
}