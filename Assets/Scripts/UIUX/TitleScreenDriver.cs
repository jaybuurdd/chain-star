using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleScreenDriver : MonoBehaviour
{
    public TMP_Text versionText, pressStartText;
    public CanvasGroup titleCG, pressStartCG, starButtonsCG, infoStarCG;
    public Transform theReallyBigStar;
    public BlackScreen bs, openingTitle;
    bool inhale, backableScreen;
    int screenNum;
    // Start is called before the first frame update
    void Start()
    {
        versionText.text = "ver. " + Application.version;
        starButtonsCG.alpha = 0.0f;
        screenNum = -1;
        bs.Invoke("Toggle",3.0f);
        StartCoroutine(TitleReveal());
        MainToTitle();
    }

    IEnumerator TitleReveal(){
        openingTitle.GetComponent<CanvasGroup>().LeanAlpha(1.0f,1.0f);
        openingTitle.Invoke("Toggle",3.0f);
        yield return new WaitForSecondsRealtime(5.0f);
        screenNum = 0;
        Destroy(openingTitle.gameObject);//no longer needed
    }
    void MainToTitle(){
        StartCoroutine(StarFlyOff());
        CrazySpin();
        if(screenNum != -1) screenNum = 0;//if waiting for title to fade, then don't accept input or start flashing Press Start
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
        pressStartCG.LeanAlpha(0.0f, 0.5f);
        starButtonsCG.alpha = 1.0f;
        StartCoroutine(StarFlyIn());
        titleCG.LeanAlpha(0.0f, 0.5f);
        pressStartCG.alpha = 0.0f;
    }
    public void MainToText(bool credits){
        StartCoroutine(StarFlyOff());
        screenNum = 2;
        backableScreen = true;
        infoStarCG.transform.GetChild(0).gameObject.SetActive(credits);
        infoStarCG.transform.GetChild(1).gameObject.SetActive(!credits);
        infoStarCG.gameObject.LeanMoveLocalY(0,1.75f).setEaseOutQuart();
    }
    void TextToMain(){
        StartCoroutine(StarFlyIn());
        infoStarCG.gameObject.LeanMoveLocalY(-2000,1.75f).setEaseInQuart();
        screenNum = 1;
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
    IEnumerator StarSingleFlash(){
        starButtonsCG.LeanAlpha(0.5f,0.15f);
        yield return new WaitForSeconds(0.15f);
        starButtonsCG.LeanAlpha(1.0f,0.15f);
        yield return new WaitForSeconds(0.15f);
    }

    IEnumerator StarConfirmFlash(){     
        for(int i = 3; i > 0; i--) {
            StartCoroutine(StarSingleFlash());
            yield return new WaitForSeconds(0.3f);
        }
    }

    IEnumerator EnterGame(float animDelay){
        bs.Invoke("Toggle", animDelay);
        yield return new WaitForSeconds(animDelay + 1.5f);
        SceneManager.LoadScene("SampleScene");
    }


    public void NewGameButton(){
        backableScreen = false;//locked into animation
        StartCoroutine(StarConfirmFlash());
        StartCoroutine(EnterGame(0.9f));
    }
    public void GitButton(){
        Application.OpenURL("https://github.com/jaybuurdd/chain-star/tree/master");
    }
    public void QuitButton(){
        //TODO - add confirm dialogue to quitting
        QuitAll();
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
        Debug.Log("Attempted quit.");
        Application.Quit(); 
    }

    public void BackScreen(){
        switch(screenNum){
            case 2:
                TextToMain();
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
