using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickObject : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    public ButtonManager buttonManager;

    private void OnMouseDown()
    {
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;

        mousePoint.z = mZCoord;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        if (gameObject.name == "RESET")
        {
            buttonManager.RESETButton();
        }
        else if (gameObject.name == "ABORT")
        {
            buttonManager.ABORTButton();
        }
        else if (gameObject.name == "PATH")
        {
            buttonManager.PATHButton();
        }
        else if (gameObject.name == "HOME")
        {
            buttonManager.HOMEButton();
        }
    }
}
