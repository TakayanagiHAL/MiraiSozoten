using System.Collections;
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


public class Hexagon : MonoBehaviour
{

    [SerializeField] MapIndex index;
    [SerializeField] HexagonType squareType;
    HexagonMethod hexagonMethod;
    // Start is called before the first frame update
    void Start()
    {
        switch (squareType)
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

}
