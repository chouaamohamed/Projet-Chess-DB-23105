using System.Collections.Generic; //pour utiliser les "List"

namespace ChessDB.Models
{
    public class Competition
    {
        public int ID { get; set; }
        public string Nom { get; set; }
        
        //liste des joueurs inscrits spécifiquement à CE tournoi
        public List<Joueur> JoueursInscrits { get; set; }
        
        //liste des matchs qui ont lieu durant ce tournoi
        public List<Match> Matchs { get; set; }

        public Competition(int id, string nom)
        {
            ID = id;
            Nom = nom;
            //on initialise les listes vides pour pouvoir ajouter des choses dedans plus tard
            JoueursInscrits = new List<Joueur>();
            Matchs = new List<Match>();
        }
    }
}