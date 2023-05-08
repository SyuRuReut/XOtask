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

    //ó���ϴ� Ʈ��ŷ�̸� �˾�â�� �߰� �ƴϸ� �� ��ϴ�.
    //�������� Ȱ��ȭ�Ҷ� ���µ� ó�� �����Ҷ� Ȱ��ȭ �Ǿ� �����Ƿ� 2���� ī��Ʈ�� ��.
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
