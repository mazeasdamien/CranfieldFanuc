using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace VarjoExample
{
    public class ControllerVRFanuc : MonoBehaviour
    {
        Controller controller;
        private bool hasSentCommand = false;

        public ButtonManager buttonManager;
        public GameObject projectileOrigin;
        public GameObject cursor;
        public LineRenderer lineRenderer;
        public float defaultLenght = 3.0f;

        private bool isInstantiate = false;
        private bool isClicked = false;
        private GameObject go = null;
        private GameObject button = null;

        void Awake()
        {
            controller = GetComponent<Controller>();
        }


        // Update is called once per frame
        void FixedUpdate()
        {
            if (controller.gripButton)
            {
                if (!hasSentCommand)
                {
                    hasSentCommand = true;

                    buttonManager.ABORTButton();

                }
            }
            else 
            {
                hasSentCommand = false;
            }
            if (projectileOrigin != null)
            {
                if (controller.primary2DAxisTouch)
                {

                    lineRenderer.SetPosition(0, projectileOrigin.transform.position);
                    lineRenderer.SetPosition(1, CalculateEnd());

                    if (!isInstantiate)
                    {
                        isInstantiate = true;
                        go = Instantiate(cursor, CalculateEnd(), Quaternion.identity);
                    }
                    go.transform.position = CalculateEnd();
                }
                else
                {
                    Destroy(go);
                    go = null;
                    isInstantiate = false;
                    lineRenderer.SetPosition(0, projectileOrigin.transform.position);
                    lineRenderer.SetPosition(1, projectileOrigin.transform.position);
                }

                if (controller.primary2DAxisClick)
                {
                    if (!isClicked)
                    {
                        isClicked = true;
                        TaskClick();
                    }
                }
                else
                {

                    isClicked = false;
                }
            }
        }

        void TaskClick()
        {
            if (button.name == "RESET")
            {
                buttonManager.RESETButton();
            }
            else if (button.name == "ABORT")
            {
                buttonManager.ABORTButton();
            }
            else if (button.name == "PATH")
            {
                buttonManager.PATHButton();
            }
            else if (button.name == "HOME")
            {
                buttonManager.HOMEButton();
            }
        }

        private Vector3 CalculateEnd()
        {
            RaycastHit hit = CreateForwardRaycast();
            Vector3 endPosition = DefaultEnd(defaultLenght);

            if (hit.collider)
            {
                button = hit.transform.gameObject;
                endPosition = hit.point;
            }
            else
            {
                button = null;
            }

            return endPosition;
        }

        private RaycastHit CreateForwardRaycast()
        {
            RaycastHit hit;
            Ray ray = new Ray(projectileOrigin.transform.position, projectileOrigin.transform.forward);

            Physics.Raycast(ray, out hit, defaultLenght);
            return hit;
        }
        
        private Vector3 DefaultEnd(float length)
        {
            return projectileOrigin.transform.position + (projectileOrigin.transform.forward * length);
        }
    }
}
