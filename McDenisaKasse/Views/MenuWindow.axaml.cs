using Avalonia.Controls;
using Avalonia.Interactivity;
using McDenisaKasse.ViewModels; // Zugriff auf ViewModel
using System.Collections.Generic;

namespace McDenisaKasse.Views
{
    public partial class MenuWindow : Window
    {
        // Ich speichere ViewModel hier in variable damit ich später benutzen kann.
        // Readonly ist gut für Sicherheit
        private readonly MenuViewModel _viewModel;

        public MenuWindow()
        {
            InitializeComponent();
            
            // Hier ich erstelle neue ViewModel und verbinde es mit View.
            // DataContext sehr wichtig damit Binding in XAML funktioniert
            _viewModel = new MenuViewModel();
            DataContext = _viewModel;
        }

        // =========================================================
        // PRODUKTE KLICKEN (UI Logik & Dialoge)
        // =========================================================
        // Methode muss async sein, weil ich mit "await" auf User warten muss (Popups).
        public async void OnProduktClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (button.Content == null) return; // Safety check

            string produktName = button.Content.ToString();
            
            // --- Logik für Popups bleibt in View (UI Sache) ---
            // ViewModel darf keine Fenster öffnen direkt.
            
            if (produktName.Contains("Menu"))
            {
                // Ich mache Name sauber (lösche "Menu" Wort)
                string burgerName = produktName.Replace(" Menu", "");
                
                // Ich erstelle Dialog Fenster für Konfiguration
                var dialog = new MenuConfigWindow(burgerName);
                await dialog.ShowDialog(this); // Code wartet hier bis User fertig
                
                // WENN User hat gespeichert (nicht null) -> Daten an ViewModel geben
                if (dialog.ResultString != null) 
                {
                    _viewModel.FuegeProduktHinzu(dialog.ResultString + " Menü");
                }
            }
            else if (produktName.Contains("Happy Meal"))
            {
                string hauptspeise = produktName.Replace("Happy Meal ", "");
                
                // Ich frage User nach Beilage mit meine Hilfsfunktion
                var beilagen = new List<string> { "Apfeltüte", "Fruchtquatsch", "McFreezy Eis" };
                string beilage = await FrageStellen("Wähle ein Nachtisch", beilagen);
                
                if (beilage == null) return; // Wenn User bricht ab

                var getraenke = new List<string> { "Capri-Sun", "Wasser", "O-Saft", "Bio-Apfelschorle" };
                string getraenk = await FrageStellen("Wähle ein Getränk:", getraenke);
                if (getraenk == null) return; 

                // Alles fertig, ich sende Daten an ViewModel Logic
                _viewModel.FuegeProduktHinzu($"Happy Meal {hauptspeise}", $"({beilage}, {getraenk})");
            }
            else if (produktName.Contains("Pommes"))
            {
                var optionen = new List<string> { "Klein", "Mittel", "Groß" };
                string wahl = await FrageStellen("Welche Größe?", optionen);
                
                if (wahl != null) 
                    _viewModel.FuegeProduktHinzu("Pommes", $"({wahl})");
            }
            else if (produktName.Contains("Nuggets"))
            {
                string menge = await FrageStellen("Wie viele?", new List<string> { "6er", "9er", "20er" });
                if (menge == null) return;
                
                string sosse = await FrageStellen("Welche Soße?", new List<string> { "Mayonaisse", "Ketchup", "Süßsauer", "Curry", "BBQ", "Senf" });
                
                if (sosse != null) 
                    _viewModel.FuegeProduktHinzu($"Nuggets {menge}", $"({sosse})");
            }
            else if (produktName.Contains("Eis") || produktName.Contains("Eis"))
            {
                var toppings = new List<string> { "Schokolade Soße", "Karamell Soße", "Ohne Soße" };
                string wahl = await FrageStellen("Topping:", toppings);
                
                if (wahl != null) 
                    _viewModel.FuegeProduktHinzu("Eis", $"({wahl})");
            }
            else if (produktName.Contains("Milkshake"))
            {
                string geschmack = await FrageStellen("Geschmack?", new List<string> { "Vanille", "Schokolade", "Erdbeere" });
                if (geschmack == null) return;
                string groesse = await FrageStellen("Größe?", new List<string> { "Klein", "Mittel", "Groß" });
                
                if (groesse != null) 
                    _viewModel.FuegeProduktHinzu($"Milkshake {geschmack}", $"({groesse})");
            }
            else if (produktName.Contains("Getränk") || produktName == "Cola" || produktName == "Fanta")
            {
                // Wenn Button heißt nur "Getränk", ich frage Sorte. Sonst ich nehme Name von Button.
                string sorte = produktName == "Getränk" ? await FrageStellen("Sorte:", new List<string> { "Cola", "Fanta", "Sprite", "Ice Tea" }) : produktName;
                if (sorte == null) return;
                string groesse = await FrageStellen("Größe?", new List<string> { "0,25l", "0,4l", "0,5l" });
                
                if (groesse != null) 
                    _viewModel.FuegeProduktHinzu(sorte, $"({groesse})");
            }
            else if (produktName.Contains("Soße"))
            {
                string wahl = await FrageStellen("Welche Soße?", new List<string> { "Mayonnaise", "Ketchup", "Süßsauer", "BBQ", "Senf" });
                if (wahl != null) _viewModel.FuegeProduktHinzu($"Soße {wahl}");
            }
            else if (produktName == "McCafé")
            {
                var kaffees = new List<string> { "Kaffee Crema", "Latte Macchiato", "Cappuccino", "Espresso", "Heiße Schokolade" };
                string wahl = await FrageStellen("Willkommen im McCafé:", kaffees);
                if (wahl != null) _viewModel.FuegeProduktHinzu($"McCafé {wahl}");
            }
            else
            {
                // Standard Produkt ohne Fragen (einfach hinzufügen)
                _viewModel.FuegeProduktHinzu(produktName);
            }
        }

