using UnityEngine;
using System.Collections;

public class TreeViewTest : MonoBehaviour {
    public TreeView treeView;
	// Use this for initialization
	void Start () {
        BaseTreeNodeData nodeData = new BaseTreeNodeData();
        treeView.data.Add(nodeData);
        BaseTreeNodeData nodeData1 = new BaseTreeNodeData();
        BaseTreeNodeData nodeData2 = new BaseTreeNodeData();
        BaseTreeNodeData nodeData3 = new BaseTreeNodeData();
        nodeData1.AddChild(nodeData3);
        nodeData.AddChild(nodeData1);
        nodeData.AddChild(nodeData2);
        nodeData = new BaseTreeNodeData();
        treeView.data.Add(nodeData);
        nodeData = new BaseTreeNodeData();
        treeView.data.Add(nodeData);
        nodeData = new BaseTreeNodeData();
        treeView.data.Add(nodeData);
        nodeData = new BaseTreeNodeData();
        treeView.data.Add(nodeData);
        nodeData = new BaseTreeNodeData();
        treeView.data.Add(nodeData);
        nodeData = new BaseTreeNodeData();
        treeView.data.Add(nodeData);
        nodeData = new BaseTreeNodeData();
        treeView.data.Add(nodeData);
        treeView.Init();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
