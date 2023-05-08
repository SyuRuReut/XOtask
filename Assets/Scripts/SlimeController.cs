using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeController : MonoBehaviour
{
    [SerializeField] private UIController ui;
    private bool checkUI;

    //ȸ�� ����
    [SerializeField] private float rotSpeed;
    private Quaternion defaultRotation;
    private Quaternion fusionRotation;
    [SerializeField] private float fusionRotationY;

    //�ѹ� �� ����
    private bool onClick;
    [SerializeField] private GameObject clickEffect;
    private bool onClickEffect;

    //���� �� ����
    [SerializeField] private float doubleTabTime;
    private float firstTabTime;

    //��ü ����
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
        //UI�� Ȯ���ؾ� ��ġ�Ͽ� �۵���.
        if (checkUI)
        {
            //�� �� �̻� ��ġ �� ���
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                //��ġ�� �Ѱ����� �������� ���̸� �߻��� �����ӿ� ���� ���
                if (touch.phase == TouchPhase.Began)
                {
                    Ray ray = Camera.main.ScreenPointToRay(touch.position);
                    RaycastHit hitInfo;

                    if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 6))
                    {
                        onClick = true;
                    }
                }
                //��ġ�� ������ ��� ������ ���� ���� ȸ��
                else if (touch.phase == TouchPhase.Moved)
                {

                    Vector3 deltaPos = touch.deltaPosition;

                    transform.Rotate(transform.up, deltaPos.x * -1.0f * rotSpeed);
                    transform.Rotate(transform.right, deltaPos.y * 1.0f * rotSpeed);

                }
                //��ġ�� ���� ��� ȸ���� ���󺹱��� ����Ŭ�� Ȯ��
                else if (touch.phase == TouchPhase.Ended)
                {
                    //��ġ�� ������ ���� �������̰� ���� ���� �������� ���
                    if (onClick)
                    {
                        Ray ray = Camera.main.ScreenPointToRay(touch.position);
                        RaycastHit hitInfo;
                        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, 1 << 6))
                        {
                            //���������� Ȯ��
                            onClick = false;
                            firstTabTime = Time.time;
                            StartCoroutine(DetectionDoubleTab());

                            //Ŭ�� ����Ʈ�� ���� ������ �ʾ��� ��� ����Ʈ�� ����
                            if (!onClickEffect)
                            {
                                clickEffect.SetActive(true);
                                onClickEffect = true;
                            }
                        }
                    }


                    //���� ���Ͽ� ������ �׷����� ���
                    if (!onClickEffect)
                        transform.rotation = defaultRotation;
                    //���� �Ͽ� ������ �޶��� ���
                    else
                        transform.rotation = fusionRotation;
                }
            }
        }

        //��ü ���� �����϶� ���� �������� ������ ��Ĩ�ϴ�.
        if (canFusion)
        {
            Vector3 direction = new Vector3();

            //�������� ���� ������
            if (fusionRotationY == 90)
                direction = Vector3.right;
            //������ ���� ������
            else
                direction = Vector3.left;
            Ray ray = new Ray(transform.position, direction);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 1f, 1 << 6))
            {
                DoFusion();
            }
        }

        //����Ʈ�� ��ǥ ��ġ�� �ٸ� ���������� �̵���Ŵ.
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
        //�ѹ� ���� ���� ���� �ð��ȿ� �ѹ� �� ���� ��� ��ü ���� ���·� ��.
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

    // Ư�� �ִϸ��̼��� ���ϰ� ��ü�� �մϴ�.
    public void DoFusion()
    {
        transform.rotation = fusionRotation;
        Animator slimeAni = GetComponent<Animator>();
        clickEffect.SetActive(false);
        slimeAni.SetTrigger("Fusion");
    }

    // ��ü�� �������� ŷ�������� Ȱ��ȭ �˴ϴ�.
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

        //�ٸ� ���� ��ü ���� �����̸� �ڽŵ� ��ü ���� ���·� Ȱ��ȭ �˴ϴ�.
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

    //����Ʈ�� Ȱ��ȭ/��Ȱ��ȭ.
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

    //UIȮ��
    public void CheckedUI()
    {
        checkUI = true;
    }
}
