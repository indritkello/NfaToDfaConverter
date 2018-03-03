using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFA_DFA_Converter
{
   public class AFJD{
      private readonly List<string> Q = new List<string>();
      public readonly List<char> Sigma = new List<char>();
      public readonly List<Kalim> Delta = new List<Kalim>();
      public List<string> Q0 = new List<string>();
      public readonly List<string> F = new List<string>();

      public AFJD(IEnumerable<string> q, IEnumerable<char> sigma,
         IEnumerable<Kalim> delta, IEnumerable<string> q0, IEnumerable<string> f){
         Q = q.ToList();
         Sigma = sigma.ToList();
         ShtoKalimet(delta);
         ShtoGjendjetFillestare(q0);
         ShtoGjendjetPerfundimtare(f);
      }

      private void ShtoKalimet(IEnumerable<Kalim> transitions){
         foreach (var transition in transitions.Where(KalimiVlefshem)){
            Delta.Add(transition);
         }
      }

      private bool KalimiVlefshem(Kalim transition){
         return Q.Contains(transition.GjendjaFillestare) &&
                Q.Contains(transition.GjendjePerfundimtare) &&
                Sigma.Contains(transition.Simbol);
      }

      private void ShtoGjendjetFillestare(IEnumerable<string> q0){
         foreach (var startingState in q0.Where(q => q != null && Q.Contains(q))){
            Q0.Add(startingState);
         }
      }

      private void ShtoGjendjetPerfundimtare(IEnumerable<string> finalStates){
         foreach (var finalState in finalStates.Where(finalState => Q.Contains(finalState))){
            F.Add(finalState);
         }
      }

      public void Accepts(string input){
         ConsoleWriter.Success("Trying to accept: " + input);
         if (Q0.Any(q0 => Accepts(q0, input, new StringBuilder()))){
            return;
         }
         ConsoleWriter.Failure("Could not accept the input: " + input);
      }

      private bool Accepts(string currentState, string input, StringBuilder steps){
         if (input.Length > 0){
            var transitions = GetAllTransitions(currentState, input[0]);
            foreach (var transition in transitions){
               var currentSteps = new StringBuilder(steps.ToString() + transition);
               if (Accepts(transition.GjendjePerfundimtare, input.Substring(1), currentSteps)){
                  return true;
               }
            }
            return false;
         }
         if (F.Contains(currentState)){
            ConsoleWriter.Success("Successfully accepted the input " + input + " " +
                                  "in the final state " + currentState +
                                  " with steps:\n" + steps);
            return true;
         }
         return false;
      }

      private IEnumerable<Kalim> GetAllTransitions(string currentState, char symbol){
         return Delta.FindAll(t => t.GjendjaFillestare == currentState &&
                                   t.Simbol == symbol);
      }
   }
}