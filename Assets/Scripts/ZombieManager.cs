using System.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class ZombieManager : MonoBehaviour
{

    private NavMeshAgent _agent;

    public List<GameObject> tableauDestination2;
    [SerializeField] public GameObject _destination;
    [SerializeField] private List<GameObject> tableauDestination;
    [SerializeField] private AudioClip[] _audiosGrr;
    
    [SerializeField] private GameObject ragdollPrefab;
    [SerializeField] private GameObject _prefabCivil;

    [SerializeField] private GameManager gameManager;
    [SerializeField] private CanvasManager CanvasManager;
    [SerializeField] private EventsManager eventsManager;

    [SerializeField] private GameObject CanvasZombi;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject healthBar;

    
    public float vitesse;



    public int _valeurZombi {get; } = -10;

    private Animator _animator;
    private AudioSource _audioZombi;
    
    // Start ******************************
    void Start()
    {
       CanvasZombi = transform.Find("CanvasZombi").gameObject;
       //healthBar = CanvasZombi.transform.Find("HealthBar").gameObject;
       player = GameObject.Find("PlayerCapsule").gameObject;

        eventsManager = GameObject.Find("EventsManager").GetComponent<EventsManager>();
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _audioZombi = GetComponent<AudioSource>();

        int i = UnityEngine.Random.Range(0,_audiosGrr.Length);
        var monSon =_audiosGrr[i];
        //_animator.enabled = false;
        _audioZombi.clip = monSon;
       _audioZombi.Play();
    
        _animator.SetFloat("decallage", UnityEngine.Random.Range(0f, 1.0f));
        //_animator.SetFloat("vitesse", (float)vitesse);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //CanvasManager = GameObject.Find("CanvasManager").GetComponent<EventsManager>();

        //velocity****************
       // _agent.velocity = new Vector3(5f,5f,0);

        vitesse= 2f;
        _animator.speed *= vitesse;
       _agent.speed *= vitesse;
    }
    


   //*********************************  /////////////////////////////////////
    float dist = Mathf.Infinity;
    GameObject CivilDestination;
    bool dead;
    private void CalculCivilProche()
    {
        dist = Mathf.Infinity; 
        //Debug.Log("tableau " +tableauDestination.Count);



        tableauDestination = GameObject.FindGameObjectsWithTag("Civil").ToList();
        //if(tableauDestination)
        if(tableauDestination.Count == 0) {

            tableauDestination.Add(player);
            player.tag = "Civil";
            _destination = player;


        }else{

            if(tableauDestination.Count == 1){        
                
                dead = false;
                _destination = player;

            }

            foreach (GameObject civil in tableauDestination)
            {
                    float distCivil = Vector3.Distance(civil.transform.position,transform.position);
                    if(distCivil < dist){
                            dist = distCivil;
                            _destination = civil;
                    }
                //}
                
               
            }
        }
       
       // Debug.Log("civilllll" + _destination.name );

    }



    int random(){

        int f = UnityEngine.Random.Range(0,6);
        return f;
    }

    ///****************************
    void FixedUpdate()
    {
         
    }


    
    void fixCanvas(){

        CanvasZombi.transform.LookAt(Camera.main.transform);
        CanvasZombi.transform.RotateAround(transform.position, transform.up, 180f);

        
        float distPlayer = Vector3.Distance(transform.position, player.transform.position);
       float percent = 1f - (distPlayer/30f) - 0.2f;
       percent *= 1/distPlayer*distPlayer;
      // Debug.Log("fix camera" + percent );
       CanvasZombi.GetComponent<CanvasGroup>().alpha = percent;

        Debug.Log("fix sante"+ sante);
        // Debug.Log("fix santeini"+ santeIni);
        float percentSante = (float)sante/(float)santeIni;
        healthBar.transform.localScale = new Vector3(percentSante,1,1);
        
    }

    void Update()
    {
      //_agent.velocity = _agent.velocity *vitesse;
      //Debug.Log("velocity"+_agent.velocity );
     
       fixCanvas();
        CalculCivilProche();
        //CalculCivilProche();

        if (!_audioZombi.isPlaying && random() < 2)
        {
            _audioZombi.Play();
        }

        var dist = Vector3.Distance(transform.position, _destination.transform.position);

        if (dist >1.5f && !_agent.isStopped)
        {
            _agent.isStopped = false;
            _agent.SetDestination(_destination.transform.position);
            _animator.SetBool("isAttacking", false);
        }
        else if(!_agent.isStopped)
        {
            // attaque
           // _agent.isStopped = true;
            _agent.SetDestination(_destination.transform.position);
            _animator.SetBool("isAttacking", true);
        }
    }


    [SerializeField]private int sante;
    [SerializeField]private int santeIni;
    [SerializeField]private AudioClip cri;
    void OnCollisionEnter(Collision other)
    {

        if ( other.gameObject.CompareTag("Grabble")){ //other.gameObject.name =="Ballon(Clone)" ||
            sante--;
            if(sante == 1){ 

               // transform.GetChild(1).GetComponent<SkinnedMeshRenderer>().material.color = Color.red;
                _audioZombi.PlayOneShot(cri);

                }

            if(sante == 0){

                _agent.isStopped = true;
                _animator.SetBool("isDead", true);
                
              // Instantiate(ragdollPrefab, transform.position,transform.rotation);
                
                
               // Destroy(gameObject);
               // Destroy(ragdollPrefab.GetComponent<AudioSource>());

                //Debug.Log("points : "+ gameManager.getPoints());
                _audioZombi.Stop();
                gameObject.tag = "dead";
                Destroy(gameObject.GetComponent<AudioSource>());
                
                Destroy(gameObject.GetComponent<Rigidbody>());
                Destroy(gameObject.transform.Find("CanvasZombi").gameObject);

                //Destroy(gameObject.GetComponent<NavMeshAgent>());
                Destroy(gameObject.GetComponent<BoxCollider>());
                //Destroy(gameObject.GetComponent<ZombieManager>());
                eventsManager.zombiDead.Invoke();
            }

            
            eventsManager.ZombiTouche.Invoke();
            
        }

        if ( other.gameObject.CompareTag("isArme")){
           gameObject.GetComponent<Rigidbody>().isKinematic = false;
            gameObject.tag = "dead";
            _agent.isStopped = true;
            Destroy(gameObject.GetComponent<NavMeshAgent>());
            _animator.SetBool("isDead", true);
            Destroy(gameObject.GetComponent<AudioSource>());
            Destroy(gameObject.transform.Find("CanvasZombi").gameObject);
            //
            sante = 0;
            eventsManager.zombiDead.Invoke();
            StartCoroutine(rbDesable(gameObject));

        }
       
     }

     IEnumerator rbDesable(GameObject gm){

        yield return new WaitForSeconds(1f);
        gm.GetComponent<Rigidbody>().isKinematic = true;
        Destroy(gm.GetComponent<BoxCollider>());

     }
}
