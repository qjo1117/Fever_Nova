using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* -------------------------------------------------------------------------------
		��� Action�� �־ ���ε��� �Լ��� �ڵ����� ȣ���ұ� ����������
		�ǹ���ٿ��� ���� �ð� üũ �ý����� ����, �Է¿� ���� �Լ� ȣ���̱⶧����
		����ϴ� ��ü���� üũ�ϴ� �������� �����޽��ϴ�.


		���� Component�� ������ ����ϴ� �� ������ �����ؼ�
		Ability�� ���������� ���� �ְ� ��������ϴ�.
------------------------------------------------------------------------------- */



// TODO :  �̰� Ŭ���� �Ŵ����� ����

[System.Serializable]
public class Ability 
{

	private float m_currentTime = 0.0f;         // ����ð�
	[SerializeField]
	private string m_name = "Unkown";
	[SerializeField]
	private float m_maxTime = 0.0f;             // �ִ�ð�
	[SerializeField]
	private bool m_isAction = false;            // üũ

	public float MaxTime { get => m_maxTime; set => m_maxTime = value; }
	public string Name { get => m_name; set => m_name = value; }
	public bool IsAction { get => m_isAction; set => m_isAction = value; }

	public void Init(float p_maxTime, string p_name)
	{
		m_maxTime = p_maxTime;
		m_name = p_name;
	}

	// �ൿ�� ������ ȣ��������.
	public void InitAction()
	{
		m_isAction = false;
		m_currentTime = 0.0f;
	}

	public void Update()
	{
		if (m_isAction == true)
		{
			return;
		}

		// �ð�üũ�ؾ��Ҷ��� üũ����.
		m_currentTime += Time.deltaTime;

		if (m_currentTime < m_maxTime)
		{
			return;
		}

		m_isAction = true;
	}
}


// ���� �̷��� �ϴ� ������ �ڷ�ƾ�� ���Ͼ

// ��Ÿ���� �����ϴ� ��ɵ鿡 ���ؼ�
// ��Ÿ�� ������ �Լ�, ���� �����ϱ� �����Ƽ� ����
public class AbilitySystem : MonoBehaviour
{
	[SerializeField]
	private List<Ability> m_listAbility = new List<Ability>();

	public List<Ability> Ability { get => m_listAbility; }

	// ����Ʈ
	//		- Ability
	//			- Name
	//			- MaxTime
	//			- IsAction


	public void Start()
	{
		
	}

	public void Clear()
	{
		// C#�� �ָ��Ѱ� �̷��� �������� �־����°ǰ�?
		m_listAbility.Clear();
	}


	public void AddAbility(string p_name, float p_maxTime)
	{
		Ability ability = new Ability();

		ability.Name = p_name;
		ability.MaxTime = p_maxTime;

		m_listAbility.Add(ability);
	}

	public void DelAbility(string p_name)
	{
		foreach(Ability ability in m_listAbility) {
			if(ability.Name.Contains(p_name) == true) {
				m_listAbility.Remove(ability);
				break;
			}
		}
	}


	// üũ�Լ�
	public bool IsAction(string p_name)
	{
		foreach (Ability ability in m_listAbility) {
			if (ability.Name.Contains(p_name) == true) {
				return ability.IsAction;
			}
		}

		return false;
	}

	public void Update()
	{
		// ��ü������ �˻����ش�.
		foreach(Ability ability in m_listAbility) {
			ability.Update();
		}
	}

}

