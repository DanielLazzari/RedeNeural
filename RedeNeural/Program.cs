//teste

public static class Program
{ 
    public static void Main()
    {
        StreamReader reader = new(File.OpenRead(@"C:\Users\dlazz\OneDrive\Área de Trabalho\Inteligência Computacional\Trabalho 2\PlanilhaPronta.csv"));

        while (!reader.EndOfStream)
        {
            double somatorio = 0;
            double saida;
            string? line = reader.ReadLine();

            if (line != null)
            {
                string[] values = line.Split(';');

                //1 classe
                //18 neurônios na entrada

                //13 neurônios no meio

                //7 neurônios na saída

                for (int i = 1; i < values.Length; i++) //não considera o values[0], pois é a classe 1000000, 0100000...
                {
                    Neuronio neuronio = new()
                    {
                        Valor = Convert.ToDouble(values[i]),
                        Peso = new Random().Next(1, 11) * 0.1
                    };

                    somatorio += neuronio.Valor * neuronio.Peso;
                }

                saida = 1 / (1 + Math.Exp(-somatorio)); //gerou um valor próximo a 0,98

                Console.WriteLine();
            }
        }
    }
}