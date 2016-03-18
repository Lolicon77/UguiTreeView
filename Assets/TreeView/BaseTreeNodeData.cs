using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseTreeNodeData {
    public int index { get; set; }
    public int rootIndex { get; set; }
    public int level { get; protected set; }
    public BaseTreeNodeData parent { get; protected set; }
    public List<BaseTreeNodeData> children = new List<BaseTreeNodeData>();
    public bool isShow { get; set; }

    public BaseTreeNodeData() {
        Init(null,0,0,0);
    }

    public virtual void Init(BaseTreeNodeData parent, int level, int rootIndex, int index)
    {
        this.rootIndex = rootIndex;
        this.index = index;
        this.level = level;
        this.parent = parent; 
    }

    public virtual void AddChild(BaseTreeNodeData child)
    {
        child.Init(this, this.level + 1, child.rootIndex, children.Count);
        setChildLevel(child);
        children.Add(child);
    }

    public virtual void RemoveChild(BaseTreeNodeData child)
    {
        children.Remove(child);
    }

    void setChildLevel(BaseTreeNodeData child)
    {
        for (int i = 0; i < child.children.Count; i++)
        {
            child.children[i].level = child.level + 1;
            setChildLevel(child.children[i]);
        }
    }
}
