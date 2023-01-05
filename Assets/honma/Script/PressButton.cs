using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PressButton : MonoBehaviour
{
    [SerializeField][Header("ClickLoadScene��script�������Ă���")]
    private NextSceneLoad _nextSceneLoadScript;

    Keyboard _keyboard;

    // Start is called before the first frame update
    void Start()
    {
        // ���݂̃L�[�{�[�h���
        _keyboard = Keyboard.current;

        // �L�[�{�[�h�ڑ��`�F�b�N
        if (_keyboard == null)
        {
            // �L�[�{�[�h���ڑ�����Ă��Ȃ���
            // Keyboard.current��null�ɂȂ�
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // B�L�[�̓��͏�Ԏ擾
        var Key_putB = _keyboard.bKey;

        if (Key_putB.wasPressedThisFrame)
        {
            _nextSceneLoadScript.LoadSceneStart("���C�����j���[Scene");//�V�[��������
        }
    }
}