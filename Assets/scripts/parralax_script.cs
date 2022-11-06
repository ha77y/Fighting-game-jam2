using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parralax_script : MonoBehaviour
{

    [SerializeField] private Transform cameraTransform;
    private float originalPos= 0;
    private float posDifference;
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        posDifference = cameraTransform.position.x - originalPos;
        
        
        cameraTransform.position =new Vector3(cameraTransform.position.x +1* Time.deltaTime ,cameraTransform.position.y,cameraTransform.position.z);

        gameObject.transform.position = new Vector3 (gameObject.transform.position.x -(1*Time.deltaTime/3) ,cameraTransform.position.y, gameObject.transform.position.z);
    }
}
