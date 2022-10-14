using System.Globalization;

public static class Program
{
    public static void Main()
    {
        int[,] matrizConfusao = new int[7,7];
        int posicaoBitCerto = 0;
        int posicaoBitErrado;

        StreamReader reader = new(File.OpenRead(@"C:\Users\dlazz\OneDrive\Área de Trabalho\Inteligência Computacional\Trabalho 2\PlanilhaPronta.csv"));

        while (!reader.EndOfStream)
        {
            string? linha = reader.ReadLine();

            if (linha != null)
            {
                string[] valoresDataSet = linha.Split(';');

                double[] valoresDouble = new double[valoresDataSet.Length-1];
                double[] classe = new double[7];

                //1 classe
                //18 na entrada
                //13 no meio
                //7 na saída
                NeuralNetwork net = new(new int[] { 18, 13, 7 });

                for (int i = 1; i < valoresDataSet.Length; i++)
                {
                    valoresDouble[i-1] = Convert.ToDouble(valoresDataSet[i].Replace(',', '.'), CultureInfo.InvariantCulture);
                }

                for (int i = 0; i < valoresDataSet[0].Length; i++)
                {
                    classe[i] = valoresDataSet[0][i] - 48;
                }

                for (int i = 0; i < 5000; i++)
                {
                    net.FeedFoward(valoresDouble);
                    net.BackProp(classe);
                }

                for (int i = 0; i < classe.Length; i++)
                {
                    if (classe[i] == 1)
                    {
                        posicaoBitCerto = i;
                        break;
                    }
                }

                double[] resultado = net.FeedFoward(valoresDouble);

                posicaoBitErrado = -1;

                for (int i = 0; i < resultado.Length; i++)
                {
                    if (Math.Round(resultado[i]) == 1 && i != posicaoBitCerto)
                    {
                        posicaoBitErrado = i;
                        break;
                    }
                }

                if (posicaoBitErrado != -1)
                {
                    matrizConfusao[posicaoBitCerto, posicaoBitErrado]++;
                }
                else
                {
                    matrizConfusao[posicaoBitCerto, posicaoBitCerto]++;
                }
            }
        }

        for (int i = 0; i < matrizConfusao.GetLength(0); i++)
        {
            switch (i)
            {
                case 0:
                    Console.Write("1000000: ");
                    break;
                case 1:
                    Console.Write("0100000: ");
                    break;
                case 2:
                    Console.Write("0010000: ");
                    break;
                case 3:
                    Console.Write("0001000: ");
                    break;
                case 4:
                    Console.Write("0000100: ");
                    break;
                case 5:
                    Console.Write("0000010: ");
                    break;
                default:
                    Console.Write("0000001: ");
                    break;
            }

            for (int j = 0; j < matrizConfusao.GetLength(1); j++)
            {
                Console.Write("{0} ", matrizConfusao[i, j]);
            }

            Console.WriteLine();
        }
    }
}