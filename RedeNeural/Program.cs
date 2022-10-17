using System.Globalization;

public static class Program
{
    public static void Main()
    {
        int[,] matrizConfusao = new int[7,7];
        int posicaoBitCerto = 0;
        int posicaoBitErrado;
        double[] recall = new double[7];
        double[] precisao = new double[7];
        double acuraciaCertos = 0;
        double acuraciaTotal = 0;

        //Lê o arquivo do dataset, sendo que o arquivo irá estar com os valores normalizados e sem o nome do cabeçalho de cada coluna
        //As classes irão estar em valores dummy
        StreamReader reader = new(File.OpenRead(@"C:\Users\dlazz\OneDrive\Área de Trabalho\Inteligência Computacional\Trabalho 2\treino.test.csv"));

        //Percorre cada linha do dataset
        while (!reader.EndOfStream)
        {
            //Leitura de cada linha
            string? linha = reader.ReadLine();

            if (linha != null)
            {
                //Separa os elementos de cada linha em um array, fazendo um split a partir do ponto e vírgula
                string[] valoresDataSet = linha.Split(';');

                double[] valoresDouble = new double[valoresDataSet.Length-1];
                double[] classe = new double[7];

                //Converte os valores em double, alterando a vírgula por ponto (caso houver)
                for (int i = 1; i < valoresDataSet.Length; i++)
                {
                    valoresDouble[i-1] = Convert.ToDouble(valoresDataSet[i].Replace(',', '.'), CultureInfo.InvariantCulture);
                }

                //Coloca o valor de cada bit da classe da linha num array
                //string '1' em ASCII é 49 e string '0' em ASCII é 48, então descontamos 48 para transformar em double
                for (int i = 0; i < valoresDataSet[0].Length; i++)
                {
                    classe[i] = valoresDataSet[0][i] - 48;
                }

                //Inicia a instância da rede neural
                //1 classe
                //18 na entrada
                //13 no meio
                //7 na saída
                RedeNeural rede = new(new int[] { 18, 13, 7 });

                //Percorre 5000 épocas para que possa treinar
                for (int i = 0; i < 5000; i++)
                {
                    rede.FeedFoward(valoresDouble);
                    rede.BackProp(classe);
                }

                //Recebe o resultado depois de treinar
                double[] resultado = rede.FeedFoward(valoresDouble);

                //Procura na classe aonde tem o bit 1
                for (int i = 0; i < classe.Length; i++)
                {
                    if (classe[i] == 1)
                    {
                        posicaoBitCerto = i;
                        break;
                    }
                }

                posicaoBitErrado = -1;

                //Procura no resultado se houve algum erro em algum bit do resultado
                for (int i = 0; i < resultado.Length; i++)
                {
                    if (Math.Round(resultado[i]) == 1 && i != posicaoBitCerto)
                    {
                        posicaoBitErrado = i;
                        break;
                    }
                }

                //Se achou um bit errado incrementa na posição da matriz
                //caso contrário incrementa na posição da matriz na diagonal principal
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

        //Mostra a matriz de confusão e soma as linhas e colunas com os valores
        //para as métricas posteriores (recall, precisão, f1-score e acurácia)
        Console.WriteLine("Matriz de confusão: ");

        double[] somaCertosColuna = new double[7];
        double[] somaTotalColuna = new double[7];

        for (int i = 0; i < matrizConfusao.GetLength(0); i++)
        {
            double somaCertosLinha = 0;
            double somaTotalLinha = 0;

            PrintClass(i);

            for (int j = 0; j < matrizConfusao.GetLength(1); j++)
            {
                Console.Write("{0} ", matrizConfusao[i, j]);

                if (i == j)
                {
                    somaCertosLinha += matrizConfusao[i, j];
                    somaCertosColuna[i] += matrizConfusao[i, j];
                }

                somaTotalLinha += matrizConfusao[i, j];
                somaTotalColuna[j] += matrizConfusao[i, j];
            }

            recall[i] = somaCertosLinha / somaTotalLinha;

            Console.WriteLine();
        }

        //Calcula cada precisão da matriz e também calcula parte da acurácia
        for (int i = 0; i < somaTotalColuna.Length; i++)
        {
            for (int j = 0; j < somaCertosColuna.Length; j++)
            {
                if (i == j)
                {
                    precisao[i] = somaCertosColuna[i] / somaTotalColuna[i];
                    break;
                }
            }

            acuraciaCertos += somaCertosColuna[i];
            acuraciaTotal += somaTotalColuna[i];
        }

        //Mostra os recalls
        Console.WriteLine();
        Console.WriteLine("Recall (linha): ");

        for (int i = 0; i < recall.Length; i++)
        {
            PrintClass(i);

            Console.Write("{0} ", recall[i]);

            Console.WriteLine();
        }

        //Mostra as precisões
        Console.WriteLine();
        Console.WriteLine("Precisão (coluna): ");

        for (int i = 0; i < precisao.Length; i++)
        {
            PrintClass(i);

            Console.Write("{0} ", precisao[i]);

            Console.WriteLine();
        }

        //Mostra os f1-scores
        Console.WriteLine();
        Console.WriteLine("F1-Score (classe): ");

        for (int i = 0; i < recall.Length; i++)
        { 
            PrintClass(i);

            Console.Write("{0} ", 2 * (recall[i] * precisao[i]) / (recall[i] + precisao[i]));

            Console.WriteLine();
        }

        //Mostra a acurácia
        Console.WriteLine();
        Console.WriteLine("Acurácia: {0}", acuraciaCertos / acuraciaTotal);
    }

    public static void PrintClass(int i)
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
    }
}