using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// ���� �����ӿ�ũ������ �۵� ����, ���� ���� �Լ��� �������� ����
// ���� ���� �����ӿ�ũ������ ���������� �۵���

// ������! Ingame Scene���� Init�Լ����� PopupMsgSetting �Լ��� ȣ���Ұ��
// UI ������Ʈ Bind�� �Ϸ���� ���� ���¹Ƿ� ������ �߻���

// �̰��� �Ŀ� MonsterManager���� Monster ������ų�� PopupMsg�� ���� �ذ��
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
    public string   Message { get => m_message;}
    public float    DelayDeleteTime { get => m_delayDeleteTime;}
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
    }

    private void PopupMsgSetting(string _message, float _delayDeleteTime)
    {
        m_message = _message;
        m_delayDeleteTime = _delayDeleteTime;

        Get<Text>((int)Texts.PopupMsg).text = _message;
        Managers.Resource.Destroy(gameObject, _delayDeleteTime);
    }

}
