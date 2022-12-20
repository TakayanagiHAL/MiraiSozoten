using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXCloud : MonoBehaviour
{
    [SerializeField]
    private float CloudVelocityX;
    [SerializeField]
    private float CloudVelocityY;
    [SerializeField][Header("â_ÇÃçÇÇ≥êßå¿")]
    private float CloudLimitPositionY;

    private Renderer _Renderer;
    private VisualEffect _VFXCloud;

    // Start is called before the first frame update
    void Start()
    {
        _Renderer = GetComponent<Renderer>();
        _VFXCloud = GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_Renderer.isVisible)//ÉJÉÅÉâäOÇ…çsÇ¡ÇΩÇ∆Ç´
        {
           // Destroy(this.gameObject);
        }

        Transform myTransform = this.transform;
        Vector3 myVector3 = myTransform.position;

        myVector3.x += CloudVelocityX;
        if (CloudLimitPositionY >= myTransform.position.y)
        {
            myVector3.y += CloudVelocityY;
        }

        myTransform.position = myVector3;

    }
    private void OnBecameInvisible()
    {
        Destroy(this.gameObject);
        
    }
}
