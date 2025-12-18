using System;
using ChessDB.Models; //pour qu'il connaisse la classe "Joueur" et "ResultatMatch"

namespace ChessDB.Services
{
    public class CalculateurELO
    {
        private const int K = 20;

        public static void UpdateELO(Joueur J1, Joueur J2, ResultatMatch resultat, out int gainJ1, out int gainJ2) //le "static" permet de dire qu'on utilise cette fonction directement et qu'on a pas besoin de créer un objet "CalculateurELO" spécifique
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
            //calcul gain (arrondi) = K * (réalité - score attendu)
            gainJ1 = (int)Math.Round(K * (scoreReelJ1 - J1Attendu), MidpointRounding.AwayFromZero);
            gainJ2 = (int)Math.Round(K * (scoreReelJ2 - J2Attendu), MidpointRounding.AwayFromZero);

            //on met à jour les elos
            J1.Elo += gainJ1;
            J2.Elo += gainJ2;
            //on sépare gainJ1 de J1.Elo car si on disait que J1.Elo += calcul gain alors J2.Elo prendrait en compte dans la ligne suivante le nv J1.Elo dans son calcul, ce qui fausserait sa valeur
        }
    }
}