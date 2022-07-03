using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;
using System;
using UnityEngine.EventSystems;

/*
 * InputManager�� �Է��� ���� �޴� �͵� ������ �ش��ϴ� 
 * �������� Ű�� ������ ���� üũ�� �Ǿ��� ��� �Լ��� ȣ�� �� �� �ֵ��� ���� ���Դϴ�.
 * ���� �ȸ��� �ɲ������� �׳� GetKey �������� Ȯ���ϴ� �� ���� �ٽ� �����߽��ϴ�.
*/

[System.Serializable]
class KeyInfo 
{
	public UserKey key = UserKey.End;
	public List<KeyCode> listKey = new List<KeyCode>();
	public bool down = false;
	public bool press = false;
	public bool up = false;
}

// ���߿� Json���� ������ �뵵
class KeyInfoJson 
{
	[System.Serializable]
	public class KeyInfos {
		public UserKey key;
		public List<KeyCode> listKey = new List<KeyCode>();
	}

	public List<KeyInfos> keyInfos = new List<KeyInfos>();
}


public class InputManager
{
    private Dictionary<UserKey, KeyInfo> m_dicKeys = new Dictionary<UserKey, KeyInfo>();

	public Action KeyEvent = null;
	public Action<Define.Mouse> MouseEvent = null;

	private bool m_isPressed = false;
	private float m_pressTime = 0.5f;

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
		foreach (var pair in m_dicKeys) {
			KeyInfo info = pair.Value;

			// �ڵ尡 ���ȴ��� üũ�Ѵ�.
			bool check = GetKeyCode(info.listKey);

			// ��������
			if(check == true) {
				// ������ Ȯ�� �ȉ�����
				if(info.down == false && info.press == false) {
					info.down = true;
					info.press = true;
				}
				// ��� ������ �־�����
				else {
					info.down = false;
				}
			}
			// �ȴ�������
			else {
				// ���� Ű�� ��������
				if(info.down == true || info.press == true) {
					info.up = true;
					info.down = false;
					info.press = false;
				}
				// ���� Ű�� ������
				else if(info.up == true) {
					info.up = false;
				}
			}
		}


		if (EventSystem.current.IsPointerOverGameObject()) {
			return;
		}

		if (Input.anyKey == true && KeyEvent != null) {
			KeyEvent.Invoke();
		}

		if (MouseEvent != null) {
			if (Input.GetMouseButton(0) == true) {
				if (m_isPressed == false) {
					MouseEvent.Invoke(Define.Mouse.PointerDown);
					m_pressTime = Time.time;
				}
				MouseEvent.Invoke(Define.Mouse.Press);
				m_isPressed = true;
			}
			else {
				if (m_isPressed == true) {
					if (Time.time < m_pressTime + 0.2f)
						MouseEvent.Invoke(Define.Mouse.Click);
					MouseEvent.Invoke(Define.Mouse.PointerUp);
				}
				m_isPressed = false;
				m_pressTime = 0;
			}
		}
	}

	public void Clear()
	{
		// TODO : ���� �����
		// ������ ����

		KeyInfoJson json = new KeyInfoJson();
		foreach(var item in m_dicKeys) {
			json.keyInfos.Add(new KeyInfoJson.KeyInfos { key = item.Value.key, listKey = item.Value.listKey });
		}

		string strJson = JsonUtility.ToJson(json);
		Debug.Log(strJson);


		m_dicKeys.Clear();
	}

	public void Load()
	{
		// TODO : �ε��� ������ �ִ��� ���θ� üũ�ϰ�
		// ������ �ε��ϰ� ������ ���´�� ����.
	}

	public void RegisterKeyEvent(Action p_keyEvent)
	{
		KeyEvent -= p_keyEvent;
		KeyEvent += p_keyEvent;
	}

	public void RegisterMouseEvent(Action<Define.Mouse> p_mouseEvent)
	{
		MouseEvent -= p_mouseEvent;
		MouseEvent += p_mouseEvent;
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
