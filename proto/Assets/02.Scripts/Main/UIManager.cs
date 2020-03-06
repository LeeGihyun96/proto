using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public void OnClickStartBtn(RectTransform rt)
    {
        Debug.Log("Scale X : " + rt.localScale.x.ToString());
    }
}
