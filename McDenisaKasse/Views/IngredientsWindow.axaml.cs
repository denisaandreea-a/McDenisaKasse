using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System.Collections.Generic;

namespace McDenisaKasse.Views
{
    public partial class IngredientsWindow : Window
    {
        // Hier speichern wir, welche Zutaten abgewählt wurden (z.B. "Ohne Salat")
        public string ResultString { get; private set; } = null;
        
        private List<CheckBox> _checkBoxes = new List<CheckBox>();

        public IngredientsWindow(string produktName, List<string> zutaten)
        {
            InitializeComponent();
            this.FindControl<TextBlock>("TitleText").Text = $"{produktName} anpassen";

            var container = this.FindControl<StackPanel>("CheckboxContainer");

            // Für jede Zutat eine Checkbox erstellen
            foreach (var zutat in zutaten)
            {
                var cb = new CheckBox();
                cb.Content = zutat;
                cb.IsChecked = true; // Standardmäßig ist alles drauf
                cb.FontSize = 18;
                cb.Margin = new Avalonia.Thickness(10, 0);
                
                _checkBoxes.Add(cb);
                container.Children.Add(cb);
            }
        }

        // Wird von Avalonia benötigt
        public IngredientsWindow() { InitializeComponent(); }

        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            var abgewaehlt = new List<string>();

            foreach (var cb in _checkBoxes)
            {
                // Wenn der Haken WEG ist (IsChecked == false), merken wir uns das
                if (cb.IsChecked == false)
                {
                    abgewaehlt.Add($"Ohne {cb.Content}");
                }
            }

            if (abgewaehlt.Count > 0)
            {
                // Wir verbinden alle Änderungen mit einem Komma
                ResultString = string.Join(", ", abgewaehlt);
            }
            else
            {
                ResultString = ""; // Nichts geändert
            }

            this.Close();
        }

        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            ResultString = null; // Abbrechen signalisieren
            this.Close();
        }
    }
}