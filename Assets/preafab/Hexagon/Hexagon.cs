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
    NONE,
    COAST,
    OFFING,
    REEF,
    PORT,
    HAPPNING,
    SEA_BASE
}

public class HexagonMethod
{
    Hexagon hexagon;
    public virtual void OnPassage(Player player) { }

    public virtual void OnReach(Player player) { }

    public void SetHexagon(Hexagon hex) { hexagon = hex; }
}

[Serializable]
public struct HexagonSpritePair
{
    public HexagonType hexagonType;
    public GameObject sprite;
}

[Serializable]
public class HexagonSpriteDictionary
{
    public List<HexagonSpritePair> keyValuePairs;

    public GameObject GetSprite(HexagonType type)
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
    [SerializeField] GameObject hexagonObject;
    HexagonMethod hexagonMethod;
    // Start is called before the first frame update
    void Start()
    {
    }

    public void Init()
    {
        switch (hexagonType)
        {
            case HexagonType.NONE:
                break;
            case HexagonType.COAST:
                hexagonMethod = new CoastHexagon();
                hexagonMethod.SetHexagon(this);
                break;
            case HexagonType.OFFING:
                hexagonMethod = new OffingHexagon();
                hexagonMethod.SetHexagon(this);
                break;
            case HexagonType.REEF:
                hexagonMethod = new ReefHexagon();
                hexagonMethod.SetHexagon(this);
                break;
            case HexagonType.PORT:
                hexagonMethod = new PortHexagon();
                hexagonMethod.SetHexagon(this);
                break;
            case HexagonType.HAPPNING:
                hexagonMethod = new HappningHexagon();
                hexagonMethod.SetHexagon(this);
                break;
            case HexagonType.SEA_BASE:
                hexagonMethod = new SeabaseHexagon();
                hexagonMethod.SetHexagon(this);
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
       
        hexagonObject = useSprites.GetSprite(hexagonType);
    }

    public void SetMapindex(int X, int Y) { index.x = X; index.y = Y; }

    public HexagonType GetHexagonType() { return hexagonType; }

    public HexagonMethod GetHexagonMethod() { return hexagonMethod; }
}
