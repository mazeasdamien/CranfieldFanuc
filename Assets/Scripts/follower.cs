using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class follower : MonoBehaviour
{
    public Transform robotcontroller;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Keypad7))
        {
            transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Keypad8))
        {
            transform.RotateAround(transform.position, Vector3.up, -20 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Keypad4))
        {
            transform.RotateAround(transform.position, Vector3.right, 20 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Keypad5))
        {
            transform.RotateAround(transform.position, Vector3.right, -20 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Keypad1))
        {
            transform.RotateAround(transform.position, Vector3.forward, 20 * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.Keypad2))
        {
            transform.RotateAround(transform.position, Vector3.forward, -20 * Time.deltaTime);
        }

        robotcontroller.position = transform.position;
        robotcontroller.eulerAngles = transform.eulerAngles;
    }
}
