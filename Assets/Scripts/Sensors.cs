using UnityEngine;


/// <summary>
/// Each car will have three sensors.
/// Each sensor will raycast the distance to the first object that could be track.
/// </summary>
public class Sensors : MonoBehaviour
{
    //starting Point of the sensor
    [SerializeField] private float frontSensorStartPoint = 2.4f;
    //the Length of the sensor
    [SerializeField] private float sensorLength = 10;
    //the horizontal distance between the sensor and the road
    [SerializeField] private float sensorHeight = 0.5f;
    //the distance between the left or right sensor and the middle sensor
    [SerializeField] private float sensorsDistance = 1; 
    //the angle between the sensors in grad
    [SerializeField] private float sensorsAngle = 30;

    // Update is called once per frame
    void Update()
    {
       ReadFrontSensor();
       ReadRightSensor();
       ReadLeftSensor();
    }

    /// <summary>
    /// The output of the sensor will be
    /// the max distance of it divided by the collision point distance from the car.
    /// </summary>
    /// <param name="angle">the degree of the sensor</param>
    /// <returns>a value from [0-1]. 
    /// If the sensor haven’t detected any object it will return 1.</returns>
    public float ReadSensor(float angle)
    {
        Vector3 pos;
        RaycastHit hit;

        pos = transform.position;
        pos += Quaternion
            .Euler(0f, angle, 0f) * transform.forward * frontSensorStartPoint;
        pos += (Vector3.up * sensorHeight);
        
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position + (Vector3.up * sensorHeight)
            , Quaternion.Euler(0f, angle, 0f) *  transform.forward
            , out hit
            , sensorLength
            , Physics.IgnoreRaycastLayer)
            )
        {       
            Debug.DrawRay(pos
                , Quaternion.Euler(0f, angle, 0f) 
                * transform.forward * sensorLength
                , Color.red);
            return (Vector3.Distance(pos, hit.point) 
                + frontSensorStartPoint) / sensorLength;
        }
            Debug.DrawRay(pos
                , Quaternion.Euler(0f, angle, 0f) 
                *  transform.forward * sensorLength
                , Color.blue);
            return 1;
        
    }

    /// <returns>a value from [0-1]. 
    /// If the sensor haven’t detected any object it will return 1.</returns>
    public float ReadFrontSensor(){
        return ReadSensor(1f);
    }

    /// <returns>a value from [0-1].
    /// If the sensor haven’t detected any object it will return 1.</returns>
    public float ReadRightSensor()
    {
        return ReadSensor(sensorsAngle);
    }

    /// <returns>a value from [0-1]. 
    /// If the sensor haven’t detected any object it will return 1.</returns>
    public float ReadLeftSensor()
    {
        return ReadSensor(sensorsAngle * -1);
    }

    /// <returns>the value of all the front Sensors</returns>
    public float[] GetValues()
    {
        return new float[] {
            ReadFrontSensor()
            , ReadRightSensor()
            , ReadLeftSensor() };
    }

}
