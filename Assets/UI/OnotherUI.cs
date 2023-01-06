using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnotherUI : MonoBehaviour
{
    [SerializeField] RawImage image;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTurnTexture(RenderTexture texture)
    {
        image.texture = texture;
    }
}
