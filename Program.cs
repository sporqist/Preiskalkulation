using System;
using System.Text;
using System.Collections.Generic;
using System.Globalization;

namespace Preiskalkulation
{
    class Program
    {
        static void Main(string[] args)
        {
            // Aktiviere UTF-8 Ausgabe auf dem Terminal, um Sonderzeichen wie Währungssymbole ausgeben zu können.
            Console.OutputEncoding = Encoding.UTF8;
            // Speichere die Regionsinformationen des aktuellen Kontexts, um automatisch 
            // das richtige Währungssymbol anzeigen zu können.
            RegionInfo region = new RegionInfo(System.Threading.Thread.CurrentThread.CurrentCulture.LCID);
            
            // Ausgabe der Bedienungsanleitung.
            Console.WriteLine();
            Console.WriteLine("=== Listenpreiskalkulation ===");
            Console.WriteLine();
            Console.WriteLine($"Die Eingabe der Listeneinkaufspreise erfolgt in {region.CurrencySymbol}.");
            Console.WriteLine("Eingaben mit \"Enter\" bestätigen. Zum Beenden und ausgeben der Ergebnisse \"exit\" eingeben.");
            Console.WriteLine();

            bool Exit;                                          // Abbruchbedingung der nächsten while-Schleife
            Queue<double[]> Ergebnisse = new Queue<double[]>(); // FiFo Queue für das Zwischenspeichern der Ergebnisse.
            
            // Header der Eingabetabelle
            Console.WriteLine("Eingabe:");
            Console.WriteLine($"Nr.\t| Listeneinkaufspreis ({region.CurrencySymbol})");
            Console.WriteLine("========+==========================================");

            do { 
                (double, bool) _Eingabe = Eingabe($"{Ergebnisse.Count + 1}\t| "); // Aufruf der Interaktiven Eingabe.
                double Listeneinkaufspreis = _Eingabe.Item1;
                Exit = _Eingabe.Item2;

                double[] Ergebnis = Kalkulation(Listeneinkaufspreis); // Aufruf der Kalkulation.
                Ergebnisse.Enqueue(Ergebnis); // Hänge das Ergebnis der Kalkulation ans Ende der FiFo Queue an.
            } while (!Exit);

            // Header der Ausgabetabelle
            Console.WriteLine();
            Console.WriteLine("Ergebnisse:");
            Console.WriteLine($"Nr.\t| Listenverkaufspreis ({region.CurrencySymbol})");
            Console.WriteLine("========+==========================================");

            for (int i = 1; i <= Ergebnisse.Count; i++) {
                double[] Ausgabe = Ergebnisse.Dequeue(); // Entfernen eines Ergebnisses aus der FiFo Queue.

                // Ausgabe des Ergebnisses einer Kalkulation mit allen Zwischenschritten.
                Console.WriteLine($"{i}\t| Listeneinkaufspreis\t= {Ausgabe[0]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Lieferrabatt\t\t↑ -{Ausgabe[1]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Zieleinkaufspreis\t= {Ausgabe[2]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Liefererskonto\t↑ - {Ausgabe[3]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Bareinkaufspreis\t= {Ausgabe[4]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Bezugskosten\t\t↑ +{Ausgabe[5]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Bezugspreis\t\t= {Ausgabe[6]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Handlungskosten\t↑ + {Ausgabe[7]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Selbstkosten\t\t= {Ausgabe[8]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Gewinn\t\t↑ + {Ausgabe[9]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Barverkaufspreis\t= {Ausgabe[10]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Kundenskonto\t\t↓ -{Ausgabe[11]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Vertreterprovision\t↓ - {Ausgabe[12]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Zielverkaufspreis\t= {Ausgabe[13]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Kundenrabatt\t\t↓ - {Ausgabe[14]} {region.CurrencySymbol}");
                Console.WriteLine($"\t| Listenverkaufspreis\t= {Ausgabe[15]} {region.CurrencySymbol}");
                Console.WriteLine("--------+------------------------------------------");
            }
        }