        // =========================================================
        // ZUTATEN ÄNDERN
        // =========================================================
        private async void OnZutatenClick(object sender, RoutedEventArgs e)
        {
            // Ich frage ViewModel: Welche Position ist selektiert in Liste?
            var selectedItem = _viewModel.SelektiertePosition;

            if (selectedItem == null) return; // Wenn nichts ausgewählt, ich mache return

            string name = selectedItem.Name;
            
            // Hier ich definiere Liste für Zutaten Fenster.
            // Logik ist hier okay weil es ist UI Darstellung.
            List<string> zutaten = new List<string>();
            if (name.Contains("Big Mac")) zutaten = new List<string> { "Fleisch", "Salat", "Käse", "Zwiebeln", "Gurke", "Big Mac Soße" };
            else if (name.Contains("Royal TS")) zutaten = new List<string> { "Fleisch", "Salat", "Käse", "Tomate", "Zwiebeln", "Soße" };
            else if (name.Contains("Hamburger")) zutaten = new List<string> { "Fleisch", "Senf", "Ketchup", "Zwiebeln", "Gurke" };
            else if (name.Contains("Cheese-burger")) zutaten = new List<string> { "Fleisch", "Käse", "Senf", "Ketchup", "Zwiebeln", "Gurke" };
            else if (name.Contains("McChicken")) zutaten = new List<string> { "Chicken Patty", "Salat", "Soße" };
            else if (name.Contains("McRib")) zutaten = new List<string> { "Pork Patty", "Zwiebeln", "Gurke", "BBQ Soße" };
            else if (name.Contains("Pommes")) zutaten = new List<string> { "Salz" };
            else return; // Produkt hat keine Zutaten Liste

            // Ich zeige Fenster für Zutaten ändern
            var fenster = new IngredientsWindow(name, zutaten);
            await fenster.ShowDialog(this);

            // Wenn Fenster ist zu und ResultString ist da
            if (fenster.ResultString != null)
            {
                // Ich sage ViewModel bitte update Item.
                // Ich baue String zusammen mit Klammern.
                string neuerZusatz = fenster.ResultString == "" ? "" : $"({fenster.ResultString})";
                _viewModel.AktualisierePosition(selectedItem, neuerZusatz);
            }
        }

        // Hilfsmethode damit ich Code nicht doppelt schreiben muss für OptionWindow
        private async System.Threading.Tasks.Task<string> FrageStellen(string titel, List<string> antworten)
        {
            var fenster = new OptionWindow(titel, antworten);
            await fenster.ShowDialog(this);
            return fenster.SelectedOption; // Gibt String zurück was User geklickt hat
        }

        private void OnSchliessenClick(object sender, RoutedEventArgs e) { this.Close(); } // Fenster zu
        
        private void OnPreiseClick(object sender, RoutedEventArgs e)
        {
            new PriceWindow().Show(); // Preis Tabelle zeigen
        }

        private void OnAbschliessenClick(object sender, RoutedEventArgs e)
        {
            new CheckoutWindow().Show(); // Zur Kasse gehen
        }
    }
}