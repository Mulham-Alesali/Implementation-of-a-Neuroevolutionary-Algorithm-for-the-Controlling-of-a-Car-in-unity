using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// drawing the fitness values on the chart
/// </summary>
public class FitnessDrawing : MonoBehaviour
{
    AIController aIController;
    UILineRenderer uILineRenderer;


   // int x = 0;
    List<Vector2> points;
    //int generation;
    // Start is called before the first frame update
    void Start()
    {
        uILineRenderer = gameObject.GetComponent<UILineRenderer>();
        aIController = FindObjectOfType<AIController>();
        uILineRenderer.points = new List<Vector2>();
        //generation = 0;
        aIController.EndOfTheGeneration += OnGenerationEnd;
    }


    void OnGenerationEnd()
    {
        UIGridRenderer grid = transform.parent.gameObject.GetComponent<UIGridRenderer>();
        if (grid.gridSize.x < aIController.FitnessList.Count)
            grid.gridSize.Set(grid.gridSize.x + 10, grid.gridSize.y);

        foreach (int i in aIController.FitnessList)
        {
            uILineRenderer.points.Add(new Vector2(aIController.Generation, aIController.BestFitness));

        }
    }
        // Update is called once per frame
        void Update()
    {

        //if (aIController.Generation > generation)
        //{


        //    generation++;

        //}

    }

    /// <summary>
    /// reset the values, this should be called when the simulation stops or a new simulation starts
    /// </summary>
    internal void Reset()
    {
        //generation = 0;
        if(points != null)
        points.Clear();
    }
}
