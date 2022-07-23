using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_Toggle : UI_Popup
{
    #region UI������Ʈ_ENUM
    enum Images
    {
        BackGround,        
        CheckFrame,         
        CheckBox,           
        LabelFrame         
    }
    

    enum Texts
    {
        ToggleLabel
    }
    #endregion

    #region ����
    private bool m_isOn = false;
    #endregion

    #region ������Ƽ
    public bool OnOff
    {
        get 
        {
            return m_isOn;
        }
        set
        {
            m_isOn = value;

            if (m_isOn)
            {
                FlagOnFunction();
            }
            else
            {
                FlagOffFunction();
            }
        }
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        // üũ�ڽ� �̹��� Ŭ�� �̺�Ʈ ���
        GameObject go = Get<Image>((int)Images.CheckBox).gameObject;
        go.BindEvent((PointerEventData data) => 
        {
            if(m_isOn)
            {
                OnOff = false;
            }
            else
            {
                OnOff = true;
            }
        }
        , Define.UIEvent.Click);
    }

    // on���·� �����
    private void FlagOnFunction()
    {
        Get<Image>((int)Images.CheckBox).color = Color.black;
        // flag on�� ȣ���� �Լ�

        //
        Debug.Log("Toggle On");
    }

    // off���·� �����
    private void FlagOffFunction()
    {
        Get<Image>((int)Images.CheckBox).color = Color.white;
        // flag off�� ȣ���� �Լ�

        // 
        Debug.Log("Toggle false");
    }
}
