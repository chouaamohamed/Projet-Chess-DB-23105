using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ChessDB.Services
{
    public static class Sauvegarde
    {
        //le nom du fichier où on va tout écrire
        private static string FichierChemin = "sauvegarde_chessdb.json";

        public static void Sauvegarder(Gestionnaire gestionnaire)
        {
            //caractéristiques du fichier json
            var options = new JsonSerializerOptions 
            { 
                WriteIndented = true, //pour que l'indentation soit respectée (mieux pour lire)
                ReferenceHandler = ReferenceHandler.Preserve 
            };

            string json = JsonSerializer.Serialize(gestionnaire, options); //sauvegardera tout ce qui est dans gestionnaire en + des options du json
            File.WriteAllText(FichierChemin, json);
        }

        public static Gestionnaire Charger()
        {
            if (!File.Exists(FichierChemin))
            {
                //si le fichier existe pas, on renvoie un nouveau gestionnaire vide (comme si on démarrait le logiciel pour la 1ère fois)
                return new Gestionnaire(); 
            }

            try
            {
                string json = File.ReadAllText(FichierChemin);
                var options = new JsonSerializerOptions 
                { 
                    ReferenceHandler = ReferenceHandler.Preserve 
                };

                var donnees = JsonSerializer.Deserialize<Gestionnaire>(json, options);
                return donnees ?? new Gestionnaire(); //si le "donnees" est nul (corrompu) --> précaution on relance un nouveau "Gestionnaire"
            }
            catch (Exception)
            {
                //en cas d'erreur de lecture, on repart à zéro pour pas crash
                return new Gestionnaire();
            }
        }
    }
}