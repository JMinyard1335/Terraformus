using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(-10, 10),SerializeField] private float _rotationSpeed = 0f;
    [SerializeField] private Vector3 _rotationAxis = new Vector3(0, 0.75f, 0);

    private void Update()
    {
        transform.Rotate(_rotationAxis, _rotationSpeed * Time.deltaTime);
    }
}
