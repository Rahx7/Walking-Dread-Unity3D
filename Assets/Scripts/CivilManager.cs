using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CivilManager : MonoBehaviour
{

    private NavMeshAgent _agent = new NavMeshAgent();

    
    [SerializeField] private Transform _destination ;
    [SerializeField] private AudioClip[] _audiosGrr;
    [SerializeField] private AudioClip _cri;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private EventsManager eventsManager;
     [SerializeField] private GameObject _prefabZombis;

    [SerializeField] private GameObject Canvas;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject healthBar;

   // [SerializeField] private GameObject ragdollPrefab;
    public bool _civilDead;
    private Animator _animator;
    private AudioSource _audioCivil;

   [SerializeField] private int sante= 2;
   [SerializeField] private int santeIni= 2;
    
    // Start is called before the first frame update
    void Start()
    {

        //Canvas = transform.Find("Canvas").gameObject;
       //healthBar = CanvasZombi.transform.Find("HealthBar").gameObject;
       player = GameObject.Find("PlayerCapsule").gameObject;

        eventsManager = GameObject.Find("EventsManager").GetComponent<EventsManager>();
        _destination = GameObject.Find("sortieCivil").transform;
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioCivil = GetComponent<AudioSource>();
        //_animator.enabled = false;

        int i = UnityEngine.Random.Range(0,_audiosGrr.Length);
        AudioClip monSon =_audiosGrr[i];
       _audioCivil.clip = monSon;

         
       
         

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    
        //var longuer = _animator.GetCurrentAnimatorClipInfo(0).;
        _animator.SetFloat("decallage",Random.Range(0f, 1.0f));

    }
    

    int random(){

        int f = Random.Range(0,6);
        return f;
    }

    void fixCanvas(){

        Canvas.transform.LookAt(Camera.main.transform);
        Canvas.transform.RotateAround(transform.position, transform.up, 180f);

        
        float distPlayer = Vector3.Distance(transform.position, player.transform.position);
       float percent = 1f - (distPlayer/30f) - 0.2f;
       percent *= 1/distPlayer*distPlayer;
      // Debug.Log("fix camera" + percent );
       Canvas.GetComponent<CanvasGroup>().alpha = percent;

        Debug.Log("fix sante"+ sante);
        // Debug.Log("fix santeini"+ santeIni);
        float percentSante = (float)sante/(float)santeIni;
        healthBar.transform.localScale = new Vector3(percentSante,1,1);
        
    }


    // ***********************************************************
    void Update()
    {
        fixCanvas();

        // direction aprendre selon les zombis
        
        //
        
     if(!_audioCivil.isPlaying  && random() < 2 && !_civilDead ) _audioCivil.Play();

        Vector3 directionP = lesZombis(); 

        var dist = Vector3.Distance(transform.position, _destination.position);


            if(!_civilDead){
                if (dist > 8f )
                {
                    _agent.isStopped = false;
                    _agent.SetDestination(directionP);
                    _animator.SetBool("isAttacked",true);
                }
                else if (dist > 4f )
                {
        // attaqué
                    _agent.isStopped = false;
                    _agent.SetDestination(directionP);
                    _animator.SetBool("isAttacked",false);
                }

                else {
                    _audioCivil.Stop();
                    _animator.SetBool("isStopped",true);
                    _agent.isStopped = true;
                    eventsManager.civilEscape.Invoke() ;
                    Debug.Log("escape : " + eventsManager._civilsEscape );
                    Destroy(gameObject);
                    
                }

            }
        
    }
    // int touche = 0 ;
    // void OnCollisionEnter(Collision other)
    // {
        
    //     Debug.Log("ttagg tagggg collider"+ other.collider.tag);
    //     Debug.Log("ttagg tagggg"+ other.gameObject.tag);

    //   if (other.gameObject.tag == "Zombi" || other.collider.tag == "colliderZombi"){ //other.gameObject.name =="Ballon(Clone)" ||
    //        // if(touche>1){ 
    //         Debug.Log("ok ok contact");
         
    //            // _agent.SetDestination(_agent.transform.position);
    //             _civilDead = true;
    //             gameObject.tag = "dead";
    //             gameObject.transform.tag = "dead";
    //            // _animator.SetBool("isAttacked",false);
                
    //             _animator.SetBool("isStopped",true);
    //             _agent.isStopped = true;
    //          // }

    //        // touche++;

    //   }
    // }


    void OnTriggerStay(Collider other)
    {
        //Debug.Log(" trigger declenché" + other.tag);

        if (other.gameObject.tag == "Zombi" || other.tag == "colliderZombi" || other.gameObject.tag == "ZombiAutre" || other.gameObject.CompareTag("Grabble")){ //other.gameObject.name =="Ballon(Clone)" ||

            if(!touche){
             StartCoroutine(toucheSec(other)) ;
            }

         }
     
    }

    bool touche = false;
    IEnumerator toucheSec(Collider other){
        touche = true;

            sante--;
               // _agent.SetDestination(_agent.transform.position);
               if(other.gameObject.CompareTag("Grabble")){

                    eventsManager.civilTouchePlayer.Invoke();
                    if(sante < 1){
                       // Debug.Log("t'as tue un civil"); 
                        die();
                        eventsManager.civilDeadPlayer.Invoke(new mesArgsCivil(gameObject));      
                        }

               }else if(sante < 1){

                    die();
                    eventsManager.civilDead.Invoke(new mesArgsCivil(gameObject));
                    
                }

        yield return new WaitForSeconds(0.5f);
        touche = false;
    }

    private void die()
    {

        _civilDead = true;
        gameObject.tag = "dead";
        Destroy(gameObject.transform.Find("Canvas").gameObject);
        GetComponent<BoxCollider>().enabled = false;
        _audioCivil.Stop();
        _animator.SetBool("isStopped", true);
        _agent.isStopped = true;
        _audioCivil.clip = _cri;
        _audioCivil.Play();

    }


    // Liste des zombis *********************
    Transform GoPosition;
    Transform newPos;
    Vector3 sommeDir;

    float distZom =200f;
    float distZombi;
    float limitDistZom = 3f;
Vector3 dirTemp ;
Vector3 dirTemp2 ;

    //detecter les zombis et choisir la destination a chaque update
    private Vector3 lesZombis(){
       
        
        List<GameObject> zombis;
        zombis = GameObject.FindGameObjectsWithTag("Zombi").ToList();
        zombis = GameObject.FindGameObjectsWithTag("ZombiAutre").ToList();
        foreach (var zombi in zombis)
        {         
            var dist = Vector3.Distance(transform.position, zombi.transform.position);
            var dir = (zombi.transform.position - transform.position).normalized;
            dir = dir*2/(dist*dist);
            sommeDir += dir;  

            
            if(dist < distZom){
                    distZom = dist;
                        //_destination = zombi;
                }

        }
        
       //Vector3 dirPoint = (_destination.position - transform.position).normalized;
       // 
        sommeDir = -sommeDir;
        Vector3 direction_destination = (_destination.position - transform.position).normalized*5;

        dirTemp =  (sommeDir*20)  + transform.position + direction_destination;
        dirTemp2 = _destination.position;
        Debug.DrawRay(transform.position, sommeDir*2, Color.red, 2f );
        //Debug.DrawRay(transform.position, dirTemp, Color.green, 0.2f );
        Debug.DrawRay(transform.position, direction_destination/9, Color.blue, 2f );
        //Debug.Log("zombi en vue ZOMBIS EN VU distance" + distZom );
        if(distZom<limitDistZom){ dirTemp2 = dirTemp; Debug.Log("zombi en vue ZOMBIS EN VU" ); }else{dirTemp2 = _destination.position;}
        //sommeDir
        distZom = 200f;
        return dirTemp2;
        //return newPos;000
    }


}
