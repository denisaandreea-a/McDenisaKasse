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
        
        private void OnBestellenClick(object sender, RoutedEventArgs e)
        {
            var menuWindow = new MenuWindow(); 
            menuWindow.Show();                 
            this.Close();               
        }

        private void OnAbbrechenClick(object sender, RoutedEventArgs e)
        {
            this.Close(); 
        }
    }
}