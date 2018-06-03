using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ListView
{
    public enum Type {Verticle, Horizontal};
    public Type listType;

    private RectTransform root;
    private List<RectTransform> items;
    private float itemSize;
    private MonoBehaviour behaviour;
    private bool isSnapScroll = false;
    public ListView(MonoBehaviour behaviour)
    {
        this.behaviour = behaviour;
        items = new List<RectTransform>();
    }

    public ListView SetType(Type listType){
        this.listType = listType;
        return this;
    }

    public ListView SetSnapScroll(bool isSnap)
    {
        isSnapScroll = true;
        return this;
    }

    public ListView SetRoot(RectTransform root)
    {
        this.root = root;
        foreach (RectTransform item in root)
        {
            items.Add(item);
        }
        return this;
    }

    public ListView SetItemSize(float itemSize)
    {
        this.itemSize = itemSize;
        return this;
    }

    public void Build()
    {
        UpdateList();
    }

    private void UpdateList()
    {
        var activeItems = GetActiveItems();
        float snapDelta = isSnapScroll ? itemSize : 0;
        if (listType == Type.Horizontal)
        {
            root.sizeDelta = new Vector2(activeItems.Count * itemSize + snapDelta, root.sizeDelta.y);
            for(int i = 0; i < activeItems.Count; i++)
            {
                var item = activeItems[i];
                item.SetLocalX(itemSize * i + item.sizeDelta.x / 2 + snapDelta/2);
            }
            
        }
        else
        {
            root.sizeDelta = new Vector2(root.sizeDelta.x, activeItems.Count * itemSize + snapDelta);
            for (int i = 0; i < activeItems.Count; i++)
            {
                var item = activeItems[i];
                item.SetLocalY(-itemSize * i - item.sizeDelta.y / 2 - snapDelta / 2);
            }
        }
    }

    public void DisappearItems(params int[] indexes)
    {
        foreach (int index in indexes)
        {
            items[index].gameObject.SetActive(false);
        }
        UpdateList();
    }

    public void DisappearItemsAfterTime(float delay, params int[] indexes)
    {
        behaviour.StartCoroutine(IEDisappearItems(indexes, delay));
    }

    private IEnumerator IEDisappearItems(int[] indexes, float delay)
    {
        yield return new WaitForSeconds(delay);
        DisappearItems(indexes);
    }

    private List<RectTransform> GetActiveItems()
    {
        return items.FindAll(x => x.gameObject.activeSelf);
    }

    public int GetActiveItemsCount()
    {
        return GetActiveItems().Count;
    }
}