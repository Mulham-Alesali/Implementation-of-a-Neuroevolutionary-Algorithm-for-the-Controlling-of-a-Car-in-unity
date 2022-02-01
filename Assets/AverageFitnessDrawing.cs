using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AverageFitnessDrawing : MonoBehaviour
{
    AIController aIController;
    UILineRenderer uILineRenderer;


    // int x = 0;
    List<Vector2> points;

    // Start is called before the first frame update
    void Start()
    {
        uILineRenderer = gameObject.GetComponent<UILineRenderer>();
        aIController = FindObjectOfType<AIController>();
        uILineRenderer.points = new List<Vector2>();

        aIController.EndOfTheGeneration += OnGenerationEnd;
    }

    void OnGenerationEnd()
    {

            UIGridRenderer grid = transform.parent.gameObject.GetComponent<UIGridRenderer>();
            if (grid.gridSize.x < aIController.FitnessList.Count)
                grid.gridSize.Set(grid.gridSize.x + 10, grid.gridSize.y);
       // Debug.Log(aIController.FitnessList.Count);
            for (int i = 0; i < aIController.FitnessList.Count; i++)
            {
                uILineRenderer.points.Add(new Vector2(aIController.Generation, (float)aIController.AverageFitness));

            }



        
    }



    /// <summary>
    /// reset the values, this should be called when the simulation stops or a new simulation starts
    /// </summary>
    internal void Reset()
    {

        if (points != null)
            points.Clear();
    }
}
