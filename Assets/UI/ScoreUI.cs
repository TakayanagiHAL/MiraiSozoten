using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] Text medalText;
    [SerializeField] Text moneyText;
    [SerializeField] Text oilText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetMoney(int money)
    {
        moneyText.text = money.ToString();
    }

    public void SetOil(SeaResource seaResource)
    {
        int oil = 0;

        oil += seaResource.plastic;
        oil += seaResource.ePlastic*3;
        oil += seaResource.wood*4;
        oil += seaResource.steel*15;
        oil += seaResource.seaFood*12;

        oilText.text = oil.ToString();
    }
}
