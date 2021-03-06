using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Define;
using UnityEngine.EventSystems;

[System.Serializable]
public class PlayerStat 
{
    public int      id = 1;
    public string   name = "Hello_Player";
    public int      hp = 100;
    public int      maxHp = 100;
    public int      attack = 70;
    public float    mass = 2.0f;
    public float    moveSpeed = 1000.0f;
    public float    evasionSpeed = 4000.0f;
    public int      score = 0;
    public int      totalScore = 0;
}


// 기획자쪽에서 스탯 조절할 목록을 받으면 Class로 따로 스피드 체력 등등을 나눈다.
public class PlayerController : MonoBehaviour
{
    public enum PlayerState {
        Idle,               // 아이들
        Run,                // 움직임
        Shoot,              // 샷
        Evasion,            // 회피
        Jump,               // 점프 하기전
        Jumping,            // 점프중
    }

    #region 변수
    [SerializeField]
    private PlayerStat          m_stat = new PlayerStat();

    [SerializeField]
    private PlayerState         m_state = PlayerState.Idle;

    private Vector3             m_move = Vector3.zero;
    private float               m_inputPress = 0.0f;
    private Vector3             m_mousePos = Vector3.zero;

    private float               m_lookRotation = 0.0f;
    private float               m_explosionDelayTime = 0.5f;
    private float               m_explosionTime = 0.0f;
    private bool                m_isExplosion = false;

    // 총 애니메이션 딜레이
    private float               m_shotMaxDelay = 3.0f;
    private float               m_shotDelay = 0.0f;

    // 폭탄 사거리 관련
    private float               m_explosionJumpRange = 5.0f;
    private float               m_explosionRange = 12.0f;
    private float               m_currentMosueRadius = 0.0f;

    // 회피 전용 쿨타임
    [SerializeField]
    private float               m_evasionDelayTime = 1.0f;      // 대기 시간
    private float               m_evasionTime = 0.0f;           // 현재 시간

    // 컴포넌트 맵핑
    private Bomb                m_bomb = null;
    private Rigidbody           m_rigid = null;
    private Animator            m_anim = null;
    private Transform           m_handler = null;


    // 죽인 몬스터의수, 멀티킬의 횟수는 플레이어 마다 다르기떄문에
    // MonsterManager에서 PlayerController로 이동시켰다.
    // --------- Goal UI Test ---------
    private UI_Goal             m_goal;

    private int                 m_hitCount;
    private int                 m_killCount;           // 죽인 몬스터 수
    private int                 m_multiKillCount;      // 멀티킬한 횟수

    // --------- player hp bar ---------
    private UI_PlayerHPBar      m_playerHPBar = null;
    private UI_BombRange        m_bombRange = null;
    #endregion

    #region 프로퍼티

    public PlayerStat Stat { get => m_stat; set => m_stat = value; }
    public PlayerState State { get => m_state; set => m_state = value; }

    public float ExplosionJumpRadius { get => m_explosionJumpRange; set => m_explosionJumpRange = value; }
    public float ExplosionRadius { get => m_explosionRange; set => m_explosionRange = value; }

    // --------- Goal UI Test ---------
    public int MonsterKillCount
    {
        get
        {
            return m_killCount;
        }
        set
        {
            m_killCount = value;
            m_goal.MonsterKillCount = m_killCount;
        }
    }

    public int MonsterMultiKillCount { get => m_multiKillCount; set => m_multiKillCount = value; }

    // 플레이어 hp바 PlayerManager에서 PlayerController로 이동

    // => Health 아이템 충돌시 PlayerController와 충돌하게 되므로 hp 바 갱신을 위해서
    // PlayerController로 플레이어 hp바를 이동하였다.
    // (Player hp Bar 생성하는 코드 자체는 PlayerManager에 존재한다. PlayerSpawn함수에서 생성하기떄문)
    // --------- player hp bar ---------
    public UI_PlayerHPBar PlayerHPBar { get => m_playerHPBar; set => m_playerHPBar = value; }
    #endregion


    private void Start()
	{
        m_rigid  = GetComponent<Rigidbody>();
        m_anim = GetComponent<Animator>();
        m_handler = Util.FindChild(gameObject, "@Handler", true).transform;

        // MainPlayer전용 UI셋팅
        UI_BombRange l_bombRange = Managers.UI.Root.GetComponentInChildren<UI_BombRange>();
        l_bombRange.RangeRadius = m_explosionRange;

        UI_BombJumpRange l_bombJumpArrow = Managers.UI.Root.GetComponentsInChildren<UI_BombJumpRange>()[1];
        l_bombJumpArrow.RangeRadius = m_explosionJumpRange;

    }

