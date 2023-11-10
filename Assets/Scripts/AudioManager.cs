using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;

<<<<<<< Updated upstream
    public static AudioManager instance;
=======
    [Header("Player sfx")]
    public GameObject sfBatSwing;
    public GameObject sfBatSwingFinal;
    public GameObject sfBatHit;
    public GameObject sfPlayerHurt;
    public GameObject sfPlayerDeath;
    public GameObject sfPlayerFootsteps;
    public GameObject sfPlayerDash;
    public GameObject sfPowerPickup;
    public GameObject sfCandyPickup;
    public GameObject sfSugarRushEntry;
    public GameObject sfSugarRushExit;
    public GameObject sfComboUp;
    public GameObject sfComboDrop;

    [Header("Ability sfx")]
    public GameObject sfPartyPopper;
    public GameObject sfPartyBlowerLaunch;
    public GameObject sfPartyBlowerHit;
    public GameObject sfSpilledPunchLaunch;
    public GameObject sfSpilledPunchLand;
    public GameObject sfDancingSkulls;

    [Header("Enemy sfx")]
    public GameObject sfMelee1Growl;
    public GameObject sfMelee2Growl;
    public GameObject sfRanged1Growl;
    public GameObject sfRanged2Growl;
    public GameObject sfPumpkinCrawlerGrowl;
    public GameObject sfMelee1Hurt;
    public GameObject sfMelee2Hurt;
    public GameObject sfRanged1Hurt;
    public GameObject sfRanged2Hurt;
    public GameObject sfPumpkinCrawlerHurt;
    public GameObject sfMelee1Death;
    public GameObject sfMelee2Death;
    public GameObject sfRanged1Death;
    public GameObject sfRanged2Death;
    public GameObject sfPumpkinCrawlerDeath;

    [Header("Miscellaneous sfx")]
    public GameObject sfMenuClick;

    [Header("Background Music")]
    public GameObject bgmMainMenu;
    public GameObject bgmGameLoop;
    public GameObject bgmSugarRush;
>>>>>>> Stashed changes

    // Start is called before the first frame update
    void Awake()
    {
<<<<<<< Updated upstream
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach(Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

   public void Play (string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound: " + name + " not found");
            return;
        }
        s.source.Play();
=======
        // AudioSource source = soundPrefab.GetComponent<AudioSource>();
        //Play sound here.
        //AudioManager.AudioPlayer(sfHit);
>>>>>>> Stashed changes
    }
}
