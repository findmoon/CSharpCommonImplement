**【WPF】树形结构TreeView的用法（MVVM）**


# [【WPF】树形结构TreeView的用法（MVVM）](https://www.cnblogs.com/guxin/p/wpf-how-to-use-treeview-by-mvvm.html)

TreeView控件的用法还是有蛮多坑点的，最好记录一下。

参考项目：

- [https://www.codeproject.com/Articles/26288/Simplifying-the-WPF-TreeView-by-Using-the-ViewMode](https://www.codeproject.com/Articles/26288/Simplifying-the-WPF-TreeView-by-Using-the-ViewMode)

-

# 静态的树形结构

如果树形结构的所有子节点都已经确定且不会改动，可以直接在控制层用C#代码来生成这个TreeView。

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

            var rootItem = new OutlineTreeData
            {
                outlineTypeName \= "David Weatherbeam",
                Children\= { new OutlineTreeData
                    {
                        outlineTypeName\="Alberto Weatherbeam",
                        Children\= { new OutlineTreeData
                            {
                                outlineTypeName\="Zena Hairmonger",
                                Children\= { new OutlineTreeData
                                    {
                                        outlineTypeName\="Sarah Applifunk",
                                    }
                                }
                            },new OutlineTreeData
                            {
                                outlineTypeName\="Jenny van Machoqueen",
                                Children\= { new OutlineTreeData
                                    {
                                        outlineTypeName\="Nick van Machoqueen",
                                    }, new OutlineTreeData
                                    {
                                        outlineTypeName\="Matilda Porcupinicus",
                                    }, new OutlineTreeData
                                    {
                                        outlineTypeName\="Bronco van Machoqueen",
                                    }
                                }
                            }
                        }
                    }, new OutlineTreeData
                    {
                        outlineTypeName\="Komrade Winkleford",
                        Children\= { new OutlineTreeData
                            {
                                outlineTypeName\="Maurice Winkleford",
                                Children\= { new OutlineTreeData
                                    {
                                        outlineTypeName\="Divinity W. Llamafoot",
                                    }
                                }
                            }, new OutlineTreeData
                            {
                                outlineTypeName\="Komrade Winkleford, Jr.",
                                Children\= { new OutlineTreeData
                                    {
                                        outlineTypeName\="Saratoga Z. Crankentoe",
                                    }, new OutlineTreeData
                                    {
                                        outlineTypeName\="Excaliber Winkleford",
                                    }
                                }
                            }
                        }
                    }
                }
            }; 

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

运行后能看到树形结构是下面的样子。

![](https://images2017.cnblogs.com/blog/759894/201710/759894-20171012145558262-1134548539.png)

-

# 获取TreeViewItem控件

前台页面xaml：

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

 <!-- 树形结构 \-->
 <TreeView x:Name\="treeView" VerticalAlignment\="Top" HorizontalAlignment\="Left" Margin\="10,-10,0,0" ItemsSource\="{Binding ItemTreeDataList}" BorderThickness\="0" Width\="215" Height\="210"\>
     <TreeView.ItemContainerStyle\>
         <Style TargetType\="{x:Type TreeViewItem}"\> <Setter Property="IsExpanded" Value="{Binding IsExpanded, Mode=TwoWay}" />
             <Setter Property="IsSelected" Value="{Binding IsSelected, Mode=TwoWay}" />
             <Setter Property="FontWeight" Value="Normal" />
             <Style.Triggers>
                 <Trigger Property="IsSelected" Value="True">
                     <Setter Property="FontWeight" Value="Bold" />
                 </Trigger> </Style.Triggers\>
         </Style\>
     </TreeView.ItemContainerStyle\>

     <TreeView.ItemTemplate\>
         <HierarchicalDataTemplate ItemsSource\="{Binding Children}"\>
             <TextBlock x:Name\="treeViewItemTB" Text\="{Binding itemName}" Tag\="{Binding itemId}"/>
         </HierarchicalDataTemplate\>
     </TreeView.ItemTemplate\>
 </TreeView\>  

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

尝试过在初始化时获取TreeViewItem，发现都是为Null。

- TreeViewItem item= (TreeViewItem)(myWindow.treeView.ItemContainerGenerator.ContainerFromIndex(0)); // 无法获取，为Null！
- VisualTreeHelper.GetChild(); // 无法获取，为Null！

谷歌一下，看到[这篇解答](https://stackoverflow.com/questions/616948/how-to-get-treeviewitem-from-hierarchicaldatatemplate-item/3177290#3177290)，下面这位跟我遇到的情况一样，用以上方法都无法获取TreeViewItem。

![](https://images2017.cnblogs.com/blog/759894/201710/759894-20171012150625230-425833414.png)

不过他给出的答案是通过点击来获取到被选中的TreeViewItem。

-

# 给TreeView默认选中一个TreeViewItem

上面的办法通过点击TreeViewItem来从事件中获得这个控件，但是如果我们想生成TreeView后不靠手动点击，立马自动选中一个默认的TreeViewItem呢？注意此时控件还未渲染，通过ItemContainerGenerator无法获取到控件。

此时只能考虑使用MVVM的绑定机制来获取控件！因为如果修改数据后，UI的更新是延迟的，无法立刻获取到前台控件。

绑定：

xaml中TreeView的ItemsSource绑定到ViewModel中的ItemTreeDataList列表，该列表中的元素类型是自定义ItemTreeData实体类。在样式中绑定好IsExpanded和IsSelected属性。TreeViewItem模板是绑定到Children属性，该属性在ItemTreeData实体类中。

ViewModel：

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

// Item的树形结构
private ObservableCollection<ItemTreeData> itemTreeDataList; public ObservableCollection<ItemTreeData> ItemTreeDataList
{ get { return itemTreeDataList; } set { SetProperty(ref itemTreeDataList, value); }
}

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

ItemTreeData实体类：

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

    public class ItemTreeData // 自定义Item的树形结构
 { public int itemId { get; set; }      // ID
        public string itemName { get; set; } // 名称
        public int itemStep { get; set; }    // 所属的层级
        public int itemParent { get; set; }  // 父级的ID

        private ObservableCollection<ItemTreeData> \_children = new ObservableCollection<ItemTreeData >(); public ObservableCollection<ItemTreeData> Children {  // 树形结构的下一级列表
            get { return \_children;
            }
        } public bool IsExpanded { get; set; } // 节点是否展开
        public bool IsSelected { get; set; } // 节点是否选中
    }

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

关于上面的层级/父级/下一级的概念，假设现在树形结构为以下结构，从上往下依此定义为根节点、零级节点、一级节点、二级节点。

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

/\* \*  rootItem
\*      |----zeroTreeItem
\*                 |----firstTreeItem
\*                            |----secondTreeItem \*/

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

控制层在生成TreeView时通过绑定的属性来设置默认选中的Item，预先将三个层级的Item分别装在不同的列表中使用。

现在尝试把二级节点中的第一个Item作为默认选中的Item。关键代码如下：

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

 // 构造轮廓选择的树形结构
        private void InitTreeView()
        { // 添加树形结构
            ItemTreeData item = GetTreeData();
            myViewModel.ItemTreeDataList.Clear();
            myViewModel.ItemTreeDataList.Add(item);
        } // 构造树形结构
        private ItemTreeData GetTreeData()
        { /\* \*  rootItem
             \*      |----zeroTreeItem
             \*                 |----firstTreeItem
             \*                            |----secondTreeItem \*/

            // 根节点
            ItemTreeData rootItem = new ItemTreeData();
            rootItem.itemId \= -1;
            rootItem.itemName \= " -- 请选择轮廓 -- ";
            rootItem.itemStep \= -1;
            rootItem.itemParent \= -1;
            rootItem.IsExpanded \= true; // 根节点默认展开
            rootItem.IsSelected \= false; for (int i = 0; i < itemViewModel.ZeroStepList.Count; i++) // 零级分类
 {
                Items zeroStepItem \= itemViewModel.ZeroStepList\[i\];
                ItemTreeData zeroTreeItem \= new ItemTreeData();
                zeroTreeItem.itemId \= zeroStepItem.itemId;
                zeroTreeItem.itemName \= zeroStepItem.itemName;
                zeroTreeItem.itemStep \= zeroStepItem.itemSteps;
                zeroTreeItem.itemParent \= zeroStepItem.itemParent; if (i == 0)
                {
                    zeroTreeItem.IsExpanded \= true; // 只有需要默认选中的第一个零级分类是展开的
 }
                zeroTreeItem.IsSelected \= false;
                rootItem.Children.Add(zeroTreeItem); // 零级节点无条件加入根节点

                for (int j = 0; j < itemViewModel.FirstStepList.Count; j++) // 一级分类
 {
                    Items firstStepItem \= itemViewModel.FirstStepList\[j\]; if (firstStepItem.itemParent == zeroStepItem.itemId) //零级节点添加一级节点
 {
                        ItemTreeData firstTreeItem \= new ItemTreeData();
                        firstTreeItem.itemId \= firstStepItem.itemId;
                        firstTreeItem.itemName \= firstStepItem.itemName;
                        firstTreeItem.itemStep \= firstStepItem.itemSteps;
                        firstTreeItem.itemParent \= firstStepItem.itemParent; if (j == 0)
                        {
                            firstTreeItem.IsExpanded \= true; // 只有需要默认选中的第一个一级分类是展开的
 }
                        firstTreeItem.IsSelected \= false;
                        zeroTreeItem.Children.Add(firstTreeItem); for (int k = 0; k < itemViewModel.SecondStepList.Count; k++) // 二级分类
 {
                            Items secondStepItem \= itemViewModel.SecondStepList\[k\]; if (secondStepItem.itemParent == firstStepItem.itemId) // 一级节点添加二级节点
 {
                                ItemTreeData secondTreeItem \= new ItemTreeData();
                                secondTreeItem.itemId \= secondStepItem.itemId;
                                secondTreeItem.itemName \= secondStepItem.itemName;
                                secondTreeItem.itemStep \= secondStepItem.itemSteps;
                                secondTreeItem.itemParent \= secondStepItem.itemParent; if (k == 0)
                                { // 默认选中第一个二级分类
                                    secondTreeItem.IsExpanded = true; // 已经没有下一级了，这个属性无所谓
                                    secondTreeItem.IsSelected = true;
                                }
                                firstTreeItem.Children.Add(secondTreeItem);
                            }
                        }
                    }
                }
            } return rootItem;
        }

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

通过初始化时给被绑定的属性赋值，使得TreeView默认选中了二级节点中的第一个TreeViewItem，效果如下图：

![](https://images2017.cnblogs.com/blog/759894/201710/759894-20171012154351793-948863740.png)

注意点：

- 只是修改目标节点的IsSelected = true还不够，还要把它的所有父节点（祖父节点）都设置IsExpanded = true才行！！！

-

# 显示选中TreeViewItem的完整分类路径

需求是如上图，要得到字符串"I户型\_1.5-1\_a2"，即通过下划线连接得到“零级节点\_一级节点\_二级节点”，然后用一个TextBlock控件显示出来。注意这里不包含root根节点。

该功能可以写在树形结构的选项改变事件中。因为初始化TreeView时就选中了其中一个Item，所以初始化时也会调用到选项改变事件。

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

        // 树形结构的选项改变事件
        private void TreeView\_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object\> e)
        {
            ItemTreeData selectedItem \= (ItemTreeData)(myWindow.treeView.SelectedItem); if (selectedItem != null)
            { // UI层找不到，只能在数据层找
                List<string\> nameList = new List<string\>(); int step = selectedItem.itemStep;        // 可能值为：-1,0,1,2
                int parentId = selectedItem.itemParent;  // 临时保存遍历中每次使用的Id
                for (int i = 0; i < step; i++)     // 零级分类虽然有父节点root，但是数据层中没有相应的itemParent值（为-1）
 {
                    ItemTreeData parent \= this.GetTreeDataById(parentId); if (parent != null)
                    {
                        nameList.Add(parent.itemName);
                        parentId \= parent.itemParent;
                    }
                } // 组拼字符串 = 零级名称 + 一级名称 + 二级名称
                string text = ""; for (int i = nameList.Count; i > 0; i--) // 倒序遍历
 { if (i == nameList.Count)
                    {
                        text \= nameList\[i - 1\];
                    } else {
                        text \= text + "\_" + nameList\[i - 1\];
                    }
                } if (string.IsNullOrEmpty(text))
                {
                    myWindow.itemSelectedTB.Text \= selectedItem.itemName;
                } else {
                    myWindow.itemSelectedTB.Text \= text + "\_" + selectedItem.itemName;
                }
            }
        } // 根据Id，获得树形结构中指定的Item
        private ItemTreeData GetTreeDataById(int targetId)
        {
            ItemTreeData data \= null; // 是否为根节点
            ItemTreeData root = myViewModel.ItemTreeDataList\[0\]; if (root.itemId == targetId)
            {
                data \= root; return data;
            } // 搜索零级大类
            foreach (ItemTreeData zeroStepData in root.Children)
            { if (zeroStepData.itemId == targetId)
                {
                    data \= zeroStepData; return data;
                } // 搜索一级分类
                foreach (ItemTreeData firstStepData in zeroStepData.Children)
                { if (firstStepData.itemId == targetId)
                    {
                        data \= firstStepData; return data;
                    } // 搜索二级分类
                    foreach (ItemTreeData secondStepData in firstStepData.Children)
                    { if (secondStepData.itemId == targetId)
                        {
                            data \= secondStepData; return data;
                        }
                    }
                }
            } return data;
        }

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

注意点：

- 同样是找不到TreeViewItem控件！UI层找不到，所以只能在数据层找它关联的ItemTreeData对象，从数据层中去获取itemName属性值。
- 因为每一级ItemTreeData对象中记录了它的父级ID，所以往根节点方向遍历父节点、祖父节点时，先加入到List中的是上一级的节点名。而需求是“零级节点\_一级节点\_二级节点”的顺序，所以在使用List时需要倒序遍历！

最后显示的完整分类如下：

![](https://images2017.cnblogs.com/blog/759894/201710/759894-20171012160001574-961578262.png)

分类: [C#](https://www.cnblogs.com/guxin/category/1030520.html), [WPF](https://www.cnblogs.com/guxin/category/1030521.html)

标签: [WPF](https://www.cnblogs.com/guxin/tag/WPF/), [C#](https://www.cnblogs.com/guxin/tag/C%23/), [MVVM](https://www.cnblogs.com/guxin/tag/MVVM/), [TreeView](https://www.cnblogs.com/guxin/tag/TreeView/), [树形结构](https://www.cnblogs.com/guxin/tag/%E6%A0%91%E5%BD%A2%E7%BB%93%E6%9E%84/)

[好文要顶](javascript:void(0);) [关注我](javascript:void(0);) [收藏该文](javascript:void(0);) [![](https://common.cnblogs.com/images/icon_weibo_24.png)](javascript:void(0); "分享至新浪微博") [![](https://common.cnblogs.com/images/wechat.png)](javascript:void(0); "分享至微信")

[![](https://pic.cnblogs.com/face/759894/20170706233845.png)](https://home.cnblogs.com/u/guxin/)

[霍莉雪特](https://home.cnblogs.com/u/guxin/)  
[粉丝 - 108](https://home.cnblogs.com/u/guxin/followers/) [关注 - 37](https://home.cnblogs.com/u/guxin/followees/)  

[+加关注](javascript:void(0);)

3

0

currentDiggType = 0;

[«](https://www.cnblogs.com/guxin/p/weixin-miniapp-how-to-use-settimeout.html) 上一篇： [【微信小程序】使用setTimeout制作定时器的思路](https://www.cnblogs.com/guxin/p/weixin-miniapp-how-to-use-settimeout.html "发布于 2017-10-12 02:43")  
[»](https://www.cnblogs.com/guxin/p/7658542.html) 下一篇： [【微信小程序】Page页面跳转（路由/返回）并传参](https://www.cnblogs.com/guxin/p/7658542.html "发布于 2017-10-12 22:25")

posted @ 2017-10-12 16:02  [霍莉雪特](https://www.cnblogs.com/guxin/)  阅读(44476)  评论(5)  [编辑](https://i.cnblogs.com/EditPosts.aspx?postid=7656527)  [收藏](javascript:void(0))  [举报](javascript:void(0))

var cb\_entryId = 7656527, cb\_entryCreatedDate = '2017-10-12 16:02', cb\_postType = 1, cb\_postTitle = '【WPF】树形结构TreeView的用法（MVVM）'; var allowComments = true, cb\_blogId = 363082, cb\_blogApp = 'guxin', cb\_blogUserGuid = '941f8109-effe-e411-b908-9dcfd8948a71'; mermaidRender.render() markdown\_highlight() zoomManager.apply("#cnblogs\_post\_body img:not(.code\_img\_closed):not(.code\_img\_opened)"); updatePostStats( \[cb\_entryId\], function(id, count) { $("#post\_view\_count").text(count) }, function(id, count) { $("#post\_comment\_count").text(count) })

  

评论列表

时间升序

   [回复](javascript:void(0);) [引用](javascript:void(0);)

[#1楼](https://www.cnblogs.com/guxin/p/wpf-how-to-use-treeview-by-mvvm.html#4024655) 2018-07-20 09:52 [\_Bowen](https://www.cnblogs.com/bowen58/)

多谢分享，如果有源码就好了

[支持(1)](javascript:void(0);) [反对(0)](javascript:void(0);)

https://pic.cnblogs.com/face/715459/20171115092022.png

   [回复](javascript:void(0);) [引用](javascript:void(0);)

[#2楼](https://www.cnblogs.com/guxin/p/wpf-how-to-use-treeview-by-mvvm.html#4032529) 2018-07-31 15:00 [veeva1n](https://home.cnblogs.com/u/1439646/)

照着做，已经搞定了，用递归可以直接无限层级，不用那么麻烦，选中项一开始只能选中第一层级的，后来我用你的方法，只需要ItemTreeData selectedItem = (ItemTreeData)(myWindow.treeView.SelectedItem);  
selectItem可以直接获得ID的。不需要写那么繁杂  
非常感謝你的分享，沒有你的分享估計要多走好多弯路

[支持(1)](javascript:void(0);) [反对(0)](javascript:void(0);)

   [回复](javascript:void(0);) [引用](javascript:void(0);)

[#3楼](https://www.cnblogs.com/guxin/p/wpf-how-to-use-treeview-by-mvvm.html#4446898) 2019-12-02 10:02 [New\_Yang](https://home.cnblogs.com/u/1884839/)

[@](https://www.cnblogs.com/guxin/p/wpf-how-to-use-treeview-by-mvvm.html#4032529 "查看所回复的评论") veeva1n  
你好，如果你看见这条回复不知道你能不能给我发一份递归生成树形的代码呢，这个功能我看了很久还是没有实现，卡了挺久的，非常感谢！

[支持(0)](javascript:void(0);) [反对(0)](javascript:void(0);)

   [回复](javascript:void(0);) [引用](javascript:void(0);)

[#4楼](https://www.cnblogs.com/guxin/p/wpf-how-to-use-treeview-by-mvvm.html#4530685) 2020-03-24 10:40 [Moon-明月](https://www.cnblogs.com/mingmingruyue20160420/)

请问：myViewModel 这个是在哪里定义的呀？可否给发一份源码呢（837682266@qq.com）？我是WPF新手，希望大侠多多指导。

[支持(0)](javascript:void(0);) [反对(0)](javascript:void(0);)

https://pic.cnblogs.com/face/940667/20160420132546.png

   [回复](javascript:void(0);) [引用](javascript:void(0);)

[#5楼](https://www.cnblogs.com/guxin/p/wpf-how-to-use-treeview-by-mvvm.html#4575839) 4575839 2020/5/13 11:18:53 2020-05-13 11:18 [\_York](https://www.cnblogs.com/tangchun/)

mvvm模式怎么在选中项目的时候获取选中值

[支持(0)](javascript:void(0);) [反对(0)](javascript:void(0);)

https://pic.cnblogs.com/face/616139/20190130133658.png