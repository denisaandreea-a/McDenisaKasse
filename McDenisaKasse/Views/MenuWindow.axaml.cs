using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Globalization; // Wichtig für Preise

namespace McDenisaKasse.Views
{
    public partial class MenuWindow : Window
    {
        public ObservableCollection<string> BestellListe { get; set; }
        private int _aktuelleMenge = 1;

        public MenuWindow()
        {
            InitializeComponent();
            BestellListe = new ObservableCollection<string>();
            DataContext = this;
        }

        // =========================================================
        // NEU: PREIS-LOGIK
        // =========================================================
        
        // 1. Hier legen wir die Preise fest
        private decimal GetEinzelPreis(string produktName)
        {
            // Menüs
            if (produktName.Contains("Menu")) return 9.49m;
            if (produktName.Contains("Happy Meal")) return 4.99m;

            // Burger
            if (produktName.Contains("Big Mac")) return 5.49m;
            if (produktName.Contains("Royal TS")) return 5.99m;
            if (produktName.Contains("McChicken")) return 4.99m;
            if (produktName.Contains("McRib")) return 5.49m;
            if (produktName.Contains("Hamburger")) return 1.79m;
            if (produktName.Contains("Cheese-burger")) return 1.99m; // Schreibweise beachten

            // Snacks & Beilagen
            if (produktName.Contains("Pommes")) return 2.99m;
            if (produktName.Contains("Nuggets")) return 4.49m;
            if (produktName.Contains("Soße")) return 0.50m;

            // Getränke & Desserts
            if (produktName.Contains("Getränk") || produktName.Contains("Cola") || produktName.Contains("Fanta")) return 2.79m;
            if (produktName.Contains("Milkshake")) return 2.99m;
            if (produktName.Contains("Eis") || produktName.Contains("McFlurry")) return 3.49m;
            if (produktName.Contains("McCafé")) return 2.99m;
            if (produktName.Contains("Apfeltasche")) return 1.49m;

            return 0.00m; // Fallback, falls wir was vergessen haben
        }

        // 2. Diese Funktion rechnet alles zusammen
        private void UpdateTotal()
        {
            decimal gesamtSumme = 0;

            foreach (var zeile in BestellListe)
            {
                // Wir suchen den Preis im Text (wir formatieren ihn immer am Ende mit "€")
                // Format ist z.B.: "2x Big Mac   (10,98 €)"
                // Wir suchen das letzte Stück Text
                try
                {
                    if (zeile.Contains("€"))
                    {
                        // Text: "2x Big Mac   10,98 €" -> Wir splitten bei ' ' und nehmen das vorletzte Element (die Zahl)
                        var teile = zeile.Split(' ');
                        // Wir suchen rückwärts nach etwas, das wie eine Zahl aussieht
                        for (int i = teile.Length - 1; i >= 0; i--)
                        {
                            if (teile[i].Contains("€")) continue; // Das Euro-Zeichen überspringen
                            if (decimal.TryParse(teile[i], out decimal preis))
                            {
                                gesamtSumme += preis;
                                break;
                            }
                        }
                    }
                }
                catch { /* Ignorieren bei Fehlern */ }
            }

            // In die Textbox schreiben
            var txtTotal = this.FindControl<TextBlock>("TxtTotal");
            if (txtTotal != null)
            {
                txtTotal.Text = $"{gesamtSumme:F2} €";
            }
        }

        // =========================================================
        // NUMMERN-LOGIK
        // =========================================================
        private void OnNummerClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            int zahl = int.Parse(btn.Content.ToString());

            if (zahl == 0) // Löschen
            {
                var listBox = this.FindControl<ListBox>("OrderListBox");
                var ausgewähltesItem = listBox.SelectedItem as string;

                if (ausgewähltesItem != null)
                {
                    BestellListe.Remove(ausgewähltesItem);
                    listBox.SelectedItem = null;
                    UpdateTotal(); // NEU: Preis neu berechnen nach dem Löschen
                }
                _aktuelleMenge = 1;
            }
            else
            {
                _aktuelleMenge = zahl;
            }
        }

