using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    public Material[] materials;

    MeshRenderer meshRenderer;

    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

        var i = Random.Range(0, materials.Length);

        meshRenderer.sharedMaterial = materials[i];
    }

    // Update is called once per frame
    void Update()
    {
        var offset = new Vector2(0, scrollSpeed * Time.deltaTime);

        meshRenderer.sharedMaterial.mainTextureOffset += offset;
    }
}
