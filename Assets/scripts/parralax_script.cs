using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class parralax_script : MonoBehaviour
{

    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform PlayerTransform;
    private Vector3 lastFrame;
    private Vector3 thisFrame;
    private Vector3 frameDiff;




    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        thisFrame = PlayerTransform.position;
        frameDiff = thisFrame - lastFrame;
        lastFrame = thisFrame;
        
        if (transform.parent.GetComponent<CameraMovement>().centerOnPlayer)
        {
            cameraTransform.position = new Vector3(PlayerTransform.position.x, cameraTransform.position.y, cameraTransform.position.z);
            //gameObject.transform.position = new Vector3(gameObject.transform.position.x - (frameDiff.x / 4), gameObject.transform.position.y, gameObject.transform.position.z);
            gameObject.transform.position = new Vector3((PlayerTransform.position.x / 2), gameObject.transform.position.y, gameObject.transform.position.z);
        }
        

        
    }
}
