using Avalonia.Controls;
using Avalonia.Interactivity;

namespace McDenisaKasse.Views
{
    public partial class CheckoutWindow : Window
    {
        public CheckoutWindow()
        {
            InitializeComponent();
        }

        private void OnBarClick(object sender, RoutedEventArgs e)
        {
            // Hier würde man die Bestellung speichern
            System.Console.WriteLine("Kunde zahlt Bar.");
            System.Console.WriteLine("Name: " + this.FindControl<TextBox>("TxtName").Text);
            
            this.Close(); // Fenster schließen
        }

        private void OnKarteClick(object sender, RoutedEventArgs e)
        {
            System.Console.WriteLine("Kunde zahlt mit Karte.");
            this.Close(); // Fenster schließen
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close(); // Nur Fenster schließen ohne Speichern
        }
    }
}