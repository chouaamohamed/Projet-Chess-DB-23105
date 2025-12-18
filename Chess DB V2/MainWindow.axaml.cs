using Avalonia.Controls;
using ChessDB.Models; //pour les joueurs
using ChessDB.Services; //important pour trouver ton Gestionnaire
using System.Linq;

namespace ChessDB
{
    public partial class MainWindow : Window
    {
        //on stocke le gestionnaire ici pour que la fenêtre puisse l'utiliser
        private Gestionnaire _gestionnaire;

        public MainWindow()
        {
            InitializeComponent();

            //on intialise gestionnaire
            _gestionnaire = new Gestionnaire();

            var laListBox = this.FindControl<ListBox>("ListeJoueurs"); //cette ligne va chercher dans le fichier .axml un composant de type ListBox ayant comme nom "ListeJoueurs"
            
            //si la variable trouve quelque chose dans le fichier .axml, celui-ci va prendre comme "item sources" tous les joueurs de gestionnaire
            if (laListBox != null)
            {
                laListBox.ItemsSource = _gestionnaire.TousLesJoueurs;
            }

            //mm chose pour les compétitions
            var listeComps = this.FindControl<ListBox>("ListeCompetitions");

            if (listeComps != null)
            {
                listeComps.ItemsSource = _gestionnaire.Competitions;
            }
            
            //mm chose pour la liste de joueurs (liste déroulante)
            var combo = this.FindControl<ComboBox>("SelecteurJoueur");

            if (combo != null)
            {
                combo.ItemsSource = _gestionnaire.TousLesJoueurs;
            }

            var listeCompMatchs = this.FindControl<ListBox>("ListeCompetitionsMatchs");

            if (listeCompMatchs != null)
            {
                //on branche la même source de données que l'autre onglet
                listeCompMatchs.ItemsSource = _gestionnaire.Competitions;
            }
        }

        //fonction appelée quand on clique sur le bouton "Ajouter le joueur"
        public void BoutonJoueur(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var inputNom = this.FindControl<TextBox>("InputNom");
            var inputPrenom = this.FindControl<TextBox>("InputPrenom");
            var inputEmail = this.FindControl<TextBox>("InputEmail");
            var inputElo = this.FindControl<TextBox>("InputElo");
            var laListBox = this.FindControl<ListBox>("ListeJoueurs");

            //obligé de mettre cette ligne de code sinon VSC veut pas lancer le code
            if (inputNom == null || inputPrenom == null || inputEmail == null || inputElo == null || laListBox == null)
            {
                return;
            }

            if (string.IsNullOrWhiteSpace(inputNom.Text) || string.IsNullOrWhiteSpace(inputPrenom.Text)) //on vérifie que le nom et prénom sont pas vides (elo et mail sont optionnels)
            {
                return; 
            }

            int elo = 1200;
            //si y a écrit qq chose dans inputElo + ce qq chose est un chiffre, on dit que elo = eloSaisi 
            if (!string.IsNullOrWhiteSpace(inputElo.Text) && int.TryParse(inputElo.Text, out int eloSaisi)) //TryParse sert à essayer la conversion de l'inputElo en chiffre, si ça marche on l'output en tant que eloSaisi
            {
                elo = eloSaisi;
            }

            string email = inputEmail.Text ?? ""; //si inputEmail.Text est nul, mettre "" à la place
            
            _gestionnaire.AjouterJoueur(inputNom.Text, inputPrenom.Text, email, elo);

            laListBox.ItemsSource = null; //obligé de redémarrer la source car ListBox va pas remarquer les changements
            laListBox.ItemsSource = _gestionnaire.TousLesJoueurs; 

            //on réinitialise les entrées
            inputNom.Text = "";
            inputPrenom.Text = "";
            inputEmail.Text = "";
            inputElo.Text = "";
        }

        public void BoutonTournoi(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var inputNomTournoi = this.FindControl<TextBox>("InputNomTournoi");
            var listeComps = this.FindControl<ListBox>("ListeCompetitions");

            //obligé de mettre cette ligne de code sinon VSC veut pas lancer le code
            if (inputNomTournoi == null || listeComps == null) return;

            //on vérifie que qq chose est écrit avant d'ajouter
            if (string.IsNullOrWhiteSpace(inputNomTournoi.Text)) return;

            //sert à ajouter le nom qu'on a écrit pour la compét
            _gestionnaire.AjouterCompetition(inputNomTournoi.Text);

            //redémarrage de la source (comme pour BoutonJoueur)
            listeComps.ItemsSource = null;
            listeComps.ItemsSource = _gestionnaire.Competitions;

            //réinitialisation
            inputNomTournoi.Text = "";
        }

