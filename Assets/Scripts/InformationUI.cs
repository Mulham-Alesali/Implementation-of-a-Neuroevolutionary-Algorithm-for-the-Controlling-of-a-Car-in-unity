using UnityEngine;
using UnityEngine.UI;

public class InformationUI : MonoBehaviour
{
    const string AIGAMECONTROLLER = "SimulationController";
    AIController aIController;
    GameObject generation;
    GameObject populationSize;
    GameObject currentAlive;
    GameObject bestFitnessEver;
    GameObject meanFitnessLastGen;
    GameObject avgFitnessLastGen;
    GameObject roundTime;
       
    // Start is called before the first frame update
    void Start()
    {
        generation = GameObject.Find("GenerationInfo");
        populationSize = GameObject.Find("PopulationInfo");
        currentAlive = GameObject.Find("CurrentAliveInfo");
        bestFitnessEver = GameObject.Find("BestFitnessEverInfo");
        meanFitnessLastGen = GameObject.Find("MeanFitnessLastGenInfo");
        avgFitnessLastGen = GameObject.Find("AverageFitnessLastGenInfo");
        roundTime = GameObject.Find("RoundTime");
        aIController = GameObject.Find(AIGAMECONTROLLER).GetComponent<AIController>();
        

    }
    
    /// <summary>
    /// Update the information in the Information window
    /// </summary>
    public void UpdateInformation()
    {
        generation.GetComponent<Text>().text = "Generation: " + aIController.Generation;
        populationSize.GetComponent<Text>().text = "Population Size: " + aIController.PopulationSize;
        currentAlive.GetComponent<Text>().text = "Current Alive: " + aIController.Alive;
        bestFitnessEver.GetComponent<Text>().text = "Best Fitness Ever: " + System.Math.Round(aIController.BestFitnessEver,2);
        meanFitnessLastGen.GetComponent<Text>().text = "Mean Fitness: " + System.Math.Round(aIController.MeanFitness,2);
        avgFitnessLastGen.GetComponent<Text>().text = "Average Fitness: " + System.Math.Round(aIController.AverageFitness,2);
        roundTime.GetComponent<Text>().text = "Round Time: " + System.Math.Round(Timer.Time);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInformation();
    }
}
