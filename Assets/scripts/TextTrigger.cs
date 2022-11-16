using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject text;
    public int textIndex;
    public Boolean triggered = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" & !triggered)
        {
            StartCoroutine(text.GetComponent<TextBox>().DisplayNext(textIndex));
            triggered = true;
        }
    }
}
