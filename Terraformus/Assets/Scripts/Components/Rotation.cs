using UnityEngine;

public class Rotation : MonoBehaviour
{

    public float speed = 10f;
    public Vector3 axis = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(axis, speed * Time.deltaTime);
    }
}
