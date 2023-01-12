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
            player.GetComponent<Player>().uiManager.SetCamera(mCamera);
        }
        else
        {
            pCamera.enabled = true;
            mCamera.enabled = false;
            player.GetComponent<Player>().uiManager.SetCamera(pCamera);
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

    public Camera GetCamera()
    {
        if (pCamera.enabled)
        {
            return pCamera;
        }
        else
        {
            return mCamera;
        }
    }

    public void SetRenderTexture(RenderTexture renderTexture)
    {
        pCamera.targetTexture = renderTexture;
        mCamera.targetTexture = renderTexture;

    }
}
