using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowlySpinning : MonoBehaviour
{
    [SerializeField]
    bool counterclockwise;
    [SerializeField]
    public float spinfactor = 1;
    // Update is called once per frame
    void Awake()
    {
        InvokeRepeating("BigSpin", 0.0f,3.0f);
    }

    public void BigSpin(){
        transform.LeanRotate(transform.eulerAngles + new Vector3(0,0, spinfactor), 3.0f);
    }
}
