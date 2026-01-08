using Avalonia.Controls;
using Avalonia.Interactivity;
using McDenisaKasse.ViewModels;
using McDenisaKasse.Models; // Zugriff auf meine Listen
using System.Collections.Generic;

namespace McDenisaKasse.Views
{
    public partial class MenuWindow : Window
    {
        private readonly MenuViewModel _viewModel;

        public MenuWindow()
        {
            InitializeComponent();
            _viewModel = new MenuViewModel();
            DataContext = _viewModel;
        }

        // =========================================================
        // PRODUKTE KLICKEN
        // =========================================================
        public async void OnProduktClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            if (button.Content == null) return; 

            string produktName = button.Content.ToString();

            // 1. MENÜS
            if (produktName.Contains("Menu"))
            {
                string burgerName = produktName.Replace(" Menu", "");
                var dialog = new MenuConfigWindow(burgerName);
                await dialog.ShowDialog(this);
                
                if (dialog.ResultString != null) 
                {
                    _viewModel.FuegeProduktHinzu(dialog.ResultString + " Menü");
                }
            }
            // 2. HAPPY MEAL
            else if (produktName.Contains("Happy Meal"))
            {
                string hauptspeise = produktName.Replace("Happy Meal ", "");
                
                string beilage = await FrageStellen("Wähle ein Nachtisch", ProductService.GetHappyMealNachtisch());
                if (beilage == null) return; 

                string getraenk = await FrageStellen("Wähle ein Getränk:", ProductService.GetHappyMealGetraenke());
                if (getraenk == null) return; 

                _viewModel.FuegeProduktHinzu($"Happy Meal {hauptspeise}", $"({beilage}, {getraenk})");
            }
            // 3. POMMES (Klein, Mittel, Groß)
            else if (produktName.Contains("Pommes"))
            {
                string wahl = await FrageStellen("Welche Größe?", ProductService.GetStandardGroessen());
                
                if (wahl != null) 
                    _viewModel.FuegeProduktHinzu("Pommes", $"({wahl})");
            }
            // 4. NUGGETS
            else if (produktName.Contains("Nuggets"))
            {
                string menge = await FrageStellen("Wie viele?", ProductService.GetNuggetAnzahl());
                if (menge == null) return;
                
                // Hier nehme ich die Liste mit Curry & Mayonaisse
                string sosse = await FrageStellen("Welche Soße?", ProductService.GetNuggetSossen());
                
                if (sosse != null) 
                    _viewModel.FuegeProduktHinzu($"Nuggets {menge}", $"({sosse})");
            }
            // 5. EIS
            else if (produktName.Contains("Eis") || produktName.Contains("Eis")) 
            {
                // Deine Toppings: Schoko Soße, Karamell Soße...
                string wahl = await FrageStellen("Topping:", ProductService.GetEisToppings());
                
                if (wahl != null) 
                    _viewModel.FuegeProduktHinzu("Eis", $"({wahl})");
            }
            // 6. MILKSHAKE
            else if (produktName.Contains("Milkshake"))
            {
                string geschmack = await FrageStellen("Geschmack?", ProductService.GetMilkshakeGeschmack());
                if (geschmack == null) return;

                // Klein, Mittel, Groß
                string groesse = await FrageStellen("Größe?", ProductService.GetStandardGroessen());
                
                if (groesse != null) 
                    _viewModel.FuegeProduktHinzu($"Milkshake {geschmack}", $"({groesse})");
            }
            // 7. GETRÄNKE
            else if (produktName.Contains("Getränk") || produktName == "Cola" || produktName == "Fanta")
            {
                string sorte = produktName;
                if (produktName == "Getränk")
                {
                    // Cola, Fanta, Sprite, Ice Tea
                    sorte = await FrageStellen("Sorte:", ProductService.GetGetraenkeSorten());
                    if (sorte == null) return;
                }

                // 0,25l, 0,4l, 0,5l
                string groesse = await FrageStellen("Größe?", ProductService.GetGetraenkeGroessen());
                
                if (groesse != null) 
                    _viewModel.FuegeProduktHinzu(sorte, $"({groesse})");
            }
            // 8. EXTRA SOßE (ohne Curry)
            else if (produktName.Contains("Soße"))
            {
                string wahl = await FrageStellen("Welche Soße?", ProductService.GetExtraSossen());
                if (wahl != null) _viewModel.FuegeProduktHinzu($"Soße {wahl}");
            }
            // 9. McCAFÉ
            else if (produktName == "McCafé")
            {
                string wahl = await FrageStellen("Willkommen im McCafé:", ProductService.GetMcCafeSorten());
                if (wahl != null) _viewModel.FuegeProduktHinzu($"McCafé {wahl}");
            }
            // 10. SONSTIGES
            else
            {
                _viewModel.FuegeProduktHinzu(produktName);
            }
        }

        // =========================================================
        // ZUTATEN ÄNDERN
        // =========================================================
        private async void OnZutatenClick(object sender, RoutedEventArgs e)
        {
            var selectedItem = _viewModel.SelektiertePosition;
            if (selectedItem == null) return; 

            List<string> zutaten = ProductService.GetZutatenFuerProdukt(selectedItem.Name);

            if (zutaten.Count == 0) return;

            var fenster = new IngredientsWindow(selectedItem.Name, zutaten);
            await fenster.ShowDialog(this);

            if (fenster.ResultString != null)
            {
                string neuerZusatz = fenster.ResultString == "" ? "" : $"({fenster.ResultString})";
                _viewModel.AktualisierePosition(selectedItem, neuerZusatz);
            }
        }

        // Hilfsmethode
        private async System.Threading.Tasks.Task<string> FrageStellen(string titel, List<string> antworten)
        {
            var fenster = new OptionWindow(titel, antworten);
            await fenster.ShowDialog(this);
            return fenster.SelectedOption; 
        }

        private void OnSchliessenClick(object sender, RoutedEventArgs e) { this.Close(); } 
        private void OnPreiseClick(object sender, RoutedEventArgs e) { new PriceWindow().Show(); }
        private void OnAbschliessenClick(object sender, RoutedEventArgs e) { new CheckoutWindow().Show(); }
    }
}