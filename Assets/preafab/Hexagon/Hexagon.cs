using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MapIndex
{
    public MapIndex(int nx, int ny) { x = nx; y = ny; }
    public static bool operator ==(MapIndex a, MapIndex b)
    {
        if (a.x == b.x && a.y == b.y) return true;

        return false;
    }
    public static bool operator !=(MapIndex a, MapIndex b)
    {
        if (a.x == b.x && a.y == b.y) return false;

        return true;
    }
    public int x;
    public int y;
}

public enum HexagonType
{
    COAST,
    OFFING,
    REEF,
    PORT
}

class HexagonMethod
{
    public virtual void OnPassage(Player player) { }

    public virtual void OnReach(Player player) { }
}

[Serializable]
public struct HexagonSpritePair
{
    public HexagonType hexagonType;
    public Sprite sprite;
}

[Serializable]
public class HexagonSpriteDictionary
{
    public List<HexagonSpritePair> keyValuePairs;

    public Sprite GetSprite(HexagonType type)
    {
        for(int i = 0; i < keyValuePairs.Count; i++)
        {
            if(type == keyValuePairs[i].hexagonType)
            {
                return keyValuePairs[i].sprite;
            }
        }
        return null;
    }
}


public class Hexagon : MonoBehaviour
{

    [SerializeField] MapIndex index;
    [SerializeField] HexagonType hexagonType;
    [SerializeField] HexagonSpriteDictionary useSprites;
    HexagonMethod hexagonMethod;
    // Start is called before the first frame update
    void Start()
    {
        switch (hexagonType)
        {
            case HexagonType.COAST:
                hexagonMethod = new CoastHexagon();
                break;
            case HexagonType.OFFING:
                hexagonMethod = new OffingHexagon();
                break;
            case HexagonType.REEF:
                hexagonMethod = new ReefHexagon();
                break;
            case HexagonType.PORT:
                hexagonMethod = new PortHexagon();
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPassage(Player player) { hexagonMethod.OnPassage(player); }

    public void OnReach(Player player) { hexagonMethod.OnReach(player); }

    public MapIndex GetMapIndex() { return new MapIndex(index.x,index.y); }

    public void SetSprite()
    {
        GetComponent<SpriteRenderer>().sprite = useSprites.GetSprite(hexagonType);
    }

}
