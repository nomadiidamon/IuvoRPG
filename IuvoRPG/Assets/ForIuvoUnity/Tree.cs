using System;

public class Tree<T>
{
    public TreeNode<T> Root { get; private set; }

    public Tree(TreeNode<T> root)
    {
        Root = root;
    }

    // Preorder traversal example
    public void TraversePreOrder(TreeNode<T> node, Action<TreeNode<T>> action)
    {
        if (node == null) return;

        action(node);
        foreach (var child in node.Children)
            TraversePreOrder(child, action);
    }
}
