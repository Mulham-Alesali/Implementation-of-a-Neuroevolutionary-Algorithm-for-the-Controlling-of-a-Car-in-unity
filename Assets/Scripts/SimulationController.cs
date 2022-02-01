using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// this script controls the simulation environment
/// 
/// </summary>
public class SimulationController : MonoBehaviour
{

    AIController aIController;

    //index of the road
    public int RoadIndex { get; set; } = 0;

    //this array contains all the roads in the simulation to choose between
    GameObject[] allRoads;



    float gameSpeed = 5; 
    /// <summary>
    /// the gamespeed property sets the speed of the simulation
    /// if GameSpeed = 0 the simulation will be paused
    /// if GameSpeed has a very high value there will be performace problems
    /// </summary>
    public float GameSpeed
    {
        get
        {
            return gameSpeed;
        }
        set
        {
            Time.timeScale = gameSpeed;//sets gamespeed, which will increase to speed up training
            gameSpeed = value;
        }
    }
    public void Update()
    {
        
        Time.timeScale = gameSpeed;//sets gamespeed, which will increase to speed up training
    }

    public static bool hideFence = false;
    //determine if the fence should be hidden or shown
    //the render component of the fence 
    public static bool HideFence
    {
        get { return HideFence; }
        set
        {
            hideFence = value;
            UpdateFenceRender();
        }
    }

    /// <summary>
    /// hide the fence gameobjects if the HideFence Property is true
    /// show the fence gameobjects if the HideFence Property is false
    /// </summary>
    static void UpdateFenceRender()
    {
        GameObject[] fences = GameObject.FindGameObjectsWithTag("fence");
        for (int i = 0; i < fences.Length; i++)
        {
            fences[i].GetComponent<MeshRenderer>().enabled = !hideFence;
        }
    }

    /// <summary>
    /// the status of the simulation
    /// running, stoped, or paused
    /// </summary>
    public enum State
    {
        running,
        stoped,
        paused
    }

    private static State simulationState = State.stoped;
    /// <summary>
    /// this property hold the state of the simulation
    /// </summary>
    public static State SimulationState {
        get { return simulationState; }
        set
        {
            simulationState = value;
        }
}

    
    
    void Start()
    {
        aIController = FindObjectOfType<AIController>(); //get the refernce of the AIController class
        allRoads = GameObject.FindGameObjectsWithTag("road"); //find the reference for all the RoadGameObjects
        SetRoad(0);//show only the road with the index 0
        Time.timeScale = GameSpeed;//sets gamespeed, which will increase to speed up training
        // StartSimulation();
    }

  
    /// <summary>
    /// starting the simulation
    /// </summary>
    public void StartSimulation()
    {
        
        aIController.Generation = -1;

        //fl.GetComponent<UILineRenderer>().points = new List<Vector2>();
        
         
        aIController.InitNetworks();
        //aIController.InitPopulation();
        SetRoad(RoadIndex);

        SimulationController.SimulationState = SimulationController.State.running;

    }


    private float oldGameSpeed;//hold the simulationspeed before the simulation paused
    
    /// <summary>
    /// pause the simulation
    /// </summary>
    public void PauseSimulation()
    {
        oldGameSpeed = GameSpeed;
        GameSpeed = 0;
        GameObject.Find("SpeedSlider").GetComponent<UnityEngine.UI.Slider>().interactable = false;
        SimulationController.SimulationState = SimulationController.State.paused;
    }

    /// <summary>
    /// resume the simulation
    /// </summary>
    public void ResumeSimulation()
    {
        GameSpeed = oldGameSpeed;
        SimulationController.SimulationState = SimulationController.State.running;
    }

    /// <summary>
    /// stop the simulation
    /// </summary>
    public void StopSimulation()
    {
        aIController.Generation = -1;
        //GameSpeed = 0;
        SimulationController.simulationState = State.stoped;
    }

    /// <summary>
    /// change the road
    /// </summary>
    /// <param name="roadIndex">the index of the road in the allRoads array</param>
    public void SetRoad(int roadIndex)
    {

       
        for (int i = 0; i < allRoads.Length; i++)
        {
            if (i == roadIndex)
                allRoads[i].SetActive(true);
            else
               allRoads[i].SetActive(false);
        }
        UpdateFenceRender();
    }


    static private bool hideDestroyedCars;//hold a boolean value, should the destroyed cars be hidden

    /// <summary>
    /// this property controls if the cars should be shown or not
    /// </summary>
    static public bool HideDestroyedCars
    {
        get
        {
            return hideDestroyedCars;
        }
        set
        {
            hideDestroyedCars = value;
            HideOrShowCars();
        }
    }

    /// <summary>
    /// vehicles will be shown or hide depends on the value of the HideDestroyedCars Property and if the vehicles are destoryed or not
    /// </summary>
    static private void HideOrShowCars()
    {
        if (hideDestroyedCars)
        {
            for (int i = 0; i < AIController.cars.Count; i++)
            {
                if (AIController.cars[i].GetComponent<Brain>().Surviving)
                {
                    AIController.cars[i].GetComponent<BodyRenderer>().Show();
                }
                else
                {
                    AIController.cars[i].GetComponent<BodyRenderer>().Hide();
                }
            }

        }
        else
        {
            for (int i = 0; i < AIController.cars.Count; i++)
                AIController.cars[i].GetComponent<BodyRenderer>().Show();
        }

    }



}
