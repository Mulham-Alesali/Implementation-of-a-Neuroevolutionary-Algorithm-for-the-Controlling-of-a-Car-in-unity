using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// this class accepts the input of the user and converts it to commands
/// </summary>
public class ApplicationController : MonoBehaviour
{
    AIController aIController;
    SimulationController simulationController;
    Camera[] allCameras;

    // Start is called before the first frame update
    private void Start()
    {
        aIController = GameObject.FindObjectOfType<AIController>();
        simulationController = GameObject.FindObjectOfType<SimulationController>();

        allCameras = Camera.allCameras;
        DisableAllCameras();
        allCameras[0].enabled = true;
        
        GameObject.Find("MutationChance").GetComponent<Slider>().value 
            = aIController.MutationChance;
        GameObject.Find("MutationStrength").GetComponent<Slider>().value 
            = aIController.MutationStrength;
        GameObject.Find("MaxTimeInSec").GetComponent<Slider>().value 
            = aIController.MaxTimeInSeconds;
        GameObject.Find("PopulationSize").GetComponent<Slider>().value 
            = aIController.PopulationSize;
        GameObject.Find("Max Generation").GetComponent<Slider>().value 
            = aIController.MaxGeneration;
        GameObject.Find("MaxGeneration").GetComponent<InputField>().text 
            = aIController.MaxGeneration + "";
       
    }

    /// <summary>
    /// deactive all cameras in the simulation
    /// </summary>
    private void DisableAllCameras()
    {
        for (int i = 0; i < allCameras.Length; i++)
        {
            allCameras[i].enabled = false;
        }
    }

    /// <summary>
    /// this methode will be called when the value of the speed slider changes
    /// </summary>
    /// <param name="speedSlider">the slider gameobject</param>
    public void OnSpeedChange(GameObject speedSlider)
    {
        float value = speedSlider.GetComponent<Slider>().value;
        simulationController.GameSpeed = value;
    }

    /// <summary>
    /// change the mutation strength of the genetic algorithm
    /// </summary>
    /// <param name="sender"></param>
    public void OnMutationStrengthChange(GameObject sender)
    {
        float value = 
            (float)System.Math.Round(sender.GetComponent<Slider>().value,2);
        aIController.MutationStrength = value;
        GameObject.Find("MutationStrengthInputField")
            .GetComponent<InputField>().text = value + "";
    }

    /// <summary>
    /// change the mutation chance of the genetic algorithm
    /// </summary>
    /// <param name="sender"></param>
    public void OnMutationChanceChange(GameObject sender)
    {
        float value = sender.GetComponent<Slider>().value;
        aIController.MutationChance = value;
        GameObject.Find("MutationChanceInputField")
            .GetComponent<InputField>().text = value + "";
    }


    public void OnMaxTimePerSecondChange(GameObject sender)
    {
        float value = sender.GetComponent<Slider>().value;
        aIController.MaxTimeInSeconds = (int)value;
        GameObject.Find("MaxTimeInSecInputField")
            .GetComponent<InputField>().text = (int)value + "";
    }

    public void OnCameraChange(GameObject sender)
    {
        float value = sender.GetComponent<Dropdown>().value;
        for(int i = 0; i < allCameras.Length; i++)
        {
            if(i == (int)value)
            {
                allCameras[i].enabled = true;
            }
            else
            {
                allCameras[i].enabled = false;
            }
        }
    }

    public void OnRoadChange(GameObject sender)
    {
        //get the selected index
        int menuIndex = sender.GetComponent<Dropdown>().value;
        simulationController.RoadIndex = menuIndex;

    }
    
    public void OnEpochMaxTimeChange(GameObject sender)
    {
        float value = sender.GetComponent<Slider>().value;
        aIController.MaxTimeInSeconds = (int)value;
    }

    public void OnStartSimulation(GameObject sender)
    {
        GameObject.Find("SpeedSlider").GetComponent<UnityEngine.UI.Slider>().interactable = true;

        sender.transform.parent.Find("PauseButton")
            .GetComponent<Button>().interactable = true;
        sender.transform.parent.Find("StopButton")
            .GetComponent<Button>().interactable = true;

        sender.GetComponent<Button>().interactable = false;
        if (SimulationController.SimulationState == SimulationController.State.stoped)
        {
            simulationController.StartSimulation();
        }
        else
        {
            simulationController.ResumeSimulation();
        }
       
        var fl = GameObject.Find("FitnessLine");
        fl.GetComponent<FitnessDrawing>().enabled = true;
        fl.GetComponent<UILineRenderer>().enabled = true;

        var afl = GameObject.Find("AverageFitnessLine");
        afl.GetComponent<AverageFitnessDrawing>().enabled = true;
        afl.GetComponent<UILineRenderer>().enabled = true;

        GameObject.Find("FollowCamera")
            .GetComponent<CameraFollow>().enabled = true;
        GameObject.Find("BarChart")
            .GetComponent<BarChartScript>().enabled = true;
        GameObject.Find("PopulationSize")
            .GetComponent<Slider>().interactable = false;
        FindObjectOfType<BarChartScript>().enabled = true;

        //SimulationController.SimulationState = SimulationController.State.running;
    }

    public void OnStopSimulation(GameObject sender)
    {
        sender.transform.parent.Find("PauseButton")
            .GetComponent<Button>().interactable = false;
        sender.transform.parent.Find("PlayButton")
            .GetComponent<Button>().interactable = true;

        sender.GetComponent<Button>().interactable = false;
        simulationController.StopSimulation();
        aIController.Reset();

        var fl = GameObject.Find("FitnessLine");
        fl.GetComponent<UILineRenderer>().ResetUILineRenderer();
        fl.GetComponent<FitnessDrawing>().enabled = false;
        fl.GetComponent<UILineRenderer>().enabled = false;

        var afl = GameObject.Find("AverageFitnessLine");
        afl.GetComponent<UILineRenderer>().ResetUILineRenderer();
        afl.GetComponent<AverageFitnessDrawing>().enabled = false;
        afl.GetComponent<UILineRenderer>().enabled = false;


        FindObjectOfType<BarChartScript>().Reset();
        FindObjectOfType<BarChartScript>().enabled = false;
        GameObject.Find("FollowCamera")
            .GetComponent<CameraFollow>().enabled = false;
        GameObject.Find("PopulationSize")
            .GetComponent<Slider>().interactable = true;
    }

    public void OnPauseSimulation(GameObject sender)
    {
        sender.transform.parent.Find("StopButton")
            .GetComponent<Button>().interactable = true;
        sender.transform.parent.Find("PlayButton")
            .GetComponent<Button>().interactable = true;

        sender.GetComponent<Button>().interactable = false;
        simulationController.PauseSimulation();
    }

    public void OnPopulationSizeChange(GameObject sender)
    {
        int value = (int)sender.GetComponent<Slider>().value;
        aIController.PopulationSize = value;
        GameObject.Find("PopulationSizeInputField")
            .GetComponent<InputField>().text = value + "";
    }

    public void OnHideDestroyedCarsChange(GameObject sender)
    {
        SimulationController.HideDestroyedCars 
            = sender.GetComponent<Toggle>().isOn;
    }

    public void OnHideFence(GameObject sender)
    {
        SimulationController.HideFence = 
            sender.GetComponent<Toggle>().isOn;
    }

    public void OnMaxGenerationChange(GameObject sender)
    {
        int value = (int)sender.GetComponent<Slider>().value;
        aIController.MaxGeneration = value;
        GameObject.Find("MaxGeneration")
            .GetComponent<InputField>().text = value + "";
    }

}
