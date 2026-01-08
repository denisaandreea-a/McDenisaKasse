using Avalonia.Controls;
using Avalonia.Interactivity;
using McDenisaKasse.ViewModels;
using McDenisaKasse.Models; // Hier ist meine neue Service-Klasse
using System.Collections.Generic;

namespace McDenisaKasse.Views
{
    public partial class MenuWindow : Window
    {
        // Ich speichere das ViewModel hier in einer Variable, damit ich es im Code benutzen kann.
        // Readonly ist gut für Sicherheit, damit man es nicht aus Versehen überschreibt.
        private readonly MenuViewModel _viewModel;

        public MenuWindow()
        {
            InitializeComponent();
            
            // Hier erstelle ich das ViewModel und verbinde es mit der View (DataContext).
            // Das ist super wichtig, damit die Bindings im XAML überhaupt funktionieren.
            _viewModel = new MenuViewModel();
            DataContext = _viewModel;
        }
        
        // PRODUKTE ANKLICKEN
        // Die Methode muss async sein, weil ich mit "await" auf die Popups warten muss.
        public async void OnProduktClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            
            //Falls Button keinen Inhalt hat, breche ich ab, sonst Crash.
            if (button.Content == null) return; 

            string produktName = button.Content.ToString();
            
            // entscheide ich, was passiert, je nach Produkt-Name.
            // Ich muss das hier im Code-Behind machen, weil Dialog-Fenster öffnen im ViewModel verboten ist (MVVM Regel).
            
            if (produktName.Contains("Menu"))
            {
                // Ich putze den Namen (lösche das Wort "Menu" raus)
                string burgerName = produktName.Replace(" Menu", "");
                
                // Konfigurations-Fenster anzeigen
                var dialog = new MenuConfigWindow(burgerName);
                await dialog.ShowDialog(this); // Code wartet hier, bis User fertig ist
                
                // Wenn User gespeichert hat (ResultString ist nicht null), ab ins ViewModel damit.
                if (dialog.ResultString != null) 
                {
                    _viewModel.FuegeProduktHinzu(dialog.ResultString + " Menü");
                }
            }
            else if (produktName.Contains("Happy Meal"))
            {
                string hauptspeise = produktName.Replace("Happy Meal ", "");
                
                // Ich benutze meine Hilfsmethode "FrageStellen", um User zu fragen.
                // Das spart Code-Zeilen.
                var beilagen = new List<string> { "Apfeltüte", "Fruchtquatsch", "McFreezy Eis" };
                string beilage = await FrageStellen("Wähle ein Nachtisch", beilagen);
                
                // Wenn User abbricht (null), mache ich return.
                if (beilage == null) return; 

                var getraenke = new List<string> { "Capri-Sun", "Wasser", "O-Saft", "Bio-Apfelschorle" };
                string getraenk = await FrageStellen("Wähle ein Getränk:", getraenke);
                
                if (getraenk != null) 
                    _viewModel.FuegeProduktHinzu($"Happy Meal {hauptspeise}", $"({beilage}, {getraenk})");
            }
            else if (produktName.Contains("Pommes"))
            {
                // Einfache Abfrage für Größe
                string wahl = await FrageStellen("Welche Größe?", new List<string> { "Klein", "Mittel", "Groß" });
                if (wahl != null) 
                    _viewModel.FuegeProduktHinzu("Pommes", $"({wahl})");
            }
            // ... Hier könnte noch mehr Logik für Nuggets etc. stehen ...
            else
            {
                // Wenn es ein einfaches Produkt ohne Fragen ist, füge ich es direkt hinzu.
                _viewModel.FuegeProduktHinzu(produktName);
            }
        }
        
        // ZUTATEN ÄNDERN
        private async void OnZutatenClick(object sender, RoutedEventArgs e)
        {
            // Ich frage das ViewModel: Was ist gerade in der Liste ausgewählt?
            var selectedItem = _viewModel.SelektiertePosition;

            // Safety Check: Wenn nichts markiert ist, mache ich return. Sonst stürzt App ab.
            if (selectedItem == null) return; 

            // Ich nutze jetzt meine ausgelagerte Klasse (Service).
            // Vorher eine riesige if-else "Wurst":))
            List<string> zutaten = ProductService.GetZutatenFuerProdukt(selectedItem.Name);

            // Wenn die Liste leer ist (Count ist 0), hat das Produkt keine Zutaten (z.B. Cola).
            // Dann mache ich einfach nichts.
            if (zutaten.Count == 0) return;

            // Ich öffne das Zutaten-Fenster 
            var fenster = new IngredientsWindow(selectedItem.Name, zutaten);
            await fenster.ShowDialog(this);

            // Wenn wir ein Ergebnis haben (nicht null)
            if (fenster.ResultString != null)
            {
                // Ich baue den String zusammen mit Klammern, sieht schöner aus.
                string neuerZusatz = fenster.ResultString == "" ? "" : $"({fenster.ResultString})";
                
                // ViewModel soll das Update machen.
                _viewModel.AktualisierePosition(selectedItem, neuerZusatz);
            }
        }

        // Meine Hilfsmethode 
        // Damit muss ich nicht für jede kleine Frage 10 Zeilen Code schreiben.
        private async System.Threading.Tasks.Task<string> FrageStellen(string titel, List<string> antworten)
        {
            var fenster = new OptionWindow(titel, antworten);
            await fenster.ShowDialog(this);
            // Ich gebe zurück, was User geklickt hat (oder null bei Abbruch).
            return fenster.SelectedOption; 
        }

        private void OnSchliessenClick(object sender, RoutedEventArgs e) 
        { 
            // Fenster einfach zumachen.
            this.Close(); 
        } 
        
        private void OnPreiseClick(object sender, RoutedEventArgs e)
        {
            // Nur das Preis-Fenster zeigen, keine Logik nötig.
            new PriceWindow().Show(); 
        }

        private void OnAbschliessenClick(object sender, RoutedEventArgs e)
        {
            // Ich öffne hier CheckoutWindow.
            new CheckoutWindow().Show(); 
        }
    }
}