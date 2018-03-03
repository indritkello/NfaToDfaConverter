using System.Collections.Generic;
using System.Linq;

namespace NFA_DFA_Converter
{
   internal class Konvertuesi
  {
      public static AFD Konverto(AFJD nd){
         var Q = new List<string>();
         var Sigma = nd.Sigma.ToList();
         var Delta = new List<Kalim>();
         var Q0 = nd.Q0.ToList();
         var F = new List<string>();

         var processed = new List<string>();
         var queue = new Queue<string>(Q0);

         while (queue.Count > 0){
            var setState = queue.Dequeue();
            processed.Add(setState);
            Q.Add(setState);

            var statesInCurrentSetState = setState.Split(',').ToList();
            foreach (var state in statesInCurrentSetState){
               if (nd.F.Contains(state)){
                  F.Add(setState);
                  break;
               }
            }
            var symbols = nd.Delta
               .Where(t => statesInCurrentSetState.Contains(t.GjendjaFillestare))
               .Select(t => t.Simbol)
               .Distinct();
            foreach (var symbol in symbols){
               var reachableStates =
                  nd.Delta
                     .Where(t => t.Simbol == symbol &&
                                 statesInCurrentSetState.Contains(t.GjendjaFillestare))
                     .OrderBy(t => t.GjendjePerfundimtare).
                     Select(t => t.GjendjePerfundimtare);
               var reachableSetState = string.Join(",", reachableStates);

               Delta.Add(new Kalim(setState, symbol, reachableSetState));

               if (!processed.Contains(reachableSetState)){
                  queue.Enqueue(reachableSetState);
               }
            }
         }
         return new AFD(Q, Sigma, Delta, Q0, F);
      }
   }
}