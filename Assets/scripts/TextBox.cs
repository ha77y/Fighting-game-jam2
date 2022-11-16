using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
using Object = System.Object;

public class TextBox : MonoBehaviour
{
    public TextMeshProUGUI text;
    public SpriteRenderer currentPortrait;
    public Sprite[] portraits;
    public GameObject[] objects;
    public GameObject player;
    public GameObject[] abilityButtons;
    public Boolean displayNext;
    public Boolean awaiting;
    public Boolean awaitingImage;
    public Boolean awaitingObject;
    public Boolean fastforwardable = false;
    public Boolean autoplay = false;
    public Boolean awaitingCamera;
    public Boolean awaitingPortrait;
    public Boolean keyDown;
    public Boolean ignoreInput;
    public Boolean italic = false;
    public Boolean bold = false;
    public Boolean shrunk = false;
    public Boolean incomingRaw = false;
    public Boolean awaitingObjectPan;
    public GameObject[] images;
    public GameObject[] imagesToDisable;
    public IEnumerator cameraInstruction;
    public int index = 0;
    public string imageIndex = "";
    public string portraitIndex = "";
    public string objectIndex = "";
    public string duration;
    public string delta;
    public string pauseDuration;
    public string movementType;
    public string[,] textList;
    public float defaultTextDelay = 0.03f;
    public float defaultTextPause = 0.2f;
    public float textDelay;
    public float textPause;
    void Start()
    {
        textList = new string[,] {
            {"blank;"},

            {"£$Sigh$£ Who are you and what do you want?\\#.#.#.;" },
            {"Oh? you have a job for me.¬Go on..¬£$This better pay well#.#.#.$£;"},
            {"The bandits in the area stole this @01           from you?;" },
            {"And, you need me to collect these @00         ?¬You'd better be paying ^extra^ for that.;" },
            {"Alright, lets get on with it then#.#.#.¬You can move me ^left or right^ by pressing the ^A or D^ keys."},
            
            {"&010.05Do you see this enemy over there?| He can shoot me with his railgun and it doesn't feel great.#.#. but thankfully I have a trick up my sleeve.;"},
            {"&000.05If you press E or Right Click then I will swing my sword infront of me and any incoming projectiles or lasers from the direcction im facing will be deflected towards your mouse position, don't let me down#.#. got it?;"},
            {">0200.05.During this, I will face towards your mouse rather than the direction you're moving. |_ This means I can walk away from an enemy while deflecting at them.# Neat, right? Try it out."}
        };
        Hide();
        textDelay = defaultTextDelay;
        textPause = defaultTextPause;
        StartCoroutine(DisplayNext(1));

        //To display an image, use the @ symbol with 2 numbers after it. eg: @11
        //You will need to drag this image into the appropriate index in the images[] list in the inspector

        //To change the current portrait use the : symbol followed by a 2 numbers after it. eg: :11
        //You will need to drag this image into the appropriate index in the portraits[] list in the inspector

        //To write any of the below symbols without their functionality, but a \\ in front of it

        //To have a longer pause use the # symbol

        //To pause until user input use the | symbol

        //To autoplay the next section, use the ; symbol at the end of the string

        //To pan & pan return the camera use the ~ symbol followed by a 3 digit number, a 3 digit decimal and a 1 digit number eg @1000.052.

        //To pan the camera in a direction use the > or < symbol followed by a 3 digit number, a 3 digit decimal and a . eg >0100.05.

        //To center the camera on the player again use the _ symbol (note: this will instantly jump the camera back to the player, to smoothly do this, use the < pan mentioned above)

        //To center the camera on a game object use the - symbol followed by 2 numbers. eg -11
        //You will need to drag this game object into the appropriate index in the objects[] list in the inspector

        //To pan the camera to a game object use the & symbol followed by 2 numbers equal to the index in objects[] and a 3 digit decimal
        //You will need to drag this game object into the appropriate index in the objects[] list in the inspector

        //To clear the current text use the ! symbol

        //To add a new line use the ¬ symbol

        //To embolden use the ^ symbol, to unembolden use the same symbol

        //To italicise use the $ symbol, to unitalicise use the same symbol

        //To shrink text use the £ symbol, to unshrink use the same symbol;


    }

    public void Hide()
    {
        foreach (var button in abilityButtons)
        {
            button.transform.position = new Vector3(button.transform.position.x, button.transform.position.y - 4, button.transform.position.z);
        }
        CleanImages();
        transform.GetChild(0).gameObject.SetActive(false);
    }