        // =========================================================
        // NEU: HINZUFÜGEN MIT PREIS
        // =========================================================
        private void FuegeHinzu(string produktName)
        {
            // Wir müssen erst den "reinen" Namen finden, um den Preis zu bestimmen
            // Falls der String schon Zutaten enthält (z.B. aus Zutaten-Fenster), ignorieren wir die für den Preis erstmal
            string reinerName = produktName.Split('(')[0].Trim(); 
            if (reinerName.Contains("\n")) reinerName = reinerName.Split('\n')[0].Trim();

            decimal einzelPreis = GetEinzelPreis(reinerName);
            decimal gesamtPreisPosition = einzelPreis * _aktuelleMenge;

            // Formatierung: "2x Big Mac            10,98 €"
            // Wir bauen den String neu
            string neuerEintrag = $"{_aktuelleMenge}x {produktName}   {gesamtPreisPosition:F2} €";

            // Prüfen, ob wir das schon haben (nur wenn exakt gleicher Text ohne Preis)
            // Da wir jetzt Preise im Text haben, ist das Zusammenfassen komplizierter.
            // Um es einfach zu halten: Wir fügen es einfach als neue Zeile hinzu, wenn es komplex wird.
            // Oder wir machen es simpel:
            BestellListe.Add(neuerEintrag);
            
            _aktuelleMenge = 1;
            UpdateTotal(); // NEU: Summe aktualisieren
        }


        // =========================================================
        // PRODUKTE KLICKEN
        // =========================================================
        public async void OnProduktClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (button.Content == null) return;

            string produktName = button.Content.ToString();
            
            // ... (Hier bleibt deine Logik für Menüs/Popups gleich, wir rufen am Ende nur FuegeHinzu auf) ...