        public void SelectionTournoi(object sender, SelectionChangedEventArgs e) //quand on clique sur un tournoi à gauche
        {
            //on récupère les parties visuelles
            var zoneGestion = this.FindControl<Border>("ZoneGestion");
            var txtTitre = this.FindControl<TextBlock>("TxtTitreTournoi");
            var listeInscrits = this.FindControl<ListBox>("ListeInscrits");
            var combo = this.FindControl<ComboBox>("ComboJoueursDispo");
            var listeComps = this.FindControl<ListBox>("ListeCompetitions");
            var listeMatchs = this.FindControl<ListBox>("ListeMatchs");

            //sécurité comme d'hab
            if (zoneGestion == null || txtTitre == null || listeInscrits == null || combo == null || listeComps == null || listeMatchs == null)
            {
                return;
            }

            //quel tournoi a été cliqué ?
            var tournoi = listeComps.SelectedItem as Competition; //on traite l'élement sélectionné comme un élément de Competition.cs

            if (tournoi == null)
            {
                zoneGestion.IsVisible = false; //si rien n'est sélectionné ou ctrl + click, on cache la zone de droite
                return;
            }

            zoneGestion.IsVisible = true; //sinon on affiche
            
            //on affiche les options manuellement

            txtTitre.Text = tournoi.Nom; //le titre du tournoi

            listeInscrits.ItemsSource = tournoi.JoueursInscrits; //la liste des joueurs inscrits pour CETTE compétition

            combo.ItemsSource = _gestionnaire.TousLesJoueurs; //la liste de tous les joueurs (combo = liste déroulante en axaml)
        }

        public void BoutonInscrire(object sender, Avalonia.Interactivity.RoutedEventArgs e) //quand on clique sur le bouton inscrire
        {
            var listeComps = this.FindControl<ListBox>("ListeCompetitions");
            var combo = this.FindControl<ComboBox>("ComboJoueursDispo");

            if (listeComps == null || combo == null)
            {
                return;
            }

            //on récupère le tournoi sélectionné à gauche et le joueur sélectionné dans le combo (liste déroulante)
            var tournoi = listeComps.SelectedItem as Competition; //considéré comme compétition
            var joueur = combo.SelectedItem as Joueur; //considéré comme joueur

            if (tournoi != null && joueur != null)
            {
                //on vérifie que le joueur est pas déjà inscrit
                if (!tournoi.JoueursInscrits.Contains(joueur))
                {
                    tournoi.JoueursInscrits.Add(joueur);
                }
                
                //quand on rajoute une personne, on réintialise l'item sélectionné
                combo.SelectedItem = null;
            }
        }

        public void SelectionTournoiMatch(object sender, SelectionChangedEventArgs e)
        {
            //on initialise les élements du nouvel onglet (Gestion Matchs)
            var zone = this.FindControl<Grid>("ZoneMatchs");
            var titre = this.FindControl<TextBlock>("TxtTitreMatchs");
            var listeMatchs = this.FindControl<ListBox>("ListeMatchs");
            var listeComps = this.FindControl<ListBox>("ListeCompetitionsMatchs");
            var zoneDetails = this.FindControl<Border>("ZoneDetailsMatch");

            if (zone == null || titre == null || listeMatchs == null || listeComps == null) return;

            //comme pour SelectionTournoi on précise quelle compétition et on traite l'élément sélectionné comme s'il venait de Competition.cs
            var tournoi = listeComps.SelectedItem as Competition;

            if (tournoi == null)
            {
                zone.IsVisible = false; //logique, si y a pas de compétitions/qu'on ne clique pas sur un tournoi, rien ne s'affichera au niveau des matchs
                if (zoneDetails != null)
                {
                    zoneDetails.IsVisible = false; //pour être sûr que la zone de détails des matchs s'affiche pas si on a pas de tournoi
                }
                return;
            }

            zone.IsVisible = true; //sinon on affiche
            titre.Text = "Matchs de : " + tournoi.Nom;
            
            //liaison visuelle <--> partie code
            listeMatchs.ItemsSource = tournoi.Matchs;
        }

        public void BoutonLancerTournoi(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            //on initalise à nouveau la liste de compétitions
            var listeComps = this.FindControl<ListBox>("ListeCompetitionsMatchs");
            
            if (listeComps == null)
            {
                return;
            }

            var tournoi = listeComps.SelectedItem as Competition;

            if (tournoi != null)
            {               
                //on parcourt tous les matchs existants pour annuler leur impact précis (logique pour reset l'elo avec une différence de gain)
                foreach (var vieuxMatch in tournoi.Matchs)
                {
                    if (vieuxMatch.Resultat != ResultatMatch.PasEncoreJoue)
                    {
                        //on inverse exactement ce que le match sélectionné avait fait (on va le faire pour tt les matchs)
                        vieuxMatch.Joueur1.Elo -= vieuxMatch.GainEloJ1;
                        vieuxMatch.Joueur2.Elo -= vieuxMatch.GainEloJ2;
                    }
                }

                tournoi.CreerMatchs();
            }
        }

