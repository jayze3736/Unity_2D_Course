using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour
{
    public int MoveSpeed = 210;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //local space and global space
        //direction이 local space와 global space를 기준 좌표계로 하여 결정됨
        /*local space는 이 스크립트를 가지고 있는 BulletTrail 오브젝트의 position이 기준좌표계의 원점이고 global space의 경우
        (0,0,0)을 기준좌표계의 원점으로 본다. 따라서 direction이 정의될때 한점만 필요하며 이 점은 방향벡터의 끝점이됨
        Translate 함수는 object를 시간에 따라 움직이는 함수라 보면됨
        아마 기본적으로 local space로 세팅이 되어있을 것임 따라서 이동하는 방향은 이 스크립트를 가지고있는
        BulletTrail이 될텐데, 이 BulletTrail이 Pistol 오브젝트에 생성되므로 결국 Pistol의 firePoint가 기준점이되어
        direction이 정의된다고 볼 수 있음
         */
        transform.Translate(Vector3.right * Time.deltaTime * MoveSpeed);
        Destroy(gameObject, 1); //1초뒤에 삭제됨
    }
}
