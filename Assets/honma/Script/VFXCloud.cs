using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXCloud : MonoBehaviour
{
    private Renderer _Renderer;

    // Start is called before the first frame update
    void Start()
    {
        _Renderer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_Renderer.isVisible)//ƒJƒƒ‰ŠO‚És‚Á‚½‚Æ‚«
        {
           // Destroy(this.gameObject);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
    }
}
