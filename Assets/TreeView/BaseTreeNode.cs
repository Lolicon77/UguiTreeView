using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using UnityEngine.Events;
using System.Collections.Generic;

public class BaseTreeNode : Selectable, IPointerClickHandler, ISubmitHandler {

    [Serializable]
    public class TreeNodeStateEvent : UnityEvent<BaseTreeNode, bool> { }

    protected TreeNodeStateEvent m_OnValueChanged = new TreeNodeStateEvent();
    public TreeNodeStateEvent onValueChanged { get { return m_OnValueChanged; } set { m_OnValueChanged = value; } }

    [Serializable]
    public class TreeNodeEvent : UnityEvent<BaseTreeNode> { }

    protected TreeNodeEvent m_OnSelect = new TreeNodeEvent();
    public TreeNodeEvent onSelect { get { return m_OnSelect; } set { m_OnSelect = value; } }

    protected TreeNodeEvent m_OnDeselect = new TreeNodeEvent();
    public TreeNodeEvent onDeselect { get { return m_OnDeselect; } set { m_OnDeselect = value; } }

    public BaseTreeNodeData data { get; protected set; }
    
    private RectTransform m_RectTransform;
    public RectTransform rectTransform
    {
        get
        {
            if (m_RectTransform == null)
                m_RectTransform = (RectTransform)transform;
            return m_RectTransform;
        }
    }

    protected BaseTreeNode() { }

    public virtual void SetPosition(Vector3 position)
    {
        rectTransform.localPosition = position;
    }

    public virtual void SetData(BaseTreeNodeData data)
    {
        this.data = data;
        gameObject.name = string.Format("TreeNode_{0}_{1}", data.level, data.index < 10 ? string.Format("0{0}", data.index) : data.index.ToString());
    }

    protected virtual void Press()
    {
        if (!IsActive() || !IsInteractable())
            return;
        data.isShow = !data.isShow;
        m_OnValueChanged.Invoke(this, data.isShow);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        Press();
    }

    public virtual void OnSubmit(BaseEventData eventData)
    {
        Press();

        // if we get set disabled during the press
        // don't run the coroutine.
        if (!IsActive() || !IsInteractable())
            return;

        DoStateTransition(SelectionState.Pressed, false);
        StartCoroutine(OnFinishSubmit());
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        m_OnSelect.Invoke(this);
    }

    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
        m_OnDeselect.Invoke(this);
    }

    private IEnumerator OnFinishSubmit()
    {
        var fadeTime = colors.fadeDuration;
        var elapsedTime = 0f;

        while (elapsedTime < fadeTime)
        {
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }

        DoStateTransition(currentSelectionState, false);
    }
}
