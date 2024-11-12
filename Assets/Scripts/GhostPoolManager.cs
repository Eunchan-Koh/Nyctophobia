using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostPoolManager : MonoBehaviour
{
    //1. 프리팹들을 보관할 변수
    public GameObject[] ghostPrefabs;
    //2. 풀 담당을 하는 리스트들
    List<GameObject>[] pools;

    void Awake(){
        pools = new List<GameObject>[ghostPrefabs.Length];

        for(int i = 0; i < ghostPrefabs.Length; i++){
            pools[i] = new List<GameObject>();
        }

    }

    public GameObject Get(int index){
        GameObject select = null;

        //선택한 풀의 놀고있는 오브젝트 접근
        foreach(GameObject obj in pools[index]){
            if(!obj.activeSelf){
                select = obj;
                select.SetActive(true);
                break;
            }
        }
        //없으면 새로 생성해서 pool에 추가 후 가져가기
        if(!select){
            select = Instantiate(ghostPrefabs[index], transform);
            pools[index].Add(select);
        }
        return select;
    }

}
