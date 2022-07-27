using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class UI_MainMenu : UI_Scene
{
    #region ENUM
    enum Images
    {
        MainImage,
    }

    enum TextMeshPros
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
        Bind<TextMeshProUGUI>(typeof(TextMeshPros));
        Bind<Button>(typeof(Buttons));

        GetButton((int)Buttons.PlayButton).gameObject.BindEvent(PlayButtonClicked);
        GetButton((int)Buttons.OptionButton).gameObject.BindEvent(OptionButtonClicked);
        GetButton((int)Buttons.ExitButton).gameObject.BindEvent(ExitButtonClicked);

        //GetObject((int)GameObjects.UI_Option).SetActive(false);
    }

    public void PlayButtonClicked(PointerEventData data)
    {
        Debug.Log("���ӽ���");
        Managers.Scene.LoadScene(Define.Scene.InGame);
    }

    public void OptionButtonClicked(PointerEventData data)
    {
        Debug.Log("ȯ�漳��");
        Managers.UI.ShowPopupUI<UI_Option>("UI_Option");
        //GameObject.Find("UI_Option").GetComponent<UI_Option>().SetOption();
    }

    public void ExitButtonClicked(PointerEventData data)
    {
        //�����ư ������ �ٷ� ����
#if UNITY_EDITOR
        Debug.Log("���� ����");
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
