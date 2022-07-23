using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_MonsterHPBar : UI_Base
{
    #region UI������Ʈ_ENUM
    protected enum Images
    {
        hpBarBackground,                                         // hp�� �޹�� (hp�� �Ͼ�� ���)
        hpBar                                                    // hp��     
    }

    protected enum GameObjects
    {
        hpLine                                                  // �Ͼ�� �簢��������(Image,Image(1)....) ���� ������Ʈ 
    }
    #endregion

    #region ����
    [SerializeField]
    private Camera              m_mainCamera;
    private MonsterController   m_target;                       // ��� ����
    private int                 m_hp;                           // Hp �ٿ� ��¿� ���� hp��

    public int                  m_verticalOffset;               // ������ġ Y��ǥ offset��
    public int m_unitHp;                                        // ���� hp 
                                                                // (���� hp�� �������� �ִ� ü���� ���������� �Ͼ�� �簢���� �����ϰ� �迭��)
    #endregion

    #region ������Ƽ
    public MonsterController Target { get => m_target; set => m_target = value; }
    

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
        Bind<Image>(typeof(Images));
        Bind<GameObject>(typeof(GameObjects));

        GetHpBoost();

        if (m_mainCamera == null)
        {
            m_mainCamera = Camera.main;
        }

        HpBarUpdate();
        CanvasScaleChange();     
    }

    private void Update()
    {
        HpBarPositionUpdate();
    }

    // �ִ� ü�·��� ���� �Ͼ�� �簢 ������ ũ�� �����ϴ� �Լ�
    public void GetHpBoost()
    {
        // (���� hp / �ִ�ü��) ����Ͽ� �Ͼ�� �簢 �������� �����ϰ� ����
        float l_scaleX = (float)m_unitHp / m_target.Stat.MaxHp;

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


    // hp���� ���� �������� ������Ʈ �ϴ� �Լ�
    private void HpBarPositionUpdate()
    {
        Collider l_parentCollider = transform.parent.GetComponent<Collider>();

        // hp���� ��ġ�� �ش� hp�ٸ� �������ִ� ����(hp���� �θ�) �� ��ġ���� �������� �ݶ��̴��� ysize�� + y��ǥ offset��ŭ ������������ ����
        transform.position = transform.parent.position + Vector3.up * (l_parentCollider.bounds.size.y + m_verticalOffset);

        // �ش� hp�ٸ� ������ ���� (hp���� �θ�)�� ȸ���ϸ� hp�ٵ� ȸ���ϱ⋚����, hp�ٸ� �׻� ī�޶� ���ϵ�����
        transform.LookAt(m_mainCamera.transform);
        // 180�� ���ư� ���¸� �ذ��ϱ����� -180�� ȸ��������
        transform.Rotate(Vector3.up, -180.0f);
    }

    // hp ���� ���� �����ϴ� �Լ�
    private void HpBarUpdate()
    {
        Get<Image>((int)Images.hpBar).fillAmount = m_target.Stat.Hp / (float)m_target.Stat.MaxHp;
    }

    // canvas�� ũ�⸦ hp�� ũ�⿡ �°� �������ִ� �Լ�
    private void CanvasScaleChange()
    {
        RectTransform l_hpbarRect = Get<Image>((int)Images.hpBarBackground).GetComponent<RectTransform>();
        Vector2 l_newCanvasSize = l_hpbarRect.sizeDelta * l_hpbarRect.localScale.x;

        GetComponent<RectTransform>().sizeDelta = l_newCanvasSize;
    }
}
