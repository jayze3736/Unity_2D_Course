using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

/* 1. 코루틴에서 yield break는 완전한 코루틴 종료를 의미한다.
  * 2. update 문에서 return은 Update문의 처음으로 돌아가는 명령어이다.
  * 
  */

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]

public class EnemyAI : MonoBehaviour
{
    //Enemy 기준의 target(player)
    public Transform target;

    //path를 update하는 빈도 또는 속도
    public float updateRate = 2f;

    //Caching
    private Seeker seeker;
    private Rigidbody2D rb;

    //알고리즘으로 계산된 path
    public Path path;

    //AI의 스피드
    public float speed = 300f;
    //AI의 물리적인 상호작용 속성을 다루기 위한 변수 fMode
    public ForceMode2D fMode;

    [HideInInspector]//public이지만 Inspector에서 해당 컴포넌트를 보이지않게함
    public bool pathIsEnded = false;

    // Path에서 다음지점까지의 최대거리
    public float nextWaypointDistance = 3f;

    // 현재 Enemy가 가고있는 Path 상의 지점
    private int currentWaypoint = 0;
    

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        if(target == null)
        {
            Debug.Log("target is null");
            return;
        }

        // start 부터 end까지 path를 계산하고 그 결과(path)를 세번째 함수의 인자로 전달하고 호출한다.
        //(calculate path from start to end and return the result to OnPathComplete func
        seeker.StartPath(transform.position, target.position, OnPathComplete);
        StartCoroutine("UpdatePath");

    }

    IEnumerator UpdatePath()
    {
        if (target == null)
        {
            Debug.Log("target is null");
            yield break;
        }

        //만약에 AI가 Player에 도달하여 Path의 end까지 도달한 다음 다시 새로운 Path를 생성해야 될 경우 FixedUpdate에서 PathIsEnded가 false가
        //될때까지 코루틴은 while문에서 대기, 무한 반복하면 게임이 멈추므로 일정시간의 딜레이를 준다.
        while (pathIsEnded)
        {
            yield return new WaitForSeconds(0.1f);
        }

        seeker.StartPath(transform.position, target.position, OnPathComplete);
        yield return new WaitForSeconds( 1.0f / updateRate);
        StartCoroutine("UpdatePath");
    }

    // AI의 움직임 구현
    public void FixedUpdate()
    {
        if(path == null)
        {
            return;
        }

        pathIsEnded = false;

        float dist = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);

        // target과의 거리를 좁히기 위한 다음 지점 탐색, nextWaypointDistance가 3이므로 3보다는 큰 거리로 항상 이동함
        if(dist < nextWaypointDistance)
        {
            if (currentWaypoint + 1 < path.vectorPath.Count)
            {
                currentWaypoint++;
            }
            else
            {
                pathIsEnded = true;
                return;
            }
                
        }

        // 움직임 구현
        Vector3 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        Vector3 velocity = dir * speed * Time.fixedDeltaTime;

        rb.AddForce(velocity, fMode);

    }


    public void OnPathComplete(Path p)
    {
        Debug.Log("We got a path, Did it have any error? " + p.error);
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;

        }

    }

    


}
