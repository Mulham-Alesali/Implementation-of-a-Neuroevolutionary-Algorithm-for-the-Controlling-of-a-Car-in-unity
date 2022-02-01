using UnityEngine;

/// <summary>
/// this class represent the AI part of the Vehicle and contains the fitness function of the individuell
/// it has a reference for the neural network and 
/// </summary>
public class Brain : MonoBehaviour
{
    public NeuralNetwork Network { get; set; }//the neural network of the individuell
    private bool surviving = true;//false if the Vehicle is destroyed otherwise it is true
    public bool Surviving {
        get { return surviving; }
        set {
            surviving = value;
            if (SimulationController.HideDestroyedCars && value == false)
                GetComponent<BodyRenderer>().Hide();
            else
                GetComponent<BodyRenderer>().Show();
        }
    }

    private void Update()
    {
        CalculateFitness();
    }

    /// <summary>
    /// get the fitness of the neural network
    /// </summary>
    /// <returns>returns the fitness of the individuell</returns>
    public float GetFitness()
    {
        return Network.Fitness;
    }

    /// <summary>
    /// this is the fitness function of the genetic algorithm
    /// it calculate the fitness according to the distance the vehicle drive in the limited time
    /// </summary>
    public void CalculateFitness()
    {
        if(Network != null)
        Network.Fitness = gameObject.GetComponent<Distance>().GetDistance() 
                / GameObject.FindObjectOfType<AIController>().MaxTimeInSeconds;                                                               
    }

    /// <summary>
    /// this methode will be called if a collision with another object happen
    /// </summary>
    /// <param name="collision">the Collision component of the other object</param>
    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "fence" | collision.gameObject.name.Contains("MT_"))//test if the other object is a fence
        {
            Surviving = false;//destroy the vehicle
        } 
       
    }
   
}
