﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 오브젝트 풀에 있는 Enemy Object를 활성화 시켜서 씬에 위치시키겠다.
public class EnemyManager : MonoBehaviour
{
    // 에너미 체력을 제어하는 코드를 싱글톤 디자인과 프로퍼티를 이용해서 관리
    public static EnemyManager Instance;

    [SerializeField] ObjectManager obMgr;
    [SerializeField] float maxCreatTime = 5.0f;

    int _hp = 3;

    public int HP
    {
        get { return _hp; }
        set
        {
            _hp = value;
            if (_hp <= 0) Destroy(gameObject);
        }
    }

    GameObject obj;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != null) Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartCoroutine(CreateEnemy());
        }
    }

    IEnumerator CreateEnemy()
    {
        float rand = Random.Range(0, maxCreatTime);

        yield return new WaitForSeconds(rand);
        obMgr.GetObject();

    }
}
