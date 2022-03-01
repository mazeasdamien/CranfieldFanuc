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
        robotcontroller.position = transform.position;
        robotcontroller.eulerAngles = transform.eulerAngles;
    }
}
