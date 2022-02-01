using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// controlling the barchart
/// barchart will be used to illustrate the fitnesses distribution of the vehicle
/// </summary>
public class BarChartScript : MonoBehaviour
{

    

    public GameObject barPrefab;//the barPrefab is used to be copied
    public AIController aIController;//pointer for the aIController, it will be used to obtain information about the vehicles

    List<GameObject> bars = new List<GameObject>();//the list of the bars to be shown in the barchart
    IList<NeuralNetwork> neuralNetworks; //list of the neual networks, the hashcode of the neural network present the color of the bar
    float chartHeight;
    float maxValue = 100;

    // Start is called before the first frame update
    void Start()
    {

        chartHeight = Screen.height + GetComponent<RectTransform>().sizeDelta.y;;
        aIController = GameObject.Find("SimulationController").GetComponent<AIController>() as AIController;

        aIController.EndOfTheGeneration += OnGenerationDie;

    }

    /// <summary>
    /// initialize the bar objects
    /// </summary>
    /// <param name="numberOfBars">numbers of bars in the chart, it equals the number of the neural networks</param>
    public void Initialise(int numberOfBars)
    {
        foreach(GameObject g in bars)
        {
            Destroy(g);
        }

        for (int i = 0; i < numberOfBars; i++)
        {
            GameObject newBar = Instantiate(barPrefab);
            newBar.transform.SetParent(transform);
            bars.Add(newBar);
          
        }

    }
   
    
    /// <summary>
    /// update the values of the chart
    /// </summary>
    /// <param name="values">fitness values</param>
    /// <param name="colors">the colors of the bars</param>
    /// <param name="maxValue">max y_value</param>
    public void UpdateValues(float[] values, Color[] colors, float maxValue)
    {
        
        if (values.Length != colors.Length) throw new System.Exception("values.length should be the the same as colors.length");
        
        if (bars.Count != neuralNetworks.Count) Initialise(neuralNetworks.Count);

                for (int i = 0; i < values.Length; i++)
                {
                Transform bar = bars[i].transform.Find("bar");
                RectTransform rt = bar.GetComponent<RectTransform>();
                rt.sizeDelta = new Vector2(rt.sizeDelta.x, chartHeight * values[i] / maxValue);
                Image image = bar.GetComponent<Image>();
                image.color = colors[i];
                }
            
    }



    public void OnGenerationDie()
    {
        neuralNetworks = aIController.networks;

        Color[] colors = new Color[neuralNetworks.Count];
        float[] values = new float[neuralNetworks.Count];

        for (int i = 0; i < neuralNetworks.Count; i++)
        {
            colors[i] = ColorsManager.ConvertToColor(neuralNetworks[i].GetHashCode());
            values[i] = neuralNetworks[i].Fitness;
            if (values[i] > maxValue) maxValue = values[i];
        }
        Array.Sort(values, colors);//sort the bars according to the value
        UpdateValues(values, colors, maxValue);
    }


    /// <summary>
    /// reset every thing to start a new simulation
    /// </summary>
    public void Reset()
    {
        if(bars != null)
        foreach (GameObject g in bars)
        {
            Destroy(g);
        }
        bars.Clear();
    }
}
