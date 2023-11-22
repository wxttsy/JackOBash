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
    public GameObject sfPlayerHurt1;
    public GameObject sfPlayerHurt2;
    public GameObject sfPlayerDeath;
    public GameObject sfPlayerFootsteps1;
    public GameObject sfPlayerFootsteps2;
    public GameObject sfPlayerFootsteps3;
    public GameObject sfPlayerFootsteps4;
    public GameObject sfPlayerDash;
    public GameObject sfPowerPickup;
    public GameObject sfCandyPickup;
    public GameObject sfSugarRushEntry;
    public GameObject sfSugarRushExit;

    [Header("Ability sfx")]
    public GameObject sfPartyPopper;
    public GameObject sfSpilledPunchLaunch;
    public GameObject sfSpilledPunchLand;
    public GameObject sfChatteringSkulls;

    [Header("Enemy sfx")]
    public GameObject sfPumpkinCrawlerMovement;
    public GameObject sfMeleeHurt1;
    public GameObject sfMeleeHurt2;
    public GameObject sfRangedHurt1;
    public GameObject sfRangedHurt2;
    public GameObject sfMeleeDeath1;
    public GameObject sfMeleeDeath2;
    public GameObject sfRangedDeath1;
    public GameObject sfRangedDeath2;
    public GameObject sfPumpkinCrawlerDeath;
    public GameObject sfRangedAttack;
    public GameObject sfMeleeAttack;

    [Header("Miscellaneous sfx")]
    public GameObject sfMenuClick;

    [Header("Background Music")]
    public GameObject bgmMainMenu;
    public GameObject bgmGameLoop;
    public GameObject bgmSugarRush;


    //Later the sound settings menu will be using this script to adjust volume
    //Aswell as - Methods here for playing a sound. Once sounds are set up as prefabs and init'd here, let me know and I'll walk through setting up the audioplayer method.

    public void PlayAudio(GameObject soundPrefab)
    {
        AudioSource audioSource = soundPrefab.GetComponent<AudioSource>();
        
            //Play sound here.
            audioSource.Play();
    }

    public void StopAudio(GameObject soundPrefab)
    {
        AudioSource audioSource = soundPrefab.GetComponent<AudioSource>();

        //Stop sound here.
        audioSource.Stop();
    }

    /*
    public void test()
    {
        //Get audio manager game object
        GameObject audioManagerObject = GameObject.FindWithTag("AudioManager");
        //Get audio manager script from the object
        AudioManager audioManager = audioManagerObject.GetComponent<AudioManager>();
        //Get the prefab you want to play
        audioManager.PlayAudio(audioManager.sfCandyPickup);
    }
   */
}
