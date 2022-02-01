using System.Collections.Generic;
using System.Linq;
using UnityEngine;


/// <summary>
/// the AIController controlls the AI part of the simulation
/// it performs the genetic algorithms
/// 
/// </summary>
public class AIController : MonoBehaviour {

    public delegate void Notify();

    public event Notify EndOfTheGeneration;
    //public event Notify StartOfTheNewGeneration;


    SimulationController simulationController;

    public GameObject carPrefab;//prefab of the vehicle
    //list of all vehicle in the population
    public static List<GameObject> cars = new List<GameObject>();
    //list of all the neural networks of the population
    public List<NeuralNetwork> networks;
    public int[] layers = new int[3] { 3, 3, 2 };//the size of the neural networks, number of the layers and number of neurons in each layer
    //public int[] layers = new int[4] { 3, 4, 3, 2 };//the size of the neural networks, number of the layers and number of neurons in each layer
    public float MutationChance { get; set; } = 0.5f;//the mutation chance value
    public float MutationStrength { get; set; } = 0.2f;//the mutation strength value
    //creates population size
    public int PopulationSize { get; set; } = 50;
    
    private int alive = 0;//current number of not destroyed Vehicles in the simulation
    public int Alive { get { return alive; } set { alive = value; } }

    public int Generation { get; set; } = -1;
    public double AverageFitness { get; set; } = 0;
    public double MeanFitness { get; set; } = 0;
    public float BestFitness { get; set; } = 0;
    public float BestFitnessEver { get; set; } = 0;
    public int MaxTimeInSeconds { get; set; } = 50;

    public GameObject BestCar { get; set; }
    public List<float> FitnessList { get; set; } = new List<float>();
    public int MaxGeneration { get; set; } = 100;


    /// <summary>
    /// reset all the value of 
    /// the AIController in cause a new simulation started
    /// </summary>
    public void Reset()
    {
        if (cars != null)
        {
            for(int i = 0;i < cars.Count; i++)
            {
                Destroy(cars[i]);
            }
        }

        cars.Clear();
        FitnessList.Clear();
        BestCar = null;
        Generation = -1;
        AverageFitness = 0;
        MeanFitness = 0;
        BestFitness = 0;
        BestFitnessEver = 0;
    }


    /// <summary>
    /// The process begins with a set of 
    /// individuals which is called a Population.
    /// Each individual is a solution to the problem you want to solve.
    /// An individual is characterized by 
    /// a set of parameters(variables) known as Genes.
    /// </summary>
    public void InitPopulation()
    {
        alive = PopulationSize;
        if (cars.Count > 0)
        {
            //this sorts the networks and mutates them
            GenerateNewGeneration();
        }

        for (int i = 0; i < PopulationSize; i++)
        {
            if (cars.Count < PopulationSize)
            {
                GameObject g = 
                Instantiate(carPrefab, carPrefab.transform.position, carPrefab.transform.rotation);
                cars.Add(g);
            }

            cars[i].GetComponent<Brain>().Network = networks[i];
            cars[i].GetComponent<BodyRenderer>().SetColor(networks[i].GetHashCode());
            cars[i].GetComponent<Brain>().Surviving = true;
            cars[i].transform.position = carPrefab.transform.position;
            cars[i].transform.rotation = carPrefab.transform.rotation;
            cars[i].GetComponent<CarController>().RemoveAllForces();
            cars[i].GetComponent<Distance>().Reset();

            Timer.Reset();
        }

        Generation++;
    }
    

    /// <summary>
    /// calculate the average and the mean fitness of the population
    /// </summary>
    public void CalculateAvgMeanAndBestFitness()
    {
            AverageFitness = 0;
            BestFitness = 0;

            BestCar = AIController.cars[0];

            for (int i = 0; i < AIController.cars.Count; i++)
            {
                if (AIController.cars[i].GetComponent<Brain>().GetFitness() > BestCar.GetComponent<Brain>().GetFitness())
                {
                    BestCar = AIController.cars[i];
                }
                AverageFitness += AIController.cars[i].GetComponent<Brain>().GetFitness();
            }

            BestFitness = BestCar.GetComponent<Brain>().GetFitness();

            if (BestFitness > BestFitnessEver)
            {
                BestFitnessEver = BestFitness;
            }

            AverageFitness = AverageFitness / networks.Count;
            MeanFitness = networks.ElementAt<NeuralNetwork>(networks.Count / 2).Fitness;
    }

