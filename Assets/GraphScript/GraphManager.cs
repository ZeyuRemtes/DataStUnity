
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GraphManager : MonoBehaviour
{
    public GameObject pointPrefab;
    public GameObject linePrefab;

    private int i,j;
    private int[,]Marix;
    private int[]Visited;
    private List<GameObject> teamline;
    private List<GameObject> teampoint;

    public Sprite point_choose;
    public Sprite point_orignal;

    public Button Connect;
    public Button Destory;
    public Button Creat;
    public Button DeepTravel;
    public Button BorderTravel;
    public Button TravelReset;
    public Button Restart;

    private Vector3 pos1;
    private Vector3 pos2;
    private LineRenderer lineRenderer;

    private bool creatflag;
    private bool destoryflag;
    private bool connectflag;
    private bool drawcolorflag;

    private bool fromorto;
    private GameObject from;
    private GameObject to;

    private Queue<GameObject> pointQueue ;
    

    // Start is called before the first frame update
    void Start()
    {
        i = 1;
        j = 1;
        Marix = new int[50,50];
        Visited = new int[50];
        teamline = new List<GameObject>();
        teampoint = new List<GameObject>();
        for (int m = 0; m < 50; m++)
        {
            for (int n = 0; n < 50; n++)
            {
                Marix[m,n] = 0;
            }
        }

        GameObject firstpoint = GameObject.Find("FirstPoint");
        teampoint.Add(firstpoint);

        pointQueue = new Queue<GameObject>();

        pos1 = new Vector3(0, 0, 0);
        pos2 = new Vector3(0, 0, 0);

        creatflag = false;
        destoryflag = false;
        connectflag = false;
        drawcolorflag = false;

        fromorto = false;
        from = null;
        to = null;


        //lineRenderer = (LineRenderer)line.GetComponent("LineRenderer");

        Creat.onClick.AddListener(onCreat);
        Destory.onClick.AddListener(onDestory);
        Connect.onClick.AddListener(onConnect);
        DeepTravel.onClick.AddListener(onDeepTravel);
        BorderTravel.onClick.AddListener(onBorderTravel);
        TravelReset.onClick.AddListener(onTravelReset);
        Restart.onClick.AddListener(onReset);
    }

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("touch area is UI");
        }
        else 
        {
            if (Input.GetMouseButtonUp(0)&& creatflag == true)
            {
                CreatPoint();
            }
            if (Input.GetMouseButtonUp(0) && destoryflag == true)
            {
                DestoryPoint();
            }
            if (Input.GetMouseButtonUp(0) && connectflag == true)
            {
                ConnectPoint();
            }
        }
        
        if (drawcolorflag == true)
        {
            GameObject temppoint = pointQueue.Dequeue();
            temppoint.GetComponent<SpriteRenderer>().sprite = point_choose;
            drawcolorflag = false;
        }

    }
    void onDestory()
    {
        creatflag = false;
        destoryflag = true;
        connectflag = false;
    }
    void onCreat()
    {
        creatflag = true;
        destoryflag = false;
        connectflag = false;
    }
    void onConnect()
    {
        creatflag = false;
        destoryflag = false;
        connectflag = true;
    }
    void onDeepTravel()
    {
        creatflag = false;
        destoryflag = false;
        connectflag = false;
        DeepTravelingfindstart();
    }
    void onBorderTravel()
    {
        creatflag = false;
        destoryflag = false;
        connectflag = false;
        BorderTravelingstart();
    }
    void onTravelReset()
    {
        pointQueue.Clear();
        GameObject temppoint;
        for (int i = 0; i < teampoint.Count; i++)
        {
            if (teampoint[i] != null)
            {
                temppoint = teampoint[i];
                temppoint.GetComponent<SpriteRenderer>().sprite = point_orignal;
                Visited[i] = 0;
            }
        }
    }
    void onReset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


    void CreatPoint()
    {
        Debug.Log("Creat Mouse up");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.point);
            Vector3 pos = hit.point;
            pos.z = 0;
            hit.point = pos;
            GameObject point = GameObject.Instantiate
                        (pointPrefab, hit.point, transform.rotation) as GameObject;
            point.GetComponentInChildren<TextMesh>().text = i.ToString();
            teampoint.Add(point);
            i++;
        }
        int length = teampoint.Count;
        print(length);
    }
    void DestoryPoint()
    {

        Debug.Log("Destory Mouse up");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log(hit.transform.name);
            if (hit.transform.name != "Plane")
            {
                
                int num =int.Parse(hit.transform.gameObject.GetComponentInChildren<TextMesh>().text);
                Debug.Log(num);
                for (int k = 0; k < 20; k++)
                {
                    if (Marix[k,num] != 0)
                    {
                        GameObject templine = teamline[Marix[k,num]-1];
                      //team.RemoveAt(Marix[k,num] - 1);
                        Debug.Log(Marix[k, num]);
                        Marix[k, num] = 0;
                        Destroy(templine);
                    }
                    if (Marix[num,k] != 0)
                    {
                        Debug.Log(Marix[num, k]);
                        GameObject templine = teamline[Marix[num,k] - 1];
                      //team.RemoveAt(Marix[num,k] - 1);
                        
                        Marix[num, k] = 0;
                        Destroy(templine);
                    }
                }
                Destroy(hit.transform.gameObject);
            }
        }
    }
    void ConnectPoint()
    {
        Debug.Log("Connect Mouse up");
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.name != "Plane")
            {
                if (fromorto == false)
                {
                    from = hit.transform.gameObject;
                    fromorto = true;

                    from.GetComponent<SpriteRenderer>().sprite = point_choose;

                }
                else if (hit.transform.gameObject!= from&&fromorto==true)
                {
                    to = hit.transform.gameObject;
                    from.GetComponent<SpriteRenderer>().sprite = point_orignal;
                    ConnectMove();
                }
                else if (hit.transform.gameObject == from)
                {
                    fromorto = false;
                    from.GetComponent<SpriteRenderer>().sprite = point_orignal;
                    from = null;
                }
            }
        }
    }
    void ConnectMove()
    {
        fromorto = false;

        Debug.Log("fron position "+from.transform.position);
        Debug.Log("to position"+ to.transform.position);
        pos1 = from.transform.position;
        pos2 = to.transform.position;

        GameObject line = GameObject.Instantiate
                (linePrefab, new Vector3(0, 0, 0), transform.rotation) as GameObject;

        lineRenderer = (LineRenderer)line.GetComponent("LineRenderer");


        lineRenderer.SetPosition(0, pos1);
        lineRenderer.SetPosition(1, pos2);

        
        int m =int.Parse(from.GetComponentInChildren<TextMesh>().text);
        int n =int.Parse(to.GetComponentInChildren<TextMesh>().text);

        Marix[m,n] = j;
        Marix[n,m] = j;

        Debug.Log(m + " " + n + " "+j);
        Debug.Log(Marix[m, n]);
        teamline.Add(line);
        Debug.Log(teamline[j-1]);
        j++;

        from = null;
        to = null;
    }
    void DFSInter(int i)
    {
        int j, flag;
        flag = 0;
        Stack<GameObject> stack = new Stack<GameObject>();
        Visited[i] = 1;
        GameObject temppoint = teampoint[i];
      //temppoint.GetComponent<SpriteRenderer>().sprite = point_choose;
        stack.Push(temppoint);
        pointQueue.Enqueue(temppoint);

       

        int time = 0;

        while (stack.Count != 0)
        {
            time++;
           
            temppoint = stack.Peek();
           
            i = int.Parse(temppoint.GetComponentInChildren<TextMesh>().text);
      

            for (j = 0; j < teampoint.Count; j++)
            {
                if (Marix[i, j] != 0 && Visited[j] == 0)
                {
           
                    Visited[j] = 1;
                    temppoint = teampoint[j];
                    pointQueue.Enqueue(temppoint);
                  //temppoint.GetComponent<SpriteRenderer>().sprite = point_choose;
                        
                    stack.Push(temppoint);
                  
                    flag = 1;
                    break;
                }
            }
            
            if (flag == 0)
            {
                if (stack.Count != 0)
                {
                    temppoint = stack.Pop();
                }
   
            }
            flag = 0;
        }
    }
    void DeepTravelingfindstart()
    {
        if (from != null)
        {
            from.GetComponent<SpriteRenderer>().sprite = point_orignal;
            from = null;
        }

        for (int k = 0; k < teampoint.Count; k++)
        {
            Visited[k] = 0;  //初始化访问状态
        }
        

        for (int m = 0; m < teampoint.Count; m++)
        {
            if (Visited[m] == 0 && teampoint[m]!=null)
            {
                DFSInter(m);
            }
        }
        
        DrawColor();
    }
    void BorderTravelingstart()
    {
        Queue<GameObject> queue = new Queue<GameObject>();
        for (int i = 0; i < teampoint.Count; i++)
        {
            Visited[i] = 0;
        }

        for (int i = 0; i < teampoint.Count; i++)
        {
            if (Visited[i] == 0 && teampoint[i]!=null)
            {
                Visited[i] = 1;
                pointQueue.Enqueue(teampoint[i]);
                queue.Enqueue(teampoint[i]);
                while (queue.Count != 0)
                {
                    GameObject temppoint = queue.Dequeue();
                    int k = int.Parse(temppoint.GetComponentInChildren<TextMesh>().text);
                    for (int j = 0; j < teampoint.Count; j++)
                    {
                        if (Marix[k,j]!= 0 && Visited[j] == 0)
                        {
                            Visited[j] = 1;
                            pointQueue.Enqueue(teampoint[j]);
                            queue.Enqueue(teampoint[j]);
                        }
                    }
                }
            }
        }
        DrawColor();
    }
    void DrawColor()
    {
        for (int m = 0; m < teampoint.Count; m++)
        {
            drawcolorflag = true;
        }
    }
    
}
