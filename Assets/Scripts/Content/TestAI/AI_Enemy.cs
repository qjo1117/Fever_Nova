using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Enemy : MonoBehaviour
{
    [SerializeField]
    public MonsterStat m_stat;
    public UI_MonsterHPBar m_hpBar;
    //��ų����Ʈ �߰��صѰ�
    //�ּ�ó���� ���� = ��ų�� ����ȭ�� ������ ����...
    //AI ���� ���� ����
    //  ����� Detect�� �����ϸ� ������ �÷��̾� ��ġ���� ����
    //  �̰� Detect ���� �� AI�� ���� skill �� ���� Ȥ�� �ൿ��Ŀ� ���� ��ų�� ���� ��
    //  ������ ��ų�� �����Ÿ������� ���ٽ�ų ����
    //  ������ ��...��ų�� ����ȭ�� ������ ���� �ؼ� �ٷ� chase�� �Ѿ�� �س���
    //  AI_Enemy_01,AI_Enemy_Boss ��ũ��Ʈ�� �ش���� ���뿹��
    //[SerializeField]
    //public List<Skill> m_skills;
    protected BT_Root m_brain;
    protected AI.EnemyType m_enemyType;

    public void Init()
    {
        CreateBehaviorTreeAIState();
    }

    public virtual void AddPatrolPoint(Vector3 _position) { }

    protected virtual void CreateBehaviorTreeAIState() { }

    protected void Update()
    {
        m_brain.Tick();
    }
}
