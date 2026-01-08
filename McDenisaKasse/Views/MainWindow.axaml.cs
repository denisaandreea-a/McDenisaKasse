using Avalonia.Controls;
using Avalonia.Interactivity;

namespace McDenisaKasse.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        
        // Event-Handler: Wird ausgeführt, wenn man auf "BESTELLEN" klickt
        private void OnBestellenClick(object sender, RoutedEventArgs e)
        {
            // 1 Erstelle eine neue Instanz des Menü-Fensters (Bestellübersicht)
            var menuWindow = new MenuWindow(); 
            
            // 2. Zeige das neue Fenster an
            menuWindow.Show();                 
            
            // 3.Schließe das aktuelle Start-Fenster (damit nur das neue offen ist)
            this.Close();               
        }

        // Event-Handler: Wird ausgeführt, wenn man auf "Abbrechen" klickt
        private void OnAbbrechenClick(object sender, RoutedEventArgs e)
        {
            // Schließt die gesamte Anwendung (da es das Hauptfenster ist)
            this.Close(); 
        }
    }
}