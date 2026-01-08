namespace McDenisaKasse.Models
{
    // Das ist Model Klasse. Hier ich speichere nur Daten, keine Logik für Fenster.
    // Ein Objekt von diese Klasse ist eine Zeile auf Rechnung.
    public class BestellPosition
    {
        // Das sind Properties (Eigenschaften).
        // { get; set; } heißt ich kann lesen und schreiben.
        public string Name { get; set; } = "";       // Name von Produkt
        public int Menge { get; set; } = 1;          // Wie viel Stück
        public decimal EinzelPreis { get; set; }     // Preis für ein Stück
        public string ZusatzInfo { get; set; } = ""; // Extra Info (z.B. ohne Soße)

        // Hier ich benutze "Expression Body" (Pfeil =>).
        // Das rechnet Preis immer neu wenn ich abfrage. 
        // Ist nur "get", man kann nicht setzen.
        public decimal GesamtPreis => Menge * EinzelPreis;

        // Ich überschreibe ToString Methode von Basisklasse object.
        // Das ist wichtig damit ListBox zeigt Text und nicht nur Klassenname
        public override string ToString()
        {
            // Ich prüfe ob ZusatzInfo ist leer.
            // Wenn ja, ich mache leer String, sonst ich mache Leerzeichen davor.
            string zusatzText = string.IsNullOrEmpty(ZusatzInfo) ? "" : $" {ZusatzInfo}";
            
            // Ich gebe String zurück mit Formatierung.
            // F2 heißt Preis hat 2 Kommastellen.
            return $"{Menge}x {Name}{zusatzText}   {GesamtPreis:F2} €";
        }
    }
}