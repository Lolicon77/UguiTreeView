using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TreeNode : BaseTreeNode
{
    [SerializeField]
    protected Text m_TextComponent;
    public Text textComponent
    {
        get { return m_TextComponent; }
        set { m_TextComponent = value; }
    }

    [SerializeField]
    protected string m_Text = string.Empty;
    public string text
    {
        get { return m_Text; }
        set
        {
            m_Text = value;
            if (m_TextComponent != null) { if (data.isShow) m_TextComponent.text = " - " + m_Text; else m_TextComponent.text = "+ " + m_Text; }
        }
    }

    void Start()
    {
        if (m_TextComponent == null) m_TextComponent = GetComponent<Text>();
        onSelect.AddListener(delegate { textComponent.color = colors.highlightedColor; });
        onDeselect.AddListener(delegate { textComponent.color = colors.normalColor; });
    }

    public override void SetData(BaseTreeNodeData data)
    {
        base.SetData(data);
        text = m_Text = gameObject.name;
    }

    protected override void Press()
    {
        base.Press();
        text = m_Text;
    }
}
