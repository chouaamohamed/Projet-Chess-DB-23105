using System;
using ChessDB.Models; //pour qu'il connaisse la classe "Joueur" et "ResultatMatch"

namespace ChessDB.Services
{
    public class CalculateurELO
    {
        private const int K = 20;

        public void UpdateELO(Joueur J1, Joueur J2, ResultatMatch resultat)
        {

            double J1Attendu = 1 / (1 + Math.Pow(10, (J2.Elo - J1.Elo) / 400)); //formule pour calculer l'elo
            
            double J2Attendu = 1 - J1Attendu; //on fait 1 - le score adverse pour avoir le score en qst

            //le vrai score du match (1 pour victoire, 0.5 nul, 0 défaite)
            double scoreReelJ1 = 0;
            if (resultat == ResultatMatch.GainJoueur1) scoreReelJ1 = 1;
            else if (resultat == ResultatMatch.Nul) scoreReelJ1 = 0.5;
            //pas besoin de mettre if (resultat == ResultatMatch.GainJoueur2) car scoreReelJ1 reste à 0 si J2 gagne

            double scoreReelJ2 = 1 - scoreReelJ1;

            //calcul des nv scores
            //nouveau = ancien + K * (réalité - score attendu)
            int nouveauEloJ1 = (int)(J1.Elo + K * (scoreReelJ1 - J1Attendu));
            int nouveauEloJ2 = (int)(J2.Elo + K * (scoreReelJ2 - J2Attendu));

            //on met à jour les elos
            J1.Elo = nouveauEloJ1;
            J2.Elo = nouveauEloJ2;
            //on fait des int nouveauElo car si on disait que J1.Elo = calcul alors J2.Elo prendrait en compte dans la ligne suivante le nv J1.Elo dans son calcul, ce qui fausserait sa valeur
        }
    }
}