    public void OnLateUpdate()
	{

    }

    public void OnUpdate()
    {
        UpdateState();
        UpdateInput();
    }


    public void Demege(int _attack)
    {
        m_hitCount++;
        m_stat.hp -= _attack;

        // Player Dead ---> Result
        if (m_stat.hp <= 0)
        {
            if (Util.FindChild<UI_ResultScreen>(Managers.UI.Root, "UI_ResultScreen") != null)
            {
                return;
            }


            // 게임 결과창 출력
            Managers.UI.ShowPopupUI<UI_ResultScreen>("UI_ResultScreen");
            UI_Result l_result = Managers.UI.ShowPopupUI<UI_Result>("UI_Result");

            l_result.PlayerId = 1;
            l_result.Result = "Defeat...";
            l_result.Score = m_stat.score;
            l_result.GameStartTime = Managers.Game.BeginPlayTime;
            l_result.KillCount = m_killCount;
            l_result.MultiKillCount = m_multiKillCount;
            l_result.HitCount = m_hitCount;
            l_result.TotalScore = m_stat.totalScore;
            return;
        }
    }

    // 플레이어 hp 체력바 회복 테스트용
    public void Recover(int p_recoverHp)
    {
        if (m_stat.hp >= m_stat.maxHp) {
            return;
        }

        m_stat.hp += p_recoverHp;
        m_playerHPBar.HP = m_stat.hp;
    }

    #region 상태 업데이트

    public void UpdateState()
	{
        switch(m_state) {
            case PlayerState.Evasion:
                EvasionState();
                break;
            case PlayerState.Jump:
                JumpState();
                break;
            case PlayerState.Run:
                RunState();
                break;
            case PlayerState.Idle:
                IdleUpdate();
                break;
		}
	}

    public void IdleUpdate()
	{
        m_anim.SetFloat("MoveX", 0.0f);
        m_anim.SetFloat("MoveZ", 0.0f);

    }

    // Player 기준으로 마우스가 앞에 있으면 앞을 보게 만들어야한다.
    public void RunState()
	{
        // 마우스와 플레이어 사이의 Normal Vector를 추출
        // 기저벡트를 이용해서 축을 판별한다.
        float l_moveX = m_mousePos.x - transform.position.x;
        float l_moveZ = m_mousePos.z - transform.position.z;

        Vector3 l_direction = Quaternion.Euler(0.0f, (Mathf.Atan2(l_moveZ, l_moveX) * Mathf.Rad2Deg) - 90.0f, 0.0f) * m_move;

        m_anim.SetFloat("MoveX", l_direction.x * m_inputPress);
        m_anim.SetFloat("MoveZ", l_direction.z * m_inputPress);
    }

    public void EvasionState()
	{

	}

    public void JumpState()
    {

    }

    #endregion


    #region Move Animation Event
    
    public void FootR()
	{

	}
    public void FootL()
    {

    }

	#endregion




