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
        starButtonsCG.alpha = 0.0f;
        MainToTitle();
    }
    void MainToTitle(){
        StartCoroutine(StarFlyOff());
        CrazySpin();
        screenNum = 0;
        backableScreen = true;
        inhale = true;
        titleCG.LeanAlpha(1.0f,1.5f);
        pressStartCG.alpha = 0.0f;
        
    }
    void TitleToMain(){
        CrazySpin();
        screenNum = 1;
        backableScreen = true;
        StopCoroutine(PressStartFlash());
        starButtonsCG.alpha = 1.0f;
        StartCoroutine(StarFlyIn());
        titleCG.LeanAlpha(0.0f, 0.5f);
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
    IEnumerator StarFlyOff(){
        starButtonsCG.gameObject.LeanMoveLocalX(-3000,1.75f).setEaseInQuart();
        yield return new WaitForSecondsRealtime(2.0f);
    }
    IEnumerator StarFlyIn(){
        starButtonsCG.gameObject.LeanMoveLocalX(0,1.75f).setEaseOutElastic();
        yield return new WaitForSecondsRealtime(2.0f);
    }

    IEnumerator StarConfirmFlash(){
        
        for(int i = 3; i > 0; i--) {
            StartCoroutine(StarSingleFlash());
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator StarSingleFlash(){
        starButtonsCG.LeanAlpha(0.5f,0.15f);
        yield return new WaitForSeconds(0.15f);
        starButtonsCG.LeanAlpha(1.0f,0.15f);
        yield return new WaitForSeconds(0.15f);
    }

    public void NewGameButton(){
        StartCoroutine(StarConfirmFlash());
        //Invoke("DipToBlack", 2.0f);
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

    IEnumerator CrazySpin(){
        SlowlySpinning ss = theReallyBigStar.GetComponent<SlowlySpinning>();
        float f = ss.spinfactor;
        ss.spinfactor = 30.0f;
        yield return new WaitForSecondsRealtime(3.0f);
        ss.spinfactor = f;
    }

    public void QuitAll(){
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
