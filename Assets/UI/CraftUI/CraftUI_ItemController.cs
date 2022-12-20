using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftUI_ItemController : MonoBehaviour
{
    GameObject SubjectCursol;
    RectTransform CursolTransform;

    int IconX;
    int IconY;

    // Start is called before the first frame update
    void Start()
    {
        SubjectCursol = this.gameObject.transform.Find("ItemMenuCursol").gameObject;
        CursolTransform = SubjectCursol.GetComponent<RectTransform>();

        IconX = 0;
        IconY = 0;
    }

    // Update is called once per frame
    void Update()
    {
        // ƒJ[ƒ\ƒ‹ˆÚ“®ˆ—
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            IconX++;
            if (IconX > 2)
            {
                IconX = 2;
            }

            CursolPositionCalculation();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            IconX--;
            if (IconX < 0)
            {
                IconX = 0;
            }

            CursolPositionCalculation();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            IconY++;
            if (IconY > 4)
            {
                IconY = 4;
            }

            CursolPositionCalculation();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            IconY--;
            if (IconY < 0)
            {
                IconY = 0;
            }

            CursolPositionCalculation();
        }
    }

    void CursolPositionCalculation()
    {
        float posx = 270.0f * (IconX - 1);
        float posy = 300 - 160 * IconY;

        CursolTransform.anchoredPosition = new Vector2(posx, posy);
    }
}
