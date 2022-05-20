using System;
using System.Text;

namespace Preiskalkulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.WriteLine();
            Console.WriteLine("Listenpreiskalkulation");
            Console.WriteLine();

            double _Listeneinkaufspreis = EingabeDouble("Einkauf Listenpreis (€)");
            double _Lieferrabatt        = EingabeDouble("Lieferrabatt (%)") / 100;
            double _Liefererskonto      = EingabeDouble("Liefererskonto (%)") / 100;
            double _Bezugskosten        = EingabeDouble("Bezugskosten (€)");
            double _Handlungskosten     = EingabeDouble("Handlungskosten (%)") / 100;
            double _Gewinn              = EingabeDouble("Gewinn (%)") / 100;
            double _Kundenskonto        = EingabeDouble("Kundenskonto (%)");
            double _Vertreterprovision  = EingabeDouble("Vertreterprovision (%)");
            double _Kundenrabatt        = EingabeDouble("Kundenrabatt (%)");

            Console.WriteLine();

            double Listeneinkaufspreis           = Math.Round(_Listeneinkaufspreis, 2);
            double Zieleinkaufspreis             = Math.Round(Listeneinkaufspreis - Listeneinkaufspreis * _Lieferrabatt, 2);
            double Bareinkaufspreis              = Math.Round(Zieleinkaufspreis - Zieleinkaufspreis * _Liefererskonto, 2);
            double Bezugspreis                   = Math.Round(Bareinkaufspreis + _Bezugskosten, 2);
            double Selbstkosten                  = Math.Round(Bezugspreis + Bezugspreis * _Handlungskosten, 2);
            double Barverkaufspreis              = Math.Round(Selbstkosten + Selbstkosten * _Gewinn, 2);
            
            double Zielverkaufspreis_Modifikator = 100 - (_Kundenskonto + _Vertreterprovision);
            double Zielverkaufspreis_Basis       = Barverkaufspreis / Zielverkaufspreis_Modifikator;
            double Zielverkaufspreis             = Math.Round(Barverkaufspreis
                                                   + Math.Round(Zielverkaufspreis_Basis * _Kundenskonto, 2) 
                                                   + Math.Round(Zielverkaufspreis_Basis * _Vertreterprovision, 2), 2);

            double Listenverkaufspreis_Modifikator = 100 - (_Kundenrabatt);
            double Listenverkaufspreis_Basis       = Zielverkaufspreis / Listenverkaufspreis_Modifikator;
            double Listenverkaufspreis           = Math.Round(Zielverkaufspreis
                                                   + Math.Round(Listenverkaufspreis_Basis * _Kundenrabatt, 2), 2);
            
            Console.WriteLine($"Listeneinkaufspreis: {Listeneinkaufspreis}");
            Console.WriteLine($"Zieleinkaufspreis:   {Zieleinkaufspreis}");
            Console.WriteLine($"Bareinkaufspreis:    {Bareinkaufspreis}");
            Console.WriteLine($"Bezugspreis:         {Bezugspreis}");
            Console.WriteLine($"Selbstkosten:        {Selbstkosten}");
            Console.WriteLine($"Barverkaufspreis:    {Barverkaufspreis}");
            Console.WriteLine($"Zielverkaufspreis:   {Zielverkaufspreis}");
            
            Console.WriteLine();
            
            Console.WriteLine($"Der Listenverkaufspreis beträgt: {Listenverkaufspreis}€ netto");
        }

        static double EingabeDouble(string frage) {
            double eingabe = 0;

            bool input_success = false;
            while (!input_success) {
                Console.Write(frage + ": ");
                string console_input = Console.ReadLine();
                if (double.TryParse(console_input, out eingabe) == false) {
                    Console.WriteLine("Ihre Eingabe war fehlerhaft. Bitte probieren sie es nocheinmal.");
                } else { input_success = true; }
            }
            return eingabe;
        }
    }
}
