namespace NFA_DFA_Converter{
   public class Kalim{
      public string GjendjaFillestare { get; private set; }
      public char Simbol { get; private set; }
      public string GjendjePerfundimtare { get; private set; }

      public Kalim(string gjendeFillestare, char simbol, string gjendjeFundore){
      GjendjaFillestare = gjendeFillestare;
      Simbol = simbol;
      GjendjePerfundimtare = gjendjeFundore;
      }

      public override string ToString(){
         return string.Format("({0}, {1}) -> {2}\n", GjendjaFillestare, Simbol, GjendjePerfundimtare);
      }
   }
}