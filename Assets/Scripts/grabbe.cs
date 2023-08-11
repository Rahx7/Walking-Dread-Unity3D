using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
 
public class grabbe : MonoBehaviour
{
    [SerializeField]private float _rayLength;
    [SerializeField] private InputActionReference _InputActionGrab;
    [SerializeField] private float _forceGrabb; 
    [SerializeField] private float _force; 
    [SerializeField] private Rigidbody _grabbObject; 
    [SerializeField] private bool isGrab;
    [SerializeField] private GameObject gunSpawn;

    void Start()
    {
        _InputActionGrab.action.Enable();
        //_InputActionGrab.action.performed += OnGrabb;
        _InputActionGrab.action.performed += OnGrabb2;
    }

    
    // Update is called once per frame

     RaycastHit hit;
    void FixedUpdate()
    {
        if(_grabbObject){
            
           // _grabbObject.velocity = Vector3.zero;
           if  (Vector3.Distance(_grabbObject.position,transform.position) > 4f)
                {
                    Debug.Log("grabb obj");
                    _grabbObject.transform.GetComponent<Rigidbody>().useGravity = true;
                    _grabbObject.transform.parent = null;
                    _grabbObject = null;          
                    isGrab = false;
                }

                 _grabbObject.transform.position = gunSpawn.transform.position;
                 _grabbObject.velocity = Vector3.zero;

        }

        grabb2();

    }

    void Update()
    {
        
    }

    private void grabb2()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out hit, _rayLength))
        {
            if (hit.transform.CompareTag("grabb"))
            {
                hit.transform.GetComponent<Renderer>().material.color = Color.blue;
                //OnGrab();
                Debug.Log("CA MARCHE !");
            }else{

                
            }

        }
    }

    void OnDrawGizmos()
    { 
        Gizmos.color = Color.red; 
        Gizmos.DrawRay(transform.GetChild(0).position, transform.GetChild(0).forward * _rayLength);
       
    }


   
    private void OnGrabb2(InputAction.CallbackContext obj){
    //Debug.Log("CA MARCHE ! on grabb");
        
        RaycastHit hit;
        if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out hit, _rayLength))
        {

            if (hit.transform.CompareTag("grabb")|| (Vector3.Distance(_grabbObject.position,transform.position) < 4f))
            {
                if(!isGrab)
                {
                    _grabbObject = hit.rigidbody;
                    //Instantiate(_grabbObject,transform.GetChild(0).position,Quaternion.identity );
                    _grabbObject.velocity = (transform.GetChild(0).position - _grabbObject.transform.position)*_forceGrabb;
                
                    Debug.Log("CA MARCHE ! on grabb");
                    isGrab = true;
                    _grabbObject.transform.position = gunSpawn.transform.position;
                    //gunSpawn.transform.parent = transform; 
                    _grabbObject.velocity = Vector3.zero;
                     //_grabbObject.transform.TransformVector(transform.parent.forward);
                    _grabbObject.useGravity = false; 
                        
                    
                }
                else {

                    _grabbObject.useGravity = true;
                    //_grabbObject.transform.parent = null;
                    _grabbObject.AddForce(gunSpawn.transform.forward * _force);
                    
                    StartCoroutine(isNotArme(hit.rigidbody));
                     
                    isGrab = false;
                    _grabbObject = null;  
                }


            }

        }

    }

    IEnumerator isNotArme(Rigidbody rb){
        _grabbObject.transform.tag = "isArme";
        _grabbObject.transform.GetComponent<Renderer>().material.color = Color.red;
        yield return new WaitForSeconds(2f);
        rb.transform.tag = "grabb";
        rb.transform.GetComponent<Renderer>().material.color = Color.blue;

    }

    // void OnTriggerEnter(Collision other)
    // {

    //     Debug.Log("bhjcbhjcsbhjsc");
    //     if(other.collider.CompareTag("grabb")) {
    //     _grabbObject.gameObject.SetActive(false);
    //     Instantiate(_grabbObject,transform.GetChild(0).position + (transform.GetChild(0).forward*2),Quaternion.identity );
    //     _grabbObject.velocity = Vector3.zero;
    //     //Destroy(_grabbObject);

    //     }
    // }

      void OnCollisionEnter(Collision other)
    {

        Debug.Log("bhjcbhjcsbhjsc");
        if(other.gameObject.CompareTag("Zombi")) {
        Debug.Log("tttttttttttttttt");
        
        //_grabbObject.gameObject.SetActive(false);
        // Instantiate(_grabbObject,transform.GetChild(0).position + (transform.GetChild(0).forward*2),Quaternion.identity );
       // _grabbObject.velocity = Vector3.zero;
        //Destroy(_grabbObject);

        }
    }



    // private void grab()
    // {
    //     RaycastHit hit;

    //     if (Physics.Raycast(transform.GetChild(0).position, transform.GetChild(0).forward, out hit, _rayLength))
    //     {

    //         if (hit.transform.CompareTag("grabb"))
    //         {
    //             //hit.transform.GetComponent<Renderer>().material.color = Color.blue ;
    //             //OnGrab();
    //             _grabbObject = hit.rigidbody;
    //         // Deplace(hit.transform.gameObject);
    //             Debug.Log("CA MARCHE ! on grabb");
    //             isGrab = true;

    //         }
    //     }
    // }

    // private void Deplace(GameObject T){
    //     _grabbObject =T.GetComponent<Rigidbody>();
    //    //.AddForce((transform.GetChild(0).position - T.transform.position)*_forceGrabb, ForceMode.Impulse );
    // }

}
