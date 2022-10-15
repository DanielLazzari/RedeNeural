public class RedeNeural
{
    readonly Camada[] camadas;

    public RedeNeural(int[] camada)
    {
        camadas = new Camada[camada.Length-1];

        for (int i = 0; i < camadas.Length; i++)
        {
            camadas[i] = new Camada(camada[i], camada[i+1]);
        }
    }

    public double[] FeedFoward(double[] entradas)
    {
        camadas[0].FeedFoward(entradas);

        for (int i = 1; i < camadas.Length; i++)
        {
            camadas[i].FeedFoward(camadas[i-1].saidas);
        }

        return camadas[camadas.Length - 1].saidas;
    }

    public void BackProp(double[] esperado)
    {
        for (int i = camadas.Length - 1; i >= 0; i--)
        {
            if (i == camadas.Length - 1)
            {
                camadas[i].BackPropSaida(esperado);
            }
            else
            {
                camadas[i].BackPropIntermediario(camadas[i+1].gamma, camadas[i+1].pesos);
            }
        }

        for (int i = 0; i < camadas.Length; i++)
        {
            camadas[i].AtualizaPesos();
        }
    }

    public class Camada
    {
        readonly int numeroDeEntradas; 
        readonly int numeroDeSaidas;

        public double[] saidas;
        public double[] entradas;
        public double[,] pesos;
        public double[,] pesosDelta;
        public double[] gamma;
        public double[] erro;
        public Random random = new();

        public Camada(int numeroDeEntradas, int numeroDeSaidas)
        {
            this.numeroDeEntradas = numeroDeEntradas;
            this.numeroDeSaidas = numeroDeSaidas;

            saidas = new double[numeroDeSaidas];
            entradas = new double[numeroDeEntradas];
            pesos = new double[numeroDeSaidas, numeroDeEntradas];
            pesosDelta = new double[numeroDeSaidas, numeroDeEntradas];
            gamma = new double[numeroDeSaidas];
            erro = new double[numeroDeSaidas];

            InicializaPesos();
        }

        public void InicializaPesos()
        {
            for (int i = 0; i < numeroDeSaidas; i++)
            {
                for (int j = 0; j < numeroDeEntradas; j++)
                {
                    pesos[i, j] = random.NextDouble(); //número aleatório entre 0.0 and 1.0
                }
            }
        }

        public double[] FeedFoward(double[] entradas)
        {
            this.entradas = entradas;

            for (int i = 0; i < numeroDeSaidas; i++)
            {
                saidas[i] = 0;

                for (int j = 0; j < numeroDeEntradas; j++)
                {
                    saidas[i] += entradas[j] * pesos[i, j];
                }

                saidas[i] = 1 / (1 + Math.Exp(-saidas[i]));
            }

            return saidas;
        }

        public static double ExpDer(double valor)
        {
            return 1 - (valor * valor);
        }

        public void BackPropSaida(double[] esperado)
        {
            for (int i = 0; i < numeroDeSaidas; i++)
            {
                erro[i] = saidas[i] - esperado[i];
            }

            for (int i = 0; i < numeroDeSaidas; i++)
            {
                gamma[i] = erro[i] * ExpDer(saidas[i]);
            }

            for (int i = 0; i < numeroDeSaidas; i++)
            {
                for (int j = 0; j < numeroDeEntradas; j++)
                {
                    pesosDelta[i, j] = gamma[i] * entradas[j];
                }
            }
        }

        public void BackPropIntermediario(double[] gammaFoward, double[,] pesosFoward)
        {
            for (int i = 0; i < numeroDeSaidas; i++)
            {
                gamma[i] = 0;

                for (int j = 0; j < gammaFoward.Length; j++)
                {
                    gamma[i] += gammaFoward[j] * pesosFoward[j, i];
                }

                gamma[i] *= ExpDer(saidas[i]);
            }

            for (int i = 0; i < numeroDeSaidas; i++)
            {
                for (int j = 0; j < numeroDeEntradas; j++)
                {
                    pesosDelta[i, j] = gamma[i] * entradas[j];
                }
            }
        }

        public void AtualizaPesos()
        {
            for (int i = 0; i < numeroDeSaidas; i++)
            {
                for (int j = 0; j < numeroDeEntradas; j++)
                {
                    pesos[i, j] -= pesosDelta[i, j] * 0.04; //Taxa de aprendizagem
                }
            }
        }
    }
}