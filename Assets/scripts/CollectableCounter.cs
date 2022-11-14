using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class CollectableCounter : MonoBehaviour
{
    public TextMeshProUGUI counter;
    public void UpdateCounter()
    {
        counter.SetText(Data.collectables + " / " + Data.MaxCollectables);
    }
}
