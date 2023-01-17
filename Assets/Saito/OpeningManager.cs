using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningManager : MonoBehaviour
{
    [SerializeField] TextController text;

    [SerializeField] ResourceEffectUI player1UI;
    [SerializeField] ResourceEffectUI player2UI;
    [SerializeField] ResourceEffectUI player3UI;
    [SerializeField] ResourceEffectUI player4UI;

    [SerializeField] SeaResource distResource;

    [SerializeField] NextSceneLoad sceneLoad;

    int EventNum;       // �e�L�X�g�I���t���O���擾������
    bool ResourceEvent; // ���\�[�X�擾�C�x���g�̒ʉ߃t���O
    bool EventClear;    // ���\�[�X�擾�C�x���g���I�����Ď��̕��͂��n�߂�

    float EfectTime;    // ���̕��͂��n�߂�܂ł̎���

    bool GetEnd;        // textEnd�t���O�̂��擾��1��ɐ�������t���O


    // Start is called before the first frame update
    void Start()
    {
        EventNum = 0;
        ResourceEvent = false;
        EventClear = false;

        EfectTime = 0.0f;

        GetEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        // �e�L�X�g�I���t���O����������EventNum�����Z
        if (text.GetTextEnd() == true)
        {
            if (GetEnd == false)
            {
                EventNum++;
                Debug.Log("EventNum:" + EventNum);

                GetEnd = true;
            }            
        }

        // �ŏ��̕��͂��I�����A�����l�����o������
        if (EventNum == 1)
        {
            if (ResourceEvent == false)
            {
                // ���\�[�X�擾�G�t�F�N�g�𓮂���
                player1UI.SetAction(distResource);
                player2UI.SetAction(distResource);
                player3UI.SetAction(distResource);
                player4UI.SetAction(distResource);

                ResourceEvent = true;
            }


            if (EfectTime > 1.0f)
            {

                if (EventClear == false)
                {
                    text.SetText("����ł͂��悢��X�^�[�g�ł��I�B���̑D���ɕ����Ȃ��悤�ɁA�撣��܂��傤�ˁI");
                    EventClear = true;

                    Debug.Log("EventClear:" + EventClear);

                    GetEnd = false;
                }
            }
            else
            {
                EfectTime += Time.deltaTime;
            }

        }

        // �e�L�X�g���I�����A�V�[�����ς��
        if (EventNum == 2)
        {
            Debug.Log("�V�[���`�F���W");
            OpeningEnd();
            EventNum++;
        }
    }

    // �V�[���ύX�p�̊֐�
    void OpeningEnd()
    {
        sceneLoad.LoadSceneStart("Scenes/SampleScene");
    }
}
