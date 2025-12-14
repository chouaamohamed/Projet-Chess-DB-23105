namespace ChessDB.Models
{
    public enum ResultatMatch
    {
        PasEncoreJoue,
        GainJoueur1,  //1-0 pour les échecs
        GainJoueur2,  //0-1 pour échecs
        Nul           //0.5-0.5
    }

    public class Match
    {
        public int ID { get; set; }

        public Joueur Joueur1 { get; set; }
        public Joueur Joueur2 { get; set; }

        public string Coups { get; set; }
        
        public ResultatMatch Resultat { get; set; }

        public Match(int id, Joueur j1, Joueur j2)
        {
            ID = id;
            Joueur1 = j1;
            Joueur2 = j2;
            Resultat = ResultatMatch.PasEncoreJoue; //par défaut, le match est pas joué
        }
    }
}