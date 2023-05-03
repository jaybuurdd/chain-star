using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackScreen : MonoBehaviour
{
    float howFar = 800.0f;
    [SerializeField]
    bool isOpen;
    [SerializeField]
    Transform tophalf, bottomhalf;
    // Start is called before the first frame update
    void Awake()
    {
        CutToState(isOpen);
    }

    public void Toggle(){
        Debug.Log("Bepis");
        if(isOpen) StartCoroutine(Close());
        else StartCoroutine(Open());
    }

    IEnumerator Close(){
        isOpen = false;
        tophalf.LeanMoveLocalY(0.0f,1.5f).setEaseOutQuad();
        bottomhalf.LeanMoveLocalY(0.0f,1.5f).setEaseOutQuad();
        yield return new WaitForSeconds(1.0f);
    }
    IEnumerator Open(){
        isOpen = true;
        tophalf.LeanMoveLocalY(howFar,1.5f).setEaseInQuad();
        bottomhalf.LeanMoveLocalY(-howFar,1.5f).setEaseInQuad();
        yield return new WaitForSeconds(1.5f);
    }

    // Update is called once per frame
    public void CutToState(bool open) {
        if(open){
            isOpen = true;
            tophalf.localPosition = new Vector3(0, howFar,0);
            bottomhalf.localPosition = new Vector3(0,-howFar,0);
        }
        else{
            isOpen = false;
            tophalf.localPosition = Vector3.zero;
            bottomhalf.localPosition = Vector3.zero;
        }
    }
}
