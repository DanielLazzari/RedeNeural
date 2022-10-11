using System.Globalization;

public static class Program
{
    public static void Main()
    {
        StreamReader reader = new(File.OpenRead(@"C:\Users\dlazz\OneDrive\Área de Trabalho\Inteligência Computacional\Trabalho 2\PlanilhaPronta.csv"));

        while (!reader.EndOfStream)
        {
            string? line = reader.ReadLine();

            if (line != null)
            {
                string[] values = line.Split(';');
                double[] valores = new double[1];

                //1 classe
                //18 neurônios na entrada

                //13 neurônios no meio

                //7 neurônios na saída

                NeuralNetwork net = new(new int[] { 1, 13, 7 });

                for (int i = 1; i < values.Length; i++)
                {
                    valores[0] = Convert.ToDouble(values[i].Replace(',', '.'), CultureInfo.InvariantCulture);

                    for (int j = 0; j < 5000; j++)
                    {
                        net.FeedFoward(valores);

                        switch (values[0])
                        {
                            case "1000000":
                                net.BackProp(new double[] { 1, 0, 0, 0, 0, 0, 0 });
                                break;
                            case "0100000":
                                net.BackProp(new double[] { 0, 1, 0, 0, 0, 0, 0 });
                                break;
                            case "0010000":
                                net.BackProp(new double[] { 0, 0, 1, 0, 0, 0, 0 });
                                break;
                            case "0001000":
                                net.BackProp(new double[] { 0, 0, 0, 1, 0, 0, 0 });
                                break;
                            case "0000100":
                                net.BackProp(new double[] { 0, 0, 0, 0, 1, 0, 0 });
                                break;
                            case "0000010":
                                net.BackProp(new double[] { 0, 0, 0, 0, 0, 1, 0 });
                                break;
                            case "0000001":
                                net.BackProp(new double[] { 0, 0, 0, 0, 0, 0, 1 });
                                break;
                        }
                    }

                    var resultado = net.FeedFoward(valores);

                    Console.WriteLine("{0}, {1}, {2}, {3}, {4}, {5}, {6}", resultado[0], resultado[1], resultado[2], resultado[3], resultado[4], resultado[5], resultado[6]);
                }
            }
        }
    }
}