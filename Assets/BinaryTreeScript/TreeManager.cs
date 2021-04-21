using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Node
{
    public int data;
    public Node Left;
    public Node Right;
    public Node Parent;
    public float x;
    public float y;
    /*public Node()
    {
        data = 0;
        Left = null;
        Right = null;
        x = 0;
        y = 0;
    }*/
    void Display()
    {
        Debug.Log(data);
    }
}

public class TreeManager : MonoBehaviour
{
    private Node rootNode;
    private LineRenderer lineRenderer;

    public GameObject pointPrefab;
    public GameObject linePrefab;
    public GameObject AllPoint;
    public GameObject AllLine;

    public Sprite point_choose;
    public Sprite point_orignal;

    public InputField insertinput;
    public InputField deleteinput;
    public InputField searchinput;
    public Button InsertButton;
    public Button DeleteButton;
    public Button PreOrderButton;
    public Button InOrderButton;
    public Button PostOrderButton;
    public Button TravelClearButton;
    public Button SearchButton;

    public Text textinfo;

    private Queue<Node> queuetravel;
    private bool traveled;
    private bool messageflag;
    private bool searchend;
    private Transform tempsearch;
    // Start is called before the first frame update
    void Start()
    {  
        rootNode = null;
        traveled = false;
        searchend = false;
        queuetravel = new Queue<Node>();
        InsertButton.onClick.AddListener(onInsert);
        DeleteButton.onClick.AddListener(onDelete);
        PreOrderButton.onClick.AddListener(onPreOrder);
        InOrderButton.onClick.AddListener(onInOrder);
        PostOrderButton.onClick.AddListener(onPostOrder);
        TravelClearButton.onClick.AddListener(onTravelClear);
        SearchButton.onClick.AddListener(onSearch);
    }

    // Update is called once per frame
    void Update()
    {
        if (messageflag == false)
        {
            textinfo.text = "";
        }
        if (searchend == false)
        {
            if (tempsearch != null)
            {
                tempsearch.GetComponent<SpriteRenderer>().sprite = point_orignal;
                tempsearch = null;
            }
        }
            
    }

