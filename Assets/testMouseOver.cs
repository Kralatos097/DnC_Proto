using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class testMouseOver : Selectable
{
    // Update is called once per frame
    void Update()
    {
        if (IsHighlighted())
        {
            Debug.Log("Highlighted");
        }
    }
}
