using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Result : UI_Popup
{
    #region UI������Ʈ_ENUM
    enum Images
    {
        ResultScreen_Background,
        BorderLine1,
        BorderLine2,

        ScoreLabel_Background,
        PlayTimeLabel_Background,
        KillCountLabel_Background,
        MultiKillLabel_Background,
        HitCountLabel_Background,
        TotalScoreLabel_Background,

        ResultText_Background,
        Score_Background,
        PlayTime_Background,
        KillCount_Background,
        MultiKill_Background,
        HitCount_Background,
        TotalScore_Background
    }

    enum Texts
    {
        PlayerText,
        ScoreLabel,
        PlayTimeLabel,
        KillCountLabel,
        MultiKillLabel,
        HitCountLabel,
        TotalScoreLabel,

        ResultText,
        ScoreText,
        PlayTimeText,
        KillCountText,
        MultiKillText,
        HitCountText,
        TotalScoreText
    }
    #endregion;

    #region ����

    private int     m_playerId;
    private string  m_result;
    private int     m_score;
    private float   m_gameStartTime;
    private int     m_killCount;
    private int     m_multiKillCount;
    private int     m_hitCount;
    private int     m_totalScore;

    private int     m_strCheckBitFlag;
    private int     m_intCheckBitFlag;
    private int     m_floatCheckBitFlag;

    // m_isReday ������ �ʿ��� ����
    // => ���� ������Ʈ ��������� �񵿱� ����̱⶧����,ShowPopup�Լ��� ���� UI�� �����ص�
    //    �ٷ� Init�Լ��� ȣ����� �ʾ� UI ������Ʈ Bind�۾��� ����� �̷������ �ʴ´�.
    //    �̻��¿��� �ٷ� ������Ƽ�� ���� ���� �ְԵǸ�, �� ����Update�Լ����� ������ �߻��Ұ��̴�.

    //    �̸� �������� m_isReady (Bind �۾��� �Ϸ�Ǿ��°�?)bool ���� ���� Update�Լ����� 
    //    bind�۾��� ���� �ʾ����� (���� Init�Լ��� ȣ����� �ʾ�����) Update �Լ��� return���� �����ϰ�
    //    �ش� ������ Update �۾��� Init�Լ����� Bind�� �Ϸ�ǰ� m_isReady�� true�� ����� �ڿ� �����Ͽ�
    //    ���������� Update ��Ű�����ؼ� m_isReady������ �ʿ��Ѱ��̴�.

    private bool    m_isReady;              // UI ������Ʈ Bind �۾��� �Ϸ�Ǿ��°�?
    #endregion

    #region ������Ƽ
    // �⺻������ ������Ƽ�� ���� �����ϸ�, �ش� ���� Update�ϴ� �Լ��� ȣ��Ǿ� �ٷ� UI�� �ݿ���

    public int PlayerId
    {
        get
        {
            return m_playerId;
        }
        set
        {
            m_playerId = value;
            StringDataTextUpdate(string.Format("(Player{0})", m_playerId), Texts.PlayerText);
        }
    }


    public string Result
    {
        get
        {
            return m_result;
        }
        set
        {
            m_result = value;
            StringDataTextUpdate(m_result, Texts.ResultText);
        }
    }

    public int Score
    {
        get
        {
            return m_score;
        }
        set
        {
            m_score = value;
            IntDataTextUpdate(m_score, Texts.ScoreText);
        }
    }

    public float GameStartTime
    {
        get
        {
            return m_gameStartTime;
        }
        set
        {
            m_gameStartTime = value;
            FloatDataTextUpdate(m_gameStartTime, Texts.PlayTimeText);
        }
    }

    public int KillCount
    {
        get
        {
            return m_killCount;
        }
        set
        {
            m_killCount = value;
            IntDataTextUpdate(m_killCount, Texts.KillCountText);
        }
    }

    public int MultiKillCount
    {
        get
        {
            return m_multiKillCount;
        }
        set
        {
            m_multiKillCount = value;
            IntDataTextUpdate(m_multiKillCount, Texts.MultiKillText);
        }
    }

    public int HitCount
    {
        get
        {
            return m_hitCount;
        }
        set
        {
            m_hitCount = value;
            IntDataTextUpdate(m_hitCount, Texts.HitCountText);
        }
    }

    public int TotalScore
    {
        get
        {
            return m_totalScore;
        }
        set
        {
            m_totalScore = value;
            IntDataTextUpdate(m_totalScore, Texts.TotalScoreText);
        }
    }
    #endregion


    public override void Init()
    {
        base.Init();

        BitFlagCreate();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));

        m_isReady = true;
        AllUpdate();
    }

    // IntDataTextUpdate,StringDataTextUpdate... ���� ���� ������Ʈ �ϴ��Լ��� Ȯ�强�� ���Ͽ� Texts enum���� �޾Ƽ�
    // enum���� �ش��ϴ� Text�� �����ϴ� ������ ������ ®��.

    // �̷���� IntData ����ϴ� UI�� ������Ʈ�ϴ� IntDataTextUpdate�Լ��� ȣ���ϴµ� floatData�� ����ϴ� UI������ enum���� �Ű������� �����ϴ�
    // ��찡 ���� �� �ֱ� ������ �̸� �����ϱ����� ��Ʈ�÷��� üũ�� ����Ͽ���.
    private void BitFlagCreate()
    {
        // string ������ Ȯ�� ��Ʈ�÷���
        m_strCheckBitFlag = (int)Texts.PlayerText | (int)Texts.ResultText;

        // int ������ Ȯ�� ��Ʈ�÷���
        m_intCheckBitFlag = (int)Texts.ScoreText | (int)Texts.KillCountText | (int)Texts.MultiKillText |
            (int)Texts.HitCountText | (int)Texts.TotalScoreText;

        // float ������ Ȯ�� ��Ʈ�÷���
        m_floatCheckBitFlag = (int)Texts.PlayTimeText;
    }

    // Bind�Ǳ��� ������Ƽ�� ���� �Էµ� ������ �ϰ� Update�ϱ� ���� �Լ�
    private void AllUpdate()
    {
        StringDataTextUpdate(string.Format("(Player{0})", m_playerId), Texts.PlayerText);
        StringDataTextUpdate(m_result, Texts.ResultText);
        IntDataTextUpdate(m_score, Texts.ScoreText);
        FloatDataTextUpdate(m_gameStartTime, Texts.PlayTimeText);
        IntDataTextUpdate(m_killCount, Texts.KillCountText);
        IntDataTextUpdate(m_multiKillCount, Texts.MultiKillText);
        IntDataTextUpdate(m_hitCount, Texts.HitCountText);
        IntDataTextUpdate(m_totalScore, Texts.TotalScoreText);
    }

    // IntData�� ����ϴ� UI�� �ؽ�Ʈ�� ������Ʈ�ϴ� �Լ�
    private void IntDataTextUpdate(int _intData, Texts _type)
    {
        if (!m_isReady) 
        {
            return;
        }

        // intData�� ����ϴ� UI�� �´��� ��Ʈ�÷��� üũ
        if (((int)_type & m_intCheckBitFlag) != (int)_type) 
        {
            Debug.LogError("�����ʴ� ������ Data Text�� ������ �õ���");
            return;
        }

        Get<Text>((int)_type).text = string.Format("{0:D5}", _intData);
    }

    // StringData�� ����ϴ� UI�� �ؽ�Ʈ�� ������Ʈ�ϴ� �Լ�
    private void StringDataTextUpdate(string _stringData, Texts _type)
    {
        if (!m_isReady)
        {
            return;
        }

        // stringData�� ����ϴ� UI�� �´��� ��Ʈ�÷��� üũ
        if (((int)_type & m_strCheckBitFlag) != (int)_type)
        {
            Debug.LogError("�����ʴ� ������ Data Text�� ������ �õ���");
            return;
        }

        Get<Text>((int)_type).text = _stringData;
    }

    // float�� ����ϴ� UI�� �ؽ�Ʈ�� ������Ʈ�ϴ� �Լ�
    private void FloatDataTextUpdate(float _floatData,Texts _type)
    {
        if (!m_isReady)
        {
            return;
        }

        // floatData�� ����ϴ� UI�� �´��� ��Ʈ�÷��� üũ
        if (((int)_type & m_floatCheckBitFlag) != (int)_type)
        {
            Debug.LogError("�����ʴ� ������ Data Text�� ������ �õ���");
            return;
        }

        if(_type == Texts.PlayTimeText)
        {
            Managers.Game.EndPlayTime = Time.time;
            int l_subTime = (int)(Managers.Game.EndPlayTime - _floatData);
            int l_minute = l_subTime / 60;
            int l_second = l_subTime % 60;

            Get<Text>((int)Texts.PlayTimeText).text = string.Format("{0:D2}:{1:D2}", l_minute, l_second);
        }
    }
}
