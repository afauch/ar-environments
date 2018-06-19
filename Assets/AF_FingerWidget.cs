using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crayon;

public class AF_FingerWidget : MonoBehaviour {

    public void Show()
    {
        gameObject.SetState(CrayonStateType.Default);
    }

    public void Hide()
    {
        gameObject.SetState(CrayonStateType.Custom, "Hidden");
    }


}
