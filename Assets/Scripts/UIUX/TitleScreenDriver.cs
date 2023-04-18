using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TitleScreenDriver : MonoBehaviour
{
    public TMP_Text versionText, pressStartText;
    public CanvasGroup titleCG, pressStartCG, starButtonsCG;
    public Transform theReallyBigStar;
    bool inhale, backableScreen;
    int screenNum;
    // Start is called before the first frame update
    void Start()
    {
        versionText.text = "ver. " + Application.version;
        MainToTitle();
    }
    void MainToTitle(){
        CrazySpin();
        screenNum = 0;
        backableScreen = true;
        inhale = true;
        titleCG.LeanAlpha(1.0f,1.5f);
        pressStartCG.alpha = 0.0f;
    }
    IEnumerator PressStartFlash(){
        if(inhale) pressStartText.text = "Press Start!";
        else pressStartText.text = "Press Z or Enter";
        pressStartCG.LeanAlpha(1.0f,1.0f);
        yield return new WaitForSeconds(6.0f);
        pressStartCG.LeanAlpha(0.0f,1.0f);
        yield return new WaitForSeconds(1.0f);
        inhale = !inhale;
    }

    void TitleToMain(){
        screenNum = 1;
        backableScreen = true;
        StopCoroutine(PressStartFlash());
        titleCG.LeanAlpha(0.0f, 1.5f);
        pressStartCG.alpha = 0.0f;
        starButtonsCG.LeanAlpha(1.0f, 1.5f);
        CrazySpin();
    }

    // Update is called once per frame
    void Update()
    {
        if(screenNum == 0){
            if(pressStartCG.alpha == 0.0f) StartCoroutine(PressStartFlash());
            if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Z)) TitleToMain();
        }

        if(Input.GetKeyDown(KeyCode.Escape) && backableScreen){ BackScreen();}
    }

    void CrazySpin(){
        theReallyBigStar.LeanRotateZ(theReallyBigStar.rotation.z + 480.0f, 5.0f).setEaseOutQuad();
    }

    void QuitAll(){
        //TODO - add confirm dialogue to quitting
        Debug.Log("Attempted quit.");
        Application.Quit(); 
    }

    void BackScreen(){
        switch(screenNum){
            case 2:
                //close overhead menu
                //open star menu
                break;
            case 1:
                starButtonsCG.LeanAlpha(0.0f, 1.5f);
                MainToTitle();
                break;
            case 0:
                QuitAll();
                return;
        }
        Debug.Log("Cool Gaming");
    }
}
