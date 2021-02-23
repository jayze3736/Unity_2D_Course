using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    //raycast, raycasthit, shooting, DrawLine(to debug), mouseclick,holdingbutton 
    // Start is called before the first frame update
    public float firerate = 0;
    public float Damage = 10;
    public LayerMask WhatToHit; //RayCast를 통해 광선을 발사 시켰을때 특정 레이어 오브젝트를 RayCastHit 대상에서 제외시키기 위해 선언

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;

    float timeToFire = 0;
    float timeToSpawn = 0;
    public float EffectSpawnRate = 10; //1초내에 10개를 초과하는 오브젝트를 생성하지못함
    Transform firePoint; //총알이 발사되는 지점


    //to initiate
    void Awake()
    {
        firePoint = transform.Find("firepoint");
        if (firePoint == null)
            Debug.Log("There is no child called firepoint");      
    }

    // Update is called once per frame
    void Update()
    {
        
        /* GetButtonDown: 해당 버튼을 한번 누르면 True를 반환한다. 일회성이므로 계속 누르고있어도 한번 True를 반환하고 이후에 False 반환
         * GetButton: 해당 버튼을 누르고 있는 동안에 True를 반환하는 Boolean 함수
        */
        if (firerate == 0)
        {   
            if(Input.GetButtonDown("Fire1"))
            {
                Shoot();
            }

        }
        else
        {
            //마우스 버튼을 계속 누르고 있는 상태
            if(Input.GetButton("Fire1") && Time.time > timeToFire)
            {
                /* 일정 delay(= 1/firerate) 만큼 이후에 다시 투사체를 발사함.
                 * 따라서 firerate값이 클수록 delay가 줄어들어 짧은 간격으로 투사체를 발사한다.
                 */
                timeToFire = Time.time + 1 / firerate;
                Shoot();
            }
        }
        
        
    }

    void Shoot()
    {
        
        Vector2 mouseposition = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
        Vector2 firePointPosition = new Vector2(firePoint.position.x, firePoint.position.y);
        RaycastHit2D hit = Physics2D.Raycast(firePointPosition, mouseposition - firePointPosition, 100, WhatToHit);
        //Debug.DrawLine(firePointPosition, (mouseposition - firePointPosition) * 100, Color.cyan);
        //hit.collider는 오브젝트의 collider가 광선(ray)에 맞으면 null이 아니고 맞지않으면 null이다.
        if(Time.time >= timeToSpawn)
        {
            Effect();
            timeToSpawn = Time.time + 1 / EffectSpawnRate;
            
        }
        

        if (hit.collider != null)
        {
            Debug.Log("We hit" + hit.collider.name + "and hit" + Damage + "Damage");
           // Debug.DrawLine(firePointPosition, hit.point, Color.red);
        }
            
    }

    void Effect()
    {
        Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation);
        //인스턴화한 MuzzleFlash만을 수정하고 다루기위해서 변수에 대입, Transform 형으로 받는 이유는 Instantiate함수가 Transform 형의 자료를 리턴하기때문
        //총구폭발을 구현하기위해 Flash를 일정 시간 간격으로 생성과 파괴를 반복하여 폭발이 총알이 발사될때마다 보이는 것처럼 구현
        Transform clone = Instantiate(MuzzleFlashPrefab, firePoint.position, firePoint.rotation);
        clone.parent = firePoint;
        float size = Random.Range(0.6f, 0.9f);
        clone.localScale = new Vector3(size, size, size);
        //0.09초 뒤에 clone에 대입된 오브젝트를 삭제
        Destroy(clone.gameObject, 0.09f);

    }
}
