//https://www.youtube.com/watch?v=L_PByyJ9g-I

public class NeuralNetwork
{
    readonly int[] layer;
    readonly Layer[] layers;

    public NeuralNetwork(int[] layer)
    {
        this.layer = new int[layer.Length];
        for (int i = 0; i < layer.Length; i++)
        {
            this.layer[i] = layer[i];
        }

        layers = new Layer[layer.Length-1];

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(layer[i], layer[i+1]);
        }
    }

    public double[] FeedFoward(double[] inputs)
    {
        layers[0].FeedFoward(inputs);

        for (int i = 1; i < layers.Length; i++)
        {
            layers[i].FeedFoward(layers[i-1].outputs);
        }

        return layers[layers.Length - 1].outputs;
    }

    public void BackProp(double[] expected)
    {
        for (int i = layers.Length - 1; i >= 0; i--)
        {
            if (i == layers.Length - 1)
            {
                layers[i].BackPropOutput(expected);
            }
            else
            {
                layers[i].BackPropHidden(layers[i+1].gamma, layers[i+1].weights);
            }
        }

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i].UpdateWeights();
        }
    }

    public class Layer
    {
        readonly int numberOfInputs;  //# of neurons in the previous layer
        readonly int numberOfOutputs; //# of neurons in the current layer

        public double[] outputs;
        public double[] inputs;
        public double[,] weights;
        public double[,] weightsDelta;
        public double[] gamma;
        public double[] error;
        public Random random = new();

        public Layer(int numberOfInputs, int numberOfOutputs)
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfOutputs = numberOfOutputs;

            outputs = new double[numberOfOutputs];
            inputs = new double[numberOfInputs];
            weights = new double[numberOfOutputs, numberOfInputs];
            weightsDelta = new double[numberOfOutputs, numberOfInputs];
            gamma = new double[numberOfOutputs];
            error = new double[numberOfOutputs];

            InitializeWeights();
        }

        public void InitializeWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] = random.NextDouble(); //random number between 0.0 and 1.0
                }
            }
        }

        public double[] FeedFoward(double[] inputs)
        {
            this.inputs = inputs;

            for (int i = 0; i < numberOfOutputs; i++)
            {
                outputs[i] = 0;

                for (int j = 0; j < numberOfInputs; j++)
                {
                    outputs[i] += inputs[j] * weights[i, j];
                }

                outputs[i] = 1 / (1 + Math.Exp(-outputs[i]));
            }

            return outputs;
        }

        public static double ExpDer(double value)
        {
            return 1 - (value * value);
        }

        public void BackPropOutput(double[] expected)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                error[i] = outputs[i] - expected[i];
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = error[i] * ExpDer(outputs[i]);
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                }
            }
        }

        public void BackPropHidden(double[] gammaFoward, double[,] weightsFoward)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = 0;

                for (int j = 0; j < gammaFoward.Length; j++)
                {
                    gamma[i] += gammaFoward[j] * weightsFoward[j, i];
                }

                gamma[i] *= ExpDer(outputs[i]);
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                }
            }
        }

        public void UpdateWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] -= weightsDelta[i, j] * 0.04;
                }
            }
        }
    }
}