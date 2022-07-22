using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ������ �������� ����
public class TargetData 
{
    public TargetData(int p_id, int p_attack, Vector3 p_force)
    {
        id = p_id;
        attack = p_attack;
        force = p_force;
    }
    public int id = -1;
    public int attack = 0;
    public Vector3 force = Vector3.zero;
}

public class MonsterManager : MonoBehaviour
{
    // ���� ����ϰ� �ִ� ������ ���� �˾Ƴ���.
    private List<MonsterController> m_listMonster = new List<MonsterController>();
    private List<TargetData> m_listTargetData = new List<TargetData>();                 // ���� ���޿�


    void Update()
    {
        AttackUpdate();
        DieUpdate();
    }

    // InGameScene���� ȣ���մϴ�.
	public void Init()
	{

    }

    [ContextMenu("TestSpawn")]
    public void TestSpawn()
	{
        for (int i = 0; i < 100; ++i) {
            Managers.Resource.Instantiate("Monster", transform);
        }
	}


	// TODO : Server
	public void Damege(int p_id, int p_attack, Vector3 p_force)
	{
        m_listTargetData.Add(new TargetData(p_id, p_attack, p_force));
    }

    public void Damege(List<TargetData> p_listTargetData)
    {
        foreach(TargetData data in p_listTargetData) {
            m_listTargetData.Add(data);
        }
    }

    // Attack����� �ִ� �༮�鿡�� ������� ������.
	private void AttackUpdate()
	{
        if(m_listTargetData.Count == 0) {
            return;
		}

		foreach(TargetData data in m_listTargetData) {
            Debug.Log(data.id);
            m_listMonster[data.id].Stat.Hp -= data.attack;
		}

        m_listTargetData.Clear();

    }

    // ������ ����?
    private void DieUpdate()
	{
   //     List<MonsterController> listDie = new List<MonsterController>();
   //     foreach (MonsterController monster in m_listMonster) {
   //         if(monster.Hp <= 0) {
   //             listDie.Add(monster);
			//}
   //     }

   //     foreach (MonsterController monster in listDie) {
   //         m_listMonster.Remove(monster);
   //     }
    }
}
