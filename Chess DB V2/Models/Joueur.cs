namespace ChessDB.Models
{
    public class Joueur
    {   
        public int ID { get; set; } //numéro unique pour identifier le joueur
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Elo { get; set; }
        public string Email { get; set; }

        //constructeur = méthode spéciale appelée quand on crée un nouveau joueur
        public Joueur(int id, string nom, string prenom, int elo, string email)
        {
            ID = id;
            Nom = nom;
            Prenom = prenom;
            Elo = elo;
            Email = email;
        }
    }
}