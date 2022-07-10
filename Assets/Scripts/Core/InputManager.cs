using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Define;
using System;
using UnityEngine.EventSystems;

/*
 * InputManager는 입력을 제어 받는 것도 있지만 해당하는 
 * 유저에서 키를 설정한 값이 체크가 되었을 경우 함수를 호출 할 수 있도록 만든 것입니다.
 * 굳이 안만들어도 될꺼같은데 그냥 GetKey 눌렸을지 확인하는 것 까지 다시 제작했습니다.
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

// 나중에 Json으로 저장할 용도
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

	#region 프레임 워크
	public void Init()
	{
		m_dicKeys.Clear();

		// 기본적인 값을 등록
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
		// 순회
		foreach (var pair in m_dicKeys) {
			KeyInfo info = pair.Value;

			// 코드가 눌렸는지 체크한다.
			bool check = GetKeyCode(info.listKey);

			// 눌렸을때
			if(check == true) {
				// 누른게 확인 안됬을때
				if(info.down == false && info.press == false) {
					info.down = true;
					info.press = true;
				}
				// 계속 누르고 있었을때
				else {
					info.down = false;
				}
			}
			// 안눌렸을때
			else {
				// 전에 키를 눌렀을때
				if(info.down == true || info.press == true) {
					info.up = true;
					info.down = false;
					info.press = false;
				}
				// 전에 키를 땠을때
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

	}

	public void Load()
	{
		// TODO : 저장 만들기
		// 아직은 귀찮

		KeyInfoJson json = new KeyInfoJson();
		foreach (var item in m_dicKeys)
		{
			json.keyInfos.Add(new KeyInfoJson.KeyInfos { key = item.Value.key, listKey = item.Value.listKey });
		}

		string strJson = JsonUtility.ToJson(json);
		Debug.Log(strJson);


		m_dicKeys.Clear();
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

	#region 기능 함수
	public bool GetKey(UserKey p_key)
	{
		// 등록된 키가 없을 경우
		if(m_dicKeys.ContainsKey(p_key) == false) {
			return false;
		}

		// 클릭햇을때
		if(m_dicKeys[p_key].down == true || m_dicKeys[p_key].press == true) {
			return true;
		}

		// 아무것도 아니였을때
		return false;
	}

	public bool GetKeyDown(UserKey p_key)
	{
		// 등록된 키가 없을 경우
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return false;
		}

		// 클릭햇을때
		if (m_dicKeys[p_key].down == true) {
			return true;
		}

		// 아무것도 아니였을때
		return false;
	}

	public bool GetKeyUp(UserKey p_key)
	{
		// 등록된 키가 없을 경우
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return false;
		}

		// 클릭햇을때
		if (m_dicKeys[p_key].up == true) {
			return true;
		}

		// 아무것도 아니였을때
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
		// 해당하는 키의 객체가 없을 경우 생성
		if(m_dicKeys.ContainsKey(p_key) == false)  {
			m_dicKeys[p_key] = new KeyInfo();
			m_dicKeys[p_key].key = p_key;
		}

		// 배열 접근 쓰기 귀찮
		KeyInfo info = m_dicKeys[p_key];

		// 만약 리스트안에 코드가 존재할 경우
		if(IsListKeyCode(info.listKey, p_keycode) == true) {
			return;
		}

		// 없을 경우
		info.listKey.Add(p_keycode);
	}
	public void DelKey(UserKey p_key, KeyCode p_keycode = KeyCode.None)
	{
		// 맵핑
		if (m_dicKeys.ContainsKey(p_key) == false) {
			return;
		}

		// 배열 접근 쓰기 귀찮
		KeyInfo info = m_dicKeys[p_key];

		// 싹다 삭제
		if (p_keycode == KeyCode.None) {
			info.listKey.RemoveRange(0, info.listKey.Count);
		}
		// 지정한게 있으면 삭제
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
