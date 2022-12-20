using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleCloudBone : MonoBehaviour
{
    [SerializeField] [Header("preafabÇÃâ_ÇÃGameObjectÇì¸ÇÍÇÈ")]
    private GameObject _cloudObject;

    [SerializeField] [Header("âΩÇ‡ì¸ÇÍÇ»Ç¢")]
    private GameObject[] _cloudArray;

    [SerializeField][Header("â_ÇÃê∂ê¨êî")]
    private int _cloudAmount;

    [Header("----------------------")]
    [SerializeField]
    [Range(-1500.0f, 0.0f)] private float MinPosition_X;
    [SerializeField]
    [Range(0.0f, 600.0f)] private float MaxPosition_X;
    [SerializeField]
    [Range(-600.0f, 0.0f)] private float MinPosition_W;
    [SerializeField]
    [Range(0.0f, 600.0f)] private float MaxPosition_W;

    [SerializeField]     private float MinPosition_Y;
    [SerializeField]     private float MaxPosition_Y;

    

    // Start is called before the first frame update
    void Start()
    {
        _cloudArray = new GameObject[_cloudAmount];

        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < _cloudAmount; i++)
        {

            float x = Random.Range(MinPosition_X, MaxPosition_X);
            float w = Random.Range(MinPosition_W, MaxPosition_W);

            float y = Random.Range(MinPosition_Y, MaxPosition_Y);

            Vector3 position = new Vector3(x, y, w);

            _cloudArray[i] = Instantiate(_cloudObject, position, Quaternion.identity, this.gameObject.transform);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        

        if(_cloudArray.Length<_cloudAmount)//â_ÇÃêîÇ™ëçó Çâ∫âÒÇ¡ÇΩéû
        {
            float x = Random.Range(MinPosition_X, MaxPosition_X);
            float w = Random.Range(MinPosition_W, MaxPosition_W);
            float y = Random.Range(MinPosition_Y, MaxPosition_Y);

            Vector3 position = new Vector3(x, y, w);
            _cloudArray[CloudArraySerch()] = Instantiate(_cloudObject, position, Quaternion.identity);
        }
    }


    //ãÛÇÃóvëfÇíTÇµÅAinstantiateÇ∑ÇÈà◊ÇÃãÛÇÃóvëfÇÃî‘çÜÇï‘Ç∑
    private int CloudArraySerch()
    {
        GameObject[] gameArray = _cloudArray;
        int index = 0;


        for(int i=0;i<_cloudAmount;i++)
        {
            if (gameArray[i] == null)
                index = i;
                break;
        }

        return index;
    }
}
