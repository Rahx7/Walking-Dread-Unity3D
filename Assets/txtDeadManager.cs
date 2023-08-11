using System.Collections;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class txtDeadManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameObject.Find("PlayerCapsule").tag = "dead";
        iTween.ScaleTo(gameObject, Vector3.one,1f);
        //GameObject.Find("PlayerCapsule").SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        Destroy( GameObject.Find("PlayerCapsule"));
       // GameObject.Find("PlayerCapsule").GetComponent<FirstPersonController>().enabled = false;
       // GameObject.Find("PlayerCapsule").GetComponent<PlayerInput>().enabled = false;
       // GameObject.Find("PlayerCapsule").GetComponents<AudioSource>()[1].enabled = false;
       //GameObject.Find("PlayerCapsule").tag = "dead";


    }

    // Update is called once per frame
    float a = 0;
    void Update()
    {
        a+= Time.deltaTime;
        gameObject.GetComponent<CanvasGroup>().alpha = a;

    }
}
