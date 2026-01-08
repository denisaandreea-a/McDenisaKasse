using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using Avalonia.Media; // WICHTIG: Das hier oben hinzufügen für die Farben!

namespace McDenisaKasse.Views
{
    public partial class OptionWindow : Window
    {
        // Hier ich speichere was User hat ausgewählt.
        // Wenn null, dann er hat nichts gewählt (oder abgebrochen).
        public string SelectedOption { get; private set; } = null;

        // Standard Konstruktor muss sein.
        public OptionWindow() { InitializeComponent(); }

        // Spezieller Konstruktor mit Frage (Titel) und Liste von Möglichkeiten.
        public OptionWindow(string frage, List<string> optionen)
        {
            InitializeComponent();
            
            // Ich schreibe die Frage in den TextBlock oben.
            this.FindControl<TextBlock>("TitleText").Text = frage;
            
            // Ich suche den StackPanel wo Buttons rein sollen.
            var container = this.FindControl<StackPanel>("ButtonContainer");

            // Jetzt ich mache Loop durch die Liste.
            // Für jede Option ich erstelle ein Button im Code (dynamisch).
            foreach (var opt in optionen)
            {
                var btn = new Button
                {
                    Content = opt, // Text auf Button
                    HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch, // Breit machen
                    Height = 50,
                    FontSize = 18,
                    HorizontalContentAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                    VerticalContentAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    
                    // Hier ich benutze Brushes für Farbe (darum Avalonia.Media oben wichtig!)
                    Background = Brushes.White,
                    Foreground = Brushes.Black,
                    BorderBrush = Brushes.Black,
                    BorderThickness = new Avalonia.Thickness(1),
                    Margin = new Avalonia.Thickness(0, 0, 0, 10) // Bisschen Platz unten lassen
                };
                
                // Ich mache Event Handler direkt hier mit Lambda (=>).
                // Wenn User klickt Button:
                btn.Click += (s, e) => {
                    SelectedOption = opt; // 1. merke mir was gewählt wurde
                    this.Close();         // 2.schließe Fenster
                };

                // Button in den Container hinzufügen, sonst man sieht nichts
                container.Children.Add(btn);
            }
        }
        
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            this.Close(); // Einfach schließen, SelectedOption bleibt null.
        }
    }
}