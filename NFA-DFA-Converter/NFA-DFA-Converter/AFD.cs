using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NFA_DFA_Converter
{
   public class AFD{
      public readonly List<string> Q = new List<string>();
      public readonly List<char> Sigma = new List<char>();
      public readonly List<Kalim> Delta = new List<Kalim>();
      public List<string> Q0 = new List<string>();
      public readonly List<string> F = new List<string>();

      public AFD(IEnumerable<string> q, IEnumerable<char> sigma, IEnumerable<Kalim> delta,
         IEnumerable<string> q0, IEnumerable<string> f){
         Q = q.ToList();
         Sigma = sigma.ToList();
         AddKalims(delta);
         ShtoGjendjetFillestare(q0);
         ShtoGjendjetPerfundimtare(f);
      }

      private void AddKalims(IEnumerable<Kalim> transitions){
         foreach (var transition in transitions.Where(ValidKalim)){
            Delta.Add(transition);
         }
      }

      private bool ValidKalim(Kalim transition){
         return Q.Contains(transition.GjendjaFillestare) &&
                Q.Contains(transition.GjendjePerfundimtare) &&
                Sigma.Contains(transition.Simbol) &&
                !KalimAlreadyDefined(transition);
      }

      private bool KalimAlreadyDefined(Kalim transition){
         return Delta.Any(t => t.GjendjaFillestare == transition.GjendjaFillestare &&
                               t.Simbol == transition.Simbol);
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
         ConsoleWriter.Success("Trying to parse: " + input);
         if (InvalidInputOrFSM(input)){
            return;
         }
         foreach (var q0 in Q0){
            var currentState = q0;
            var steps = new StringBuilder();
            foreach (var symbol in input.ToCharArray()){
               var transition = Delta.Find(t => t.GjendjaFillestare == currentState &&
                                                t.Simbol == symbol);
               if (transition == null){
                  ConsoleWriter.Failure("No transitions for current state and symbol");
                  ConsoleWriter.Failure(steps.ToString());
                  continue;
               }
               currentState = transition.GjendjePerfundimtare;
               steps.Append(transition + "\n");
            }
            if (F.Contains(currentState)){
               ConsoleWriter.Success("Accepted the input with steps:\n" + steps);
               return;
            }
            ConsoleWriter.Failure("Stopped in state " + currentState +
                                  " which is not a final state.");
            ConsoleWriter.Failure(steps.ToString());
         }
      }

      private bool InvalidInputOrFSM(string input){
         if (InputContainsNotDefinedSymbols(input)){
            return true;
         }
         if (InitialStateNotSet()){
            ConsoleWriter.Failure("No initial state has been set");
            return true;
         }
         if (NoFinalStates()){
            ConsoleWriter.Failure("No final states have been set");
            return true;
         }
         return false;
      }

      private bool InputContainsNotDefinedSymbols(string input){
         foreach (var symbol in input.ToCharArray().Where(symbol => !Sigma.Contains(symbol))){
            ConsoleWriter.Failure("Could not accept the input since the symbol " + symbol + " is not part of the alphabet");
            return true;
         }
         return false;
      }

      private bool InitialStateNotSet(){
         return Q0.Count == 0;
      }

      private bool NoFinalStates(){
         return F.Count == 0;
      }
   }
}