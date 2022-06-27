using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBT : BehaviorTree
{
    [SerializeField]
    private Vector3[] m_wayPoints;

    [SerializeField]
    private float m_moveSpeed = 5.0f;

    [SerializeField]
    private float m_checkRange = 5.0f;

    // Start, Update�� ���� ȣ�� �����൵ ��
    protected override BehaviorNode SetupTree()
	{
        // �̶� ������ ������ ���ش�.
        SetData("MoveSpeed", m_moveSpeed);
        SetData("CheckRange", m_checkRange);

        // Sequence : ���� ��ȸ�ؼ� ����
        // Selector : �ѳ༮�� ����
        BehaviorNode root = new Selector(new List<BehaviorNode>
        {
            new Sequence(new List<BehaviorNode>
			{
                new TestCheckRange(this),
                new TestMoveToTarget(this),
                new Inverter(new TestAttack(this)),
			}),
            new TestPatrol(this, m_wayPoints),
        });

        return root;
    }


	private void OnDrawGizmos()
	{
        DrawWayPoint();
        DrawCheckRange();
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

    private void DrawCheckRange()
	{
        Gizmos.color = Color.green;

        Gizmos.DrawWireSphere(transform.position, m_checkRange);
    }

}
