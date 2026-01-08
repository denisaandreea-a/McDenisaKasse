namespace McDenisaKasse.Models
{
    // Diese Klasse ist unser "Model". Sie repräsentiert eine einzelne Zeile auf der Bestellung.
    // Statt mit Strings wie "2x Burger 5€" zu rechnen, speichern wir hier die echten Daten.
    public class BestellPosition
    {
        public string Name { get; set; } = "";       // z.B. "Big Mac"
        public int Menge { get; set; } = 1;          // z.B. 2
        public decimal EinzelPreis { get; set; }     // z.B. 5.49
        public string ZusatzInfo { get; set; } = ""; // z.B. "(Ohne Gurke)"

        // Diese Eigenschaft berechnet den Gesamtpreis für diese Position automatisch.
        // Der Pfeil "=>" immer live berechnet wird, wenn man darauf zugreift.
        public decimal GesamtPreis => Menge * EinzelPreis;

        // ToString() bestimmt, wie das Objekt in der ListBox angezeigt wird.
        // Wir bauen hier den Text zusammen, den man auf dem Bildschirm sieht.
        public override string ToString()
        {
            // Formatierung: Preis mit 2 Nachkommastellen (F2) und Euro-Zeichen
            // Wenn ZusatzInfo leer ist, zeigen wir nichts an, sonst ein Leerzeichen davor.
            string zusatzText = string.IsNullOrEmpty(ZusatzInfo) ? "" : $" {ZusatzInfo}";
            return $"{Menge}x {Name}{zusatzText}   {GesamtPreis:F2} €";
        }
    }
}

