using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using Define;

public class UI_MainMenu : UI_Scene
{
    #region ENUM
    enum Images
    {
        MainImage,
    }

    enum Texts
    {
        GameTitle,
    }

    enum Buttons
    {
        PlayButton,
        OptionButton,
        ExitButton,
    }
    #endregion

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.PlayButton).gameObject.BindEvent(PlayButtonClicked);
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OptionButtonClicked);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(ExitButtonClicked);
    }

    #region ButtonClicked
    public void PlayButtonClicked(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            Managers.Scene.LoadScene(Define.Scene.InGame);
        }
    }

    public void OptionButtonClicked(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
            Managers.UI.ShowPopupUI<UI_Option>("UI_Option");
        }
    }

    public void ExitButtonClicked(PointerEventData data)
    {
        if (data.button == PointerEventData.InputButton.Left)
        {
#if UNITY_EDITOR
            Debug.Log("���� ����");
            UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
        }
    }
    #endregion
}