    public void CleanImages()
    {
        foreach (GameObject image in imagesToDisable)
        {
            image.gameObject.SetActive(false);
        }
        imagesToDisable = new GameObject[0];
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
            if (keyDown == false)
            {
                keyDown = true;
            }
            if (awaiting & !transform.parent.GetComponent<CameraMovement>().panning)
            {
                awaiting = false;
                if (autoplay)
                {
                    displayNext = true;
                }
                else
                {
                    Hide();
                    transform.parent.GetComponent<CameraMovement>().centerOnPlayer = true;
                    player.GetComponent<PlayerStats>().UnFreeze();
                }
            }
            if (fastforwardable)
            {
                if (ignoreInput)
                {
                    ignoreInput = false;
                } else
                {
                    textDelay = 0f;
                    textPause = 0f;
                }
            }
        }
        if (displayNext)
        {
            displayNext = false;
            transform.parent.GetComponent<CameraMovement>().centerOnPlayer = false;
            StartCoroutine(DisplayNext(index++));
            
        }
        
    }
    public IEnumerator DisplayNext(int Startindex)
    {
        index = Startindex;
        awaiting = false;
        autoplay = false;
        player.GetComponent<PlayerStats>().Freeze();
        if (!transform.GetChild(0).gameObject.activeSelf)
        {
            Show();
        }

        text.text = "";
        CleanImages();
        fastforwardable = true;
        foreach (char c in textList[index, 0])
        {
            if (incomingRaw)
            {
                text.text += c.ToString();
                incomingRaw = false;
                continue;
            }
            if (awaitingImage)
            {
                imageIndex += c.ToString();
                if (imageIndex.Length >= 2)
                {
                    int i = int.Parse(imageIndex);
                    images[i].gameObject.SetActive(true);
                    imagesToDisable = imagesToDisable.Concat(new GameObject[] { images[i] }).ToArray();
                    imageIndex = "";
                    awaitingImage = false;
                }
            } else if (awaitingPortrait)
            {
                portraitIndex += c.ToString();
                if (portraitIndex.Length >= 2)
                {
                    int i = int.Parse(portraitIndex);
                    currentPortrait.sprite = portraits[i];
                    portraitIndex = "";
                    awaitingPortrait = false;
                }
            } else if (awaitingObject)
            {
                objectIndex += c.ToString();
                if (objectIndex.Length >= 2)
                {
                    int i = int.Parse(objectIndex);
                    transform.gameObject.GetComponent<CameraMovement>().CenterOn(objects[i]);
                    objectIndex = "";
                    awaitingObject = false;
                }
            } else if (awaitingObjectPan)
            {
                
                if (objectIndex.Length >= 2)
                {
                    delta += c.ToString();
                    if (delta.Length > 3)
                    {
                        int i = int.Parse(objectIndex);
                        print(i);
                        print(delta);
                        print(objects[i].transform.position.x);
                        StartCoroutine(transform.parent.GetComponent<CameraMovement>().PanTo(objects[i], float.Parse(delta)));
                        objectIndex = "";
                        delta = "";
                        awaitingObjectPan = false;

                    }
                } else
                {
                    objectIndex += c.ToString();
                }
            }
            else if (awaitingCamera) {
                if (duration.Length > 2)
                {
                    if (delta.Length > 3)
                    {
                        pauseDuration += c.ToString();
                        if (movementType == "PanReturn") {
                            StartCoroutine(transform.parent.GetComponent<CameraMovement>().PanReturn(float.Parse(duration, CultureInfo.InvariantCulture), float.Parse(delta, CultureInfo.InvariantCulture), float.Parse(pauseDuration, CultureInfo.InvariantCulture)));
                        } else if (movementType == "PanRight")
                        {
                            
                            StartCoroutine(transform.parent.GetComponent<CameraMovement>().Pan(float.Parse(duration, CultureInfo.InvariantCulture), float.Parse(delta, CultureInfo.InvariantCulture)));
                        } else if (movementType == "PanLeft")
                        {
                            StartCoroutine(transform.parent.GetComponent<CameraMovement>().Pan(-float.Parse(duration, CultureInfo.InvariantCulture), -float.Parse(delta, CultureInfo.InvariantCulture)));
                        }
                        awaitingCamera = false;
                        duration = "";
                        delta = "";
                        pauseDuration = "";
                    } else
                    {
                        delta += c.ToString();
                    }


                } else
                {
                    duration += c.ToString();
                }
                            
                
            }

            else if (c.ToString() == "~")
            {
                awaitingCamera = true;
                movementType = "PanReturn";
            } else if (c.ToString() == ">")
            {
                awaitingCamera = true;
                movementType = "PanRight";
            } else if (c.ToString() == "<")
            {
                awaitingCamera = true;
                movementType = "PanLeft";
            }
            else if (c.ToString() == "@")
            {
                awaitingImage = true;
            }
            else if (c.ToString() == "-") {
                awaitingObject = true;
            }
            else if (c.ToString() == "#")
            {
                yield return new WaitForSeconds(textPause);
            }
            else if (c.ToString() == ";"){
                autoplay = true;
            } else if (c.ToString() == "|"){
                keyDown = false;
                ignoreInput = true;
                textDelay = defaultTextDelay;
                textPause = defaultTextPause;
                yield return new WaitUntil(() => keyDown);

            } else if (c.ToString() == "!")
            {
                text.text = "";
            } else if (c.ToString() == ":")
            {
                awaitingPortrait = true;
            } else if (c.ToString() == "_")
            {
                transform.parent.GetComponent<CameraMovement>().Recenter();
            } else if (c.ToString() == "¬")
            {
                text.text += "<br>";
            } else if (c.ToString() == "^")
            {
                if (bold)
                {
                    text.text += "</b>";
                    bold = false;
                } else
                {
                    text.text += "<b>";
                    bold = true;
                }
            } else if (c.ToString() == "$")
            {
                if (italic)
                {
                    text.text += "</i>";
                    italic = false;
                } else
                {
                    text.text += "<i>";
                    italic = true;
                }
            } else if (c.ToString() == "£")
            {
                if (shrunk)
                {
                    text.text += "<size=100%>";
                    shrunk = false;
                } else
                {
                    text.text += "<size=70%>";
                    shrunk = true;
                }
            } else if (c.ToString() == "\\")
            {
                incomingRaw = true;
            } else if (c.ToString() == "&")
            {
                awaitingObjectPan = true;
            } 
            else
            {
                text.text += c.ToString();
            }
            if (c.ToString() != " " & c.ToString() != "@" & !awaitingImage)
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


