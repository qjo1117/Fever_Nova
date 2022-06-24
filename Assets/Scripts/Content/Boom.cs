using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;

public class Boom : MonoBehaviour
{
    [SerializeField]
    private float               m_moveSpeed = 400.0f;               // ��ź�� �������� ��
    [SerializeField]
    private float               m_explosionRange = 5.0f;            // ���� �ݰ�
    [SerializeField]
    private float               m_detectRange = 2.0f;               // ���� �ݰ�

    // �̰͸� �ʿ��Ҳ����Ƽ� �����Ƽ�� �Ⱦ�
    private float               m_explosionDelayTime = 0.0f;        // ���� ���� �ð�
    [SerializeField]
    private float               m_explosionMaxDelayTime = 5.0f;     // �ִ� ���� �ð�
    private bool                m_isExplosion = false;

    private PlayerController    m_player = null;
    private Rigidbody           m_rigid = null;

    private int                 m_layer = 1 << (int)Layer.Monster | 1 << (int)Layer.Player;

    public void Create(Vector3 p_dir, PlayerController p_player)
	{
        gameObject.SetActive(true);
        m_player = p_player;
        transform.position = p_player.transform.position + p_dir;
        m_explosionDelayTime = 0.0f;
        m_isExplosion = false;
        m_rigid.AddForce(p_dir * m_moveSpeed);
    }

    void Start()
    {
        m_rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ExplosionDaleyCheck();
        Explosion();
    }

    private void ExplosionDaleyCheck()
	{
        m_explosionDelayTime += Time.deltaTime;

        if(m_explosionMaxDelayTime > m_explosionDelayTime) {
            return;
		}

        m_isExplosion = true;
    }

    

    private void Explosion()
	{
        if(m_isExplosion == false) {
            return;
		}

        // �� �κ��� ��üũ ���� ���ΰ� �����Ǹ� �Ҳ�
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_explosionRange, (int)m_layer);

        // �̺κ��� ������ Ȯ������ ����ɲ�
        List<TargetData> listMonster = new List<TargetData>();
        List<TargetData> listPlayer = new List<TargetData>();

        // ���� ������ ���� ������Ʈ�� ID, �����, ���߷� ���� ���� �����Ѵ�.
        foreach(Collider collider in colliders) {
            Vector3 dist = transform.position - collider.transform.position;        // ���� ���� �Ÿ��� üũ
            dist = dist.normalized;
            // ���� ���̾�����
            if (Layer.Monster.HasFlag((Layer)collider.gameObject.layer) == true) {
                MonsterController monster = collider.GetComponent<MonsterController>();
                listMonster.Add(new TargetData(monster.ID, monster.Attack, dist / 2.0f));
            }
            // �÷��̾� ���̾�����
            else if (Layer.Player.HasFlag((Layer)collider.gameObject.layer) == true) {
                // �� Cube (Ground)�� ������
                PlayerController player = collider.GetComponent<PlayerController>();
                Managers.Game.Player.Attack(player.ID, 30, dist);

            }
        }

        // �ϴ� ���������� ����Ŷ� �̷��� ����Ʈ�� ������ �ƴ�
        if (listMonster.Count != 0) {
            Managers.Game.Monster.Attack(listMonster);
        }

        gameObject.SetActive(false);
    }


	private void OnDrawGizmos()
	{
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, m_explosionRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_detectRange);
    }
}
