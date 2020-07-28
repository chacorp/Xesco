﻿using UnityEngine;

// ========================================================================
// 키패드 1, 2, 3, 4, 5 를 누르면 
// 1. 아이템을 바꾼다.---------------------------------------------------[ ]
// 2. 퀵 메뉴를 띄웠다가 다시 내린다. ------------------------------------[O]
// ========================================================================

public class CSH_QuickMenu : MonoBehaviour
{        
    // 퀵 메뉴 보여줄 속도
    public float moveSpeed = 200f;

    // UI 트랜스폼
    RectTransform RT;

    // UI 올라올 위치
    float Ypos = 540f;

    // UI 현재 위치
    float Ypos_D;

    // 타이머
    float timer;
    public float SetTimer
    {
        set 
        {
            timer = value;
        }
    }

    // 퀵 메뉴 온/오프 여부
    public bool showQM;
    public bool QM_Control;

    void Start()
    {
        // 퀵 메뉴 보여주기
        showQM = true;
        RT = GetComponent<RectTransform>();

        // 현재 위치 가져오기
        Ypos_D = RT.anchoredPosition.y; // 540

        // 시작하면서 퀵 메뉴 올려놓기
        RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, Ypos);
        showQM = true;
    }

    void Show_QuickMenu()
    {
        if (showQM)
        {
            // 천천히 올리기
            RT.anchoredPosition += new Vector2(0, 1) * moveSpeed * Time.deltaTime;

            // 적당한 위치에서 멈추기
            if (RT.anchoredPosition.y >= Ypos)
            {
                RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, Ypos);
                if (QM_Control) return;

                timer += Time.deltaTime;
                if (timer > 2)
                {
                    timer = 0;
                    showQM = false;
                }
            }
        }
        else
        {
            RT.anchoredPosition -= new Vector2(0, 1) * moveSpeed * Time.deltaTime;
            if (RT.anchoredPosition.y <= Ypos_D)
            {
                RT.anchoredPosition = new Vector2(RT.anchoredPosition.x, Ypos_D);
            }
        }
    }

    void Update()
    {
        // 1,2,3,4,5 중에 하나라도 누르면
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            showQM = true;
            timer = 0;
        }

        // 퀵 메뉴 보여주기
        Show_QuickMenu();
    }
}