using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleObjectMoving : MonoBehaviour
{
    [SerializeField]
    [Header("prefabから動かしたいオブジェクトを追加(岩など)")]
    private List<GameObject> _movingObjectList;

    [SerializeField]
    [Header("この半径でオブジェクトを避ける")]
    private float _playerRadius;

    [SerializeField]
    [Header("実際には岩のスピード")]
    private float _playerMoveSpeed;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
