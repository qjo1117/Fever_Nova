using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------------------------------------------------
		��� Action�� �־ ���ε��� �Լ��� �ڵ����� ȣ���ұ� ����������
		�ǹ���ٿ��� ���� �ð� üũ �ý����� ����, �Է¿� ���� �Լ� ȣ���̱⶧����
		����ϴ� ��ü���� üũ�ϴ� �������� �����޽��ϴ�.
------------------------------------------------------------------------------- */


// ��Ÿ���� �����ϴ� ��ɵ鿡 ���ؼ�
// ��Ÿ�� ������ �Լ�, ���� �����ϱ� �����Ƽ� ����
public class Ability : MonoBehaviour
{
	private float m_currentTime = 0.0f;         // ����ð�
	[SerializeField]
	private string m_name = "Unkown";
	[SerializeField]
	private float m_maxTime = 0.0f;             // �ִ�ð�
	[SerializeField]
	private bool m_isAction = false;			// üũ

	public void Init(float p_maxTime, string p_name)
	{
		m_maxTime = p_maxTime;
		m_name = p_name;
	}

	// �̷� üũ�Ǵ� �͵��� �Լ��� ���ؼ� ����ϴ�.
	public bool IsAction() 
	{ 
		return m_isAction; 
	}

	// �ൿ�� ������ ȣ��������.
	public void Action()
	{
		m_isAction = false;
		m_currentTime = 0.0f;
	}

	private void Update()
	{
		if(m_isAction == true) {
			return;
		}

		// �ð�üũ�ؾ��Ҷ��� üũ����.
		m_currentTime += Time.deltaTime;

		if (m_currentTime < m_maxTime) {
			return;
		}

		m_isAction = true;
	}
}

