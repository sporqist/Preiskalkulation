using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace Preiskalkulation
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            var region = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentCulture.LCID);
            
            Console.WriteLine();
            Console.WriteLine("=== Listenpreiskalkulation ===");
            Console.WriteLine();
            Console.WriteLine($"Die Eingabe der Listeneinkaufspreise erfolgt in {region.CurrencySymbol}.");
            Console.WriteLine("Eingaben mit \"Enter\" bestätigan. Zum Beenden und ausgeben der Ergebnisse \"exit\" eingeben.");
            Console.WriteLine();

            bool Exit;
            int Iteration = 1;
            Queue<double> Ergebnisse = new Queue<double>();
            
            Console.WriteLine("Eingabe:");
            Console.WriteLine($"Nr.\t| Listeneinkaufspreis ({region.CurrencySymbol})");
            Console.WriteLine("--------+------------------------");
            do {
                (double, bool) _Eingabe = Eingabe($"{Iteration}\t| ");
                double Listeneinkaufspreis = _Eingabe.Item1;
                Exit = _Eingabe.Item2;

                double Ergebnis = Kalkulation(Listeneinkaufspreis);
                Ergebnisse.Enqueue(Ergebnis);
                Iteration++;
            } while (!Exit);

            Console.WriteLine();
            Console.WriteLine("Ergebnisse:");
            Console.WriteLine($"Nr.\t| Listenverkaufspreis ({region.CurrencySymbol})");
            Console.WriteLine("--------+------------------------");

            for (int i = 1; i <= Ergebnisse.Count; i++) {
                double Ausgabe = Ergebnisse.Dequeue();

                Console.WriteLine($"{i}\t| {Ausgabe}");
            }
        }

        static double Kalkulation(double _Listeneinkaufspreis) {
            
            double _Lieferrabatt        = 0.20;
            double _Liefererskonto      = 0.03;
            double _Bezugskosten        = 5.40;
            double _Handlungskosten     = 0.3215;
            double _Gewinn              = 0.15;
            double _Kundenskonto        = 0.02;
            double _Vertreterprovision  = 0.05;
            double _Kundenrabatt        = 0.10;


            double Listeneinkaufspreis           = Math.Round(_Listeneinkaufspreis, 2);
            double Zieleinkaufspreis             = Math.Round(Listeneinkaufspreis - Listeneinkaufspreis * _Lieferrabatt, 2);
            double Bareinkaufspreis              = Math.Round(Zieleinkaufspreis - Zieleinkaufspreis * _Liefererskonto, 2);
            double Bezugspreis                   = Math.Round(Bareinkaufspreis + _Bezugskosten, 2);
            double Selbstkosten                  = Math.Round(Bezugspreis + Bezugspreis * _Handlungskosten, 2);
            double Barverkaufspreis              = Math.Round(Selbstkosten + Selbstkosten * _Gewinn, 2);
            
            _Kundenskonto       *= 100;
            _Vertreterprovision *= 100;
            _Kundenrabatt       *= 100;
            double Zielverkaufspreis_Modifikator = 100 - (_Kundenskonto + _Vertreterprovision);
            double Zielverkaufspreis_Basis       = Barverkaufspreis / Zielverkaufspreis_Modifikator;
            double Zielverkaufspreis             = Math.Round(Barverkaufspreis
                                                   + Math.Round(Zielverkaufspreis_Basis * _Kundenskonto, 2) 
                                                   + Math.Round(Zielverkaufspreis_Basis * _Vertreterprovision, 2), 2);

            double Listenverkaufspreis_Modifikator = 100 - (_Kundenrabatt);
            double Listenverkaufspreis_Basis       = Zielverkaufspreis / Listenverkaufspreis_Modifikator;
            double Listenverkaufspreis           = Math.Round(Zielverkaufspreis
                                                   + Math.Round(Listenverkaufspreis_Basis * _Kundenrabatt, 2), 2);
            
            //Console.WriteLine($"Listeneinkaufspreis: {Listeneinkaufspreis}");
            //Console.WriteLine($"Zieleinkaufspreis:   {Zieleinkaufspreis}");
            //Console.WriteLine($"Bareinkaufspreis:    {Bareinkaufspreis}");
            //Console.WriteLine($"Bezugspreis:         {Bezugspreis}");
            //Console.WriteLine($"Selbstkosten:        {Selbstkosten}");
            //Console.WriteLine($"Barverkaufspreis:    {Barverkaufspreis}");
            //Console.WriteLine($"Zielverkaufspreis:   {Zielverkaufspreis}");
            //
            //Console.WriteLine();


            return Listenverkaufspreis;
        }

        static (double, bool) Eingabe(string frage) {
            double eingabe_ = 0;
            bool exit_ = false;

            bool input_success = false;
            while (!input_success) {
                Console.Write(frage);

                string console_input = Console.ReadLine();
                if (console_input == "exit") {
                    exit_ = true;
                }

                console_input = console_input.Replace(',', '.');
                if (double.TryParse(console_input, NumberStyles.Any, CultureInfo.InvariantCulture, out eingabe_) == false && exit_ == false) {
                    Console.WriteLine("Ihre Eingabe war fehlerhaft. Bitte probieren sie es nocheinmal.");
                } else { input_success = true; }
            }
            return (eingabe_, exit_);
        }
    }
}
