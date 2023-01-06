using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceEffectUI : MonoBehaviour
{
    [SerializeField] RectTransform panel;
    [SerializeField] Text plasticScore;
    [SerializeField] Text ePlaScore;
    [SerializeField] Text woodScore;
    [SerializeField] Text steelScore;
    [SerializeField] Text seaFoodScore;

    bool isAction = false;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isAction)
        {
            panel.localPosition = (new Vector3(0.0f, panel.localPosition.y +1.5f , 0));
        }
    }

    public void SetAction(SeaResource seaResource)
    {
        plasticScore.text    = seaResource.plastic.ToString();
        ePlaScore.text       = seaResource.ePlastic.ToString();
        woodScore.text       = seaResource.wood.ToString();
        steelScore.text      = seaResource.steel.ToString();
        seaFoodScore.text    = seaResource.seaFood.ToString();

        isAction = true;
        panel.localPosition = new Vector3();
    }
}
