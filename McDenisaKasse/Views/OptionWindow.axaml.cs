using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using Avalonia.Media; // WICHTIG: Das hier oben hinzufügen für die Farben!

namespace McDenisaKasse.Views
{
    public partial class OptionWindow : Window
    {
        public string SelectedOption { get; private set; } = null;

        public OptionWindow() { InitializeComponent(); }

        public OptionWindow(string frage, List<string> optionen)
        {
            InitializeComponent();
            
            this.FindControl<TextBlock>("TitleText").Text = frage;
            var container = this.FindControl<StackPanel>("ButtonContainer");

            foreach (var opt in optionen)
            {
                var btn = new Button
                {
                    Content = opt,
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
                    Height = 50,
                    FontSize = 18,
                    HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    Background = Brushes.White,
                    Foreground = Brushes.Black,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Avalonia.Thickness(1),
                    Margin = new Avalonia.Thickness(0, 0, 0, 10) // Abstand nach unten
                };
                
                btn.Click += (s, e) => {
                    SelectedOption = opt;
                    this.Close();
                };

                container.Children.Add(btn);
            }
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}