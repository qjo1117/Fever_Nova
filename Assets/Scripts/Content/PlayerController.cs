using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

// ��ȹ���ʿ��� ���� ������ ����� ������ Class�� ���� ���ǵ� ü�� ����� ������.

public class PlayerController : MonoBehaviour
{
    public enum PlayerState {
        Idle,
        Run,
        Shoot,
        Evasion,
    }

    [SerializeField]
    private PlayerState m_state = PlayerState.Idle;             // ���� ����
    private PlayerState m_beState = PlayerState.Idle;           // ���� ����

    private float       m_moveSpeed = 10.0f;
    private Vector3     m_move = Vector3.zero;

    private float       m_evasionSpeed = 500.0f;
    private Vector3     m_evasionTarget = Vector3.zero;

    private int         m_id = -1;                      // ���Ͱ� �ν��� �� �ְ� �ҷ��� �߱� �޴� ���̵�
    private bool        m_isMain = false;               // �����̿��� Ű���� �Է��� ���� �� �ֱ� ����

    private Rigidbody   m_rigid = null;
    private Ability     m_evasionAbility = null;                // ȸ�� üũ
    private Ability     m_boomAbility = null;                   // ��ź üũ

	#region ������Ƽ
	public Rigidbody Rigid { get { return m_rigid; } }
    public PlayerState State { get { return m_state; } }
    public int ID { get { return m_id; } }
	#endregion

	private void Start()
	{
        m_rigid = GetComponent<Rigidbody>();
        m_evasionAbility = GetComponent<Ability>();
        m_evasionAbility.Init(2.0f, "Evasion");
        m_boomAbility.Init(0.3f, "Boom");

    }

    private void Update()
    {
        InputUpdate();
        UpdateState();
    }

	private void FixedUpdate()
	{

    }

	// �����ϱ� ���� �ʱ�ȭ�� �� �ؼ� ���̵� �� �߱�����.
	public void Init(int p_id, bool p_isMain)
	{
        // ���̵�� ���������� �Ǻ������� ���� ��
        m_id = p_id;
        m_isMain = p_isMain;
    }


	private void InputUpdate()
    {
        // ���� �÷��̾ �ƴϸ� ����
        if(m_isMain == false) {
            return;
		}

        // Up 
        InputMouseRotation();

        if (Input.anyKey == false) {
            return;
        }

        InputMove();
        InputAction();
    }


    private void UpdateState()
	{
        // ���� �˻縦 ���� ���ص� �ɶ�
        if(m_beState == m_state) {

            // ���� ��ȯ �˻縦 �ؾ��Ҷ�
            switch (m_state) {
                case PlayerState.Evasion:
                    EvasionPressState();
                    break;
                case PlayerState.Run:
                    RunPressState();
                    break;
            }
        }
        else {
            // ���� ��ȯ�� �Ͼ���� �ִϸ��̼��� ����ϸ鼭 �ϴ� ó������ �����ؾ��ϴ� ���� �����Ѵ�.
            switch(m_state) {
                case PlayerState.Idle:
                    IdleEnterState();
                    break;

                case PlayerState.Run:
                    RunEnterState();
                    break;

                case PlayerState.Evasion:
                    EvasionEnterState();
                    break;

                case PlayerState.Shoot:
                    ShootEnterState();
                    break;
			}


            // �ൿ�� �ٲ������ �ؾ��ϴ� �͵�
            m_beState = m_state;
        }
    }

    private void IdleEnterState()
	{

	}

    private void RunEnterState()
    {
        m_rigid.AddForce(m_move * m_moveSpeed);
    }

    private void ShootEnterState()
    {

    }

    private void EvasionEnterState()
	{
        // �ϴ� ����� ����� �ٵ� ������ ��� ��� �ƴ�
        // �׸��� RigidBody������ �Ϻ��ϰ� ��ǥ ���������� ���� (���� � ���п� ������ Ʋ����)
        m_evasionTarget = m_move * m_evasionSpeed;
        m_rigid.AddForce(m_evasionTarget);
	}

    private void EvasionPressState()
    {
        m_state = PlayerState.Idle;
    }

    private void RunPressState()
    {
        m_rigid.AddRelativeForce(m_move);
    }

    private void InputAction()
	{
        // Ű�� ������
        if(Managers.Input.GetKeyDown(UserKey.Evasion)) {
            // �����̽��ٿ� ����Ű�� �Էµȴٸ�
            if (m_evasionAbility.IsAction() == true && m_move != Vector3.zero) {
                // �����δ�.
                m_state = PlayerState.Evasion;
                m_evasionAbility.Action();
            }
		}
	}

    // ���� ����ΰ� ������ �ǽð������ �̿��ؼ� ��ǥ�� ���� ������ �����̴� ���� �� ������
    private void InputMove()
    {
        m_move = Vector3.zero;

        if (Managers.Input.GetKey(UserKey.Forward)) {
            m_move.z += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Backward)) {
            m_move.z -= 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Right)) {
            m_move.x += 1.0f;
        }
        if (Managers.Input.GetKey(UserKey.Left)) {
            m_move.x -= 1.0f;
        }

        // ����ȭ
        if (m_move.magnitude > 1.6f) {
            m_move = m_move.normalized;
        }

        m_rigid.AddForce(m_move * m_moveSpeed);


    }

	// �������ѱ� ������ ����
	private void InputMouseRotation()
	{
		Vector3 playerToScreenPos = Camera.main.WorldToScreenPoint(transform.position);

		Vector3 mousePos = Input.mousePosition;
		Vector3 tempPos = mousePos - playerToScreenPos;
		float lookRadius = Mathf.Atan2(tempPos.y, tempPos.x);

		transform.rotation = Quaternion.Euler(0f
			, (-lookRadius * Mathf.Rad2Deg) + 90f
			, 0f);
	}

	private void OnDrawGizmos()
	{
		switch(m_state) {
            case PlayerState.Evasion:
                EvasionGizmos();
                break;
		}
	}

    private void EvasionGizmos()
	{
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, m_evasionTarget);
	}
}