        public void SelectionMatch(object sender, SelectionChangedEventArgs e)
        {
            var zoneDetails = this.FindControl<Border>("ZoneDetailsMatch");
            var txtAffiche = this.FindControl<TextBlock>("TxtAfficheMatch");
            var inputCoups = this.FindControl<TextBox>("InputCoups");
            var listeMatchs = this.FindControl<ListBox>("ListeMatchs");

            if (zoneDetails == null || txtAffiche == null || inputCoups == null || listeMatchs == null)
            {
                return;
            }
            
            var match = listeMatchs.SelectedItem as Match;

            if (match == null)
            {
                zoneDetails.IsVisible = false; //pour être sûr que la zone de détails des matchs s'affiche pas si on a pas de match sélectionné
                return;
            }

            //on affiche la zone d'arbitrage
            zoneDetails.IsVisible = true;
            
            //on remplit les infos
            txtAffiche.Text = $"{match.Joueur1.Nom} VS {match.Joueur2.Nom}";
            
            //pour ajouter les coups du match
            inputCoups.Text = match.Coups;
        }

        public void BoutonResultat(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            //récupérer le match sélectionné
            var listeMatchs = this.FindControl<ListBox>("ListeMatchs");
            var inputCoups = this.FindControl<TextBox>("InputCoups");
            
            //on récupère le bouton cliqué pour savoir qui a gagné (grâce au "Tag")
            var boutonClique = sender as Avalonia.Controls.Button;

            if (listeMatchs == null || inputCoups == null || boutonClique == null)
            {
                return;
            }

            var match = listeMatchs.SelectedItem as Match;

            if (match == null) return;

            //sauvegarder les coups
            match.Coups = inputCoups.Text ?? ""; //les ?? c'est une mesure de sécurité, si jms le inputCoups est null, on écrit "", donc au final rien

            //si le match avait déjà été joué on annule les points exacts qu'il est censé donné
            if (match.Resultat != ResultatMatch.PasEncoreJoue)
            {
                match.Joueur1.Elo -= match.GainEloJ1; //on enlève ce qu'on avait ajouté
                match.Joueur2.Elo -= match.GainEloJ2;
                
                //+ on remet les compteurs du match à zéro par sécurité
                match.GainEloJ1 = 0;
                match.GainEloJ2 = 0;
            }

            //déterminer le résultat selon le Tag du bouton (1, X, 2)
            string? code = boutonClique.Tag?.ToString(); //le ? dans Tag et string veut dire vérifie que Tag/string existe avant de convertir en texte

            //on définit d'abord le résultat dans l'objet Match
            if (code == "1") 
            {
                match.Resultat = ResultatMatch.GainJoueur1;
            }
            else if (code == "2") 
            {
                match.Resultat = ResultatMatch.GainJoueur2;
            }
            else //nul
            {
                match.Resultat = ResultatMatch.Nul;
            }

            //calcul Elo pour pouvoir le mettre à jour avec le "match.Resultat" qu'on vient de déterminer
            int pointsJ1, pointsJ2;
            Services.CalculateurELO.UpdateELO(match.Joueur1, match.Joueur2, match.Resultat, out pointsJ1, out pointsJ2);

            //on les sauvegarde DANS le match pour plus tard (pour pouvoir annuler ou reset)
            match.GainEloJ1 = pointsJ1;
            match.GainEloJ2 = pointsJ2;
        }

        public void BoutonSupprimerTournoi(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var bouton = (Avalonia.Controls.Button)sender;
            
            //le tournoi attaché au bouton est récupéré avec le Tag
            var tournoiASupp = bouton.Tag as Models.Competition;

            if (tournoiASupp != null)
            {
                //on supp le tournoi de la liste observable
                _gestionnaire.Competitions.Remove(tournoiASupp);
            }
        }

        public void BoutonSupprimerJoueur(object sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            var bouton = (Avalonia.Controls.Button)sender;
            var joueurASupp = bouton.Tag as Models.Joueur;

            if (joueurASupp != null)
            {
                //on vérifie d'abord si le joueur en qst est inscrit dans un tournoi avant de le supp
                bool estInscrit = _gestionnaire.Competitions.Any(c => c.JoueursInscrits.Contains(joueurASupp));

                if (estInscrit)
                {
                    bouton.Content = "⚠️ Occupé !"; //si le joueur en qst est inscrit, on affiche juste le message "occupé", rien de plus
                    return; 
                }

                //sinon on peut supprimer le joueur
                _gestionnaire.TousLesJoueurs.Remove(joueurASupp);
            }
        }
    }
}