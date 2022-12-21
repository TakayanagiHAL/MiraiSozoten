using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXCloud : MonoBehaviour
{
    [SerializeField]
    private float CloudVelocityX = 0.1f;
    [SerializeField]
    private float CloudVelocityY = 0.03f;
    [SerializeField]
    [Header("â_ÇÃçÇÇ≥êßå¿")]
    private float CloudLimitPositionY = 350.0f;

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
        this.gameObject.SetActive(false);   //DestroyÇµÇ»Ç¢Ç≈åüçıÇ≈Ç´ÇÈÇÊÇ§Ç…Ç∑ÇÈ
    }
   
}