            if (produktName.Contains("Menu"))
            {
                string burgerName = produktName.Replace(" Menu", "");
                var dialog = new MenuConfigWindow(burgerName);
                await dialog.ShowDialog(this);
                if (dialog.ResultString != null) FuegeHinzu(dialog.ResultString + " Menü"); // Wort Menü anhängen für Preis
            }
            else if (produktName.Contains("Happy Meal"))
            {
                string hauptspeise = produktName.Replace("Happy Meal ", "");
                var beilagen = new List<string> { "Apfeltüte", "Fruchtquatsch", "McFreezy Eis" };
                string beilage = await FrageStellen("Wähle ein Nachtisch", beilagen);
                if (beilage == null) return; 
                var getraenke = new List<string> { "Capri-Sun", "Wasser", "O-Saft", "Bio-Apfelschorle" };
                string getraenk = await FrageStellen("Wähle ein Getränk:", getraenke);
                if (getraenk == null) return; 

                FuegeHinzu($"Happy Meal {hauptspeise} \n ({beilage}, {getraenk})");
            }
            else if (produktName.Contains("Pommes"))
            {
                var optionen = new List<string> { "Klein", "Mittel", "Groß" };
                string wahl = await FrageStellen("Welche Größe?", optionen);
                if (wahl != null) FuegeHinzu($"Pommes ({wahl})");
            }
            else if (produktName.Contains("Nuggets"))
            {
                string menge = await FrageStellen("Wie viele?", new List<string> { "6er", "9er", "20er" });
                if (menge == null) return;
                string sosse = await FrageStellen("Welche Soße?", new List<string> { "Mayonaisse", "Ketchup", "Süßsauer", "Curry", "BBQ", "Senf" });
                if (sosse != null) FuegeHinzu($"Nuggets {menge} \n ({sosse})");
            }
            else if (produktName.Contains("Eis") || produktName.Contains("McFlurry"))
            {
                var toppings = new List<string> { "Schokolade Soße", "Karamell Soße", "Ohne Soße" };
                string wahl = await FrageStellen("Topping:", toppings);
                if (wahl != null) FuegeHinzu($"Eis \n ({wahl})");
            }
            else if (produktName.Contains("Milkshake"))
            {
                string geschmack = await FrageStellen("Geschmack?", new List<string> { "Vanille", "Schokolade", "Erdbeere" });
                if (geschmack == null) return;
                string groesse = await FrageStellen("Größe?", new List<string> { "Klein", "Mittel", "Groß" });
                if (groesse != null) FuegeHinzu($"Milkshake {geschmack} \n ({groesse})");
            }
            else if (produktName.Contains("Getränk") || produktName == "Cola" || produktName == "Fanta")
            {
                string sorte = produktName == "Getränk" ? await FrageStellen("Sorte:", new List<string> { "Cola", "Fanta", "Sprite", "Ice Tea" }) : produktName;
                if (sorte == null) return;
                string groesse = await FrageStellen("Größe?", new List<string> { "0,25l", "0,4l", "0,5l" });
                if (groesse != null) FuegeHinzu($"{sorte} \n ({groesse})");
            }
            else if (produktName.Contains("Soße"))
            {
                string wahl = await FrageStellen("Welche Soße?", new List<string> { "Mayonnaise", "Ketchup", "Süßsauer", "BBQ", "Senf" });
                if (wahl != null) FuegeHinzu($"Soße {wahl}");
            }
            else if (produktName == "McCafé")
            {
                var kaffees = new List<string> { "Kaffee Crema", "Latte Macchiato", "Cappuccino", "Espresso", "Heiße Schokolade" };
                string wahl = await FrageStellen("Willkommen im McCafé:", kaffees);
                if (wahl != null) FuegeHinzu($"McCafé {wahl}");
            }
            else
            {
                // Standard Burger etc.
                FuegeHinzu(produktName);
            }
        }

        // =========================================================
        // ZUTATEN ÄNDERN (ANGEPASST AN PREISANZEIGE)
        // =========================================================
        private async void OnZutatenClick(object sender, RoutedEventArgs e)
        {
            var listBox = this.FindControl<ListBox>("OrderListBox");
            if (listBox.SelectedItem == null) return;

            string originalZeile = listBox.SelectedItem.ToString();
            
            // Text säubern um Produkt zu finden: "1x Big Mac   5,49 €" -> "Big Mac"
            // Alles zwischen "x " und "   " (oder Preis)
            string reinerName = originalZeile;
            
            // Menge abschneiden
            if (reinerName.Contains("x ")) reinerName = reinerName.Substring(reinerName.IndexOf("x ") + 2);
            
            // Preis am Ende abschneiden (wir suchen nach einer Zahl mit Komma)
            // Einfacher Hack: Wir nehmen an, der Name hört auf, wenn 3 Leerzeichen kommen oder eine Klammer
            if (reinerName.Contains("   ")) reinerName = reinerName.Split(new[]{"   "}, System.StringSplitOptions.None)[0];
            if (reinerName.Contains("\n")) reinerName = reinerName.Split('\n')[0].Trim();
            if (reinerName.Contains("(")) reinerName = reinerName.Split('(')[0].Trim();

            // Zutaten Liste (gleich wie vorher)
            List<string> zutaten = new List<string>();
            if (reinerName.Contains("Big Mac")) zutaten = new List<string> { "Fleisch", "Salat", "Käse", "Zwiebeln", "Gurke", "Big Mac Soße" };
            else if (reinerName.Contains("Royal TS")) zutaten = new List<string> { "Fleisch", "Salat", "Käse", "Tomate", "Zwiebeln", "Soße" };
            else if (reinerName.Contains("Hamburger")) zutaten = new List<string> { "Fleisch", "Senf", "Ketchup", "Zwiebeln", "Gurke" };
            else if (reinerName.Contains("Cheese-burger")) zutaten = new List<string> { "Fleisch", "Käse", "Senf", "Ketchup", "Zwiebeln", "Gurke" };
            else if (reinerName.Contains("McChicken")) zutaten = new List<string> { "Chicken Patty", "Salat", "Soße" };
            else if (reinerName.Contains("McRib")) zutaten = new List<string> { "Pork Patty", "Zwiebeln", "Gurke", "BBQ Soße" };
            else if (reinerName.Contains("Pommes")) zutaten = new List<string> { "Salz" };
            else return; 

            var fenster = new IngredientsWindow(reinerName, zutaten);
            await fenster.ShowDialog(this);

            if (fenster.ResultString != null)
            {
                int index = BestellListe.IndexOf(originalZeile);
                
                // Wir müssen den Preis aus dem alten String retten!
                // Wir nehmen einfach alles ab den 3 Leerzeichen "   "
                string preisTeil = "";
                if (originalZeile.Contains("   "))
                {
                    preisTeil = originalZeile.Substring(originalZeile.LastIndexOf("   "));
                }

                // Basis Name wiederherstellen (mit Menge)
                string basis = originalZeile.Split(new[]{"   "}, System.StringSplitOptions.None)[0];
                if (basis.Contains("\n")) basis = basis.Split('\n')[0].Trim();

                if (fenster.ResultString != "")
                {
                    // Format: "1x Big Mac (Ohne Gurke)   5,49 €"
                    BestellListe[index] = $"{basis} \n ({fenster.ResultString}){preisTeil}";
                }
                else
                {
                    // Zurück zum Original ohne Änderungen
                    BestellListe[index] = $"{basis}{preisTeil}";
                }
                // Summe muss hier nicht neu berechnet werden, da sich der Preis nicht ändert
            }
        }

        private async System.Threading.Tasks.Task<string> FrageStellen(string titel, List<string> antworten)
        {
            var fenster = new OptionWindow(titel, antworten);
            await fenster.ShowDialog(this);
            return fenster.SelectedOption;
        }

        private void OnSchliessenClick(object sender, RoutedEventArgs e) { this.Close(); }
        
        private void OnPreiseClick(object sender, RoutedEventArgs e)
        {
            var priceWindow = new PriceWindow();
            priceWindow.Show(); 
        }

        private void OnAbschliessenClick(object sender, RoutedEventArgs e)
        {
            var checkout = new CheckoutWindow();
            checkout.Show(); 
        }
    }
}