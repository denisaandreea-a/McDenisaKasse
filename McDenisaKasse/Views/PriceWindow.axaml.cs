using Avalonia.Controls;
using Avalonia.Interactivity;

namespace McDenisaKasse.Views
{
    public partial class PriceWindow : Window
    {
        public PriceWindow()
        {
            InitializeComponent();
        }

        private void OnCloseClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}