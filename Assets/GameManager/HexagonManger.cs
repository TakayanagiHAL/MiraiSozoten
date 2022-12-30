using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;


public class HexagonManger : StrixBehaviour
{
    [SerializeField] int mapWidth;
    [SerializeField] int mapHeight;
    Hexagon[,] map;
    // Start is called before the first frame update
    void Start()
    {
        map = new Hexagon[mapHeight,mapWidth];

        Hexagon[] hexagons = FindObjectsOfType<Hexagon>();

        for(int i = 0; i < hexagons.Length; i++)
        {
            MapIndex index = hexagons[i].GetMapIndex();
            map[index.y,index.x] = hexagons[i];

        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetMapPos(MapIndex index) {

        Vector3 pos = new Vector3();
        pos = map[index.y, index.x].transform.position;
        return pos;

    }

    public MapIndex GetMapScale() { return new MapIndex(mapWidth, mapHeight); }
    public Hexagon GetHexagon(MapIndex index) { return map[index.y, index.x]; }
}
