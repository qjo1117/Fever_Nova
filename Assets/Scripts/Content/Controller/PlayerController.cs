using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;

[System.Serializable]
public class PlayerStat 
{
    public int      id = 1;
    public string   name = "Hello_Player";
    public int      hp = 100;
    public int      attack = 70;
    public float    moveSpeed = 300.0f;
    public float    evasionSpeed = 10.0f;
    public int      score = 0;
}

// ��ȹ���ʿ��� ���� ������ ����� ������ Class�� ���� ���ǵ� ü�� ����� ������.
public class PlayerController : MonoBehaviour
{
    public enum PlayerState {
        Idle,
        Run,
        Shoot,
        Evasion,
        Jump,
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

    private float           m_explosionJumpRadius = 4.0f;

    [SerializeField]
    private float           m_evasionDelayTime = 1.0f;      // ��� �ð�
    private float           m_evasionTime = 0.0f;           // ���� �ð�

    private Boom            m_boom = null;

    private Rigidbody       m_rigid = null;
    private Animator        m_anim = null;

    private Transform       m_handler = null;

    #endregion

    #region ������Ƽ

    public PlayerStat Stat { get => m_stat; set => m_stat = value; }
    public PlayerState State { get => m_state; set => m_state = value; }

    #endregion


    private void Start()
	{
        m_rigid  = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_handler = Util.FindChild(gameObject, "@Handler", true).transform;
    }

    private void Update()
    {
        UpdateState();
        UpdateInput();
    }

	private void FixedUpdate()
	{

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

		transform.rotation = Quaternion.Euler(0f, (-m_lookRotation * Mathf.Rad2Deg) + 90f, 0f);

        // ----------------------------------------------------------
        // ��ź ������ ���� ������ �ִϸ��̼� ��ȯ�� �ϴ��� ���⼭ �ϴ� ��
        // TODO : ������ ��ġ�� ã���� �Լ��� ���� �ļ� ��ġ�� ��
        Ray l_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit l_hit;
        Physics.Raycast(l_ray, out l_hit);
        Vector3 l_position = l_hit.point - transform.position;

        // ��ź �ݰ溸�� ������
        if (l_position.sqrMagnitude < m_explosionJumpRadius * m_explosionJumpRadius) {
            Managers.Log("Animation Change");
		}

        Debug.DrawRay(Camera.main.transform.position, l_ray.direction * 1000.0f, Color.red);
    }

    public void InputShoot()
	{
        if (Managers.Input.GetKeyDown(UserKey.Shoot) == true) {
            // ���� ���� �����̸� �Ʒ��� ��ź�� ����
            if(m_state == PlayerState.Jump) {

			}

			// ���� �ֽ��� ��ź�� ������ �ִ´�.
			m_boom = Managers.Resource.Instantiate("Boom", Managers.Game.Boom.transform).GetComponent<Boom>();
            m_boom.Shoot(m_handler.position, transform.forward);
		}
        

        // ���� Press���϶� ī��Ʈ�� ��� ���� ���°��� ��ȯ��Ų��.
        if (Managers.Input.GetKey(UserKey.Shoot) == true) {
            m_isExplosion = true;
            m_explosionTime += Time.deltaTime;
        }

        // Up�� ������ ���� ���¸� ���� �ʱ�ȭ���ش�.
        if (Managers.Input.GetKeyUp(UserKey.Shoot) == true) {
            // TODO : �б��ؼ� ���������϶��� �ٸ� ��쵵 üũ���ش�.
            if(m_boom.State == Boom.BoomState.Delay) {
                m_boom.Explosion();
			}

            m_explosionTime = 0.0f;
            m_isExplosion = false;
        }


        // ��ź�� ���� ���
        if (m_isExplosion == true) {
            // ������ �ð��� �ʰ��� ���
            if(m_explosionTime >= m_explosionDelayTime) {
                // ��ź�� ���¸� ��ȯ��Ų��.
                m_boom.State = Boom.BoomState.Delay;
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

		if (Managers.Input.GetKeyDown(UserKey.Evasion) == true && m_state == PlayerState.Run) {
            m_rigid.AddForce(m_move * m_stat.evasionSpeed);
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
            m_rigid.AddForce(m_stat.moveSpeed * m_move.normalized);
        }
	}

	#endregion

	private void OnDrawGizmosSelected()
	{
        Vector3 l_position = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(l_position, m_explosionJumpRadius);
	}
}
