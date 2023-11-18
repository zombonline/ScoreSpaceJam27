using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    [SerializeField] float backgroundScrollSpeed = 0.5f;
    [SerializeField] Material buildMaterial, battleMaterial;
    Renderer renderer;
    Vector2 offSet;



    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        offSet = new Vector2(backgroundScrollSpeed, backgroundScrollSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if(renderer.material == null)
        {
            return;
        }
        renderer.material.mainTextureOffset += offSet * Time.deltaTime;
    }

    public void UpdateBackground()
    {
        var currentTextureOffset = renderer.material.mainTextureOffset;
        if (WaveSystem.gameMode == GameMode.Battle)
        {
            renderer.material = battleMaterial;
        }
        else
        {
            renderer.material = buildMaterial;
        }
        renderer.material.mainTextureOffset = currentTextureOffset;
    }

}
