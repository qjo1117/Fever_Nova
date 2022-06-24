using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterController : MonoBehaviour
{

    private int                 m_id = -1;
    private int                 m_attack = 30;
    private int                 m_hp = 100;

    // �Ƹ� �߰� �ɲ�������
    private int m_playerId = -1;

    private AbilitySystem       m_abilitySystem = null;
    private MonsterSkill        m_currentSkill = MonsterSkill.None;

    // ���Ÿ�, �ٰŸ�
    public enum MonsterSkill {
        None,
        Attack,

	}

    public int ID { get => m_id; set => m_id = value; }
    public int Attack { get => m_attack; }
    public int Hp { get => m_hp; set => m_hp = value; }

    // Start is called before the first frame update
    void Start()
    {
        m_abilitySystem = GetComponent<AbilitySystem>();

        m_abilitySystem.AddSkill(MonsterSkill.Attack.ToString(), 1.0f, PlayerAttack, "Explosion_FX");
    }

    public void PlayerAttack()
	{
        Debug.Log("����");
	}

    // Update is called once per frame
    void Update()
    {
        foreach(Ability ability in m_abilitySystem.Ability) {
            if(ability.IsAction == true) {
                ability.Action();
			}
		}
    }


    private void CheckToSelectSkill()
	{
        if(m_currentSkill != MonsterSkill.None) {
            return;
		}

        List<int> listSkill = new List<int>();
        List<Ability> abilitys = m_abilitySystem.Ability;
        int size = abilitys.Count;

        // ��ų �ߵ� ���� ���θ� üũ�Ѵ�.
        for (int i = 0; i < size; ++i) {
            if(abilitys[i].IsAction == true) {
                listSkill.Add(i);
            }
        }

        // ���� ������ �ε������� ��ų ������ �����Ѵ�.
        size = listSkill.Count;
        for (int i = 0; i < size; ++i) {
            // Skill�� ���
            if((abilitys[i] as Skill) != null) {
                Skill skill = (Skill)abilitys[i];

                // ���⼭ ���ʹ� �����ֱ��
                // PlayerController target = Manager.Game.Player[m_playerId];
                // Vector3 dist = transform.position - target.transform.position;
                // if(dist.magnitude > skill.DetectedRange) {           // ���������ȿ� ���� ���
                //      // ������ �����ϵ� �� �ε��� ���ͼ� Ȯ�� �����ϵ� ����
                // }
			}
        }
	}
}
