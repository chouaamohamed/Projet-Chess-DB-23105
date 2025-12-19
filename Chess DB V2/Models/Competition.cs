using System.Collections.ObjectModel; //pour des listes qui se mettent à jour
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ChessDB.Models
{
    public class Competition : INotifyPropertyChanged //INotifyPropertyChanged = méthode qui notifie le système d'un chngmt (ici le nom du tournoi)
    {
        public int ID { get; set; }

        private string _nom = ""; //le = "" c'est pour initaliser avec une chaine vide (au cas où VSC a peur qu'il y a rien comme valeur --> parano)
        public string Nom //fonction mettant à jour le nom pour l'affichage (en cas de modif)
        {
            get { return _nom; }
            set
            {
                if (_nom != value) //si la valeur est différente de ce qu'on a déjà
                {
                    _nom = value; //on enregistre la nouvelle valeur
                    OnPropertyChanged(); //+ on lance l'alerte
                }
            }
        }
        
        //liste des joueurs inscrits spécifiquement à CE tournoi
        public ObservableCollection<Joueur> JoueursInscrits { get; set; } //observablecollection sert à mettre à jour l'écran dès que y a un chgmt
        
        //liste des matchs qui ont lieu durant ce tournoi
        public ObservableCollection<Match> Matchs { get; set; } //au final les "list" étaient une mauvaise idée pour accompagner une ui

        public Competition() //constructeur vide pour la sauvegarde
        {
            //on initialise les listes pour éviter les crash (si le json est vide)
            JoueursInscrits = new ObservableCollection<Joueur>();
            Matchs = new ObservableCollection<Match>();
        }

        public Competition(int id, string nom)
        {
            ID = id;
            Nom = nom;
            //on initialise les listes vides pour pouvoir ajouter des choses dedans plus tard
            JoueursInscrits = new ObservableCollection<Joueur>();
            Matchs = new ObservableCollection<Match>();
        }
        
        public void CreerMatchs()
        {
            Matchs.Clear(); //quand on rappelera la fonction on nettoiera tout ce qui a déjà été créé comme matchs
            
            //condition minimum pour avoir un match : 2 joueurs
            if (JoueursInscrits.Count < 2)
            {
                return;
            }

            int compteurID = 1;

            //i considéré comme joueur 1
            for (int i = 0; i < JoueursInscrits.Count; i++)
            {
                //j considéré comme joueur 2, les boucles for sont imbriqués de sorte à ce que tout le monde puisse s'affronter sans doublons
                for (int j = i + 1; j < JoueursInscrits.Count; j++)
                {
                    var joueur1 = JoueursInscrits[i];
                    var joueur2 = JoueursInscrits[j];

                    Match nouveauMatch = new Match(compteurID, joueur1, joueur2); //création du match
                    
                    Matchs.Add(nouveauMatch); //à chaque itération on ajoutera nouveauMatch à la "liste" de matchs
                    
                    compteurID++; //sert à avoir un ID pour les matchs (et pouvoir les différencier)
                }
            }
        }

        //fonctions nécessaires pour implémenter les "notifications" à chaque modif du nom du tournoi
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}