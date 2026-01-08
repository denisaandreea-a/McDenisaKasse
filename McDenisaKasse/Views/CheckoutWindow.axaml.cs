using Avalonia.Controls;
using Avalonia.Interactivity;
using McDenisaKasse.ViewModels;

namespace McDenisaKasse.Views
{
    public partial class CheckoutWindow : Window
    {
        public CheckoutWindow()
        {
            InitializeComponent();
            
            // WICHTIG: Hier verbinde ich Fenster mit ViewModel.
            // Ohne das funktioniert Binding nicht.
            DataContext = new CheckoutViewModel();
        }

        // Dieser Button nur schließt Fenster, deswegen ok im Code-behind.
        // Hat keine Geschäftslogik.
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}