    /// <summary>
    /// generate the new population
    /// </summary>
    public void GenerateNewGeneration()
    {
        /*pause the simulation 
        in case of exceeding the max number of generations*/
        if (Generation >= MaxGeneration)
        {
            PauseTheSimulation();
        }

        networks.Sort();
        CalculateAvgMeanAndBestFitness();
        FitnessList.Add((float)AverageFitness);
        //invoke the EndOfTheGeneration event
        EndOfTheGeneration.Invoke();
        SelectCrossoverAndMutatePopulation();
    }

    /// <summary>
    /// This methode makes a copy of the best half of the neural network 
    /// and removes the other half
    /// The new half will be mutated 
    /// and a new generation will be created
    /// Mutation occurs to maintain diversity within the population
    /// and prevent premature convergence.
    /// </summary>
    public void SelectCrossoverAndMutatePopulation()
    {
        for (int i = 0; i < PopulationSize / 2; i++)
        {
        //make a deep copy of the neural network
        networks[i] = networks[i + cars.Count / 2]
        .Clone(new NeuralNetwork(layers));

        //crossover only on 5% of the population
        if (Random.Range(0, 99) >= 90)
        {
            networks[i] = networks[i]
            .Crossover(
            networks[Random.Range(PopulationSize / 2, PopulationSize)]
            );
        }

        //mutate the copy
        networks[i]. Mutate((MutationChance), MutationStrength);
        }
    }

    public void PauseTheSimulation()
    {

            simulationController.PauseSimulation();
            GameObject.Find("PauseButton").GetComponent<UnityEngine.UI.Button>().interactable = false;
            GameObject.Find("StopButton").GetComponent<UnityEngine.UI.Button>().interactable = true;
            GameObject.Find("PlayButton").GetComponent<UnityEngine.UI.Button>().interactable = true;

   }


   
    private void Start()
    {
        simulationController = GetComponent<SimulationController>();
    }


    /// <summary>
    /// MonoBehaviour.FixedUpdate has the frequency of the physics system
    /// it is called every fixed frame-rate frame.
    /// Compute Physics system calculations after FixedUpdate.
    /// 0.02 seconds (50 calls per second) is the default time between calls. 
    /// </summary>
    void FixedUpdate()
    {
        if (SimulationController.SimulationState != SimulationController.State.running)
            return;
        foreach (GameObject car in cars)
        {
            var carController = car.GetComponent<CarController>();
            var carBrain = car.GetComponent<Brain>();
            var carSensor = car.GetComponent<Sensors>();
           
         
           // float[] input = carBrain.input;

            if ((carController != null & carBrain != null) & carBrain.Surviving)
            {
                // input for the neural network from the sensors
                float[] output = carBrain.Network.FeedForward(carSensor.GetValues());
                carController.Control(output);

            }
        }

        bool allDead = true;
        foreach (GameObject b in cars)
        {
            if (b.GetComponent<Brain>().Surviving)
            {
                allDead = false;
            }
        }

        
            if(allDead || Timer.Seconds >= MaxTimeInSeconds)
            {

                InitPopulation();
                simulationController.SetRoad(simulationController.RoadIndex);
                allDead = false;
            }

        alive = 0;
        for(int i = 0; i < PopulationSize; i++)
        {
            if (cars[i].GetComponent<Brain>().Surviving) alive++;
        }
    }


   /// <summary>
   /// this methode initializes the neural networks of the population
   /// </summary>
    public void InitNetworks()
    {
        networks = new List<NeuralNetwork>();
        for (int i = 0; i < PopulationSize; i++)
        {
            NeuralNetwork net = new NeuralNetwork(layers);
            networks.Add(net);
        }
        
    }

}
