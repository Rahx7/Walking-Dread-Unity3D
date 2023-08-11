using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class difficulteManager : MonoBehaviour
{
    bool fade = false;
    float a = 0;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 0f;
        //iTween.FadeTo(gameObject, 0f,0.01f);
        //iTween.FadeTo(gameObject, 0f,0.8f);
    }



    // Update is called once per frame
    void Update()
    {
      if(!fade){
        a+= Time.deltaTime;
        gameObject.GetComponent<CanvasGroup>().alpha = a;

        if (a>1) {
            fade = true;
        }
      }
    }


}