    void onInsert()
    {
        messageflag = false;
        searchend = false;
        int data =int.Parse(insertinput.text.ToString());
        insertinput.text = "";
        if (data > 99 || data < 0)
        {
            messageflag = true;
            textinfo.text = "提示:数据范围为0-99的整数";
        }
        else if (SearData(data) != null) 
        {
            messageflag = true;
            textinfo.text = "提示:该节点已存在";
        }
        else
        {
            Insert(data);
            Clear();
            Draw(rootNode, 0, 0);
        }
    }
    public void Insert(int data)
    {
        Node Parent;
        //将所需插入的数据包装进节点
        Node newNode = new Node();
        newNode.data = data;

        //如果为空树，则插入根节点
        if (rootNode == null)
        {
            newNode.Parent = null;
            rootNode = newNode;
        }
        //否则找到合适叶子节点位置插入
        else
        {
            Node Current = rootNode;
            while (true)
            {
                Parent = Current;
                if (newNode.data < Current.data)
                {
                    Current = Current.Left;
                    if (Current == null)
                    {
                        newNode.Parent = Parent;
                        Parent.Left = newNode;
                        //插入叶子后跳出循环
                        break;
                    }
                }
                else
                {
                    Current = Current.Right;
                    if (Current == null)
                    {
                        newNode.Parent = Parent;
                        Parent.Right = newNode;
                        //插入叶子后跳出循环
                        break;
                    }
                }
            }
        }
    }
    void onDelete()
    {
        messageflag = false;
        searchend = false;
        int data = int.Parse(deleteinput.text.ToString());
        deleteinput.text = "";

        if (SearData(data) == null)
        {
            messageflag = true;
            textinfo.text = "提示:树中不存在该节点，无法删除";
        }
        else
        {
            Delete(data);
            Clear();
            Draw(rootNode, 0, 0);
        }
    }
    public void Delete(int data)
    {
        Debug.Log(data);
        bool flag = false;
        Node parent = rootNode.Parent;
        Node current = rootNode;
        //首先找到需要被删除的节点&其父节点
        while (true)
        {
            if (data < current.data)
            {
                if (current.Left == null)
                    break;
                parent = current;
                current = current.Left;
            }
            else if (data > current.data)
            {
                if (current == null)
                    break;
                parent = current;
                current = current.Right;
            }
            //找到被删除节点，跳出循环
            else
            {
                flag = true;
                break;
            }
        }

        if (flag == true)
        {
            //找到被删除节点后，分四种情况进行处理
            //情况一，所删节点是叶子节点时，直接删除即可
            if (current.Left == null && current.Right == null)
            {
                //如果被删节点是根节点，且没有左右孩子
                if (current == rootNode && rootNode.Left == null && rootNode.Right == null)
                {
                    rootNode = null;
                }
                else if (current.data < parent.data)
                    parent.Left = null;
                else
                    parent.Right = null;
                current = null;
            }
            //情况二，所删节点只有左孩子节点时
            else if (current.Left != null && current.Right == null)
            {
                Debug.Log("情况二");
               if (current == rootNode)
               {
                    rootNode = rootNode.Left;
                    rootNode.Parent = null;
               }
               else
               {
                    if (current.data < parent.data)
                        parent.Left = current.Left;
                    else
                        parent.Right = current.Left;
                    current.Left.Parent = parent;
                    current = null;
               }
               
                
            }
            //情况三，所删节点只有右孩子节点时
            else if (current.Left == null && current.Right != null)
            {
                Debug.Log("情况三");
                if (current == rootNode)
                {
                    rootNode = rootNode.Right;
                    rootNode.Parent = null;
                }
                else
                {
                    if (current.data < parent.data)
                        parent.Left = current.Right;
                    else
                        parent.Right = current.Right;
                    current.Right.Parent = parent;
                    current = null;
                }
                
            }
            //情况四，所删节点有左右两个孩子
            else
            {

                Debug.Log("情况四");
                Node temp;
                //当被删节点是根节点，并且有两个孩子时
                /*左子树的最大点，右子树的最小点都可以*/

                if (current == rootNode)
                {
                    temp = current.Left;
                    if (temp.Right == null)
                    {
                        temp.Right = current.Right;
                        current.Right.Parent = temp;
                        rootNode = temp;
                    }
                    else
                    {
                        while (temp.Right != null)
                        {
                            temp = temp.Right;
                        }
                        if (temp.Left != null)
                        {
                            temp.Parent.Right = temp.Left;
                            temp.Left.Parent = temp.Parent;
                        }
                        else temp.Parent.Right = null;

                        temp.Parent = null;
                        rootNode = temp;

                        temp.Left = current.Left;
                        current.Left.Parent = temp;

                        temp.Right = current.Right;
                        current.Right.Parent = temp;
                    }
                    current = null;
                }
                

                else
                {
                    //current是被删的节点

                    //先判断是父节点的左孩子还是右孩子
                    //父节点的左孩子
                    if (current.data < parent.data)
                    {
                        //temp是被删节点的右子树的最小值的点
                        temp = current.Right;

                        if (temp.Left == null)
                        {
                            parent.Left = temp;
                            temp.Parent = parent;
                            temp.Left = current.Left;
                            current.Left.Parent = temp;
                        }
                        else
                        {
                            while (temp.Left != null)
                            {
                                temp = temp.Left;
                            }
                            if (temp.Right != null)
                            {
                                temp.Parent.Left = temp.Right;
                                temp.Right.Parent = temp.Parent;
                            }
                            else temp.Parent.Left = null;

                            parent.Left = temp;
                            temp.Parent = parent;

                            temp.Left = current.Left;
                            current.Left.Parent = temp;

                            temp.Right = current.Right;
                            current.Right.Parent = temp;
                        }
                        current = null;
                    }

                    //右孩子
                    else if (current.data > parent.data)
                    {
                        //temp是被删节点的左子树的最大值的点
                        temp = current.Left;
                        if (temp.Right == null)
                        {
                            parent.Right = temp;
                            temp.Parent = parent;
                            temp.Right = current.Right;
                            current.Right.Parent = temp;
                        }
                        else
                        {
                            while (temp.Right != null)
                            {
                                temp = temp.Right;
                            }
                            if (temp.Left != null)
                            {
                                temp.Parent.Right = temp.Left;
                                temp.Left.Parent = temp.Parent;
                            }
                            else temp.Parent.Right = null;

                            parent.Right = temp;
                            temp.Parent = parent;

                            temp.Left = current.Left;
                            current.Left.Parent = temp;

                            temp.Right = current.Right;
                            current.Right.Parent = temp;
                        }
                        current = null;

                    }
                }
            }
        }
    }
    void onPreOrder()
    {
        messageflag = false;
        searchend = false;
        if (traveled == false)
            PreOrder(rootNode);
        int tempdata = queuetravel.Dequeue().data;
        foreach (Transform child in AllPoint.gameObject.transform)
        {
            if (int.Parse(child.GetComponentInChildren<TextMesh>().text) == tempdata)
            {
                child.GetComponent<SpriteRenderer>().sprite = point_choose;
            }
        }
    }
    void onInOrder()
    {
        messageflag = false;
        searchend = false;
        if (traveled == false)
            InOrder(rootNode);
        int tempdata = queuetravel.Dequeue().data;
        foreach (Transform child in AllPoint.gameObject.transform)
        {
            if (int.Parse(child.GetComponentInChildren<TextMesh>().text) == tempdata)
            {
                child.GetComponent<SpriteRenderer>().sprite = point_choose;
            }
        }
    }
    void onPostOrder()
    {
        messageflag = false;
        searchend = false;
        if (traveled == false)
            PostOrder(rootNode);
        int tempdata = queuetravel.Dequeue().data;
        foreach (Transform child in AllPoint.gameObject.transform)
        {
            if (int.Parse(child.GetComponentInChildren<TextMesh>().text) == tempdata)
            {
                child.GetComponent<SpriteRenderer>().sprite = point_choose;
            }
        }
    }
    void onTravelClear()
    {
        messageflag = false;
        searchend = false;
        foreach (Transform child in AllPoint.gameObject.transform)
        {
            child.GetComponent<SpriteRenderer>().sprite = point_orignal;
        }
        queuetravel.Clear();
    }
    void onSearch()
    {
        messageflag = false;
        searchend = false;
        int data = int.Parse(searchinput.text.ToString());
        searchinput.text = "";
        tempsearch = SearData(data);
        if (tempsearch != null)
        {
            tempsearch.GetComponent<SpriteRenderer>().sprite = point_choose;
            searchend = true;
        }
        else
        {
            textinfo.text = "提示:树中不存在该节点";
            messageflag = true;
        }
    }

