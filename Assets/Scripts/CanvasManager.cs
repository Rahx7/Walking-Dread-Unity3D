using System.Drawing;
using System.Net.Mime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{

    [SerializeField] private EventsManager eventsManager;

    [SerializeField] private TextMeshProUGUI textSante;
    [SerializeField] private TextMeshProUGUI textInfos;
    [SerializeField] private TextMeshProUGUI textZombiDead;
    [SerializeField] private TextMeshProUGUI nbCivilEscape;
    [SerializeField] private TextMeshProUGUI CivilsDead;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI win;


    [SerializeField] private GameObject ibarreZombis;
    [SerializeField] private GameObject ibarreCivils;
    [SerializeField] private GameObject ibarreSante;
    [SerializeField] private GameObject ibarreCivilsDead;

    private Sprite monSprite;
    private GameObject iCible;
    private Image iEcranPlayer;

    private int nbZombis;
    private int nbCivils;


    void Start()
    {
      // Debug.Log("barre SANTE name"+ gameObject.transform.Find("barreSante").name);
       //Debug.Log("barre SANTE name"+ gameObject.transform.Find("barreSante").transform);

        // BARRE D'INDICATION ***************
        ibarreCivils = GameObject.Find("barreCivil");
        ibarreZombis = GameObject.Find("barreZombi");
        ibarreSante = GameObject.Find("barreSante");
        ibarreCivilsDead = GameObject.Find("barreCivilsDead");
        ibarreCivils.transform.localScale = new Vector3(0 ,0,0) ;
        ibarreZombis.transform.localScale = new Vector3(0 ,0,0) ;
        ibarreCivilsDead.transform.localScale = new Vector3(0 ,0,0) ;

        ibarreSante.transform.localScale = new Vector3(3.80f,1,1) ;
        iCible = GameObject.Find("cible");
        iEcranPlayer = GameObject.Find("ecranNearDead").GetComponent<Image>();
        iEcranPlayer.color = new UnityEngine.Color(255,0,0,0);

        //************************* LES EVENTS

        eventsManager = GameObject.Find("EventsManager").GetComponent<EventsManager>();
        eventsManager.PlayerEvent.AddListener(ModifBarreSante);
        eventsManager.civilDead.AddListener(civilDead);
        eventsManager.civilDeadPlayer.AddListener(civilDeadPlayer);
        eventsManager.civilTouchePlayer.AddListener(civilTouchePlayer);
        eventsManager.ZombiTouche.AddListener(ZombiTouche);
        eventsManager.zombiDead.AddListener(zombiDead);
        eventsManager.civilEscape.AddListener(CivilsEscape);
        eventsManager.score.AddListener(Score);
        eventsManager.playerDead.AddListener(PlayerDead);
        eventsManager.playerNearDead.AddListener(PlayerNearDead);

        nbCivils = GameObject.Find("EventsManager").GetComponent<EventsManager>()._nombreCivilIni;
        nbZombis = GameObject.Find("EventsManager").GetComponent<EventsManager>()._nombreDeZombiesIni;

        nbCivilEscape.text = "Civils sauvés : " + eventsManager._civilsEscape +"/"+ nbCivils;
        textZombiDead.text = "Zombis tués : " + eventsManager._ZombisDead + "/"+ nbZombis;
        textSante.text = "Etat de santé : " + eventsManager._santePlayer + "/"+ eventsManager._santePlayerInit;
        textInfos.text += "INFOS : "; 


    }

    void PlayerDead(){
        textInfos.text += "vous êtes mort "; 

    }
    void Awake()
    {
        StartCoroutine(fadeIntro());
    }

    IEnumerator fadeIntro(){

        for (float i = 0; i < 1; i+=Time.deltaTime)
        {
            gameObject.GetComponent<CanvasGroup>().alpha = i;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        gameObject.GetComponent<CanvasGroup>().alpha = 1;
    }

    UnityEngine.Color color2;
    bool coroutineFade; 
    IEnumerator FadeIn(Image im)
    {
        coroutineFade = true;
        color2 = new UnityEngine.Color(255,0,0,0);
        // loop over 1 second
        for (float i = 0; i <= 0.03 ; i += (Time.deltaTime/20))
        {
            
            color2.a = i;
            im.color = color2;
            yield return null;

        }
    }

    IEnumerator FadeOut(Image im)
    {
        
        color2 = new UnityEngine.Color(255,0,0,0.03f);
        
        // loop over 1 second backwards
        for (float i = 0.03f; i >= 0; i -= (Time.deltaTime/20))
        {
            // set color with i as alpha
            color2.a = i;
            im.color = color2;
            yield return null;
        }
        color2 = new UnityEngine.Color(255,0,0,0);
        coroutineFade = false;
        //FadeInAndOut();

       
    }


    void PlayerNearDead(){
        string str = "vous allez mourir ";
        textInfos.text.Insert(0, str);
        if(!coroutineFade){
        StartCoroutine(FadeIn(iEcranPlayer));
        }
        
      // iTween.ColorTo(iEcranPlayer.gameObject, iTween.Hash("color", UnityEngine.Color.blue ,"time", 2.0f ,"loopType",iTween.LoopType.pingPong )); 
       iTween.ColorTo(GameObject.Find("ecranNearDead"), UnityEngine.Color.blue, 1f);   
         // , "easetype",iTween.EaseType.easeInExpo,"oncomplete", "afterPlayerMove",  //   "oncompleteparams", iTween.Hash("value", _fieldIndex)

    }

    void Score(){
        score.text = "SCORE : " + eventsManager._points; 
        win.text = "Your Score is : " + eventsManager._points;
    }


    float percentSante;

IEnumerator uneSecBarreSante(){
        yield return new WaitForSeconds(.02f);
        textSante.text = "Etat de santé : " + eventsManager._santePlayer + "/"+ eventsManager._santePlayerInit;
        float sante = eventsManager._santePlayer;
        float santeIni = eventsManager._santePlayerInit;
       percentSante = (sante/santeIni);
       percentSante = percentSante*3.80f;
        ibarreSante.transform.localScale = new Vector3(percentSante,1,1);

        if(eventsManager._santePlayer>=50 && coroutineFade){
            StartCoroutine(FadeOut(iEcranPlayer));
            }

    } 

    void ModifBarreSante(mesArgsPlayer e) {
        ////Debug.Log("avant 3sec ");
        StartCoroutine(uneSecBarreSante());

        Debug.Log("percent player ! : " + percentSante);
    }


IEnumerator uneSecCivil(){
        yield return new WaitForSeconds(.01f);
        CivilsDead.text = "Civils tués : "+ eventsManager._civilsDead;
        float percentCivilsDead = (float)eventsManager._civilsDead/(float)nbCivils;
        percentCivilsDead = percentCivilsDead*3.80f;
        ibarreCivilsDead.transform.localScale = new Vector3((float)percentCivilsDead ,1,1) ;
    } 
    void civilDead(mesArgsCivil args){
        string str = "un civil est mort !!\nnooon ! c'etait " + args.gameObj.name + "\n";
        textInfos.text = textInfos.text.Insert(0, str);

        //iCible.GetComponent<Image>().color = UnityEngine.Color.green;
        StartCoroutine(uneSecCivil());

       //StartCoroutine(reloadCible());
    }

    void civilDeadPlayer(mesArgsCivil args){

        string str = "Vous avez tué un civil !\nc'etait " + args.gameObj.name + " ! mais on s'en balek !\n";
        textInfos.text = textInfos.text.Insert(0, str);
        //iCible.GetComponent<Image>().color = UnityEngine.Color.green;

         StartCoroutine(uneSecCivil());

        //StartCoroutine(reloadCible());

    }

    void civilTouchePlayer(){

        string str = "Vous tirez sur un civil !\nfaites attention !\n";
        textInfos.text = textInfos.text.Insert(0, str);

        iCible.GetComponent<Image>().color = UnityEngine.Color.green;

        StartCoroutine(reloadCible());

    }


    IEnumerator CivilsEscapeCoroutine(){
        yield return new WaitForSeconds(.01f);
        nbCivilEscape.text = "Civils sauvés : " + eventsManager._civilsEscape +"/"+ nbCivils;

        float percentCivilsEscape = (float)eventsManager._civilsEscape/(float)nbCivils;
        percentCivilsEscape = percentCivilsEscape*3.80f;

       ibarreCivils.transform.localScale = new Vector3(percentCivilsEscape ,1,1) ;
        
        
        }

    void CivilsEscape(){
        StartCoroutine(CivilsEscapeCoroutine());
    }




    Image i;
    IEnumerator secZombi(){
        yield return new WaitForSeconds(.01f);

        // string str = "Vous avez tué un Zombi \n";
        // textInfos.text = textInfos.text.Insert(0, str);
        textZombiDead.text = "Zombis tués : " + eventsManager._ZombisDead + "/"+ nbZombis;

       float dead = eventsManager._ZombisDead ;
       float zomb = nbZombis ;

        float percentzombiDead = dead/zomb;
        percentzombiDead = percentzombiDead*3.80f;

       ibarreZombis.transform.localScale = new Vector3(percentzombiDead,1,1);
    }
    
    void zombiDead(){
        StartCoroutine(secZombi());
    }



    IEnumerator reloadCible(){
            yield return new WaitForSeconds(0.8f);
            iCible.GetComponent<Image>().color = UnityEngine.Color.white;
    }


    void ZombiTouche(){

        //string str = "Vous avez touché un zombi \n";
        //textInfos.text = textInfos.text.Insert(0, str);
       // Debug.Log("cible+ " + iCible.GetComponent<Image>());
        iCible.GetComponent<Image>().color = UnityEngine.Color.red;

       StartCoroutine(reloadCible());


    }

    // Update is called once per frame
    void Update()
    {

        

    }



}
