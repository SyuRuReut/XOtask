using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summon : MonoBehaviour
{
    [SerializeField] private GameObject summonCard;
    private bool canSummon;
    [SerializeField] private Material[] summonCardMaterials;
    [SerializeField] private int materialsTurn = 0;
    [SerializeField] private float changeTime;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private IEnumerator TrySummon()
    {
        //일정시간마다 머터리얼을 바꿔서 이미지를 바꿉니다.
        yield return new WaitForSeconds(changeTime);
        if (materialsTurn >= summonCardMaterials.Length - 1)
            materialsTurn = 0;
        else
            materialsTurn++;
        summonCard.GetComponent<MeshRenderer>().material = summonCardMaterials[materialsTurn];
        canSummon = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canSummon)
        {
            StartCoroutine(TrySummon());
            canSummon = false;
        }
    }

    private void OnEnable()
    {
        canSummon = true;
    }
}
