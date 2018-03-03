using System.Collections.Generic;
using NFA_DFA_Converter;
using System;
using System.IO;
using System.Linq;

namespace NFA_DFA_Converter{
   internal class Program{
      private static void Main(){

      using (StreamReader str = new StreamReader("Rastet.txt"))
      {
        while (!str.EndOfStream)
        {
          string[] parts = str.ReadLine().Split(' ');

          var Q = parts[0].Split(',');
          var BashkesiaE = new List<char>();
          foreach(var gj in parts[1].Split(','))
          {
            BashkesiaE.Add(Convert.ToChar(gj));
          }

          var Q0 = parts[2].Split(',');
          var F = parts[3].Split(',');
          string[] kalimet = parts[4].Split(',');
          var Kalimet = new List<Kalim>();
          foreach(var kalim in kalimet)
          {
            string[] elements = kalim.Split('-');
            Kalim kalimiIRradhes = new Kalim(elements[0], Convert.ToChar(elements[1]), elements[2]);
            Kalimet.Add(kalimiIRradhes);
          }
      
         
          var afjd = new AFJD(Q, BashkesiaE, Kalimet, Q0, F);
          var afd = Konvertuesi.Konverto(afjd);
          foreach(var kalim in afd.Delta)
          {
            Console.WriteLine(kalim.ToString());
          }
          Console.WriteLine("Shtyp nje celes per te pare automatin e rradhes!");
          Console.ReadKey();
        }
      }
      Console.ReadLine();
      }
   }
}