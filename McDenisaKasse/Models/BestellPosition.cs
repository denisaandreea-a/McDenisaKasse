namespace McDenisaKasse.Models
{
    // Model Klasse (speichere nur Daten, keine Logik für Fenster)
    // Ein Objekt von diese Klasse ist eine Zeile auf Rechnung.
    public class BestellPosition
    {
        // Properties (Eigenschaften).
        // get, set kann lesen und schreiben.
        public string Name { get; set; } = "";       // Name von Produkt
        public int Menge { get; set; } = 1;          // Wie viel Stück
        public decimal EinzelPreis { get; set; }     // Preis für ein Stück
        public string ZusatzInfo { get; set; } = ""; // Extra Info (z.B. ohne Soße)

        // "Expression Body" (Pfeil =>).
        // Das rechnet Preis immer neu wenn ich abfrage. 
        // Ist nur get, man kann nicht setzen.
        public decimal GesamtPreis => Menge * EinzelPreis;

        // Ich überschreibe ToString Methode von Basisklasse object.
        //wichtig damit ListBox zeigt Text und nicht nur Klassenname
        public override string ToString()
        {
            // prüfe ob ZusatzInfo ist leer.
            // Wenn ja, ich mache leer String, sonst ich mache Leerzeichen davor.
            string zusatzText = string.IsNullOrEmpty(ZusatzInfo) ? "" : $" {ZusatzInfo}";
            
            // Ich gebe String zurück mit Formatierung.
            // F2 heißt Preis hat 2 Kommastellen.
            return $"{Menge}x {Name}{zusatzText}   {GesamtPreis:F2} €";
        }
    }
}