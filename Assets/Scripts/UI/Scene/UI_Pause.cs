using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Pause : UI_Scene
{
    #region UI������Ʈ_ENUM
    enum Buttons
    {
        PauseButton             // �Ͻ����� ��ư
    }


    enum Images
    {
        PauseImage              // �Ͻ����� ��ư �̹���
    }
    #endregion

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
    }

   
}
