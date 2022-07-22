using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : BaseController
{

    [SerializeField]
    private MonsterStat         m_stat = new MonsterStat();

    private BehaviorTree        m_behaviorTree = null;

    private UI_MonsterHPBar     m_monsterHPBar;
    private UI_Score            m_score;

    public MonsterStat          Stat { get => m_stat; set => m_stat = value; }

    void Start()
    {
        Init();

        // ���� hp�� ����
        m_monsterHPBar = Managers.UI.ShowSceneUI<UI_MonsterHPBar>("UI_MonsterHPBar");
        m_monsterHPBar.Target = this;

        m_score = Managers.UI.Root.GetComponentInChildren<UI_Score>();

        m_behaviorTree = GetComponent<BehaviorTree>();
    }

	public void FixedUpdate()
	{
        OnUpdate();

        if (m_monsterHPBar != null)
        {
            m_monsterHPBar.SetHpBarPosition();
        }
    }

	public void PlayerAttack()
	{
        Debug.Log("����");
	}


    // ������� �Ծ����� �ٵ� �ǰ� ��尡 ���� ���Ƿ� ��� ���� �����
    public void Damege(int p_hp, Vector3 p_force)
    {

    }
    
    public void Damege(int p_hp)
	{
        m_stat.Hp -= p_hp;
        m_monsterHPBar.HpBarUpdate();

        if (m_stat.Hp <= 0)
        {
            // ��� ó��
            Managers.Resource.Destroy(gameObject);
            m_monsterHPBar.CloseSceneUI();

            m_score.ScoreLogCreate(m_stat.Score);
            return;
        }
    }

    void Update()
    {
        // ���� ü�¹� �׽�Ʈ ����� �ֱ�
        if (Input.GetKeyDown(KeyCode.Keypad4))
        {
            Damege(10);
        }

        if (Input.GetKeyDown(KeyCode.Keypad5))
        {
            if (m_stat.Hp >= m_stat.MaxHp)
            {
                return;
            }

            m_stat.Hp += 10;
            m_monsterHPBar.HpBarUpdate();
        }
    }



}
