using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private UIController ui;
    private bool checkUI;

    //회전 관련
    [SerializeField] private float rotSpeed;
    private Quaternion defaultRotation;
    private Quaternion fusionRotation;
    [SerializeField] private float fusionRotationY;

    //한번 탭 관련
    private bool onClick;
    [SerializeField] private GameObject clickEffect;
    private bool onClickEffect;

    //더블 탭 관련
    [SerializeField] private float doubleTabTime;
    private float firstTabTime;

    //합체 관련
    [SerializeField] private SlimeController partnerController;
    private bool canFusion;
    [SerializeField] private GameObject fusionPartner;
    [SerializeField] private GameObject myFusionCard;
    [SerializeField] private GameObject kingSlime;
    [SerializeField] private GameObject lightingEffect;
    [SerializeField] private Transform lightingTarget;

    // Start is called before the first frame update
    void Start()
    {
        defaultRotation = transform.rotation;
        fusionRotation = Quaternion.Euler(0, fusionRotationY, 0);
    }

    // Update is called once per frame
    void Update()
    {
        //UI를 확인해야 터치하여 작동함.
        if (checkUI)
        {
            //한 곳 이상 터치 할 경우
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                //터치를 한곳에서 정면으로 레이를 발사해 슬라임에 닿을 경우
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 6))
                    {
                        onClick = true;
                    }
                }
                //터치가 움직일 경우 움직인 량에 따라 회전
                else if (touch.phase == TouchPhase.Moved)
                {

                    Vector3 deltaPos = touch.deltaPosition;

                    transform.Rotate(transform.up, deltaPos.x * -1.0f * rotSpeed);
                    transform.Rotate(transform.right, deltaPos.y * 1.0f * rotSpeed);

                }
                //터치가 끝날 경우 회전량 원상복구와 더블클릭 확인
                else if (touch.phase == TouchPhase.Ended)
                {
                    //터치를 시작한 곳이 슬라임이고 끝난 곳도 슬라임일 경우
                    if (onClick)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hitInfo;
                        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 6))
                        {
                            //더블탭인지 확인
                            onClick = false;
                            firstTabTime = Time.time;
                            StartCoroutine(DetectionDoubleTab());

                            //클릭 이펙트이 아직 만들지 않았을 경우 이펙트를 생성
                            if (!onClickEffect)
                            {
                                clickEffect.SetActive(true);
                                onClickEffect = true;
                            }
                        }
                    }


                    //탭을 안하여 각도가 그래로인 경우
                    if (!onClickEffect)
                        transform.rotation = defaultRotation;
                    //탭을 하여 각도가 달라진 경우
                    else
                        transform.rotation = fusionRotation;
                }
            }
        }

        //합체 가능 상태일때 옆에 슬라임이 있으면 합칩니다.
        if (canFusion)
        {
            Vector3 direction = new Vector3();

            //오른쪽을 보는 슬라임
            if (fusionRotationY == 90)
                direction = Vector3.right;
            //왼쪽을 보는 슬라임
            else
                direction = Vector3.left;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 1f, 1 << 6))
            {
                DoFusion();
            }
        }

        //라이트닝 목표 위치를 다른 슬라임으로 이동시킴.
        if(lightingEffect.activeSelf && partnerController.enabled)
        {
            lightingTarget = partnerController.transform;
        }
        else
        {
            SetLighting(false);
        }
    }

    private IEnumerator DetectionDoubleTab()
    {
        //한번 탭한 이후 일정 시간안에 한번 더 탭할 경우 합체 가능 상태로 됨.
        while(Time.time < firstTabTime + doubleTabTime)
        {
            if (onClick && !canFusion)
            {
                canFusion = true;
                fusionPartner.SetActive(true);
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        
    }

    // 특정 애니메이션을 취하고 합체를 합니다.
    public void DoFusion()
    {
        transform.rotation = fusionRotation;
        Animator slimeAni = GetComponent<Animator>();
        clickEffect.SetActive(false);
        slimeAni.SetTrigger("Fusion");
    }

    // 합체된 슬라임인 킹슬라임이 활성화 됩니다.
    public void MakeKing()
    {
        kingSlime.SetActive(true);
        kingSlime.transform.localPosition = new Vector3(Mathf.Abs(transform.position.x - partnerController.transform.position.x) / 2,
            Mathf.Abs(transform.position.y - partnerController.transform.position.y) / 2,
            Mathf.Abs(transform.position.z - partnerController.transform.position.z) / 2);
        
    }

    private void OnEnable()
    {
        ui.OnPopUp();

        //다른 쪽이 합체 가능 상태이면 자신도 합체 가능 상태로 활성화 됩니다.
        if (partnerController.canFusion)
        {
            canFusion = true;
            clickEffect.SetActive(true);
            onClickEffect = true;
            transform.rotation = fusionRotation;
            SetLighting(true);
            partnerController.SetLighting(true);
        }
        myFusionCard.SetActive(false);
    }

    //라이트닝 활성화/비활성화.
    public void SetLighting(bool boolean)
    {
        if (boolean)
        {
            lightingEffect.SetActive(true);
            lightingTarget = partnerController.transform;
        }
        else
        {
            lightingEffect.SetActive(false);
        }
    }

    //UI확인
    public void CheckedUI()
    {
        checkUI = true;
    }
}
