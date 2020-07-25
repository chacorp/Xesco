﻿using System.Collections.Generic;
using UnityEngine;

// ===========================================================================
// 마우스 커서를 아이템에 갖다 대면,
// 1. 화면에 안내 메시지 띄우기 ---------------------------------------------[ ]
// 2. [E] 키를 누르면 아이템 눈 앞으로 가져오기 ------------------------------[O]

// 아이템을 가져온 상태에서,
// 1. 마우스 우클릭을 하고 마우스를 움직이면 아이템 회전하기 ------------------[O]
// 2. 마우스 좌클릭을 하면 아이템 던져버리기!! -------------------------------[O]
// ===========================================================================

public class CSH_ItemGrab : MonoBehaviour
{
    public static CSH_ItemGrab Instance;
    CSH_ItemGrab()
    {
        Instance = this;
    }



    [Header("Interacting Object")]
    // 현재 마우스 커서로 가리키고 있는 아이템
    public GameObject pointingItem;

    // 선택한 아이템
    GameObject selectedItem;

    // 텍스트 UI
    public GameObject PressE;

    // [특수 템]을 위한 인벤토리
    public Transform inventory;
    List<GameObject> invntry = new List<GameObject>();

    // 아이템의 컴포넌트를 담아둘 전역변수
    CSH_ItemSelect itemSelect;
    Rigidbody itemRB;




    [Header("Properties")]
    // 던져버릴 속도
    public float throwSpeed = 15f;
    // 가져올 속도
    public float grabSpeed = 100f;

    // 현재 [아이템]을 잡고 있나?
    public bool hasItem;
    bool grabing;

    private void Start()
    {
        grabing = false;
        hasItem = false;

        PressE.SetActive(false);
    }

    void Grab_item()
    {
        // 선택한 [아이템]의 컴포넌트를 전역변수에 넣어두기
        if (selectedItem != null)
        {
            itemSelect = selectedItem.GetComponent<CSH_ItemSelect>();
            itemRB = selectedItem.GetComponent<Rigidbody>();
        }

        // 잡기 시작
        if (grabing)
        {
            // [특수 템]이라면,
            if (itemSelect.isSpecial)
            {
                // 인벤토리 리스트에 추가하기
                invntry.Add(pointingItem);
                selectedItem.transform.SetParent(inventory);
                // 비활성화하기
                selectedItem.SetActive(false);

                // 잡기 탈출
                grabing = false;
            }

            // 그냥 [아이템]이라면,
            else
            {
                // 아이템의 rigidbody 물리엔진 끄기
                itemRB.isKinematic = true;

                // [아이템]의 위치를 (this)의 위치로 바꾸기
                Vector3 dir = transform.position - selectedItem.transform.position;
                //selectedItem.transform.position = Vector3.Lerp(selectedItem.transform.position, transform.position, 20 * Time.deltaTime);
                selectedItem.transform.position += dir.normalized * grabSpeed * Time.deltaTime;

                if (dir.magnitude <= 1f)
                {
                    selectedItem.transform.position = transform.position;
                    // [아이템]의 부모를 (this)로 설정하기
                    // = [아이템]을 (this)의 자식으로 가져오기
                    selectedItem.transform.SetParent(transform);

                    // 아이템의 csh_itemselect한테  <잡힌 상태> 라고 알려주기
                    itemSelect.isGrabed = true;

                    // 현재 [아이템]을 갖고 있다!
                    hasItem = true;

                    // 잡기 탈출
                    grabing = false;
                }
            }
        }
    }

    void Spin_item()
    {
        // 마우스 인풋 가져오기
        float mx = Input.GetAxis("Mouse X");
        float my = Input.GetAxis("Mouse Y");

        // 마우스 인풋값으로 [아이템] 돌리기
        pointingItem.transform.localEulerAngles += new Vector3(my, -mx, 0);
    }

    void Throw_item()
    {
        // ===============================================
        //   아이템의 컴포넌트 가져오기를 또 할 필요는 없다. 
        //       왜냐하면 이미 전역변수로 갖고 있으니까!
        // ===============================================
        // itemSelect = pointingItem.GetComponent<CSH_ItemSelect>();
        // itemRB = pointingItem.GetComponent<Rigidbody>();

        // 아이템의 Rigidbody 물리엔진 켜기
        itemRB.isKinematic = false;

        // 아이템의 CSH_ItemSelect한테  <안 잡힌 상태>  라고 알려주기
        itemSelect.isGrabed = false;

        // [아이템]의 부모를 [null]로 설정하기
        // => 현재 갖고 있던 자식 비우기
        pointingItem.transform.SetParent(null);

        // 보고 있는 방향의 정면으로 던져버리기!!!!!!
        itemRB.AddForce(Camera.main.transform.forward * throwSpeed, ForceMode.Impulse);

        // 위에서 고정했던 pointingItem을 다시 비워두기!!!
        pointingItem = null;

        // 현재 잡고 있는 [아이템] 없음
        hasItem = false;
    }

    void Show_PressE()
    {
        // 현재 [아이템]을 잡고 있다면, 패쓰!
        if (hasItem) return;

        // 가리키는 [아이템]이 있다면, 텍스트 보여주기
        if(pointingItem) PressE.SetActive(true);

        // 가리키는 [아이템]이 없다면, 텍스트 가리기
        else PressE.SetActive(false);
    }

    private void Update()
    {
        // -------------------------------------< [E] 키를 누르면 아이템 눈 앞으로 가져오기 >
        // 1. 커서로 가리키는 아이템이 있고   &또한&   현재 아이템을 잡고 있지 않다면,
        if (!hasItem)
        {
            if (pointingItem)
            {
                // 2. [E] 키를 눌러서 아이템 가져오기
                if (Input.GetKeyDown(KeyCode.E))
                {
                    // 커서로 가리킨 [아이템]을 선택한 [아이템]으로 설정한다
                    selectedItem = pointingItem;
                    grabing = true;
                }
            }
            // " Press E " 띄우기
            Show_PressE();

            // 잡기 함수 실행!
            Grab_item();
        }

        // 현재 [아이템]을 잡고 있다면
        if (hasItem)
        {
            // 현재 잡고 있는 아이템을 자기 자식으로 가져온 아이템으로 고정하기
            pointingItem = transform.GetChild(0).gameObject;

            //   :: 이렇게 하는 이유 ::
            //
            //   CSH_ItemSelect 때문에 아이템 위에 커서가 없으면,
            //   아이템을 잡고 있음에도, pointingItem이 계속 null이 떠버린다!!!



            // -------------------------------------< 마우스 우클릭을 한 채로 움직이면 아이템 회전하기 >
            // 1. 마우스 우클릭을 지속하는 중이고
            if (Input.GetMouseButton(1))
            {
                // 2. 마우스를 움직이면 아이템 회전하기
                Spin_item();
            }

            // -------------------------------------< 마우스 좌클릭을 하면 아이템 던져버리기!! >
            // 1. 마우스 좌클릭을 했다면,
            if (Input.GetMouseButtonDown(0))
            {
                // 2. 보고 있는 방향으로 아이템 던지기
                Throw_item();
            }
        }
    }
}
