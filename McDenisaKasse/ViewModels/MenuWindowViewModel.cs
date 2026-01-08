using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Linq;
using McDenisaKasse.Models; 

namespace McDenisaKasse.ViewModels
{
    // Logik getrennt von View (Oberfläche)
    // erbe von ViewModelBase, damit ich die Funktionen für Updates habe.
    public partial class MenuViewModel : ViewModelBase
    {
        // Ich benutze ObservableCollection und nicht normale List.
        // Liste sagt der GUI automatisch Bescheid, wenn was neu kommt oder weg geht.
        public ObservableCollection<BestellPosition> Bestellungen { get; } = new();
        
        [ObservableProperty]
        private int aktuelleMenge = 1;

        // Text für Summe unten. Observable, ändert sich wenn Preis anders ist.
        [ObservableProperty]
        private string gesamtSummeText = "0,00 €";

        // Hier ich speichere was User angeklickt hat in der Liste.
        //Es sagt dem Löschen-Button automatisch Prüf mal ob du jetzt aktiv sein darfst".
        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(LoeschePositionCommand))] 
        private BestellPosition? selektiertePosition;

        //BEFEHLE (COMMANDS)
        // RelayCommand macht aus Methode ein Command für den Button
        [RelayCommand]
        public void NummerEingabe(string nummerStr)
        {
            //String in Zahl umwandeln.
            if (int.TryParse(nummerStr, out int zahl))
            {
                if (zahl == 0)
                {
                    // 0 Reset auf 1.
                    AktuelleMenge = 1; 
                }
                else
                {
                    // Property setzen -> UI bekommt Update sofort.
                    AktuelleMenge = zahl;
                }
            }
        }

        // Command für Löschen. 
        // CanExecute macht Button grau wenn man nichts ausgewählt hat.
        // besser für User Experience, damit kein Fehler passiert.
        [RelayCommand(CanExecute = nameof(KannLoeschen))]
        public void LoeschePosition()
        {
            if (SelektiertePosition != null)
            {
                // entferne Item aus Collection -> ListBox zeigt das sofort.
                Bestellungen.Remove(SelektiertePosition);
                SelektiertePosition = null; // Auswahl leer machen
                UpdateGesamtSumme(); // Preis neu rechnen
                AktuelleMenge = 1;   // Reset Menge
            }
        }

        // Hilfsfunktion, true oder false.
        private bool KannLoeschen()
        {
            // Nur true wenn wirklich was da ist (nicht null).
            return SelektiertePosition != null;
        }

        //LOGIK METHODEN 
        // ruft die View auf
        // Daten nehmen und in Liste speichern.
        public void FuegeProduktHinzu(string name, string zusatz = "")
        {
            // 1. Ich hole Preis von meiner Liste unten
            decimal preis = ErmittlePreis(name);

            // 2.Ich mache neues Objekt für die Position
            var neu = new BestellPosition
            {
                Name = name,
                Menge = AktuelleMenge, // Nimmt Wert von Property
                EinzelPreis = preis,
                ZusatzInfo = zusatz
            };

            // 3. Ich tue Objekt in die Liste.
            Bestellungen.Add(neu);

            // 4. Alles resetten für nächstes Mal
            AktuelleMenge = 1;
            UpdateGesamtSumme();
        }

        // Wenn User ändert Zutaten, muss Item updaten.
        public void AktualisierePosition(BestellPosition pos, string neuerZusatz)
        {
            pos.ZusatzInfo = neuerZusatz;
            
            // damit UI merkt dass Text anders ist,
            // ich tausche das Item kurz aus in der Liste.
            // Dann feuert das Event für Update.
            int index = Bestellungen.IndexOf(pos);
            if (index != -1)
            {
                Bestellungen[index] = pos; 
            }
        }

        // Private Methode um alles zusammen zu rechnen.
        // Sum() Funktion mit Lambda
        private void UpdateGesamtSumme()
        {
            decimal summe = Bestellungen.Sum(p => p.GesamtPreis);
            GesamtSummeText = $"{summe:F2} €"; // Formatieren mit Euro Zeichen
        }

        // PREIS DATENBANK 
        //prüfe Name und gebe Preis zurück
        private decimal ErmittlePreis(string produktName)
        {
            // Contains guckt ob Wort im Namen steht.
            
            // Menüs
            if (produktName.Contains("Menu")) return 9.49m;
            if (produktName.Contains("Happy Meal")) return 4.99m;

            // Burger
            if (produktName.Contains("Big Mac")) return 5.49m;
            if (produktName.Contains("Royal TS")) return 5.99m;
            if (produktName.Contains("McChicken")) return 4.99m;
            if (produktName.Contains("McRib")) return 5.49m;
            if (produktName.Contains("Hamburger")) return 1.79m;
            if (produktName.Contains("Cheese-burger")) return 1.99m;

            // Snacks & Beilagen
            if (produktName.Contains("Pommes")) return 2.99m;
            if (produktName.Contains("Nuggets")) return 4.49m;
            if (produktName.Contains("Soße")) return 0.50m;

            // Getränke & Desserts
            if (produktName.Contains("Getränk") || produktName.Contains("Cola") || produktName.Contains("Fanta")) return 2.79m;
            if (produktName.Contains("Milkshake")) return 2.99m;
            if (produktName.Contains("Eis") || produktName.Contains("McFlurry")) return 3.49m;
            if (produktName.Contains("McCafé")) return 2.99m;
            if (produktName.Contains("Apfeltasche")) return 1.49m;

            return 0.00m; // Wenn ich nichts finde
        }
    }
}