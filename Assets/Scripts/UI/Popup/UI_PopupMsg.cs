using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ShowPopupUI�� ȭ���� ����, ������Ƽ�� �̿��ؼ� message��, delete delay time�� �����Ͽ� ����ϸ�ȴ�.

public class UI_PopupMsg : UI_Popup
{
    #region UI������Ʈ_ENUM
    enum Texts
    {
        PopupMsg            
    }
    #endregion

    #region ����
    private string  m_message;
    private float   m_delayDeleteTime;
    #endregion

    #region ������Ƽ
    public string   Message { get => m_message; set => m_message = value; }
    public float    DelayDeleteTime { get => m_delayDeleteTime; set => m_delayDeleteTime = value; }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
    }

    private void Update()
    {
        PopupMsgUpdate();
    }

    private void PopupMsgUpdate()
    {
        Get<Text>((int)Texts.PopupMsg).text = m_message;
        Managers.Resource.Destroy(gameObject, m_delayDeleteTime);
    }

}
