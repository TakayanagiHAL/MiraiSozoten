using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SoftGear.Strix.Unity.Runtime;


public class HexagonManger : StrixBehaviour
{
    [SerializeField] int mapWidth;
    [SerializeField] int mapHeight;
    Hexagon[,] map;

    int nowMedal = -1;

    List<Hexagon> seabase;
    // Start is called before the first frame update
    void Start()
    {
        seabase = new List<Hexagon>();
        map = new Hexagon[mapHeight,mapWidth];

        Hexagon[] hexagons = FindObjectsOfType<Hexagon>();

        for(int i = 0; i < hexagons.Length; i++)
        {
            MapIndex index = hexagons[i].GetMapIndex();
            map[index.y,index.x] = hexagons[i];
            hexagons[i].Init();
            if(hexagons[i].GetHexagonType() == HexagonType.SEA_BASE)
            {
                seabase.Add(hexagons[i]);
            }
        }

        SetNextMedal();
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
    public Hexagon GetHexagon(MapIndex index) { return map[index.y, index.x];}

    public void SetNextMedal()
    {
        int m = Random.Range(0, seabase.Count);
        while(nowMedal == m)
        {
            m = Random.Range(0, seabase.Count);
        }

        for(int i = 0; i < seabase.Count; i++)
        {
            if(i == m)
            {
                seabase[m].GetHexagonMethod<SeabaseHexagon>().SetMedal();
                seabase[m].SetHexagonType(HexagonType.SEA_BASE);
                seabase[m].SetSprite();
            }
            else
            {
                seabase[i].SetHexagonType(HexagonType.SEA_BASE_OFF);
                seabase[i].SetSprite();
            }
        }
        
        nowMedal = m;
    }
}
