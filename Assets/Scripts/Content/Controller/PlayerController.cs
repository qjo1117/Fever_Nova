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

    public enum PlayerAbility {
        Evasion,
        Boom,
	}

    [SerializeField]
    private PlayerState         m_state = PlayerState.Idle;             // ���� ����
    private PlayerState         m_beState = PlayerState.Idle;           // ���� ����

    private Vector3             m_move = Vector3.zero;

    private float               m_boomSpeed = 100.0f;
    private float               m_moveSpeed = 10.0f;
    private int                 m_hp = 100;


    private float               m_evasionSpeed = 500.0f;
    private Vector3             m_evasionTarget = Vector3.zero;

    private int                 m_id = -1;                              // ���Ͱ� �ν��� �� �ְ� �ҷ��� �߱� �޴� ���̵�
    private bool                m_isMain = false;                       // �����̿��� Ű���� �Է��� ���� �� �ֱ� ����
    private bool                m_isDead = false;

    private Rigidbody           m_rigid = null;
    private AbilitySystem       m_ability = null;                       // ȸ�� üũ

    private Boom                m_boom = null;

	#region ������Ƽ
	public Rigidbody Rigid { get { return m_rigid; } }
    public PlayerState State { get { return m_state; } }
    public int ID { get { return m_id; } }
    public int HP { get { return m_hp; } set => m_hp = value; }
    public bool IsDead { get => m_isDead; set => m_isDead = value; }
	#endregion

	private void Start()
	{
        m_rigid = GetComponent<Rigidbody>();
        m_ability = GetComponent<AbilitySystem>();

        m_boom = Managers.Resource.NewPrefab("Boom").GetComponent<Boom>();

        // ȸ��
        m_ability.AddAbility(PlayerAbility.Evasion.ToString(), 1.0f);
        // ��ź
        m_ability.AddAbility(PlayerAbility.Boom.ToString(), 0.5f);
    }

    private void Update()
    {
        InputUpdate();
        UpdateState();
    }

	private void FixedUpdate()
	{

    }

    // ������� �Ծ������ Manager���� ȣ���� ����
    public void Demege(int p_demege)
	{
        m_hp -= p_demege;
        if(m_hp <= 0) {
            IsDead = true;
		}
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


	#region ���º�ȭ
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
        m_boom.Create(transform.forward, this);
        m_state = PlayerState.Idle;
    }

    private void EvasionEnterState()
	{
        // �ϴ� ����� ����� �ٵ� ������ ��� ��� �ƴ�
        // �׸��� RigidBody������ �Ϻ��ϰ� ��ǥ ���������� ���� (���� � ���п� ������ Ʋ����)
        m_evasionTarget = m_move * m_evasionSpeed;
        m_rigid.AddForce(m_evasionTarget);

        // TODO : ���ٷ� �ٲٸ� �ȵǱ���
        m_state = PlayerState.Idle;
    }

    private void EvasionPressState()
    {
        m_state = PlayerState.Idle;
    }

    private void RunPressState()
    {
        m_rigid.AddRelativeForce(m_move);
    }

	#endregion

	#region �Է�
	private void InputAction()
	{
        // Ű�� ������
        if(Managers.Input.GetKeyDown(UserKey.Evasion) == true) {
            // �����̽��ٿ� ����Ű�� �Էµȴٸ�
            if (m_ability.Ability[(int)PlayerAbility.Evasion].IsAction == true && m_move != Vector3.zero) {
                // �����δ�.
                m_state = PlayerState.Evasion;
                m_ability.Ability[(int)PlayerAbility.Evasion].Action();
            }
		}
        
        if (Managers.Input.GetKeyDown(UserKey.Shoot) == true) {
            if (m_ability.Ability[(int)PlayerAbility.Boom].IsAction == true) {
                m_state = PlayerState.Shoot;
                m_ability.Ability[(int)PlayerAbility.Boom].Action();
            }
        }
    }

    // ���� ����ΰ� ������ �ǽð������ �̿��ؼ� ��ǥ�� ���� ������ �����̴� ���� �� ������
    private void InputMove()
    {
        m_move = Vector3.zero;
        
        if (Managers.Input.GetKey(UserKey.Forward) == true) {
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

        // ����ȭ
        if (m_move.magnitude > 1.6f) {
            m_move = m_move.normalized;
        }

        m_rigid.AddForce(m_move * m_moveSpeed);


    }

	#endregion

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
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, m_evasionTarget);
    }

}
