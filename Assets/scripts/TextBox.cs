using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Object = System.Object;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI text;
    public GameObject portrait;
    public GameObject[] abilityButtons;
    public Boolean displayNext;
    public Boolean awaiting;
    public Boolean awaitingImage;
    public Boolean fastforwardable = false;
    public GameObject[] images;
    public GameObject[] imagesToDisable;
    public int index = 0;
    public string imageIndex = "";
    public string[,] textList;
    public float defaultTextDelay = 0.05f;
    public float defaultTextPause = 0.2f;
    public float textDelay;
    public float textPause;
    void Start()
    {
        Hide();
        textDelay = defaultTextDelay;
        textPause = defaultTextPause;

        //To display an image, use the @ symbol with 2 numbers after it. eg: @11
        //the @ tells the script an image index is incoming and the numbers correspond to that index
        //You will need to drag this image into the appropriate index in the images[] list in the inspector

        //To have a longer pause use the # symbol
        //To indicate the end of a text cutscene, use the ; symbol

        textList = new string[,] {
    {"player", "blank"},
    {"player", "so, what did you #f#u#c#k# me for again?#.#.#."},
    {"player", "and, you need me to collect these @01         ? "}

    };
    }

    public void Hide()
    {
        foreach (var button in abilityButtons)
        {
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - 4, button.transform.position.z);
            foreach (GameObject image in imagesToDisable)
            {
                image.gameObject.SetActive(false);
            }
        }
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void Show()
    {
        transform.GetChild(0).gameObject.SetActive(true);
        foreach (var button in abilityButtons)
        {
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y + 4, button.transform.position.z);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown)
        {
            if (awaiting)
            {
                Hide();
            }
            if (fastforwardable)
            {
                textDelay = 0f;
                textPause = 0f;
            }
        }
        if (displayNext)
        {
            displayNext = false;
            StartCoroutine(DisplayNext());
        }
        
    }
    public IEnumerator DisplayNext()
    {
        awaiting = false;
        if (!transform.GetChild(0).gameObject.activeSelf)
        {
            Show();
        }

        text.text = "";
        fastforwardable = true;
        foreach (char c in textList[index, 1])
        {
            
            if (awaitingImage)
            {
                imageIndex += c.ToString();
                if (imageIndex.Length >= 2)
                {
                    int i = int.Parse(imageIndex) - 1;
                    images[i].gameObject.SetActive(true);
                    imagesToDisable.Append(images[i]);
                    awaitingImage = false;
                }
            }

            else if (c.ToString() == "@")
            {
                awaitingImage = true;
            } else if (c.ToString() == "#")
            {
                yield return new WaitForSeconds(textPause);
            } else
            {
                text.text += c.ToString();
            }
            if (c.ToString() != "" & c.ToString() != "@" & !awaitingImage)
            {
                yield return new WaitForSeconds(textDelay);
            }
            
        }
        index++;
        fastforwardable = false;
        awaiting = true;
        textDelay = defaultTextDelay;
        textPause = defaultTextPause;
    }
}


