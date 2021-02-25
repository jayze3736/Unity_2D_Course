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
    public int Damage = 10;
    public LayerMask WhatToHit; //RayCast를 통해 광선을 발사 시켰을때 특정 레이어 오브젝트를 RayCastHit 대상에서 제외시키기 위해 선언

    public Transform BulletTrailPrefab;
    public Transform MuzzleFlashPrefab;
    public Transform GunShotParticlePrefab;

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
       
        

        if (hit.collider != null)
        {
            /*hit.collider.gameObject는 raycast에서 투사되는 오브젝트를 의미하고,
            *만약 존재한다면 Enemy일 것이라 가정한 다음, Enemy 오브젝트는 Enemy.cs 스크립트를 가지고 있으므로
            *그 컴포넌트를 가져와서 DamageEnemy메소드로 투사된 오브젝트에 damage를 전달한다.
            *
            */
            Enemy enemy = hit.collider.gameObject.GetComponent<Enemy>();
            if(enemy != null)
            {
                enemy.DamageEnemy(Damage);
            }


            Debug.Log("We hit" + hit.collider.name + "and hit" + Damage + "Damage");
           // Debug.DrawLine(firePointPosition, hit.point, Color.red);
        }
        
        if (Time.time >= timeToSpawn)
        {
            //필요할때만 사용하도록 코딩하는 것이 깔끔하다.
            Vector3 hitpos;
            //사용하지 않는 벡터를 hitnormal 벡터에 할당하여 raycast에 투사되는 물체가 없을 경우를 해결
            Vector3 hitnormal = new Vector3(9999,9999,9999);
            if (hit.collider == null)
            {
                hitpos = mouseposition * 30;        
            }
            else
            {
                /* Raycast로 투사된 지점을 나타낼때 hit.collider.transform.position 가 아니라 hit.point로 해야한다.
                 * hit.collider.transform.position로 설정하면 투사되는 물체의 좌표를 반환하기때문에
                 * firepoint를 원점으로 맞은 지점 방향으로 bullettrail이 생성되지않고 항상 맞은 물체의 좌표 방향으로
                 * bullettrail이 생성된다. 즉, 고정적인 방향으로 bullettrail이 생성된다. 
                 */
                hitnormal = hit.normal;
                hitpos = hit.point;
            }

            Effect(hitpos, hitnormal);
            timeToSpawn = Time.time + 1 / EffectSpawnRate;

        }

    }

    void Effect(Vector3 hitpos, Vector3 hitnormal)
    {
        Transform trail = Instantiate(BulletTrailPrefab, firePoint.position, firePoint.rotation).transform;
        LineRenderer linerenderer = trail.GetComponent<LineRenderer>();

        if(linerenderer != null)
        {
            linerenderer.SetPosition(0, firePoint.position);
            linerenderer.SetPosition(1, hitpos);
        }

        if(hitnormal != new Vector3(9999, 9999, 9999))
        {
            Transform ShotEffect = Instantiate(GunShotParticlePrefab, hitpos, Quaternion.FromToRotation(Vector3.right, hitnormal));
            Destroy(ShotEffect.gameObject, 0.5f);
        }
        

        


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
