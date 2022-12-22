using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ResourceUI : MonoBehaviour
{
    Player player;
    [SerializeField] Text plasticScore;
    [SerializeField] Text ePlaScore;
    [SerializeField] Text woodScore;
    [SerializeField] Text steelScore;
    [SerializeField] Text seaFoodScore;
    [SerializeField] Text stackScore;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetResource(SeaResource resource)
    {
        plasticScore.text   = resource.plastic.ToString();
        ePlaScore.text      = resource.ePlastic.ToString();
        woodScore.text      = resource.wood.ToString();
        steelScore.text     = resource.steel.ToString();
        seaFoodScore.text   = resource.seaFood.ToString();
    }

    public void SetStack(int stack)
    {
        stackScore.text = stack.ToString();
    }
}
