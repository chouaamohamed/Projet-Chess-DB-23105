using Avalonia.Controls;
using ChessDB.Models; //pour les joueurs
using ChessDB.Services; //important pour trouver ton Gestionnaire

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

        public void BoutonCompet(object sender, Avalonia.Interactivity.RoutedEventArgs e)
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
    }
}