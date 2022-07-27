using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ResultScreen : UI_Popup
{
    #region UI������Ʈ_ENUM
    enum Buttons
    {
        RestartButton,          // �ٽý��� ��ư
        MainScreenButton        // ����ȭ�� ��ư
    }

    enum Texts
    {
        RestartText,            // �ٽý��� ��ư�� �ؽ�Ʈ ('�ٽý���')
        MainScreenText          // ����ȭ�� ��ư�� �ؽ�Ʈ ('����ȭ��')
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        Get<Button>((int)Buttons.RestartButton).onClick.AddListener(() => RestartButtonClicked());
        Get<Button>((int)Buttons.MainScreenButton).onClick.AddListener(() => MainScreenButtonClicked());

    }

    // �ٽ��ϱ� ��ư Ŭ���� ����
    private void RestartButtonClicked()
    {
        // UI_Root���� �����Ǿ��ִ� UI_Result ���� closepopup
        // (���߿� 2���÷��̰� �Ǿ�����, ���â 2���� ������� �ǹǷ� 2���� ���â�� ����� ���� �ش� �۾��� ����)
        UI_Result[] uiResults = Managers.UI.Root.GetComponentsInChildren<UI_Result>();
        foreach(UI_Result item in uiResults)
        {
            item.ClosePopupUI();
        }

        ClosePopupUI();
    }

    // ����ȭ�� ��ư Ŭ���� ����
    private void MainScreenButtonClicked()
    {
        UI_Result[] uiResults = Managers.UI.Root.GetComponentsInChildren<UI_Result>();
        foreach (UI_Result item in uiResults)
        {
            item.ClosePopupUI();
        }

        Managers.Scene.LoadScene(Define.Scene.Main);
        ClosePopupUI();
    }

}
