using UnityEngine;

/// <summary>
/// this script control the camera to chase 
/// the best fitness not destroyed vehicle
/// </summary>
public class CameraFollow : MonoBehaviour
{
 
    [SerializeField] private Vector3 offset = new Vector3(0f,3f,-6f);
    [SerializeField] private Transform target; //the target vehicle to follow
    [SerializeField] private float translateSpeed = 10;
    [SerializeField] private float rotationSpeed = 10;
    
    /// <summary>
    /// change the position of the followcamera 
    /// object each time the fittest vehicle move
    /// </summary>
    private void FixedUpdate()
    {
        
        if (AIController.cars == null 
            | AIController.cars.Count == 0) return;
        Transform newTarget = AIController.cars[0].transform;
        Brain bestBrain = newTarget.gameObject
            .GetComponent<Brain>();
        for(int i = 0; i < AIController.cars.Count; i++)
        {
            Brain b = AIController.cars[i].GetComponent<Brain>();
            if ((b.GetFitness() > bestBrain.GetFitness() & b.Surviving) 
                | !bestBrain.Surviving)
            {
                newTarget = AIController.cars[i].transform;
                bestBrain = AIController.cars[i].GetComponent<Brain>();
            }
        }
        this.target = newTarget;

        HandleTranslation();
        HandleRotation();
    }

    /// <summary>
    /// handling the rotation of the camera
    /// </summary>
    private void HandleRotation()
    {
        var targetPosition = target.TransformPoint(offset);
        transform.position = Vector3
        .Lerp(transform.position
        , targetPosition, translateSpeed * Time.deltaTime);
    }

    /// <summary>
    /// handling the translation of the camera
    /// </summary>
    private void HandleTranslation()
    {
        var direction = 
        target.position - transform.position;
        var rotation = Quaternion
            .LookRotation(direction, Vector3.up);
        transform.rotation = 
        Quaternion.Lerp(transform.rotation, rotation
        , rotationSpeed * Time.deltaTime);
    }
}
