using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// �÷��̾� HP���� ��� �ٸ� �÷��̾��� HP�ٸ� ��½�ų���� �����Ƿ�
// Main Player�� ������ų ���� �ƴ϶� ������ PlayerController�� �޾ƿ;���.

public class UI_PlayerHPBar : UI_Scene
{
    #region UI������Ʈ_ENUM
    protected enum Images
    {
        hpBarBackground,                                         // hp�� �޹�� (hp�� �Ͼ�� ���)
        hpBar                                                    // hp��       (��� hp��)
    }

    protected enum GameObjects
    {
        hpLine                                                  // �Ͼ�� �簢��������(Image,Image(1)....) ���� ������Ʈ 
    }
    #endregion

    #region ����
    private PlayerController m_target = null;
    private int              m_hp = 0;                          // hp�� ��¿� ���� hp��

    public int m_unitHp;                                        // ���� hp 
                                                                // (���� hp�� �������� �ִ� ü���� ���������� �Ͼ�� �簢���� �����ϰ� �迭��)
    #endregion

    #region ������Ƽ
    public PlayerController Target { get => m_target; set => m_target = value; }

    // ������Ƽ�� ���� hp���� �����ϸ� �ڵ����� hpBar�� ���¸� �����ϵ�����
    public int HP
    {
        get 
        { 
            return m_hp; 
        }
        set
        {
            m_hp = value;
            HpBarUpdate();
        }
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        GetHpBoost();
        HpBarUpdate();
    }

    private void Update()
    {
        // �÷��̾��� �ִ� ü������ (test��)
        if (Input.GetKeyDown(KeyCode.Keypad9))
        {
            Managers.Game.Player.MainPlayer.Stat.maxHp += 100;
            Debug.Log($"current maxhp : {m_target.Stat.maxHp}");
            GetHpBoost();
        }
    }

    public void GetHpBoost()
    {

        // (���� hp / �ִ�ü��) ����Ͽ� �Ͼ�� �簢 �������� �����ϰ� ����
        float l_scaleX = (float)m_unitHp / m_target.Stat.maxHp;

        // hpLine ������Ʈ�� �ڽĵ� (�Ͼ�� �簢 �����ӵ�)�� �����ϰ��� ����
        // (HorizontalLayoutGroup ������Ʈ�� Ȱ��ȭ �Ǿ������� Scale�� ����� ������ �ȵǹǷ� �������� ��� ��Ȱ��ȭ ������)

        GameObject l_hpLine = Get<GameObject>((int)GameObjects.hpLine);
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(false);
        foreach (Transform child in l_hpLine.transform)
        {
            child.gameObject.transform.localScale = new Vector3(l_scaleX, 1, 1);
        }
        l_hpLine.GetComponent<HorizontalLayoutGroup>().gameObject.SetActive(true);
    }


    // hp ���� ���� �����ϴ� �Լ�
    private void HpBarUpdate()
    {
        Get<Image>((int)Images.hpBar).fillAmount = m_target.Stat.hp / (float)m_target.Stat.maxHp;
    }
}
