using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyar;

public class UIController : MonoBehaviour
{
    private int count = 0;

    [SerializeField] private GameObject popUpWindow;
    [SerializeField] SlimeController slimeBlue;
    [SerializeField] SlimeController slimeRed;

    //처음하는 트래킹이면 팝업창이 뜨고 아니면 안 뜹니다.
    //슬라임이 활성화할때 쓰는데 처음 시작할때 활성화 되어 있으므로 2개의 카운트를 셈.
    public void OnPopUp()
    {
        if (count==2)
        {
            popUpWindow.SetActive(true);
            count++;
        }
        else if(count <2)
        {
            count++;
        }
    }

    public void OffPopUP()
    {
        popUpWindow.SetActive(false);
        slimeBlue.CheckedUI();
        slimeRed.CheckedUI();
    }

}
