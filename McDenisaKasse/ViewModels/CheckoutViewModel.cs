using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;

namespace McDenisaKasse.ViewModels
{
    // Diese Klasse ist für Logik von Kasse. 
    // Erbt von ObservableObject damit UI update bekommt automatisch.
    public partial class CheckoutViewModel : ObservableObject
    {
        // =========================================================
        // DATEN (Properties)
        // =========================================================
        
        // Mit [ObservableProperty] ich muss nicht viel Code schreiben.
        // Computer macht automatisch getter, setter und PropertyChanged Event.
        
        [ObservableProperty]
        private string _vorname;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _adresse;

        // Default Wert ist leerer String damit nicht null error kommt
        [ObservableProperty]
        private string _kommentar = "";

        // =========================================================
        // COMMANDS (Aktionen für Buttons)
        // =========================================================

        // Das ersetzt "Click" Event. RelayCommand ist profi Weg für MVVM.
        [RelayCommand]
        private void BarZahlen()
        {
            // Hier ich simuliere speichern.
            // Später vielleicht Datenbank Verbindung machen.
            Console.WriteLine("Zahlung BAR gestartet...");
            Console.WriteLine($"Kunde: {Vorname} {Name}");
            
            if (string.IsNullOrEmpty(Kommentar))
            {
                Console.WriteLine("Kein Kommentar da.");
            }
            else
            {
                Console.WriteLine($"Kunde sagt: {Kommentar}");
            }
            
            Console.WriteLine("Fertig mit Barzahlung.");
        }

        [RelayCommand]
        private void KarteZahlen()
        {
            Console.WriteLine("Zahlung KARTE gestartet...");
            // Ich prüfe ob Adresse da ist für Lieferung
            if (string.IsNullOrEmpty(Adresse))
            {
                Console.WriteLine("ACHTUNG: Keine Adresse gegeben!");
            }
            else
            {
                Console.WriteLine($"Lieferung geht an: {Adresse}");
            }
            
            Console.WriteLine("Karte akzeptiert. Geld weg.");
        }
    }
}