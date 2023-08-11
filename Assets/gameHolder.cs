using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHolder : MonoBehaviour
{
    // Start is called before the first frame update
    public int _nbCivils;
    public int _nbZombis;
    void Start()
    {
        
    }

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void nbCivil(int _nbCivil){
        _nbCivils = _nbCivil;
    }

    
    public void nbZombi(int _nbZombi){
        _nbZombis = _nbZombi;
    }
}