    public void Draw(Node pointee, int rank, int leforrig)
    {
        if (pointee != null)
        {
            Vector3 pos;
            if (rank == 0)
            {
                pos = new Vector3(0,(float)2.5,0);
                pointee.x = 0;
                pointee.y = (float)2.5;
            }
            else 
            {
                float disx = (float)1.5;
                float disy = 1;
                if (rank == 1)
                {
                    Debug.Log("开始");
                }
                else if (rank >= 2)
                {
                    disx = disy/ 2;
                    disy = disy* 2;
                }


                pointee.x = pointee.Parent.x + disx * leforrig;
                pointee.y = pointee.Parent.y - disy;
                

                pos = new Vector3(pointee.x, pointee.y, 0);
                Vector3 posparent = new Vector3(pointee.Parent.x, pointee.Parent.y, 0);

                GameObject line = GameObject.Instantiate
                        (linePrefab, new Vector3(0, 0, 0), transform.rotation) as GameObject;
                line.transform.parent = AllLine.transform;

                lineRenderer = (LineRenderer)line.GetComponent("LineRenderer");
                
                lineRenderer.SetPosition(0, pos);
                lineRenderer.SetPosition(1, posparent);

            }

            GameObject point = GameObject.Instantiate
                (pointPrefab, pos, transform.rotation) as GameObject;
            point.GetComponentInChildren<TextMesh>().text = pointee.data.ToString();
            point.transform.parent = AllPoint.transform;

            Draw(pointee.Left,rank+1,-1);
            Draw(pointee.Right,rank+1,1);
        }
    }
    public void Clear()
    {
        foreach (Transform child in AllPoint.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in AllLine.gameObject.transform)
        {
            Destroy(child.gameObject);
        }
    }
    public Transform SearData(int data)
    {
        foreach (Transform child in AllPoint.gameObject.transform)
        {
            if (int.Parse(child.GetComponentInChildren<TextMesh>().text) == data)
            {
                return child;
            }
        }
        return null;
    }

    /*
     三种遍历
     */
    //中序
    public void InOrder(Node theRoot)
    {
        if (theRoot != null)
        {
            InOrder(theRoot.Left);
            queuetravel.Enqueue(theRoot);
            InOrder(theRoot.Right);
        }
    }
    //先序
    public void PreOrder(Node theRoot)
    {
        if (theRoot != null)
        {
            queuetravel.Enqueue(theRoot);
            PreOrder(theRoot.Left);
            PreOrder(theRoot.Right);
        }
    }
    //后序
    public void PostOrder(Node theRoot)
    {
        if (theRoot != null)
        {
            PostOrder(theRoot.Left);
            PostOrder(theRoot.Right);
            queuetravel.Enqueue(theRoot);
        }
    }
}
