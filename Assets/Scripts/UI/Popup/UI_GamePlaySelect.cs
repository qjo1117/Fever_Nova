using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GamePlaySelect : UI_Popup
{
    enum Images
    {
        Background
    }

    enum Buttons
    {
        BackButton,
        SinglePlayButton,
        MultiPlayButton
    }

    enum Texts
    {
        BackButtonText,
        SinglePlayButtonText,
        MultiPlayButtonText
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Button>(typeof(Buttons));
        Bind<Text>(typeof(Texts));

        Get<Button>((int)Buttons.BackButton).onClick.AddListener(() => { ClosePopupUI(); });
        Get<Button>((int)Buttons.SinglePlayButton).onClick.AddListener(SinglePlayClick);
        Get<Button>((int)Buttons.SinglePlayButton).onClick.AddListener(MultiPlayClick);
    }

    // �̱��÷��� ��ư Ŭ����
    private void SinglePlayClick()
    {
        Managers.UI.ShowPopupUI<UI_StageSelect>();
    }

    // ��Ƽ �÷��� ��ư Ŭ����  (���� �̱���,��Ƽ�÷��� ���� �߰� X)
    private void MultiPlayClick()
    {

    }

}
