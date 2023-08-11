using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class logoAnim : MonoBehaviour
{
    public 
    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.color = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //iTween.MoveTo(gameObject, Vector3.up,2f);
    }

    public void transition1(){

        Vector3 position = gameObject.transform.position + new Vector3(0,120,0);
         iTween.MoveTo(gameObject, position,1f);

         Vector3 scale = new Vector3(0.8f,0.8f,0.8f);
         iTween.ScaleTo(gameObject, scale,1f);
         

    }
}
