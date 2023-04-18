using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlySpinning : MonoBehaviour
{
    [SerializeField]
    bool counterclockwise;
    [SerializeField]
    float spinfactor = 1;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward* (counterclockwise ? -1 : 1) * Time.deltaTime *2 * spinfactor);
    }

    public void BigSpin(){
        
    }
}
