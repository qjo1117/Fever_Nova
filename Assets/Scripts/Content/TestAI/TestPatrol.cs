using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPatrol : Exacution {
    private Vector3[] m_wayPoints = null;

    private bool m_isWait = false;
    private float m_waitCount = 0.0f;
    private float m_waitTime = 1.0f;
    private int m_currentWayIndex = 0;

    private float m_speed = 0.0f;

    // �ڷᰡ �ʿ��� ��� �ʱ�ȭ�� �޴´�.
    // ���� �����͸� ������ private�� �ݴ°ͺ��ٴ� public���� ��� �����Ϳ��� ������ �����ϰ� ���������� ��
    public TestPatrol(BehaviorTree p_tree, Vector3[] p_wayPoints) : base(p_tree)
    {
        m_wayPoints = p_wayPoints;

        m_speed = m_tree.GetData<float>("MoveSpeed");
    }

    public override BehaviorStatus Update()
    {
        if (m_isWait == true) {
            // ��� ��ٷ��ش�.
            m_waitCount += Time.deltaTime;
            if (m_waitCount >= m_waitTime) {
                m_isWait = false;
            }
        }
        else {
            // �ƹ��͵� ������ ����
            if (m_wayPoints.Length == 0) {
                return BehaviorStatus.Failure;
            }

            // ��������Ʈ�� �����ͼ� �˻縦 �Ѵ�.
            Vector3 wayPoint = m_wayPoints[m_currentWayIndex];
            // ��������Ʈ�� ������ ��� ����Ʈ �ε����� ��ü���ش�.
            if (Vector3.Distance(m_transform.position, wayPoint) < 0.01f) {
                m_transform.position = wayPoint;
                m_waitCount = 0f;
                m_isWait = true;

                m_currentWayIndex = (m_currentWayIndex + 1) % m_wayPoints.Length;
            }
            // �ƴҰ�� ��������Ʈ������ ����.
            else {
                m_transform.position = Vector3.MoveTowards(m_transform.position, wayPoint, m_speed * Time.deltaTime);
                m_transform.LookAt(wayPoint);
            }
        }

        // ���� ���´� ���жǴ� �����̰� �ȴ�.
        m_status = BehaviorStatus.Running;
        return m_status;

    }
}
