using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;
using System;

/*
 * InputManager�� �Է��� ���� �޴� �͵� ������ �ش��ϴ� 
 * �������� Ű�� ������ ���� üũ�� �Ǿ��� ��� �Լ��� ȣ�� �� �� �ֵ��� ���� ���Դϴ�.
 * ���� �ȸ��� �ɲ������� �׳� GetKey �������� Ȯ���ϴ� �� ���� �ٽ� �����߽��ϴ�.
*/

class KeyInfo 
{
	public UserKey key = UserKey.End;
	public List<KeyCode> listKey = new List<KeyCode>();
	public bool down = false;
	public bool press = false;
	public bool up = false;
	public List<Action> listDownEvent = new List<Action>();
	public List<Action> listPressEvent = new List<Action>();
	public List<Action> listUpEvent = new List<Action>();
}

// ���߿� Json���� ������ �뵵
class KeyInfoJson 
{
	public UserKey key = UserKey.End;
	public List<KeyCode> listKey = new List<KeyCode>();
}

public class InputManager
{
    private Dictionary<UserKey, KeyInfo> m_dicKeys = new Dictionary<UserKey, KeyInfo>();

	#region ������ ��ũ
	public void Init()
	{
		m_dicKeys.Clear();

		// �⺻���� ���� ���
		AddKey(UserKey.Forward, KeyCode.W);
		AddKey(UserKey.Forward, KeyCode.UpArrow);
		AddKey(UserKey.Backward, KeyCode.S);
		AddKey(UserKey.Backward, KeyCode.DownArrow);
		AddKey(UserKey.Right, KeyCode.D);
		AddKey(UserKey.Right, KeyCode.RightArrow);
		AddKey(UserKey.Left, KeyCode.A);
		AddKey(UserKey.Left, KeyCode.LeftArrow);
		AddKey(UserKey.Evasion, KeyCode.Space);
		AddKey(UserKey.Shoot, KeyCode.Mouse0);

	}

	public void Update()
	{
		// ��ȸ
		foreach(var pair in m_dicKeys) {
			KeyInfo info = pair.Value;

			// �ڵ尡 ���ȴ��� üũ�Ѵ�.
			bool check = GetKeyCode(info.listKey);

			// ��������
			if(check == true) {
				// ������ Ȯ�� �ȉ�����
				if(info.down == false && info.press == false) {
					info.down = true;
					info.press = true;

					foreach(Action action in info.listDownEvent) {
						action.Invoke();
					}
				}
				// ��� ������ �־�����
				else {
					info.down = false;

					foreach (Action action in info.listPressEvent) {
						action.Invoke();
					}
				}
			}
			// �ȴ�������
			else {
				// ���� Ű�� ��������
				if(info.down == true || info.press == true) {
					info.up = true;
					info.down = false;
					info.press = false;

					foreach (Action action in info.listUpEvent) {
						action.Invoke();
					}
				}
				// ���� Ű�� ������
				else if(info.up == true) {
					info.up = false;
				}
			}
		}

	}

	public void End()
	{
		// TODO : ���� �����
		// ������ ����
	}

	public void Load()
	{
		// TODO : �ε��� ������ �ִ��� ���θ� üũ�ϰ�
		// ������ �ε��ϰ� ������ ���´�� ����.
	}
	#endregion

	#region ��� �Լ�
	public bool GetKey(UserKey p_key)
	{
		// ��ϵ� Ű�� ���� ���
		if(m_dicKeys.ContainsKey(p_key) == false) {
			return false;
		}

		// Ŭ��������
		if(m_dicKeys[p_key].down == true || m_dicKeys[p_key].press == true) {
			return true;
		}

		// �ƹ��͵� �ƴϿ�����
		return false;
	}

	public bool GetKeyDown(UserKey p_key)
	{
		// ��ϵ� Ű�� ���� ���
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return false;
		}

