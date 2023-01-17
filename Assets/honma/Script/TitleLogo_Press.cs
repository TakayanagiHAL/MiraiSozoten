using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TitleLogo_Press : MonoBehaviour
{
    [SerializeField]
    private GameObject TitleUi_I;
    [SerializeField]
    private GameObject TitleUi_T;

    private float _uiTimer;
    private bool on_off;

    // Start is called before the first frame update
    void Start()
    {

        _uiTimer = 0.0f;
        TitleUi_I.SetActive(true);
        TitleUi_T.SetActive(true);

        on_off = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B) || Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space");
            //TitleUi.SetActive(!TitleUi.activeSelf);
            on_off = !on_off;
        }
        if (!on_off)
        {
            
            GetComponent<Animator>().SetBool("is", false);

        }
        else
        {
            GetComponent<Animator>().SetBool("is", true);
        }
    }

    private void UiSpawn()
    {
        _uiTimer += Time.deltaTime;

        if (_uiTimer >= 60.0f)
        {
            GetComponent<Animator>().SetBool("is", false);
        }
        if (_uiTimer >= 120.0f)
        {
            GetComponent<Animator>().SetBool("is", true);
            _uiTimer = 0.0f;
        }


    }
}
