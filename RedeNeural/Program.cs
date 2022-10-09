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

                //1 classe
                //18 neurônios na entrada

                //13 neurônios no meio

                //7 neurônios na saída

                NeuralNetwork net = new(new int[] {3, 25, 25, 1}); //talvez será {18, 13, 7}

                for (int i = 0; i < 5000; i++)
                {
                    net.FeedFoward(new float[] { 0, 0, 0 });
                    net.BackProp(new float[] { 0 });

                    net.FeedFoward(new float[] { 0, 0, 1 });
                    net.BackProp(new float[] { 1 });

                    net.FeedFoward(new float[] { 0, 1, 0 });
                    net.BackProp(new float[] { 1 });

                    net.FeedFoward(new float[] { 0, 1, 1 });
                    net.BackProp(new float[] { 0 });

                    net.FeedFoward(new float[] { 1, 0, 0 });
                    net.BackProp(new float[] { 1 });

                    net.FeedFoward(new float[] { 1, 0, 1 });
                    net.BackProp(new float[] { 0 });

                    net.FeedFoward(new float[] { 1, 1, 0 });
                    net.BackProp(new float[] { 0 });

                    net.FeedFoward(new float[] { 1, 1, 1 });
                    net.BackProp(new float[] { 1 });
                }
            }
        }
    }
}