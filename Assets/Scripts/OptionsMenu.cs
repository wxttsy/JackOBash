using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject UIGroup;
<<<<<<< Updated upstream
   // public GameObject Menu2;
=======
   
    // public GameObject Menu2;
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Awake ()
    {
        UIGroup.SetActive (true);
        optionsMenu.SetActive (false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onClickOptions()
    {
        UIGroup.SetActive(false);
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        optionsMenu.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("onClickOptions CLicked");

    }

    public void BackButton()
    {
        UIGroup.SetActive(true);
<<<<<<< Updated upstream
=======
    
>>>>>>> Stashed changes
        optionsMenu.SetActive(false);
        Time.timeScale = 0f;
        Debug.Log("BACK CLICKED");
    }
}
