using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Example of setting up a Prefab for sound
/// *
/// *   GameObject sf[NameOfSound] !! for sound effects
/// *   GameObject bgm[NameOfSound] !! for bgms
/// *
/// </summary>

public class AudioManager : MonoBehaviour
{
    //Init game object prefabs for sound here:

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
    public GameObject sfRanged1Hit;
    public GameObject sfRanged2Hit;

    [Header("Miscellaneous sfx")]
    public GameObject sfMenuClick;

    [Header("Background Music")]
    public GameObject bgmMainMenu;
    public GameObject bgmGameLoop;
    public GameObject bgmSugarRush;


    //Later the sound settings menu will be using this script to adjust volume
    //Aswell as - Methods here for playing a sound. Once sounds are set up as prefabs and init'd here, let me know and I'll walk through setting up the audioplayer method.

    public void AudioPlayer(GameObject soundPrefab)
    {
        //Play sound here.
    }
}
