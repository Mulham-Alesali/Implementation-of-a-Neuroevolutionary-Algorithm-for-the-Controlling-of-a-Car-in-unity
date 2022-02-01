using UnityEngine;

public class Distance : MonoBehaviour
{
    //the total distance the vehicle moved
    private float totalDistance;
    //last position of the vehicle
    private Vector3 lastPosition;
    private float speed;
    private float speedPerSec;

    public float SpeedPerSec {
        get { return speedPerSec; }
    }

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
        totalDistance = 0;
    }

    void Update()
    {
        totalDistance += Vector3.Distance(lastPosition, transform.position);
        speedPerSec = Vector3.Distance(lastPosition, transform.position) 
         / Time.deltaTime;
        speed = Vector3.Distance(lastPosition, transform.position);
        lastPosition = transform.position;

        if (this.speedPerSec == 0)
        {
            GetComponent<Brain>().Surviving = false;
            GetComponent<BodyRenderer>().Hide();
        }
    }

    public void Reset()
    {
        totalDistance = 0;
    }

    public float GetDistance()
    {
        return this.totalDistance;
    }
    
}
