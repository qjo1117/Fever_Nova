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
    public float    moveSpeed = 50.0f;
    public float    evasionSpeed = 10.0f;
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

    private Vector3         m_move = Vector3.zero;

    private float           m_lookRotation = 0.0f;
    private float           m_explosionDelayTime = 0.5f;
    private float           m_explosionTime = 0.0f;
    private bool            m_isExplosion = false;

    [SerializeField]
    private float           m_evasionDelayTime = 1.0f;      // ��� �ð�
    private float           m_evasionTime = 0.0f;           // ���� �ð�

    private List<Boom>      m_listBoom = new List<Boom>();

    #endregion

    #region ������Ƽ

    public PlayerStat Stat { get => m_stat; set => m_stat = value; }
    public PlayerState State { get => m_state; set => m_state = value; }

    #endregion


    private void Start()
	{
        base.Init();
    }

    private void Update()
    {
        UpdateState();
        UpdateInput();
    }

	private void FixedUpdate()
	{
        base.OnUpdate();
    }

	#region ���� ������Ʈ

	public void UpdateState()
	{
        switch(m_state) {
            case PlayerState.Evasion:
                EvasionState();
                break;
		}
	}

    public void EvasionState()
	{
        if(m_rigidValue.Velocity == Vector3.zero) {
            m_state = PlayerState.Idle;
		}
	}

	#endregion


	#region �Է� ������Ʈ

	public void InputMouse()
	{
        InputMouseRotation();
        InputShoot();
    }

    private void InputMouseRotation()
    {
        Vector3 playerToScreenPos = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 mousePos = Input.mousePosition;
        Vector3 tempPos = mousePos - playerToScreenPos;
        m_lookRotation = Mathf.Atan2(tempPos.y, tempPos.x);

        transform.rotation = Quaternion.Euler(0f , (-m_lookRotation * Mathf.Rad2Deg) + 90f , 0f);
    }

    public void InputShoot()
	{
        if(Managers.Input.GetKeyDown(UserKey.Shoot) == true){
            Managers.Resource.Instantiate("Boom", Managers.Game.Player.transform);
		}

        // Ŭ��������
        if (Managers.Input.GetKey(UserKey.Shoot) == true) {
            m_isExplosion = true;
            m_explosionTime += Time.deltaTime;
            
        }

        // ��������
        if (Managers.Input.GetKeyUp(UserKey.Shoot) == true) {
            // TODO : �б��ؼ� ���������϶��� �ٸ� ��쵵 üũ���ش�.
            m_explosionTime = 0.0f;
            m_isExplosion = false;
        }


        // ��ź�� ���� ���
        if (m_isExplosion == true) {
            // ������ �ð��� �ʰ��� ���
            if(m_explosionTime <= m_explosionDelayTime) {
                // TODO : ��ź�� ���¸� ��ȯ��Ų��.

			}
		}
    }

    public void UpdateInput()
	{
        // ȸ�����϶��� �ٸ� ���´� �Ұ���
        if (m_state == PlayerState.Evasion) {
            return;
        }

        InputMouse();

        m_state = PlayerState.Idle;
        if (Input.anyKey == false) {
            return;
		}

        InputMove();
        InputAddForce();
    }

    public void InputAddForce()
	{
        m_evasionTime += Time.deltaTime;

        if (m_evasionDelayTime >= m_evasionTime) {
            return;
		}

        if(Managers.Input.GetKeyDown(UserKey.Evasion) == true && m_state == PlayerState.Run) {
            AddForce(m_move * m_stat.evasionSpeed);
            m_evasionTime = 0.0f;
            m_state = PlayerState.Evasion;
        }
	}

    public void InputMove()
	{
        m_move = Vector3.zero;

        if(Managers.Input.GetKey(UserKey.Forward) == true) {
            m_move.z += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Backward) == true) {
            m_move.z -= 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Right) == true) {
            m_move.x += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Left) == true) {
            m_move.x -= 1.0f;
        }

        if(m_move != Vector3.zero) {
            m_state = PlayerState.Run;
        }

        AddMovement(m_move.normalized * m_stat.moveSpeed);
    }

	#endregion
}
