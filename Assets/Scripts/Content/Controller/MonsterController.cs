using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : BaseController
{

    [SerializeField]
    private MonsterStat         m_stat = new MonsterStat();

    private BehaviorTree        m_behaviorTree = null;

    public MonsterStat          Stat { get => m_stat; set => m_stat = value; }

    void Start()
    {
        Init();

        m_behaviorTree = GetComponent<BehaviorTree>();
    }

	public void FixedUpdate()
	{
        OnUpdate();
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

	}

    void Update()
    {
    }



}
