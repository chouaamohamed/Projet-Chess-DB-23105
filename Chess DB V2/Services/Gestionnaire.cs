using System.Collections.Generic;
using System.Linq;
using ChessDB.Models;
using System.Collections.ObjectModel;

namespace ChessDB.Services
{
    public class Gestionnaire
    {
        //la "base de données" en mémoire
        public ObservableCollection<Joueur> TousLesJoueurs { get; set; }
        public ObservableCollection<Competition> Competitions { get; set; }

        public Gestionnaire()
        {
            TousLesJoueurs = new ObservableCollection<Joueur>();
            Competitions = new ObservableCollection<Competition>();
        }

        //ajouter un nouveau joueur à la fédération
        public void AjouterJoueur(string nom, string prenom, string email, int elo = 1200) //on met 1200 par défaut à elo dans le cas où le joueur n'a pas déjà d'elo à lui et qu'il débute
        {
            //on calcule l'ID automatiquement en comptant le nombre de joueurs total et en prenant le dernier + 1
            //si la liste est vide, on commence à 1 (condition ? conséquence : alternative)
            int nouvelId = TousLesJoueurs.Count > 0 ? TousLesJoueurs.Max(j => j.ID) + 1 : 1;
            
            Joueur nouveau = new Joueur(nouvelId, nom, prenom, email, elo);
            TousLesJoueurs.Add(nouveau);
        }

        //créer une nouvelle compétition
        public void AjouterCompetition(string nom)
        {
            int nouvelId = Competitions.Count > 0 ? Competitions.Max(c => c.ID) + 1 : 1;
            Competition nouvelleComp = new Competition(nouvelId, nom);
            Competitions.Add(nouvelleComp);
        }

        //inscrire un joueur à une compétition
        public void InscrireJoueur(int idCompetition, int idJoueur)
        {
            //on trouve la compétition
            Competition? comp = Competitions.FirstOrDefault(c => c.ID == idCompetition);
            
            //on trouve le joueur
            Joueur? joueur = TousLesJoueurs.FirstOrDefault(j => j.ID == idJoueur);

            //si les deux existent on inscrit le joueur
            if (comp != null && joueur != null)
            {
                //on vérifie que le joueur est pas déjà inscrit pour éviter les doublons
                if (!comp.JoueursInscrits.Contains(joueur))
                {
                    comp.JoueursInscrits.Add(joueur);
                }
            }
        }

        //permet d'organiser les matchs
        public void AjouterMatch(int idCompetition, int idJoueur1, int idJoueur2)
        {
            //récupère le tournoi
            Competition? comp = Competitions.FirstOrDefault(c => c.ID == idCompetition);

            //récupère les 2 joueurs
            Joueur? j1 = TousLesJoueurs.FirstOrDefault(j => j.ID == idJoueur1);
            Joueur? j2 = TousLesJoueurs.FirstOrDefault(j => j.ID == idJoueur2);

            //vérifs
            if (comp != null && j1 != null && j2 != null)
            {
                //on vérifie que les joueurs sont bien inscrits dans ce tournoi
                if (comp.JoueursInscrits.Contains(j1) && comp.JoueursInscrits.Contains(j2))
                {
                    //on crée le match + on calcule un ID unique pour le match (comme pour les joueurs)
                    int nouvelIdMatch = comp.Matchs.Count > 0 ? comp.Matchs.Max(m => m.ID) + 1 : 1;

                    Match nouveauMatch = new Match(nouvelIdMatch, j1, j2);
                    
                    //ajout à la liste des matchs du tournoi
                    comp.Matchs.Add(nouveauMatch);
                }
            }
        }

        public void EnregistrerResultat(int idCompetition, int idMatch, ResultatMatch resultat)
        {
            //trouver la compétition
            Competition? comp = Competitions.FirstOrDefault(c => c.ID == idCompetition);
            if (comp == null) return; // Sécurité

            //trouver le match DANS cette compétition
            Match? match = comp.Matchs.FirstOrDefault(m => m.ID == idMatch);
            
            //si le match existe et n'a pas encore été joué
            if (match != null && match.Resultat == ResultatMatch.PasEncoreJoue)
            {
                //on enregistre le résultat dans le match (on passe de PasEncoreJoue à GainJoueur1 ou 2 ou Nul)
                match.Resultat = resultat;
            }
        }
    }
}