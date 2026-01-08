using Avalonia.Controls;
using Avalonia.Interactivity;

namespace McDenisaKasse.Views
{
    // Code-behind für Preis-Fenster.
    // "partial" heißt Klasse ist geteilt (hier C#, Rest macht Compiler aus XAML).
    public partial class PriceWindow : Window
    {
        public PriceWindow()
        {
            // Wichtig: Ladet UI aus XAML. 
            // Ohne das ist Fenster leer.
            InitializeComponent();
        }

        // Event wenn User klickt Schließen.
        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            // Fenster zu machen.
            this.Close();
        }
    }
}