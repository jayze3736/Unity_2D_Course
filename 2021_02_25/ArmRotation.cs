using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmRotation : MonoBehaviour
{
    public int rot_offset = 0;

    // Update is called once per frame
    void Update()
    {
        // calculating vector for difference between mouse position and object position
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        //Atan2: calculating angle(Rad) with y and x coordinate
        //Rad2Deg: converting Radian to Degree
        //rotZ: angle of the vector as Degree



        transform.rotation = Quaternion.Euler(0f, 0f, rotZ + rot_offset);
       
    }
}
