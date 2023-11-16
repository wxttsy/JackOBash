using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject UIGroup;
<<<<<<< Updated upstream:Assets/Scripts/OptionsMenu.cs
<<<<<<< Updated upstream
   // public GameObject Menu2;
=======
   
    // public GameObject Menu2;
>>>>>>> Stashed changes
=======
   // public GameObject Menu2;
>>>>>>> Stashed changes:Assets/Scripts/Ui/OptionsMenu.cs

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
<<<<<<< Updated upstream:Assets/Scripts/OptionsMenu.cs
<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes:Assets/Scripts/Ui/OptionsMenu.cs
        optionsMenu.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("onClickOptions CLicked");

    }

    public void BackButton()
    {
        UIGroup.SetActive(true);
<<<<<<< Updated upstream:Assets/Scripts/OptionsMenu.cs
<<<<<<< Updated upstream
=======
    
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes:Assets/Scripts/Ui/OptionsMenu.cs
        optionsMenu.SetActive(false);
        Time.timeScale = 0f;
        Debug.Log("BACK CLICKED");
    }
}
