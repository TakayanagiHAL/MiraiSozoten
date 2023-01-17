using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TextController : MonoBehaviour
{
    // �e�L�X�g�I�u�W�F�N�g
    Text textline1; // ��1�s26�����܂�(�S�p)
    Text textline2;
    Text textline3;
    Text textline4;

    RectTransform textRect1;
    RectTransform textRect2;
    RectTransform textRect3;
    RectTransform textRect4;

    [SerializeField , TextArea(1, 6)] string EventText; // �C�x���g�̕��͂̑S��

    string[] DisplayText;                   // EventText���u�B�v�ŋ�؂�������
                                            
    List<string> ProcessedDispText;         // DisplayText���X�ɕ�����������(�ŏI�I�ɕ\�����镶��)
                                            
    int EventCarryOut;                      // EventText�̕������̐i�݋
                                            
    int NowProcessedNum;                    // ProcessedDispText�̌��ݕ\�����Ă��鐔

    bool TextDispFrag;                      // �e�L�X�g��\�����邩�̃t���O
    float DispTime;                         // ���̍s��\������o�ߎ���
    [SerializeField] float NextDispTime;    // ���̍s��\������܂ł̎���
                                            
    bool MoveTextFlag;                      // �e�L�X�g�ړ��t���O
    float TextMoveTime;                     // �e�L�X�g�ړ��̌o�ߎ���
    [SerializeField] float TextMoveTimeMax; // �e�L�X�g�̈ړ�����


    // �e�e�L�X�g�̈ړ��O�|�W�V�������ꎞ�I�ɕۑ�����ϐ�
    float BeforeTextPosY1;
    float BeforeTextPosY2;
    float BeforeTextPosY3;
    float BeforeTextPosY4;

    bool TextEnd;
    
    // Start is called before the first frame update
    void Start()
    {
        // �e�L�X�g���擾
        GameObject MaskPanek = this.gameObject.transform.Find("TextFrame").gameObject.transform.Find("MaskPanel").gameObject;

        textline1 = MaskPanek.transform.Find("TextLine1").gameObject.GetComponent<Text>();
        textline2 = MaskPanek.transform.Find("TextLine2").gameObject.GetComponent<Text>();
        textline3 = MaskPanek.transform.Find("TextLine3").gameObject.GetComponent<Text>();
        textline4 = MaskPanek.transform.Find("TextLine4").gameObject.GetComponent<Text>();

        textRect1 = MaskPanek.transform.Find("TextLine1").gameObject.GetComponent<RectTransform>();
        textRect2 = MaskPanek.transform.Find("TextLine2").gameObject.GetComponent<RectTransform>();
        textRect3 = MaskPanek.transform.Find("TextLine3").gameObject.GetComponent<RectTransform>();
        textRect4 = MaskPanek.transform.Find("TextLine4").gameObject.GetComponent<RectTransform>();

        // EventText���u�B�v�ŋ�؂�
        DisplayText = EventText.Split("�B");

        EventCarryOut = 0;

        ProcessedDispText = new List<string>();
        CreateDisplayText();// �ŏ��ɕ\�����镶������擾

        NowProcessedNum = 0;

        TextDispFrag = false;
        DispTime = NextDispTime + 0.1f;

        MoveTextFlag = false;
        TextMoveTime = 0.0f;
        

        BeforeTextPosY1 = 0.0f;
        BeforeTextPosY2 = 0.0f;
        BeforeTextPosY3 = 0.0f;
        BeforeTextPosY4 = 0.0f;

        TextEnd = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Enter�L�[�ŕ��͂�i�܂���
        if (Input.GetKeyDown(KeyCode.Return) /*|| Gamepad.current.bButton.wasPressedThisFrame*/)
        {
            // �\�����e��������
            TextClear();

            // ����DisplayText���쐬
            if (EventCarryOut < DisplayText.Length - 1)
            {
                EventCarryOut++;
                CreateDisplayText();
                DispTime = NextDispTime + 0.1f;

                //Debug.Log("DisplayText.Length:" + DisplayText.Length);
                //Debug.Log("EventCarryOut:" + EventCarryOut);
            }
            else
            {
                TextEnd = true;
                Debug.Log("textEnd:" + TextEnd);
            }
        }

        // ���Ԍo�߂Ńe�L�X�g��\������t���O�𗧂Ă�
        TimeDispFrag();

        // �e�L�X�g��\������
        ExecutionTextDisp();
    }

    /* ==========�\�����镶�͂��쐬========== */
    void CreateDisplayText()
    {        
        // �C�x���g�̐i�ݐ���DisplayText�̗v�f���ȓ����ǂ�������
        if (EventCarryOut < DisplayText.Length)
        {
            // �擪��"\n"�������Ă�����폜����
            int HeadBreakSymbol = DisplayText[EventCarryOut].IndexOf("\n");
            if (HeadBreakSymbol == 0)
            {
                DisplayText[EventCarryOut] = DisplayText[EventCarryOut].Remove(0, 1);
            }

            // ���X�g�̒��g��S�č폜
            ProcessedDispText.Clear();

            int StringNum = 0;
            int LoopNum = 0;
            while (true)
            {
                // �z��̒������擾
                StringNum = DisplayText[EventCarryOut].Length;

                //Debug.Log("StringNum:" + StringNum);

                // ���͂̒�����0�������ꍇ���[�v�𔲂���
                if (StringNum == 0)
                {
                    break;
                }

                string provisionalText; // �ꎞ�I��26���������e�L�X�g
                int actualTextNum;      // �擪����̕�����
                if (StringNum >= 26)
                {
                    actualTextNum = 26;
                }
                else
                {
                    actualTextNum = StringNum;
                }

                // Substring(�����o���n�߂镶����,�����o��������)
                provisionalText = DisplayText[EventCarryOut].Substring(0, actualTextNum);

                // �擾����26������"\n"���܂܂�Ă��邩���ׂ�
                // �����Ă�����擪����̕��������擾���A����������26����
                int partCharNum = provisionalText.IndexOf("\n") + 1;
                if (partCharNum != 0) // �܂܂�Ă���
                {
                    actualTextNum = partCharNum;
                }

                // �擾�������͂�ProcessedDispText�ɒǉ�
                // "\n"���܂܂�Ă����ꍇ�͍X�ɍŌ������"\n"�𔲂�
                ProcessedDispText.Add(provisionalText.Substring(0, actualTextNum)); //= provisionalText.Substring(0, actualTextNum);
                ProcessedDispText[LoopNum].Replace("\n", "");
                //Debug.Log("ProcessedDispText[i]:" + ProcessedDispText[LoopNum]);


                // �����o��������������DisplayText[EventCarryOut]�̐擪���當�����폜����
                DisplayText[EventCarryOut] = DisplayText[EventCarryOut].Remove(0, actualTextNum);

                LoopNum++;
            }

            // ���݂̕\���s����0�ɂ���
            NowProcessedNum = 0;
        }
    }

    /* ==========���Ԍo�߂Ńe�L�X�g��\������t���O�𗧂Ă�========== */
    void TimeDispFrag()
    {
        // �\�����Ă���ԍ���ProcessedDispText�̗v�f�����z���Ă��Ȃ���
        if (NowProcessedNum< ProcessedDispText.Count)
        {
            // �e�L�X�g�ړ��t���O�������Ă��Ȃ���
            if (MoveTextFlag == false)
            {
                DispTime += Time.deltaTime;

                // DispTime��1.5f�ȏ�
                if (DispTime > NextDispTime)
                {
                    // NowProcessedNum��3�ȏ�̏ꍇ��textline����ɂ��炵�Ĉ�ԏ�̕�����ԉ��Ɉړ�������
                    // �ړ�������t���O�𗧂Ă�
                    if (NowProcessedNum >= 3)
                    {
                        MoveTextFlag = true;

                        // �e�e�L�X�g�̈ړ��O�|�W�V�������ꎞ�I�ɕۑ�����
                        BeforeTextPosY1 = textRect1.anchoredPosition.y;
                        BeforeTextPosY2 = textRect2.anchoredPosition.y;
                        BeforeTextPosY3 = textRect3.anchoredPosition.y;
                        BeforeTextPosY4 = textRect4.anchoredPosition.y;
                    }
                    else
                    {
                        TextDispFrag = true;
                    }                    
                }
            }            
        }        
    }

    /* ==========���ۂɃe�L�X�g��\������========== */
    void ExecutionTextDisp()
    {
        // �e�L�X�g����Ɉړ�������
        MoveTextLine();

        // �ړ����I�������e�L�X�g��\������
        if (TextDispFrag == true)
        {
            TextSet();
            NowProcessedNum++;
            DispTime = 0.0f;

            TextDispFrag = false;
        }
    }

    /* ==========3�s�ȏ�\������ꍇ�ɕ\������s�����炷========== */
    void MoveTextLine()
    {
        if (NowProcessedNum >= 3)
        {
            if (MoveTextFlag == true)  // �ړ��t���O����������
            {
                // ���Ԍv��
                TextMoveTime += Time.deltaTime;

                // 
                if (TextMoveTime < TextMoveTimeMax)  // �ړ���
                {
                    // �o�ߎ��Ԃ���ړ��ʂ��v�Z����
                    float MovePosition = 65.0f * (TextMoveTime / TextMoveTimeMax);

                    // �ړ��O�|�W�V����+MovePosition
                    textRect1.anchoredPosition = new Vector3(0.0f, BeforeTextPosY1 + MovePosition, 0.0f);
                    textRect2.anchoredPosition = new Vector3(0.0f, BeforeTextPosY2 + MovePosition, 0.0f);
                    textRect3.anchoredPosition = new Vector3(0.0f, BeforeTextPosY3 + MovePosition, 0.0f);
                    textRect4.anchoredPosition = new Vector3(0.0f, BeforeTextPosY4 + MovePosition, 0.0f);
                }
                else // �ړ��I��
                {
                    // ���ꂼ��ړ��O����65��Ɉړ������l��������
                    textRect1.anchoredPosition = new Vector3(0.0f, BeforeTextPosY1 + 65.0f, 0.0f);

                    textRect2.anchoredPosition = new Vector3(0.0f, BeforeTextPosY2 + 65.0f, 0.0f);
                    textRect3.anchoredPosition = new Vector3(0.0f, BeforeTextPosY3 + 65.0f, 0.0f);
                    textRect4.anchoredPosition = new Vector3(0.0f, BeforeTextPosY4 + 65.0f, 0.0f);

                    // y��130�𒴂��Ă������ԉ�(-130)�Ɉړ�
                    if (textRect1.anchoredPosition.y > 129.9f)
                    {
                        textRect1.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);
                        textline1.text = "";
                    }
                    else if (textRect2.anchoredPosition.y > 129.9f)
                    {
                        textRect2.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);
                        textline2.text = "";
                    }
                    else if (textRect3.anchoredPosition.y > 129.9f)
                    {
                        textRect3.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);
                        textline3.text = "";
                    }
                    else if (textRect4.anchoredPosition.y > 129.9f)
                    {
                        textRect4.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);
                        textline4.text = "";
                    }


                    TextMoveTime = 0.0f;

                    MoveTextFlag = false;
                    TextDispFrag = true;
                }
            }
        }
    }
    

    /* ==========�e�L�X�g�̕��͂��Z�b�g========== */
    void TextSet()
    {
        if(NowProcessedNum< ProcessedDispText.Count)
        {
            // ���݂̕\���ԍ����O�`�R�ɕϊ�
            int num = NowProcessedNum % 4;

            if (num == 0)
            {
                textline1.text = ProcessedDispText[NowProcessedNum].ToString();
            }
            else if (num == 1)
            {
                textline2.text = ProcessedDispText[NowProcessedNum].ToString();
            }
            else if (num == 2)
            {
                textline3.text = ProcessedDispText[NowProcessedNum].ToString();
            }
            else if (num == 3)
            {
                textline4.text = ProcessedDispText[NowProcessedNum].ToString();
            }
        }
    }

    /* ==========�e�L�X�g�̕��͂ƃ|�W�V�������N���A========== */
    void TextClear()
    {
        // Text�̃|�W�V�������Z�b�g
        textRect1.anchoredPosition = new Vector3(0.0f, 65.0f, 0.0f);
        textRect2.anchoredPosition = new Vector3(0.0f, 0.0f, 0.0f);
        textRect3.anchoredPosition = new Vector3(0.0f, -65.0f, 0.0f);
        textRect4.anchoredPosition = new Vector3(0.0f, -130.0f, 0.0f);



        // Text�̒��g�N���A
        textline1.text = "";
        textline2.text = "";
        textline3.text = "";
        textline4.text = "";

        // ProcessedDispText�̒��g��S�ăN���A
        ProcessedDispText.Clear();


        // �e��t���O�Ǝ��Ԍv���p�̕ϐ���������
        NowProcessedNum = 0;   // ���݂̕\���e�L�X�g��
        TextDispFrag = false;  // �e�L�X�g�\���t���O
        DispTime = 0.0f;       // ���̍s��\������o�ߎ���

        MoveTextFlag = false;  // �e�L�X�g�ړ��̃t���O
        TextMoveTime = 0.0f;   // �e�L�X�g�ړ��̌o�ߎ���
    }

    /* ==========�e�L�X�g�̓ǂݏグ�I��������擾����========== */
    public bool GetTextEnd()
    {
        return TextEnd;
    }

    /* ==========�e�L�X�g��ǉ��œ��͂���========== */
    // �����e���㏑������̂ŁA�㏑�����Ă���薳�������S���\���������Ă���ǉ��œ���邱��
    public void SetText(string text)
    {
        TextClear();                     // �e�L�X�g��S�ď�����

        DisplayText = text.Split("�B");  // �\���e�L�X�g���e���㏑��

        EventCarryOut = 0;               // �e�L�X�g�̐i�݋��������

        TextEnd = false;                 // �e�L�X�g�I���t���O��false�ɂ���

        CreateDisplayText();             // �e�L�X�g�����H

        //Debug.Log("DisplayText.Length:" + DisplayText.Length);
    }



    /* ==========string�^�e�L�X�g�������̕�����T���Đ��𒲂ׂ� ========== */
    int SpecificCharNumSearch(string Text, string search)
    {
        int NowPoint = 0;
        int PartNum = 0;// "\n"�̐�

        // Textnum����"\n"�̐�������o��
        while (true)
        {
            // �擪����̕��������擾
            int PartStrNum = Text.IndexOf(search, NowPoint);

            // "\n"�������ꍇ�̓��[�v���o��
            if (PartStrNum == -1)
            {
                break;
            }

            NowPoint = PartStrNum + 1;

            PartNum++;
        }

        return PartNum;
    }

}
