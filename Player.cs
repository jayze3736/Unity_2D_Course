using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Player : MonoBehaviour
{
    /* 중첩 클래스 inner Class: 클래스 내에 클래스를 두어 멤버 변수와 메소드등을 편리하게 또는 알아보기 쉽게 수정및 관리하기위해서 사용
    Player의 경우에도 정보가 여러가지 있을 수 있는데, 예를 들면 플레이어의 스탯 클래스, 타 등장인물 또는 몬스터와의 상호작용 클래스, 스킬 클래스 등 Player의 정보도 내부 클래스를 통해
    관리가 가능하다.*/
    

    // 중첩 클래스 내에 public 멤버 변수가 있을때 사용
    [System.Serializable]


    public class PlayerStats
    {
        public int health = 100;
    }

    // 객체를 생성해야 클래스의 변수및 메소드 사용가능
    public PlayerStats playerstats = new PlayerStats();
    public int fallBoundary = -20;
    private bool Executed = false;


    private void Update()
    {

        if (transform.position.y <= fallBoundary && !Executed)
        {

            //very large integer number to make sure killing the player
            this.gameObject.SetActive(false);
            DamagePlayer(999999);
            Executed = true;

        }

    }

    


    public void DamagePlayer(int Damage)
    {
        playerstats.health -= Damage;

        if(playerstats.health <= 0)
        {
            
            GameMaster.KillPlayer(this);
            Debug.Log("GAME OVER");
        }
    }

}
