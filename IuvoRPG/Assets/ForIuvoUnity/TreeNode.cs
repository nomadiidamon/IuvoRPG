using System.Collections.Generic;

public class TreeNode<T>
{
    public Link<T> Link { get; private set; }
    public TreeNode<T> Parent { get; private set; }
    public List<TreeNode<T>> Children { get; private set; }

    public T Data
    {
        get => Link.GetData();
        set => Link.SetData(value);
    }

    public TreeNode(T data)
    {
        Link = new Link<T>(data);
        Children = new List<TreeNode<T>>();
    }

    public void AddChild(TreeNode<T> child)
    {
        if (child == null) return;
        child.Parent = this;
        Children.Add(child);
    }

    public bool RemoveChild(TreeNode<T> child)
    {
        if (child == null) return false;
        bool removed = Children.Remove(child);
        if (removed)
            child.Parent = null;
        return removed;
    }
}
