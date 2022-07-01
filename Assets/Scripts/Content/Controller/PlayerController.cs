using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

[System.Serializable]
public class PlayerStat 
{
    public int      id = 1;
    public int      hp = 100;
    public int      attack = 70;
    public float    moveSpeed = 10.0f;
    public int      score = 0;
}

// ��ȹ���ʿ��� ���� ������ ����� ������ Class�� ���� ���ǵ� ü�� ����� ������.
public class PlayerController : BaseController
{
    public enum PlayerState {
        Idle,
        Run,
        Shoot,
        Evasion,
    }

    #region ����

    [SerializeField]
    private PlayerStat      m_stat = new PlayerStat();
    [SerializeField]
    private PlayerState     m_state = PlayerState.Idle;

    #endregion

    #region ������Ƽ

    public PlayerStat Stat { get => m_stat; set => m_stat = value; }
    public PlayerState State { get => m_state; set => m_state = value; }

    #endregion


    private void Start()
	{
        base.Init();
        Managers.Input.RegisterKeyEvent(InputUpdate);
    }

    private void Update()
    {
    }

	private void FixedUpdate()
	{
        base.OnUpdate();
    }

    public void InputUpdate()
	{


        if(Input.anyKey == false) {
            return;
		}

        InputMove();
        InputAddForce();
    }

    public void InputAddForce()
	{
        if(Managers.Input.GetKeyDown(UserKey.Evasion) == true) {
            AddForce(Vector3.right * 100.0f);
		}
	}

    public void InputMove()
	{
        Vector3 move = Vector3.zero;
        if(Managers.Input.GetKey(UserKey.Forward) == true) {
            move.z += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Backward) == true) {
            move.z -= 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Right) == true) {
            move.x += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Left) == true) {
            move.x -= 1.0f;
        }

        AddMovement(move.normalized * m_stat.moveSpeed);
    }

}
