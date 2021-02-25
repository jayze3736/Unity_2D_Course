using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class test : MonoBehaviour
{
    int i = 0;
    
    IEnumerator Show()
    {
        
        yield return new WaitForSeconds(0.5f);
        StartCoroutine("Show");
        i++;
    }

    private void Start()
    {
        StartCoroutine("Show");
    }

    public void Update()
    {
        if(i > 100)
        {

        }
    }


}
