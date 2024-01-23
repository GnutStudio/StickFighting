using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaMove : MonoBehaviour
{
    private static readonly int offsetId = Shader.PropertyToID("_MainTex");
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Vector2 speed;
    private Vector2 currentOffet;
    private Material currentMaterial;
    // Start is called before the first frame update
    void Start()
    {
        currentMaterial = spriteRenderer.material;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentMaterial) {
            currentOffet += speed * Time.deltaTime;
            currentMaterial.SetTextureOffset(offsetId, currentOffet);
        }
    }
}
