using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    // Start is called before the first frame update

    public EventSystem eventSystem;
    public GameObject continueButton;
    public GameObject splash1;
    public GameObject splash2;
    int splashIndex;
    void Start()
    {
        splashIndex = 0;
        splash1.SetActive(true);
        splash2.SetActive(false);
        eventSystem.SetSelectedGameObject(continueButton);
    }

    public void Next()
    {
        if (splashIndex == 0)
        {
            splash1.SetActive(false);
            splash2.SetActive(true);
            splashIndex = 1;
        }
        else if (splashIndex == 1)
        {
            SceneManager.LoadScene("DebugScene");
        }
    }

}
    
    
