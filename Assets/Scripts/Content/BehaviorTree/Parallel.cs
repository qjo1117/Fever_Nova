using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallel : Composite
{
	private bool m_isAllSuccessFail = false;
	private bool m_isSuccessOnAll = false;
	private bool m_isFailOnAll = false;

	private int m_minSuccess = 0;
	private int m_minFail = 0;

	public Parallel(bool p_successOnAll = true, bool p_failOnAll = true)
	{
		m_isAllSuccessFail = true;
		m_isSuccessOnAll = p_successOnAll;
		m_isFailOnAll = p_failOnAll;
	}

	public Parallel(int p_minSuccess, int p_minFail)
	{
		m_minSuccess = p_minSuccess;
		m_minFail = p_minFail;
	}


	public override BehaviorStatus Update() 
	{
		// üũ�ؾ��� �� ������ Ȯ���ϱ��� ĳ���Ѵ�.
		int successSize = m_minSuccess;
		int failSize = m_minFail;

		// ���� ���� ���� Flag�� �����ִ��� Ȯ���Ѵ�.
		if(m_isAllSuccessFail == true) {
			// ������ �����ִ� ���
			if(m_isSuccessOnAll == true) {
				successSize = m_listChildren.Count;
			}
			else {
				successSize = 1;
			}

			// ���а� �����ִ� ���
			if(m_isFailOnAll == true) {
				failSize = m_listChildren.Count;
			}
			else {
				failSize = 1;
			}
		}

		// ����, ���� ������ ī������ ����
		int successCount = 0;
		int failCount = 0;

		// ��ȸ�� ���鼭 ����Ƚ��, ����Ƚ���� ī�����Ѵ�.
		int size = m_listChildren.Count;
		for (int i = 0; i < size; ++i) {
			BehaviorStatus status = m_listChildren[i].Update();

			if(status == BehaviorStatus.Success) {
				successCount += 1;
			}
			else if (status == BehaviorStatus.Failure){
				failCount += 1;
			}
		}

		// ���� ������ ���� Parallel ���¸� �����Ѵ�.
		if(successCount >= successSize) {
			m_status = BehaviorStatus.Success;
		}
		else if (failCount >= failSize) {
			m_status = BehaviorStatus.Failure;
		}
		else {
			m_status = BehaviorStatus.Running;
		}

		return m_status;
	}
}
