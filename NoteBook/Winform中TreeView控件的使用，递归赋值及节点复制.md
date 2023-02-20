**Winform中TreeView控件的使用，递归赋值及节点复制**

[toc]

[C#-Winform - 树控件（TreeView）的基本使用](https://blog.csdn.net/weixin_42118716/article/details/108264625)


https://www.cnblogs.com/luyj00436/p/13996922.html

https://blog.csdn.net/zdw_wym/article/details/6561233



# 复制节点

```C#
 
TreeView tv1=new TreeView();
 
TreeView tv2=new TreeView();
 
tv2.Nodes.Add((TreeNode)tv1.Nodes[0].Clone());
```