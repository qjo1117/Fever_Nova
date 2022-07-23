using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Score : UI_Scene
{
    #region UI������Ʈ_ENUM
    enum Texts
    {
        CurrentScoreText,                           // ���� ���� ǥ�� �ؽ�Ʈ
        ScoreLogText                                // ȹ�� ���� ǥ�� �ؽ�Ʈ
    }

    enum Images
    {
        Background                                  // ���ȭ��
    }
    #endregion

    #region ����
    private bool            m_timerIsRun = false;   // coreLogTimer �ڷ�ƾ �Լ��� �������ΰ�?
    private List<int>       m_killScoreList;        // ȹ���� ���� ����Ʈ

    public float            m_deleteDelay = 5.0f;   // ų�α� ���� �����ð�
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Text>(typeof(Texts));
        Bind<Image>(typeof(Images));

        m_killScoreList = new List<int>();
    }

    // ���� ���� UI ���� �����ϴ� �Լ�
    private void ScoreTextUpdate()
    {
        Get<Text>((int)Texts.CurrentScoreText).text = $"���� : {Managers.Game.Player.MainPlayer.Stat.score}";
    }

    // ȹ�� ���� �ؽ�Ʈ �����ϴ� �Լ�
    public void ScoreLogCreate(int _score)
    {
        // ȹ�� ���� ����Ʈ�� ���� �߰�
        m_killScoreList.Add(_score);

        
        // �ڷ�ƾ �Լ� ���� �ȵȰ��, �Լ� �����ؼ� 0.5�ʵ��� ���� ȹ���� �������, 0.5�ʰ� ������
        // �Է¹��� ȹ�� ������ ������ Ȯ���Ͽ� ȹ�� ���� �ؽ�Ʈ�� �����Ҽ� �ֵ�����
        if (!m_timerIsRun)
        {
            StartCoroutine(ScoreLogTimer());
        }
    }

    // 0.5�ʾȿ� �Էµ� ������ üũ�Ͽ� Kill �Ǵ� MultiKill �α׸� ���������� �������ִ� �Լ�
    IEnumerator ScoreLogTimer()
    {
        // 0.5�ʵ��� ���
        m_timerIsRun = true;
        yield return new WaitForSeconds(0.5f);

        Text l_logText = Get<Text>((int)Texts.ScoreLogText);
        PlayerController l_player = Managers.Game.Player.MainPlayer;

        // ȹ���� ������ 1���϶� (�̱� ų�ϋ�)
        if(m_killScoreList.Count <= 1)
        {
            if (l_logText.text.Length == 0)
            {
                l_logText.text = $"Kill + {m_killScoreList[0]}\n";
            }
            else
            {
                l_logText.text = l_logText.text.Insert(l_logText.text.Length, $"Kill + {m_killScoreList[0]}\n");
            }

            // �÷��̾� Score Stat ������Ʈ
            l_player.Stat.score += m_killScoreList[0];
        }
        // ȹ���� ������ 1�� �̻��ϋ� (��Ƽ ų�϶�)
        else
        {
            int l_scoreSum = 0;

            // �� ȹ�� ���� �ջ�
            foreach (int item in m_killScoreList) 
            {
                l_scoreSum += item;
            }

            if (l_logText.text.Length == 0)
            {
                l_logText.text = $"MultiKill + {l_scoreSum}\n";
            }
            else
            {
                l_logText.text = l_logText.text.Insert(l_logText.text.Length, $"MultiKill + {l_scoreSum}\n");
            }

            // �÷��̾� Score Stat ������Ʈ
            l_player.Stat.score += l_scoreSum;
        }

        ScoreTextUpdate();
        // �߰��� �ؽ�Ʈ �����ð� �ڿ� �����ɼ��ֵ��� Delete���� �ô� �ڷ�ƾ�Լ� ����
        StartCoroutine(ScoreLogDelete());
        m_killScoreList.Clear();
        m_timerIsRun = false;
    }

    // ȹ�� ���� �ؽ�Ʈ �����ϴ� �ڷ�ƾ �Լ�
    IEnumerator ScoreLogDelete()
    {
        // ������ �����ð� ��ŭ ���
        yield return new WaitForSeconds(m_deleteDelay);

        // �� ������ �ؽ�Ʈ�� ����
        Text l_logText = Get<Text>((int)Texts.ScoreLogText);
        int l_emptyCharIndex = l_logText.text.IndexOf('\n');
        l_logText.text = l_logText.text.Remove(0, l_emptyCharIndex + 1);
    }

}
