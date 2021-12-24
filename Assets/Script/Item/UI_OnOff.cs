using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_OnOff : MonoBehaviour
{
    [SerializeField]
    Canvas canvas;
    bool UIon = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
            UIon = !UIon;

        if (UIon)
        {
            canvas.enabled = true;
            Cursor.visible = true;
        }
        else
        {
            canvas.enabled = false;
            Cursor.visible = false;

        }
    }
    // Update is called once per frame
}
