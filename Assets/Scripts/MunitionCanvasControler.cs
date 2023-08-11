using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MunitionCanvasControler : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject ballesPrefabCanvas;

    void Start()
    {
        // installBalleCanvas();
        //GameObject.Instantiate(ballesPrefabCanvas,new Vector3(12f, 0f,0f ),Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
      
    }
    Vector3 positionBalle = new Vector3(50f, 0f,0f );
    void installBalleCanvas(){
        int munition = 50;
        int munitionRestante = 20;

        float fl = ((float)munitionRestante/ (float)munition)/15;
        int nBalles = (int)fl;
        Debug.Log("instanciation BALLLES ");
     
            for (int i = 0; i < 15 ; i++)
            {
                //positionBalle = transform.parent.position;
                positionBalle.x = i*0.2f;
                Debug.Log("instanciation BALLLES "+positionBalle);
                GameObject GO = GameObject.Instantiate(ballesPrefabCanvas, Vector3.zero,Quaternion.identity);

                GO.transform.parent = gameObject.transform;
                GO.transform.localScale = Vector3.one;
                GO.transform.position = Vector3.zero;
                
            }
                //
            
            
        }

    }



