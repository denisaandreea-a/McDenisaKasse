using System.Collections.Generic;

namespace McDenisaKasse.Models 
{
    // Ich mache statische Klasse weil ich brauche keine Instanz.
    // "Hilfs-Werkzeug" für Produkte.
    public static class ProductService
    {
        // Methode gibt Liste von Zutaten zurück für String Name
        public static List<string> GetZutatenFuerProdukt(string produktName)
        {
            //checke Name und gebe richtige Liste zurück
            if (produktName.Contains("Big Mac")) 
            {
                return new List<string> { "Fleisch", "Salat", "Käse", "Zwiebeln", "Gurke", "Big Mac Soße" };
            }
            
            if (produktName.Contains("Royal TS")) 
            {
                return new List<string> { "Fleisch", "Salat", "Käse", "Tomate", "Zwiebeln", "Soße" };
            }

            if (produktName.Contains("Hamburger"))
            {
                return new List<string> { "Fleisch", "Senf", "Ketchup", "Zwiebeln", "Gurke" };
            }

            if (produktName.Contains("Cheese-burger"))
            {
                return new List<string> { "Fleisch", "Käse", "Senf", "Ketchup", "Zwiebeln", "Gurke" };
            }

            if (produktName.Contains("McChicken"))
            {
                return new List<string> { "Chicken Patty", "Salat", "Soße" };
            }

            if (produktName.Contains("McRib"))
            {
                return new List<string> { "Pork Patty", "Zwiebeln", "Gurke", "BBQ Soße" };
            }
            
            if (produktName.Contains("Pommes"))
            {
                return new List<string> { "Salz" };
            }

            // Wenn nichts gefunden, ich gebe leere Liste zurück (nicht null, sicherer)
            return new List<string>();
        }
    }
}