using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickOnPicture : MonoBehaviour
{
    Ray2D ray;
    RaycastHit2D Hit;


    void FixedUpdate()
    {
        ClickedPictureTag();
    }



    public string ClickedPictureTag()
    { 
        if (FindClickedPicture() && Input.GetKey(KeyCode.Mouse0))
            return Hit.collider.tag;

        else
            return "NotFound";

    }


    RaycastHit2D FindClickedPicture()
    {
        Vector3 MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePosition.z = -0.5f;
        Hit = Physics2D.Raycast(MousePosition, -MousePosition + new Vector3(MousePosition.x, MousePosition.y, -2f));

        return Hit;
    }
}
