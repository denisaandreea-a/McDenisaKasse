using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using System.Collections.Generic;

namespace McDenisaKasse.Views
{
    public partial class IngredientsWindow : Window
    {
        // Hier ich speichere das Ergebnis. 
        // Wenn User klickt Speichern, hier steht was er nicht will (z.B. "Ohne Gurke").
        // Wenn null, dann er hat Abbrechen gedrückt.
        public string ResultString { get; private set; } = null;
        
        // Liste um mir alle Checkboxen zu merken für später.
        private List<CheckBox> _checkBoxes = new List<CheckBox>();

        // Das ist Konstruktor. Wir bekommen Produktname und Liste von Zutaten.
        public IngredientsWindow(string produktName, List<string> zutaten)
        {
            InitializeComponent();
            
            // Ich suche den TextBlock oben und schreibe Name rein (z.B. "Big Mac anpassen").
            this.FindControl<TextBlock>("TitleText").Text = $"{produktName} anpassen";

            // Ich hole den StackPanel aus XAML. Da kommen gleich die Boxen rein.
            var container = this.FindControl<StackPanel>("CheckboxContainer");

            // Jetzt ich mache Loop für jede Zutat in der Liste
            foreach (var zutat in zutaten)
            {
                // Ich erstelle neue Checkbox mit Code (nicht in Designer)
                var cb = new CheckBox();
                cb.Content = zutat;      // Name von Zutat (z.B. "Käse")
                cb.IsChecked = true;     // Am Anfang ist Haken drin (Standard ist immer mit alles)
                cb.FontSize = 18;        // Schrift größer machen
                cb.Margin = new Avalonia.Thickness(10, 0); // Bischen Abstand lassen
                
                // Ich tue Checkbox in meine private Liste _checkBoxes (zum merken)
                _checkBoxes.Add(cb);
                
                // Und ich tue Checkbox in das Fenster (in StackPanel), damit man sieht.
                container.Children.Add(cb);
            }
        }

        // Das braucht Avalonia System, sonst Error
        public IngredientsWindow() { InitializeComponent(); }

        // Wenn User drückt auf "Speichern" (Grün Button)
        private void OnSaveClick(object sender, RoutedEventArgs e)
        {
            // Liste für Sachen die Kunde NICHT will (wo Haken weg ist)
            var abgewaehlt = new List<string>();

            // Ich laufe durch alle Checkboxen die ich erstellt habe
            foreach (var cb in _checkBoxes)
            {
                // Wenn IsChecked ist false, dann User hat Haken weggemacht
                if (cb.IsChecked == false)
                {
                    // Dann ich merke mir "Ohne" + Name (z.B. "Ohne Zwiebeln")
                    abgewaehlt.Add($"Ohne {cb.Content}");
                }
            }

            // Wenn Liste hat Elemente (Count > 0), dann es gibt Änderungen
            if (abgewaehlt.Count > 0)
            {
                // Ich mache alles in einen String mit Komma getrennt.
                // Ergebnis sieht so aus: "Ohne Zwiebeln, Ohne Gurke"
                ResultString = string.Join(", ", abgewaehlt);
            }
            else
            {
                ResultString = ""; // Leer String bedeutet: Alles normal, keine Extra Wünsche
            }

            this.Close(); // Fenster zu machen fertig
        }

        // Wenn User drückt "Abbrechen" (Rot Button)
        private void OnCancelClick(object sender, RoutedEventArgs e)
        {
            ResultString = null; // Null heißt: Vorgang abgebrochen, nichts ändern
            this.Close(); // Einfach zu machen
        }
    }
}