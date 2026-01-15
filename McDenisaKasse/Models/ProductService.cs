using System.Collections.Generic;

namespace McDenisaKasse.Models
{
    public static class ProductService
    {
        // ZUTATEN (für den "Zutaten ändern" Button)
        public static List<string> GetZutatenFuerProdukt(string produktName)
        {
            // Die Abfrage mit .Contains sorgt dafür, dass z.B. "Big Mac Menu" 
            // die gleichen Zutaten anzeigt wie der "Big Mac" einzeln.
            if (string.IsNullOrEmpty(produktName)) return new List<string>();

            if (produktName.Contains("Big Mac")) 
                return new List<string> { "Fleisch", "Salat", "Käse", "Zwiebeln", "Gurke", "Big Mac Soße" };

            if (produktName.Contains("Royal TS")) 
                return new List<string> { "Fleisch", "Salat", "Käse", "Tomate", "Zwiebeln", "Soße" };

            if (produktName.Contains("Royal Käse")) 
                return new List<string> { "Fleisch", "Käse", "Senf", "Ketchup", "Zwiebeln", "Gurke" };

            if (produktName.Contains("Double-cheese-burger"))
                return new List<string> { "2x Fleisch", "2x Käse", "Senf", "Ketchup", "Zwiebeln", "Gurke" };

            if (produktName.Contains("Cheese-burger"))
                return new List<string> { "Fleisch", "Käse", "Senf", "Ketchup", "Zwiebeln", "Gurke" };

            if (produktName.Contains("Hamburger"))
                return new List<string> { "Fleisch", "Senf", "Ketchup", "Zwiebeln", "Gurke" };

            if (produktName.Contains("McChicken"))
                return new List<string> { "Chicken Patty", "Salat", "Soße" };

            if (produktName.Contains("Chicken-burger"))
                return new List<string> { "Chicken Patty", "Salat", "Sweet Chilli Soße" };

            if (produktName.Contains("McRib"))
                return new List<string> { "Pork Patty", "Zwiebeln", "Gurke", "BBQ Soße" };

            if (produktName.Contains("Pommes"))
                return new List<string> { "Salz" };

            return new List<string>(); 
        }

        // OPTIONEN
        public static List<string> GetStandardGroessen() 
        {
            return new List<string> { "Klein", "Mittel", "Groß" };
        }

        public static List<string> GetNuggetAnzahl() 
        {
            return new List<string> { "6er Nuggets", "9er Nuggets", "20er Nuggets" };
        }

        public static List<string> GetNuggetSossen()
        {
            return new List<string> { "Mayonaisse", "Ketchup", "Süßsauer", "Curry", "BBQ", "Senf" };
        }

        public static List<string> GetExtraSossen()
        {
            return new List<string> { "Mayonnaise", "Ketchup", "Süßsauer", "Curry", "BBQ", "Senf" };
        }

        public static List<string> GetEisToppings()
        {
            return new List<string> { "Schokolade Soße", "Karamell Soße", "Ohne Soße" };
        }

        public static List<string> GetMilkshakeGeschmack()
        {
            return new List<string> { "Vanille", "Schokolade", "Erdbeere" };
        }

        public static List<string> GetGetraenkeSorten()
        {
            return new List<string> { "Cola", "Fanta", "Sprite", "Ice Tea" };
        }
        
        public static List<string> GetGetraenkeGroessen()
        {
            return new List<string> { "0,25l", "0,4l", "0,5l" };
        }

        public static List<string> GetMcCafeSorten()
        {
            return new List<string> { "Kaffee Crema", "Latte Macchiato", "Cappuccino", "Espresso", "Heiße Schokolade" };
        }
        
        public static List<string> GetHappyMealNachtisch()
        {
            return new List<string> { "Apfeltüte", "Fruchtquatsch", "McFreezy Eis" };
        }
        
        public static List<string> GetHappyMealGetraenke()
        {
            return new List<string> { "Capri-Sun", "Wasser", "O-Saft", "Bio-Apfelschorle" };
        }
    }
}