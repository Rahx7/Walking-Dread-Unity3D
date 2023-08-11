using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventsManager : MonoBehaviour
{

    public UnityEvent<mesArgsPlayer> PlayerEvent;
    public UnityEvent playerDead;
    public UnityEvent playerNearDead;
    public UnityEvent _SantePlayer;
    public UnityEvent zombiDead;
    public UnityEvent ZombiTouche;
    public UnityEvent<mesArgsCivil> civilDead;
    public UnityEvent<mesArgsCivil> civilDeadPlayer;
    public UnityEvent civilTouchePlayer;
    public UnityEvent civilEscape;
    public UnityEvent Win;
    public UnityEvent score;

    
    [SerializeField]private gameHolder gameHolder;

    public int _points {get; set;}
    public int _ZombisDead {get; set;} = 0;
    public int _civilsEscape {get; set;}
    public int _civilsDead {get; set;}

    public int _santePlayerInit {get; set;} = 100;
    public int _santePlayer {get; set;} = 100;

    public int _nombreDeZombiesIni;
    public int _nombreCivilIni;




    //public HealthChangedEvent healthChanged;
    void Awake()
    {
        gameHolder = GameObject.Find("GameHolder").GetComponent<gameHolder>();
        _nombreDeZombiesIni = gameHolder._nbZombis;
        _nombreCivilIni = gameHolder._nbCivils;
    }

    void Start()
    {
        //_SantePlayer.AddListener(SantePlayer);
        _santePlayer = _santePlayerInit;
        zombiDead.AddListener(ZombiDead);
        civilDead.AddListener(CivilDead);
        PlayerEvent.AddListener(SantePlayer);
        civilEscape.AddListener(CivilEscape);
        civilDeadPlayer.AddListener(CivilDead);

        //playerNearDead.AddListener(PlayerNearDead);
    }


    GameObject leZombi;

    void SantePlayer(mesArgsPlayer args){
        Debug.Log(args);
        _santePlayer += args.degat;
    if(_santePlayer>_santePlayerInit){_santePlayer = _santePlayerInit;} 
        leZombi = args.gameObj;

    }

    void ZombiDead(){
        _ZombisDead++;
        _points += 10;
        score.Invoke();
    }

    public void nbInit(){
        Destroy( GameObject.Find("GameHolder"));
    }





    void CivilDead(mesArgsCivil args){
        _civilsDead++;
        _points -= 20;
        score.Invoke();
        if(_civilsEscape+_civilsDead==_nombreCivilIni){

            Debug.Log("WINNNNNNNNNNNNNNNNNNN");
            Win.Invoke();

        }
    }

    void CivilEscape(){

        _civilsEscape++;
        _points += 50;
        score.Invoke();
        if(_civilsEscape+_civilsDead==_nombreCivilIni){

            Debug.Log("WINNNNNNNNNNNNNNNNNNN");
            Win.Invoke();
        }
    }
}

// [System.Serializable]
// public class HealthChangedEvent : UnityEvent<EventArgs> {}
 
// public class EventArgs
// {
//     public object sender;
//     public int health;
 
//     public EventArgs (object sender, int health)
//     {
//         this.sender = (object)sender;
//         this.health = health;
//     }
// }

// [System.Serializable]
// public class ChangedEvent : UnityEvent<mesArgs> {}

// public class PlayerEvenArgs : EventArgs
// {

// }

public class mesArgsPlayer
{
    public GameObject gameObj {get;}
    public int degat {get;}

    public mesArgsPlayer(GameObject obj, int _degat){
        this.gameObj = (GameObject)obj;
        this.degat = _degat;
    }
}

public class mesArgsCivil
{
    public GameObject gameObj {get;}


    public mesArgsCivil(GameObject obj){
        this.gameObj = (GameObject)obj;       
    }
}