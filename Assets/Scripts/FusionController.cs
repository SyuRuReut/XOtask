using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FusionController : MonoBehaviour
{
    public bool canFusion;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    //��ü ���� ���� ����
    public void CanFusion()
    {
        canFusion = true;
    }

    public void CantFusion()
    {
        canFusion = false;
    }
}
