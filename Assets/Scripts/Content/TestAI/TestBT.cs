using System.Collections.Generic;
using UnityEngine;


public class TestBT : SkillTree 
{
    // �ν����Ϳ��� ���� �������ش�. ���� : ��ȹ�ʿ��� �����ϰڴٰ� �Ѵ�.
    [SerializeField]
    private Vector3[] m_wayPoints;
    [SerializeField]
    private float m_range = 5.0f;

    protected override SkillNode SetupTree()
    {
        // ������ �̸� ����
        SetData("MoveSpeed", (float)5.0f);
        SetData("CheckRange", m_range);

        // Sequence : ���� ��ȸ�ؼ� ����
        // Selector : �ѳ༮�� ����
        SkillNode root = new Selector(new List<SkillNode>
        {
            new Sequence(new List<SkillNode> {
                new TestCheckEnemy(transform),              // ��ġ üũ��
                new TestTargetToRun(transform),             // �߰� (���� �غ�)
                new TaskAttack(transform),                  // ����
            }),
            new TaskPatrol(transform, m_wayPoints),

		});
        
        return root;
    }


	private void OnDrawGizmos()
    { 
        DrawWayPoint();
        DrawCheckRange();

    }

    private void DrawCheckRange()
	{
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, m_range);
	}

    private void DrawWayPoint()
	{
        Gizmos.color = Color.red;

        if (m_wayPoints.Length == 0) {
            return;
        }
        int size = m_wayPoints.Length;
        for (int i = 0; i < size - 1; ++i) {
            Gizmos.DrawLine(m_wayPoints[i], m_wayPoints[i + 1]);
            Gizmos.DrawSphere(m_wayPoints[i], 0.5f);
        }
        Gizmos.DrawSphere(m_wayPoints[size - 1], 0.5f);
    }
}