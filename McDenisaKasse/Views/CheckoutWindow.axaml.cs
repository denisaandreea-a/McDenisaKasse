using Avalonia.Controls;
using Avalonia.Interactivity;

namespace McDenisaKasse.Views
{
    // Code-Behind für das Checkout-Fenster
    // Logik, wenn man auf die Buttons klickt.
    public partial class CheckoutWindow : Window
    {
        // Der Konstruktor: Wird aufgerufen, wenn "new CheckoutWindow()" gemacht wird.
        public CheckoutWindow()
        {
            InitializeComponent(); // Lädt das XAML (das Design)
        }
        
        // 1. BARZAHLUNG
        // Wird ausgelöst, wenn der grüne "Bar zahlen" Button geklickt wird.
        private void OnBarClick(object sender, RoutedEventArgs e)
        {
            // Simulation der Speicherung:ich gebe die Daten erstmal nur in der Konsole aus.
            // später würde hier der Code stehen, der das in eine Datenbank schreibt.
            System.Console.WriteLine("Kunde zahlt Bar.");

            // DATEN AUSLESEN:
            // Um an den Text aus der TextBox im XAML zu kommen, nutze ich FindControl.
            // "TxtName" ist der Name, den ich im XAML mit x:Name="TxtName" vergeben habe.
            var nameBox = this.FindControl<TextBox>("TxtName");
            
            // prüfen, ob die Box gefunden wurde (null-check), 
            // obwohl sie eigentlich da sein muss.
            if (nameBox != null)
            {
                System.Console.WriteLine("Besteller: " + nameBox.Text);
            }
            
            // Fenster schließen, da der Vorgang abgeschlossen ist.
            this.Close(); 
        }

        // 2. KARTENZAHLUNG
        // Wird ausgelöst, wenn der blaue "Mit Karte zahlen" Button geklickt wird.
        private void OnKarteClick(object sender, RoutedEventArgs e)
        {
            // Hier könnte man später ein Kartenterminal oder so einfügen, programmieren.
            System.Console.WriteLine("Kunde zahlt mit Karte.");
            
            // Auch hier lesen wir optional die Adresse aus, falls nötig.
            var adresseBox = this.FindControl<TextBox>("TxtAdresse");
            if (adresseBox != null)
            {
                 System.Console.WriteLine("Lieferadresse: " + adresseBox.Text);
            }

            // Vorgang beendet -> Fenster zu.
            this.Close(); 
        }

        // 3. ABBRECHEN
        // Wird ausgelöst, wenn der graue "Zurück" Button geklickt wird.
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            // Hier wird nichts gespeichert oder ausgegeben.
            // Wir schließen einfach nur das Fenster und kehren zum Hauptmenü zurück.
            this.Close(); 
        }
    }
}