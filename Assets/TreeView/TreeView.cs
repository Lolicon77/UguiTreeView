using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TreeView : UIBehaviour
{

    [SerializeField]
    private BaseTreeNode m_NodePrefab;
    public BaseTreeNode nodePrefab { get { return m_NodePrefab; } set { m_NodePrefab = value; } }

    [SerializeField]
    private RectTransform m_Content;
    public RectTransform content { get { return m_Content; } set { m_Content = value; } }

    [SerializeField]
    private int m_CellPadiding = 5;
    public int cellPadiding { get { return m_CellPadiding; } set { m_CellPadiding = value; } }

    [SerializeField]
    private int m_CellLevelOffset = 30;
    public int cellLevelOffset { get { return m_CellLevelOffset; } set { m_CellLevelOffset = value; } }

    [SerializeField]
    private int m_CellHeightg = 50;
    public int cellHeight { get { return m_CellHeightg; } set { m_CellHeightg = value; } }

    private bool isUpdateView = false;

    private int _index = -1;
    private int _viewCount;
    private Queue<BaseTreeNode> _unShowView = new Queue<BaseTreeNode>();
    private List<BaseTreeNode> _useList = new List<BaseTreeNode>();
    private List<BaseTreeNodeData> _itemList = new List<BaseTreeNodeData>();

    private List<BaseTreeNodeData> _data = new List<BaseTreeNodeData>();
    public List<BaseTreeNodeData> data
    {
        get { return _data; }
        set
        {
            _data = value;
            Init();
        }
    }
    
    private int _nodeCound=0;
    public int nodeCound { get { return _nodeCound; } set { _nodeCound = value; UpdateContentView(); } }

    private RectTransform m_ViewRect;

    protected RectTransform viewRect
    {
        get
        {
            if (m_ViewRect == null)
                m_ViewRect = (RectTransform)transform;
            return m_ViewRect;
        }
    }

	// Use this for initialization
    protected override void Start()
    {
        Init();
	}
    public void Init() {
        InitQueue();
        onValueChanged(Vector2.zero);
    }

    public void onValueChanged(Vector2 pos)
    {
        int index = GetPosIndex();
        if ((_index != index && index > -1) || isUpdateView)
        {
            _index = index;
            if (isUpdateView) isUpdateView = false;
            BaseTreeNode node;
            for (int i = _useList.Count; i > 0; i--)
            {
                node = _useList[i - 1];
                if (node.data.rootIndex < index || (node.data.rootIndex >= index + _viewCount))
                {
                    RemoveShowViewList(node);
                }
            }

            Vector3 nodePos;
            for (int i = _index; i < _index + _viewCount; i++)
            {
                if (i < 0) continue;
                int t = i - _index;
                if (i > _nodeCound - 1) continue;
                bool isOk = false;
                for (int j = 0; j < _useList.Count; j++)
                {
                    if (_useList[j].data.rootIndex == i) isOk = true;
                }
                if (isOk) continue;

                if (_unShowView.Count > 0)
                {
                    node = _unShowView.Dequeue();
                    node.onValueChanged.RemoveAllListeners();
                    node.gameObject.SetActive(true);
                }
                else
                {
                    node = null;
                }
                node = GetView(i, node);
                node.onValueChanged.AddListener(onNodeValueChanged);
                _useList.Add(node);
                nodePos = GetPosition(i);
                nodePos.x = node.data.level * cellLevelOffset;
                node.SetPosition(nodePos);
            }
        }
    }

    private void InitQueue(int clickIndex=-1) {
        _viewCount = Mathf.FloorToInt(this.viewRect.sizeDelta.y / cellHeight) + 1;
        isUpdateView = true;
        BaseTreeNode node;
        if (clickIndex > -1)
        {
            for (int i = _useList.Count; i > 0; i--)
            {
                node = _useList[i - 1];
                if (node.data.rootIndex > clickIndex)
                {
                    RemoveShowViewList(node);
                }
            }
        }
        int index = 0;
        _itemList.Clear();
        for (int i = 0; i < data.Count; i++)
        {
            data[i].index = i;
            index = InitQueue(data[i], index);
        }
        nodeCound = index;
    }

    private int InitQueue(BaseTreeNodeData nodeData,int index) {
        nodeData.rootIndex = index;
        index++;
        _itemList.Add(nodeData);
        if (nodeData.isShow && nodeData.children.Count > 0)
        {
            for (int i = 0; i < nodeData.children.Count; i++) {
                index = InitQueue(nodeData.children[i], index);
            }
        }
        return index;
    }

    private void onNodeValueChanged (BaseTreeNode node,bool isShow){
        if (node.data.children.Count > 0)
        {
            InitQueue(node.data.rootIndex);
            onValueChanged(Vector2.zero);
        }
    }

    private void RemoveShowViewList(BaseTreeNode node)
    {
        node.name = "-1";
        node.SetPosition(GetPosition(-10));
        _useList.Remove(node);
        _unShowView.Enqueue(node);
        node.gameObject.SetActive(false);
    }
    private BaseTreeNodeData GetData(int index) {
        return _itemList[index];
    }
    
    private BaseTreeNode GetView(int index,BaseTreeNode convertView)
    {
        if (convertView==null)
        {
            convertView = CreatNodeInView();
        }
        convertView.SetData(GetData(index));
        return convertView;
    }

    private int GetPosIndex()
    {
        return Mathf.FloorToInt(content.anchoredPosition.y / (cellHeight + cellPadiding));
    }

    public Vector3 GetPosition(int index)
    {
        return new Vector3(0f, index * -(cellHeight + cellPadiding), 0f);
    }

    private void UpdateContentView()
    {
        content.sizeDelta = new Vector2(content.sizeDelta.x, cellHeight * _nodeCound + cellPadiding * (_nodeCound - 1));
    }

    private BaseTreeNode CreatNodeInView()
    {
        if (nodePrefab == null) {
            Debug.LogError("创建错误: nodePrefab=null");
            return null;
        }
        BaseTreeNode node = GameObject.Instantiate(nodePrefab) as BaseTreeNode;

        if (node != null && content != null)
        {
            Transform t = node.transform;
            t.SetParent(content, false);
            node.gameObject.layer = content.gameObject.layer;
        }
        return node;
    }
}
