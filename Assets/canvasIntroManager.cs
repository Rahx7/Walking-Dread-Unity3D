using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class canvasIntroManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<CanvasGroup>().alpha = 1f;
    }

    public void startCoroutineGame(){

       StartCoroutine( startGame());
    }


    IEnumerator startGame(){
        for (float i = 1; i >= 0; i -= (Time.deltaTime*2))
        {
            gameObject.GetComponent<CanvasGroup>().alpha = i;
            yield return new WaitForSeconds(Time.deltaTime/2);
        }

        gameObject.GetComponent<CanvasGroup>().alpha = 0;
        SceneManager.LoadScene(1);

    }
}
