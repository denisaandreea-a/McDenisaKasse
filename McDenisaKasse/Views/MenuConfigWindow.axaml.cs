using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace McDenisaKasse.Views
{
    public partial class MenuConfigWindow : Window
    {
        // Hier speichern wir das fertige Ergebnis für das Hauptfenster
        public string ResultString { get; private set; } = null;
        
        // Hier merken wir uns intern den Namen (z.B. "Big Mac")
        private string _burgerName;

        public MenuConfigWindow()
        {
            InitializeComponent();
        }

        
        public MenuConfigWindow(string burgerName) : this()
        {
            _burgerName = burgerName;

            // Wir suchen den Titel im Design und ändern ihn
            var titelBlock = this.FindControl<TextBlock>("TitelText"); 
            if (titelBlock != null)
            {
                titelBlock.Text = burgerName + " Menü anpassen";
            }
        }

        private void OnDrinkClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            string getraenk = btn.Content.ToString();

            var sossen = new List<string>();
            if (this.FindControl<CheckBox>("ChkMayo").IsChecked == true) sossen.Add("Mayo");
            if (this.FindControl<CheckBox>("ChkKetchup").IsChecked == true) sossen.Add("Ketchup");
            if (this.FindControl<CheckBox>("ChkSuessSauer").IsChecked == true) sossen.Add("Süßsauer");
            if (this.FindControl<CheckBox>("ChkCurry").IsChecked == true) sossen.Add("Curry");
            if (this.FindControl<CheckBox>("ChkBBQ").IsChecked == true) sossen.Add("BBQ");
            if (this.FindControl<CheckBox>("ChkSenf").IsChecked == true) sossen.Add("Senf");

            string sossenText = sossen.Count > 0 ? string.Join(", ", sossen) : "ohne Soße";

            // Hier bauen wir den Text zusammen
            ResultString = $"{_burgerName} Menü ({getraenk}, {sossenText})";
            
            this.Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}