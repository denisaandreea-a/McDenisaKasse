using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.ComponentModel.DataAnnotations; // für Validierung
using System.Threading.Tasks; //für Nebenläufigkeit (async)

namespace McDenisaKasse.ViewModels
{
    public partial class CheckoutViewModel : ViewModelBase
    {
        [ObservableProperty]
        [NotifyDataErrorInfo] // Zeigt Fehler in der UI an
        [Required(ErrorMessage = "Vorname ist erforderlich.")]
        [MinLength(2, ErrorMessage = "Name zu kurz.")]
        [NotifyCanExecuteChangedFor(nameof(BarZahlenCommand))]
        [NotifyCanExecuteChangedFor(nameof(KarteZahlenCommand))]
        private string _vorname = "";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Name ist erforderlich.")]
        [MinLength(2, ErrorMessage = "Name zu kurz.")]
        [NotifyCanExecuteChangedFor(nameof(BarZahlenCommand))]
        [NotifyCanExecuteChangedFor(nameof(KarteZahlenCommand))]
        private string _name = "";

        [ObservableProperty]
        [NotifyDataErrorInfo]
        [Required(ErrorMessage = "Adresse fehlt für die Lieferung.")]
        [MinLength(10, ErrorMessage = "Bitte vollständige Adresse angeben.")]
        [NotifyCanExecuteChangedFor(nameof(BarZahlenCommand))]
        [NotifyCanExecuteChangedFor(nameof(KarteZahlenCommand))]
        private string _adresse = "";

        [ObservableProperty]
        private string _kommentar = "";

        public CheckoutViewModel()
        {
            // Prüft alle Felder beim Start, damit Buttons ggf. gesperrt sind
            ValidateAllProperties();
        }

        // Hilfsmethode: Buttons sind nur klickbar, wenn HasErrors "false" ist
        private bool KannBezahlen() => !HasErrors;

        [RelayCommand(CanExecute = nameof(KannBezahlen))]
        private async Task BarZahlen()
        {
            // Simuliert einen asynchronen Speichervorgang (Nebenläufigkeit)
            Console.WriteLine("Verarbeite Barzahlung...");
            await Task.Delay(2000); 
            Console.WriteLine($"Erfolgreich! Danke {Vorname}.");
        }

        [RelayCommand(CanExecute = nameof(KannBezahlen))]
        private async Task KarteZahlen()
        {
            // Simuliert Verbindung zum Kartenterminal
            Console.WriteLine("Verbindung zum Terminal...");
            await Task.Delay(3000);
            Console.WriteLine("Zahlung autorisiert.");
        }
    }
}