using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class ButtonControl : MonoBehaviour
{
    public GameObject firstpoint;
    public GameObject prefabpoint;
    public GameObject imtarget;
    public Button btn;
    private GameObject temppoint;
    private List<GameObject> team ;
    private int num = 0;

    

    // Start is called before the first frame update
    void Start()
    {
        temppoint = firstpoint;
        btn.onClick.AddListener(delegate () {
            DeletePoint(); 
        });
        team = new List<GameObject>();
    }

    void Update()
    {
        if (num == 0)
        {
            btn.gameObject.SetActive(false);
        }
        else
        {
            btn.gameObject.SetActive(true);
        }
    }
    public void AddPoint(){

         Vector3 pos = temppoint.transform.position;//pos并非引用。
         pos.x += 0.3f;
         
         temppoint = Instantiate(prefabpoint,pos,temppoint.transform.rotation);
         temppoint.transform.parent = imtarget.transform;
       
         
         team.Add(temppoint);
         print(team[num].transform.position.x);
         num++;
         print(num);
    }

    public void DeletePoint(){
        
        if(num != 0) {
            num--;
            team.RemoveAt(num);
            print(num);
            Destroy(temppoint);
            if (num != 0)
            {
                num--;
                temppoint = team[num];
                num++;
            }
            else
            {
                temppoint = firstpoint;
            }
        }
    }
}
                                                                     