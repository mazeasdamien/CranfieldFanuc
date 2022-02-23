using UnityEngine;

public class PublisherKeyboardController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotSpeed = 50f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q))
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.forward);

        if (Input.GetKey(KeyCode.W))
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.back);

        if (Input.GetKey(KeyCode.A))
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.left);

        if (Input.GetKey(KeyCode.S))
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.right);

        if (Input.GetKey(KeyCode.Z))
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.up);

        if (Input.GetKey(KeyCode.X))
            transform.Translate(moveSpeed * Time.deltaTime * Vector3.down);

        if (Input.GetKey(KeyCode.E))
            transform.Rotate(rotSpeed * Time.deltaTime * Vector3.forward);

        if (Input.GetKey(KeyCode.R))
            transform.Rotate(rotSpeed * Time.deltaTime * Vector3.back);

        if (Input.GetKey(KeyCode.D))
            transform.Rotate(rotSpeed * Time.deltaTime * Vector3.left);

        if (Input.GetKey(KeyCode.F))
            transform.Rotate(rotSpeed * Time.deltaTime * Vector3.right);

        if (Input.GetKey(KeyCode.C))
            transform.Rotate(rotSpeed * Time.deltaTime * Vector3.up);

        if (Input.GetKey(KeyCode.V))
            transform.Rotate(rotSpeed * Time.deltaTime * Vector3.down);

    }
}
