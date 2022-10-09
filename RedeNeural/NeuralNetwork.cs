﻿//https://www.youtube.com/watch?v=L_PByyJ9g-I

public class NeuralNetwork
{
    readonly Layer[] layers;

    public NeuralNetwork(int[] layer)
    {
        layers = new Layer[layer.Length-1];

        for (int i = 0; i < layers.Length; i++)
        {
            layers[i] = new Layer(layer[i], layer[i+1]);
        }
    }

    public float[] FeedFoward(float[] inputs)
    {
        layers[0].FeedFoward(inputs);

        for (int i = 1; i < layers.Length; i++)
        {
            layers[i].FeedFoward(layers[i-1].outputs);
        }

        return layers[layers.Length - 1].outputs;
    }

    public void BackProp(float[] expected)
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

        public float[] outputs;
        public float[] inputs;
        public float[,] weights;
        public float[,] weightsDelta;
        public float[] gamma;
        public float[] error;
        public Random random = new();

        public Layer(int numberOfInputs, int numberOfOutputs)
        {
            this.numberOfInputs = numberOfInputs;
            this.numberOfOutputs = numberOfOutputs;

            outputs = new float[numberOfOutputs];
            inputs = new float[numberOfInputs];
            weights = new float[numberOfOutputs, numberOfInputs];
            weightsDelta = new float[numberOfOutputs, numberOfInputs];
            gamma = new float[numberOfOutputs];
            error = new float[numberOfOutputs];

            InitializeWeights();
        }

        public void InitializeWeights()
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weights[i, j] = (float)random.NextDouble() - 0.5f;
                }
            }
        }

        public float[] FeedFoward(float[] inputs)
        {
            this.inputs = inputs;

            for (int i = 0; i < numberOfOutputs; i++)
            {
                outputs[i] = 0;

                for (int j = 0; i < numberOfInputs; j++)
                {
                    outputs[i] += inputs[j] * weights[i, j];
                }

                outputs[i] = (float)Math.Tanh(outputs[i]);

                //Sigmoid ??
                //outputs[i] = (float)(2 / (1 + Math.Exp(-2 * outputs[i])) - 1);
            }

            return outputs;
        }

        public static float TanHDer(float value)
        {
            return 1 - (value * value);

            //Sigmoid ??
            //return (float)(1 - (Math.Pow(s, 2)));
        }

        public void BackPropOutput(float[] expected)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                error[i] = outputs[i] - expected[i];
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = error[i] * TanHDer(outputs[i]);
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                for (int j = 0; j < numberOfInputs; j++)
                {
                    weightsDelta[i, j] = gamma[i] * inputs[j];
                }
            }
        }

        public void BackPropHidden(float[] gammaFoward, float[,] weightsFoward)
        {
            for (int i = 0; i < numberOfOutputs; i++)
            {
                gamma[i] = 0;

                for (int j = 0; j < gammaFoward.Length; j++)
                {
                    gamma[i] += gammaFoward[j] * weightsFoward[j, i];
                }

                gamma[i] *= TanHDer(outputs[i]);
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
                    weights[i, j] -= weightsDelta[i, j] * 0.033f;  //Learning Curve, edit to verify accuracy ??
                }
            }
        }
    }
}