using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject Ui1;
    public GameObject Ui2;

    // Start is called before the first frame update
    void Awake ()
    {
        optionsMenu.SetActive (false);
    }


    public void onClickOptions()
    {
        Ui1.SetActive(false);
        Ui2.SetActive(false);
        optionsMenu.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("onClickOptions CLicked");

    }

    public void BackButton()
    {
        Ui1.SetActive(true);
        Ui2.SetActive(true);
        optionsMenu.SetActive(false);
        Time.timeScale = 0f;
        Debug.Log("BACK CLICKED");
    }
}