        static double[] Kalkulation(double _Listeneinkaufspreis) {

            // Definiere alle Variablen für die Kalkulation statisch.
            double _Lieferrabatt        = 0.20;
            double _Liefererskonto      = 0.03;
            double _Bezugskosten        = 5.40;
            double _Handlungskosten     = 0.3215;
            double _Gewinn              = 0.15;
            double _Kundenskonto        = 0.02;
            double _Vertreterprovision  = 0.05;
            double _Kundenrabatt        = 0.10;
            _Kundenskonto       *= 100;
            _Vertreterprovision *= 100;
            _Kundenrabatt       *= 100;

            // Die Kalkulation
            double Listeneinkaufspreis             = Math.Round(_Listeneinkaufspreis, 2);
            double Lieferrabatt                    = Math.Round(Listeneinkaufspreis * _Lieferrabatt, 2);
            double Zieleinkaufspreis               = Math.Round(Listeneinkaufspreis - Lieferrabatt, 2);
            double Liefererskonto                  = Math.Round(Zieleinkaufspreis * _Liefererskonto, 2);
            double Bareinkaufspreis                = Math.Round(Zieleinkaufspreis - Liefererskonto, 2);
            double Bezugspreis                     = Math.Round(Bareinkaufspreis + _Bezugskosten, 2);
            double Handlungskosten                 = Math.Round(Bezugspreis * _Handlungskosten, 2);
            double Selbstkosten                    = Math.Round(Bezugspreis + Handlungskosten, 2);
            double Gewinn                          = Math.Round(Selbstkosten * _Gewinn, 2);
            double Barverkaufspreis                = Math.Round(Selbstkosten + Gewinn, 2);
            
            double Zielverkaufspreis_Modifikator   = 100 - (_Kundenskonto + _Vertreterprovision);
            double Zielverkaufspreis_Basis         = Barverkaufspreis / Zielverkaufspreis_Modifikator;
            double Kundenskonto                    = Math.Round(Zielverkaufspreis_Basis * _Kundenskonto, 2);
            double Vertreterprovision              = Math.Round(Zielverkaufspreis_Basis * _Vertreterprovision, 2);
            double Zielverkaufspreis               = Math.Round(Barverkaufspreis + Kundenskonto + Vertreterprovision, 2);

            double Listenverkaufspreis_Modifikator = 100 - (_Kundenrabatt);
            double Listenverkaufspreis_Basis       = Zielverkaufspreis / Listenverkaufspreis_Modifikator;
            double Kundenrabatt                    = Math.Round(Listenverkaufspreis_Basis * _Kundenrabatt, 2);
            double Listenverkaufspreis             = Math.Round(Zielverkaufspreis + Kundenrabatt, 2);
            
            // Rückgabe des Ergebnisses.
            return new double[] {Listeneinkaufspreis, Lieferrabatt, Zieleinkaufspreis, Liefererskonto,
                                 Bareinkaufspreis, _Bezugskosten, Bezugspreis, Handlungskosten, Selbstkosten, 
                                 Gewinn, Barverkaufspreis, Kundenskonto, Vertreterprovision, Zielverkaufspreis, 
                                 Kundenrabatt, Listenverkaufspreis};
        }

        static (double, bool) Eingabe(string frage) {
            double eingabe_ = 0;
            bool exit_ = false;

            bool input_success = false; // Abbruchbedingund der folgenden while-Schleife
            while (!input_success) {
                Console.Write(frage);

                string console_input = Console.ReadLine();
                if (console_input == "exit") { // Prüfen, ob der User fertig mit seiner Eingabe ist.
                    exit_ = true;
                }

                console_input = console_input.Replace(',', '.');
                // Check und Parsen der Eingabe des Users
                if (exit_ == false && double.TryParse(console_input, NumberStyles.Any, CultureInfo.InvariantCulture, out eingabe_) == false) {
                    Console.WriteLine("Ihre Eingabe war fehlerhaft. Bitte probieren sie es nocheinmal.");
                } else { input_success = true; }
            }
            return (eingabe_, exit_);
        }
    }
}
