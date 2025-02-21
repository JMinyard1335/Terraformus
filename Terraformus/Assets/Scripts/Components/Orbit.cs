using UnityEngine;

public class Orbit : MonoBehaviour
{
    public Transform target;
    public float speed = 10f;
    public Vector3 axis = Vector3.up;

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(target.position, axis, speed * Time.deltaTime);
    }
}
