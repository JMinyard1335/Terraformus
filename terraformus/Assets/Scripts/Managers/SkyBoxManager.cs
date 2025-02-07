using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Allows the storing of multiple images to be used as skyboxes
/// This 
/// </summary>
[RequireComponent(typeof(Skybox))]
public class SkyBoxManager : MonoBehaviour
{
    [SerializeField] List<Material> skyBoxesMaterials;

    Skybox skybox;

    void Awake() 
    {
        skybox = GetComponent<Skybox>();
    }

    void OnEnable() 
    {
        changeSkybox(0);
    }

    private void changeSkybox(int index)
    {   
        if(skybox != null && index >= 0 && index < skyBoxesMaterials.Count)
        {
            skybox.material = skyBoxesMaterials[index];
        } 
    }
}
