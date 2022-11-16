using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Boolean centerOnPlayer = true;
    public Boolean panning = false;

    void Start()
    {
        
        //StartCoroutine(PanReturn(10f, 0.05f, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public IEnumerator PanReturn(float amount, float delta, float pauseDuration)
    {
        yield return StartCoroutine(PanCustomReturn(amount, delta, pauseDuration, amount, delta));
    }

    public IEnumerator PanCustomReturn(float amount, float delta, float pauseDuration, float secondAmount, float secondDelta)
    {
        centerOnPlayer = false;
        yield return StartCoroutine(Pan(amount, delta));
        yield return new WaitForSeconds(pauseDuration);
        yield return StartCoroutine(Pan(-secondAmount, -secondDelta));
        centerOnPlayer = true;
    }

public IEnumerator Pan(float amount, float delta) {
        panning = true;
        float i = 0;
        while ((amount > 0 & (i += delta) < amount) | (amount < 0 & (i += delta) > amount))
        {
            transform.position = new Vector3(transform.position.x + delta, transform.position.y, transform.position.z);
            yield return new WaitForSeconds(Time.deltaTime);
        }
        panning = false;
    }

public IEnumerator Recenter()
    {
        centerOnPlayer = true;
        yield return null;
        yield return null;
        centerOnPlayer = false;
    }
}
