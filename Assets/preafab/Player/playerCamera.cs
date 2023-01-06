using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerCamera : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] Camera pCamera;
    [SerializeField] Camera mCamera;
    Vector3 mapCameraPos;
    // Start is called before the first frame update
    void Start()
    {
        mapCameraPos = mCamera.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }

    public void SetMapCamera(bool isMap)
    {
        if (isMap)
        {
            pCamera.enabled = false;
            mCamera.enabled = true;
        }
        else
        {
            pCamera.enabled = true;
            mCamera.enabled = false;
        }
    }

    public void MapCameraMove(Vector3 delta)
    {
        mCamera.transform.Translate(delta);
    }

    public void ResetMapCamera()
    {
        mCamera.transform.position = mapCameraPos;
    }
}
