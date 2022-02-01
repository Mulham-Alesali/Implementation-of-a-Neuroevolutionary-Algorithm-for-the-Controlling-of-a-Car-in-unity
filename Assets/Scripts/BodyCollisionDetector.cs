using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyCollisionDetector : MonoBehaviour
{
 
    private void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponentInParent<Brain>().Surviving = false;
        //gameObject.GetComponentInParent<BodyRenderer>().Hide();
    }
}
