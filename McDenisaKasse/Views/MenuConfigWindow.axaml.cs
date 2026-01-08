using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

namespace McDenisaKasse.Views
{
    public partial class MenuConfigWindow : Window
    {
        // Hier speichern wir das Ergebnis. 
        // Public weil Hauptfenster muss das lesen später.
        public string ResultString { get; private set; } = null;
        
        // Private Variable. Ich muss mir den Name merken für später.
        private string _burgerName;

        // Das ist Standard Konstruktor, Avalonia braucht das.
        public MenuConfigWindow()
        {
            InitializeComponent();
        }

        // Hier ich bekomme Name von Burger beim Start (z.B. "Big Mac").
        // "this()" ruft den Konstruktor oben auf für InitializeComponent.
        public MenuConfigWindow(string burgerName) : this()
        {
            _burgerName = burgerName; // Ich speichere in Variable

            // Ich suche den TextBlock in XAML Design um Titel zu ändern.
            // (Achtung: Im XAML muss x:Name="TitelText" sein!)
            var titelBlock = this.FindControl<TextBlock>("TitelText"); 
            if (titelBlock != null)
            {
                // Ich schreibe Name in Titel, sieht besser aus für User.
                titelBlock.Text = burgerName + " Menü anpassen";
            }
        }

        // Wenn User klickt auf ein Getränk Button (Cola, Fanta...)
        // Alle Buttons nutzen diese gleiche Funktion.
        private void OnDrinkClick(object sender, RoutedEventArgs e)
        {
            // Ich muss wissen welcher Button wurde geklickt.
            // Sender ist der Button.
            var btn = (Button)sender;
            string getraenk = btn.Content.ToString(); // Ich hole Text von Button

            // Ich mache leere Liste für Soßen sammeln
            var sossen = new List<string>();
            
            // Jetzt ich prüfe jede Checkbox einzeln.
            // Wenn IsChecked ist true, dann Kunde will diese Soße.
            if (this.FindControl<CheckBox>("ChkMayo").IsChecked == true) sossen.Add("Mayo");
            if (this.FindControl<CheckBox>("ChkKetchup").IsChecked == true) sossen.Add("Ketchup");
            if (this.FindControl<CheckBox>("ChkSuessSauer").IsChecked == true) sossen.Add("Süßsauer");
            if (this.FindControl<CheckBox>("ChkCurry").IsChecked == true) sossen.Add("Curry");
            if (this.FindControl<CheckBox>("ChkBBQ").IsChecked == true) sossen.Add("BBQ");
            if (this.FindControl<CheckBox>("ChkSenf").IsChecked == true) sossen.Add("Senf");

            // Hier ich baue Text für Soßen.
            // Wenn Liste hat Elemente, ich mache String mit Komma (z.B. "Mayo, Ketchup").
            // Wenn Liste leer, dann Text ist "ohne Soße".
            string sossenText = sossen.Count > 0 ? string.Join(", ", sossen) : "ohne Soße";

            // Alles zusammen bauen in ResultString.
            // Format ist dann: "Big Mac Menü (Cola, Mayo)"
            ResultString = $"{_burgerName} Menü ({getraenk}, {sossenText})";
            
            this.Close(); // Fenster fertig, mache zu.
        }

        // Wenn User drückt Abbrechen
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close(); // Einfach schließen, ResultString bleibt null.
        }
    }
}