		// Ŭ��������
		if (m_dicKeys[p_key].down == true) {
			return true;
		}

		// �ƹ��͵� �ƴϿ�����
		return false;
	}

	public bool GetKeyUp(UserKey p_key)
	{
		// ��ϵ� Ű�� ���� ���
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return false;
		}

		// Ŭ��������
		if (m_dicKeys[p_key].up == true) {
			return true;
		}

		// �ƹ��͵� �ƴϿ�����
		return false;
	}

	public bool GetKeyUpOrAll(UserKey p_key1, UserKey p_key2, UserKey p_key3, UserKey p_key4)
	{
		return GetKeyUp(p_key1) || GetKeyUp(p_key2) || GetKeyUp(p_key3) || GetKeyUp(p_key4);
	}

	private bool GetKeyCode(List<KeyCode> p_keycode)
	{
		foreach (KeyCode code in p_keycode) {
			if (Input.GetKey(code)) {
				return true;
			}
		}

		return false;
	}

	public void AddKey(UserKey p_key, KeyCode p_keycode)
	{
		// �ش��ϴ� Ű�� ��ü�� ���� ��� ����
		if(m_dicKeys.ContainsKey(p_key) == false)  {
			m_dicKeys[p_key] = new KeyInfo();
			m_dicKeys[p_key].key = p_key;
		}

		// �迭 ���� ���� ����
		KeyInfo info = m_dicKeys[p_key];

		// ���� ����Ʈ�ȿ� �ڵ尡 ������ ���
		if(IsListKeyCode(info.listKey, p_keycode) == true) {
			return;
		}

		// ���� ���
		info.listKey.Add(p_keycode);
	}


	public void AddDownListner(UserKey p_key, Action p_action)
	{
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return;
		}

		// �迭 ���� ���� ����
		KeyInfo info = m_dicKeys[p_key];
		// ���� ���
		info.listDownEvent.Add(p_action);
	}

	public void AddPressListner(UserKey p_key, Action p_action)
	{
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return;
		}

		// �迭 ���� ���� ����
		KeyInfo info = m_dicKeys[p_key];
		// ���� ���
		info.listPressEvent.Add(p_action);
	}


	public void AddUpListner(UserKey p_key, Action p_action)
	{
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return;
		}

		// �迭 ���� ���� ����
		KeyInfo info = m_dicKeys[p_key];
		// ���� ���
		info.listUpEvent.Add(p_action);
	}

	public void DelUpListner(UserKey p_key, Action p_action)
	{
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return;
		}

		// �迭 ���� ���� ����
		KeyInfo info = m_dicKeys[p_key];
		// ���� ���
		info.listUpEvent.Remove(p_action);
	}

	public void DelPressListner(UserKey p_key, Action p_action)
	{
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return;
		}

		// �迭 ���� ���� ����
		KeyInfo info = m_dicKeys[p_key];
		// ���� ���
		info.listPressEvent.Remove(p_action);
	}


	public void DelDownListner(UserKey p_key, Action p_action)
	{
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return;
		}

		// �迭 ���� ���� ����
		KeyInfo info = m_dicKeys[p_key];
		// ���� ���
		info.listDownEvent.Remove(p_action);
	}

	public void DelKey(UserKey p_key, KeyCode p_keycode = KeyCode.None)
	{
		// ����
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return;
		}

		// �迭 ���� ���� ����
		KeyInfo info = m_dicKeys[p_key];

		// �ϴ� ����
		if (p_keycode == KeyCode.None) {
			info.listKey.RemoveRange(0, info.listKey.Count);
		}
		// �����Ѱ� ������ ����
		else {
			info.listKey.Remove(p_keycode);
		}
	}

	private bool IsListKeyCode(List<KeyCode> p_listKey, KeyCode p_keycode)
	{
		foreach (KeyCode key in p_listKey) {
			if (key == p_keycode) {
				return true;
			}
		}

		return false;
	}
	#endregion
}
