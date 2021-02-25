using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class GameMaster : MonoBehaviour
{

    //GameMaster Script는 몬스터를 스폰하거나, 아니면 일종의 플레이어 이동, 씬 변경 등을 관리하고 수행하도록 하는데 사용
    //Static function은 모든 script에서 공유됨
    int index = 0;
    //single tone pattern, 유일한 클래스 객체
    public static GameMaster gm;

    private void Start()
    {
        if(gm == null)
        {
            //내가 보기엔 GameObject 클래스의 오브젝트 중에서 Tag가 GM인 오브젝트의 Component 성분인 GameMaster를 가져오라고 하는 뜻인듯
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
            
    }

    

    public Transform PlayerPrefab;
    public Transform SpawnPoint;
    public float SpawnDelay = 2;
    public Transform SpawnPrefab;

    



    //단순히 Player를 리스폰하는 함수
    public IEnumerator RespawnPlayer()
    {
        

        AudioSource audio = GetComponent<AudioSource>();
        audio.Play();
        Debug.Log("TODO: Respawning Effect");
        yield return new WaitForSeconds(SpawnDelay);

        
        Transform PlayerObj = Instantiate(PlayerPrefab, SpawnPoint.position, SpawnPoint.rotation);
        
        
        

        //Spawn Effect
        Transform clone = Instantiate(SpawnPrefab, SpawnPoint.position, SpawnPoint.rotation);
        Destroy(clone.gameObject, 3f);

        Debug.Log("TODO: Add Spawn Particles");



    }


    //static 함수이기때문에 외부에서 접근가능
    public static void KillPlayer(Player player)
    {
        
        Destroy(player.gameObject);
        gm.StartCoroutine(gm.RespawnPlayer());
    }

    public static void KIllEnemy(Enemy enemy)
    {

        Destroy(enemy.gameObject);

    }




}
