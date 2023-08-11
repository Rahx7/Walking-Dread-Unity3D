using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerControl : MonoBehaviour
{
    [SerializeField] private EventsManager eventsManager;
    
    [SerializeField] private GameManager _GM;
    public AudioSource _respiration;
    public AudioSource _tir;
    public AudioClip[] _tirs;





    void Start()
    {
        eventsManager = GameObject.Find("EventsManager").GetComponent<EventsManager>();
        _GM = GameObject.Find("GameManager").GetComponent<GameManager>();

        
    }


    void Update()
    {
        
    }

    [SerializeField] private int degat;
    [SerializeField] private int santePlus = 50 ;
    
    void OnTriggerEnter(Collider other)
    {

        //Debug.Log("caollision" +other.tag);

        if(other.CompareTag("santePlus")){

            if(eventsManager._santePlayer>eventsManager._santePlayerInit) {
                eventsManager._santePlayer = eventsManager._santePlayerInit;
                }

            //mesArgsPlayer args = 
            eventsManager.PlayerEvent.Invoke(new mesArgsPlayer(other.gameObject, 50));
            other.gameObject.SetActive(false);
            StartCoroutine(reloadSante(other.gameObject));
        
        }

        
        
  

       // other = null;

    }

    // IEnumerator reloadArgs(mesArgsPlayer args){

        
    //     yield return new WaitForSeconds(0.2f);
    //     eventsManager.PlayerEvent.Invoke(args);

    // }
    bool bDelayZombi;
    void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Zombi")){

            degat = other.gameObject.GetComponent<ZombieManager>()._valeurZombi;
            if(!bDelayZombi){ StartCoroutine(DelayZombi());}
            

        }
        if(other.CompareTag("ZombiAutre")){

            degat = other.gameObject.GetComponent<DragonManager>()._valeurZombi;
            if(!bDelayZombi){ StartCoroutine(DelayZombi());}
            

        }
    }

    IEnumerator DelayZombi(){

        bDelayZombi = true;
        //mesArgsPlayer args = ;

        eventsManager.PlayerEvent.Invoke(new mesArgsPlayer(gameObject, degat));
        // StartCoroutine(reloadArgs(args));

        if(eventsManager._santePlayer < 2){
            eventsManager.playerDead.Invoke();
        }

        if(eventsManager._santePlayer <= 30){
            eventsManager.playerNearDead.Invoke();
        }
    //
        yield return new WaitForSeconds(0.5f);
        bDelayZombi = false;



    }


    IEnumerator reloadSante(GameObject obj){

        
        yield return new WaitForSeconds(10f);
        obj.SetActive(true);

    }
}