    #region 입력 업데이트

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
        // 폭탄 점프에 대한 것으로 애니메이션 전환을 일단은 여기서 하는 것
        // TODO : 괜찮은 위치를 찾으면 함수를 따로 파서 배치할 것
        // ----------------------------------------------------------
        Ray l_ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit l_hit;
        if(Physics.Raycast(l_ray, out l_hit, 100.0f, 1 << (int)Define.Layer.Ground) == true) {
            Vector3 l_position = l_hit.point - transform.position;
            l_position.y = 0.0f;
            m_mousePos = l_hit.point;

            Color l_color = Color.red;
            float l_magnitude = l_position.sqrMagnitude;
            // 폭탄 반경보다 좁으면
            if (l_magnitude <= m_explosionJumpRange * m_explosionJumpRange) {
                m_state = PlayerState.Jump;
                l_color = Color.blue;
            }
            float l_explosionRange = m_explosionRange * m_explosionRange;
            if (l_magnitude >= l_explosionRange) {
                l_magnitude = l_explosionRange;
            }
            l_magnitude = Mathf.Clamp(l_magnitude, Mathf.Pow(m_explosionJumpRange, 2.5f), l_explosionRange) / l_explosionRange;
            m_anim.SetFloat("Aiming", l_magnitude);

            Debug.DrawRay(Camera.main.transform.position, l_ray.direction * 1000.0f, l_color);
        }

    }

    public void InputShoot()
	{
        if (Managers.Input.GetKeyDown(UserKey.Shoot) == true) {
            m_shotDelay = 0.0f;
            if (m_state == PlayerState.Jump) {
                m_anim.CrossFade("GunJump", 0.3f);
            }
            else {
                m_anim.SetBool("Shot", true);
            }
        }

        m_shotDelay += Time.deltaTime;
        if (m_shotDelay > m_shotMaxDelay) {
            m_anim.SetBool("Shot", false);
            return;
        }

        m_anim.SetFloat("Fire", m_shotDelay);

        // 현재 폭탄을 가지고 있지 않는다면 넘어가자.
        if (m_bomb?.gameObject.IsValid() == false) {
            return;
		}

        // 현재 Press중일때 카운트를 세어서 현재 상태값을 전환시킨다.
        if (Managers.Input.GetKey(UserKey.Shoot) == true) {
            m_isExplosion = true;
            m_explosionTime += Time.deltaTime;
        }

        // Up을 했을때 현재 상태를 전부 초기화해준다.
        if (Managers.Input.GetKeyUp(UserKey.Shoot) == true) {
            // TODO : 분기해서 지연상태일때는 다른 경우도 체크해준다.
            if (m_bomb?.State == Bomb.BoomState.Delay) {
                m_bomb.Explosion();
            }

            m_explosionTime = 0.0f;
            m_isExplosion = false;
        }


        // 폭탄이 있을 경우
        if (m_isExplosion == true) {
            // 정해진 시간을 초과할 경우
            if (m_explosionTime >= m_explosionDelayTime) {
                // 폭탄의 상태를 변환시킨다.
                m_bomb.State = Bomb.BoomState.Delay;
            }
		}
    }

    // Animation Player-Shooting-CM / Event
    public void ShotEnd()
	{
        // 가장 최신의 폭탄을 가지고 있는다.      
        Vector3 l_subVector = m_mousePos - m_handler.position;
        l_subVector.y = 0.0f;

        float l_dist = (m_mousePos - transform.position).magnitude;
        m_currentMosueRadius = l_dist > m_explosionRange ? m_explosionRange : l_dist;

        m_bomb = Managers.Game.Boom.ShootSpawn(m_handler.position, l_subVector.normalized, m_currentMosueRadius);
    }

    public void ShotJumpEnd()
	{
        if (m_state == PlayerState.Jump || m_state == PlayerState.Run) {
            Managers.Game.Boom.JumpSpawn(m_handler.position);
        }
    }

    public void EvasionEnd()
	{
        m_state = PlayerState.Idle;
	}

    public void UpdateInput()
	{
        // 회피중일때는 다른 상태는 불가능
        if (m_state == PlayerState.Evasion) {
            return;
        }
        if(EventSystem.current.IsPointerOverGameObject() == true) {
            return;
		}

        m_state = PlayerState.Idle;

        InputMouse();

        if(Managers.Input.GetKeyUpOrAll(UserKey.Forward, UserKey.Backward, UserKey.Left, UserKey.Right) == true) {
            m_inputPress = 0.2f;
        } 

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
            m_anim.Play("Player-Evasion");
            m_move *= 2.0f;
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
            m_rigid.AddForce((m_stat.moveSpeed * m_stat.mass * 4.0f * Time.deltaTime) * m_move.normalized);
            if (m_inputPress >= 0.5f) {
                m_inputPress = 0.5f;
            }
            else {
                m_inputPress += 0.5f * Time.deltaTime;
            }
        }
	}

    #endregion

    //// 쿨타임 테스트용
    //IEnumerator BombCoolTimeTimer()
    //{
    //    yield return new WaitForSeconds(m_coolTime);

    //    Managers.UI.Root.GetComponentInChildren<UI_Aim>().ColorChange(Color.green);
    //    Managers.UI.Root.GetComponentInChildren<UI_BombDropPoint>().ColorChange(Color.green);
    //    m_isCoolTime = false;
    //}


    private void OnDrawGizmosSelected()
	{
        Vector3 l_position = transform.position;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(l_position, m_explosionJumpRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, transform.forward * (transform.position - m_mousePos).magnitude);

    }
}
