using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NeuralNetwork : IComparable<NeuralNetwork>
{
    private int[] layers;//layers
    //this will be used to 
    //hold the values generated during the feedforward algorithm
    private float[][] neurons;
    private float[][] biases;//biasses
    private float[][][] weights;//weights
    public float Fitness { get; set; } = 0;//fitness
    private int hashCode;

    private static Random rndm = new Random();
    public static Random Rndm
    {
        get { return rndm; }
        set { rndm = value; }
    }

    /// <summary>
    /// Define the dimensions of the network and initialize all the appropriate arrays.
    /// </summary>
    /// <param name="layers">an array of the network dimensions</param>
    public NeuralNetwork(int[] layers)
    {
        hashCode = getRandomHash() ;
        this.layers = (int[])layers.Clone();

        InitNeurons();
        InitBiases();
        InitWeights();
    }

    //create empty storage array for the neurons in the network.
    private void InitNeurons()
    {
        neurons = new float[layers.Length][];
        for(int i = 0; i < layers.Length; i++)
        {
            neurons[i] = new float[layers[i]];
        }
    }

    //initializes and populates array for the biases being held within the network.
    private void InitBiases()
    {
        biases = new float[layers.Length][];
        for(int i = 0; i < biases.Length; i++)
        {
            float[] bias = new float[layers[i]];
            Array.ConvertAll(bias, j => UnityEngine.Random.Range(-0.5f, 0.5f));
            biases[i] = bias;
        }
    }

    //initializes random array for the weights being held in the network.
    private void InitWeights()
    {
        List<float[][]> weightsList = new List<float[][]>();
        for (int i = 1; i < layers.Length; i++)
        {
            List<float[]> layerWeightsList = new List<float[]>();
            int neuronsInPreviousLayer = layers[i - 1];
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float[] neuronWeights = new float[neuronsInPreviousLayer];
                for (int k = 0; k < neuronsInPreviousLayer; k++)
                {
                    //float sd = 1f / ((neurons[i].Length + neuronsInPreviousLayer) / 2f);
                    neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
                }
                layerWeightsList.Add(neuronWeights);
            }
            weightsList.Add(layerWeightsList.ToArray());
        }
        weights = weightsList.ToArray();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inputs">Input values for the neural networks</param>
    /// <returns>the output of the neural network</returns>
    public float[] FeedForward(float[] inputs)
    {
        for (int i = 0; i < inputs.Length; i++)
        {
            neurons[0][i] = inputs[i];
        }
        for (int i = 1; i < layers.Length; i++)
        {
            int layer = i - 1;
            for (int j = 0; j < neurons[i].Length; j++)
            {
                float value = 0f;
                for (int k = 0; k < neurons[i - 1].Length; k++)
                {
                    value += weights[i - 1][j][k] * neurons[i - 1][k];
                }
                neurons[i][j] = Activate(value + biases[i][j]);
            }
        }
        return neurons[neurons.Length - 1];
    }

    public static float Activate(float value)
    {
        return (float)Math.Tanh(value);
    }


    /// <summary>
    /// crossover two neural networks
    /// </summary>
    /// <param name="other">the other neural network to 
    /// crossover with the current one</param>
    /// <returns>
    /// The result is a combination between the two neural networks.
    /// </returns>
    public NeuralNetwork Crossover(NeuralNetwork other)
    {
        NeuralNetwork result = Clone(this);

        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {
                if(UnityEngine.Random.Range(0f, 1f) > 0.5)
                    result.biases[i][j] = other.biases[i][j];
            }
        }

        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    if (UnityEngine.Random.Range(0f, 1f) > 0.5)
                        result.weights[i][j][k] = other.weights[i][j][k];
                }
            }
        }
        result.hashCode = Convert.ToInt32(0x000000FF);
        return result;
    }


    //For cloning neural network attributes.
    public NeuralNetwork Clone(NeuralNetwork nn)
    {
        for (int i = 0; i < biases.Length; i++)
        {
            for (int j = 0; j < biases[i].Length; j++)
            {
                nn.biases[i][j] = biases[i][j];
            }
        }
        for (int i = 0; i < weights.Length; i++)
        {
            for (int j = 0; j < weights[i].Length; j++)
            {
                for (int k = 0; k < weights[i][j].Length; k++)
                {
                    nn.weights[i][j][k] = weights[i][j][k];
                }
            }
        }
        int hc = (rndm.Next(0, (int)Math.Pow(2, 24)) / 2);

        nn.hashCode = hashCode;
        return nn;
    }


    /// <summary>
    /// mutate the values of the neural network
    /// </summary>
    /// <param name="chance">mutation chance</param>
    /// <param name="strength">max mutation Strength</param>
    public void Mutate(float chance, float strength)
    {
        for (int i = 0; i < biases.Length; i++)
        {
        for (int j = 0; j < biases[i].Length; j++)
        {
            biases[i][j] = 
                (UnityEngine.Random.Range(0f, 1f) < chance) ?
                biases[i][j] += UnityEngine.Random.Range(-strength, strength) 
                : biases[i][j];
        }
        }

        for (int i = 0; i < weights.Length; i++)
        {
        for (int j = 0; j < weights[i].Length; j++)
        {
        for (int k = 0; k < weights[i][j].Length; k++)
        {
            weights[i][j][k] 
            = (UnityEngine.Random.Range(0f, 1f) < chance) ?
            weights[i][j][k] += UnityEngine.Random.Range(-strength, strength) 
            : weights[i][j][k];
        }
        }
        }

        hashCode = MutateHash(hashCode);
    }


    /// <summary>
    /// mutate the hashcode
    /// the hashcode represent the color of the car in the simulation
    /// </summary>
    /// <param name="color">the color to be mutated</param>
    /// <returns>the mutated color</returns>
    private static int MutateHash(int color)
    {

        byte[] bytes = System.BitConverter.GetBytes(color);
        byte colorMutationValue = 20;

    for (int i = 0; i < bytes.Length; i++)
    {

        if (bytes[i] + (int)colorMutationValue > 255)
        {
            bytes[i] -= (byte)(colorMutationValue * 3);
        }
        else if (bytes[i] - (int)colorMutationValue < 0)
        {
            bytes[i] += (byte)(colorMutationValue * 3);
        }
        else
        {
            int addOrSubtract = rndm.Next(0, 1);
            if (addOrSubtract == 0)
            {
                bytes[i] += colorMutationValue;
            }
            else
            {
                bytes[i] -= colorMutationValue;
            }
        }
        }

        int result;
        result = System.BitConverter.ToInt32(bytes, 0);

        return result;
    }





    public int CompareTo(NeuralNetwork other) //Comparing For NeuralNetworks performance.
    {
        if (other == null) return 1;

        if (Fitness > other.Fitness)
            return 1;
        else if (Fitness < other.Fitness)
            return -1;
        else
            return 0;
    }
  
    private int getRandomHash()
    {
        return (rndm.Next(255) << 16) + (rndm.Next(255) << 8) + rndm.Next(255);
    }
    public override int GetHashCode()
    {
        return this.hashCode;
    }

}
