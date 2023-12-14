using FishSoft.Math;
using FishSoft.Math.Converter;
internal static class Debug {
    public static void Main() {
        while(true) {
            string input = Console.ReadLine();
            Console.Clear();
            StringToMathTerm s = new StringToMathTerm(input);
            MathTermCalculator m = new MathTermCalculator(s.Term);
            Console.WriteLine(m.ToString());
            Console.ReadKey();
            Console.Clear();
        }
    }
}
