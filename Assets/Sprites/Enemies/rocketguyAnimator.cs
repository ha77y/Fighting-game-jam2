using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rocketguyAnimator : MonoBehaviour
{
    // Start is called before the first frame update
    public RocketLauncher rlscript;
    public Enemy enemyscript;
    public Transform prefabParent;
    public Rigidbody2D rocket;
    public Transform firingPoint;
    void Start()
    {
        rlscript = prefabParent.GetComponent<RocketLauncher>();
        enemyscript = prefabParent.GetComponent<Enemy>();
        enemyscript.ammo = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Reload()
    {
        enemyscript.ammo = 1;
        rlscript.reloading = false;
        enemyscript.canWalk = true;
    }
    public void Fire()
    {
        enemyscript.ammo = 0;
        Rigidbody2D b = Instantiate(rocket, firingPoint.position, Quaternion.identity, prefabParent);
    }
    public void FireFinish()
    {
        rlscript.firing = false;
        enemyscript.canWalk = true;
    }
}
