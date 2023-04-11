**C#判断图片类型，通过文件头字节判断真实的图片类型，获取图片缩略图的介绍**

[toc]

C#中判断图片类型的方法有很多。最不应该使用的是通过扩展名判断图片，很容易被伪装为图片后缀的木马病毒程序危害到。

判断图片类型最常用的场景是Web浏览器中的图片上传，必须在后端服务器上进行判断和错误文件过滤。比如，防止恶意程序通过修改扩展名伪装为图片，破坏系统；其它格式的图片仅修改扩展名冒充另一种格式等。

> CMCode 项目下创建 `ImgFileType\CheckImgFormat.cs` 文件。

# 用image对象判断是否为图片

```js
/// <summary>
/// 判断文件是否为图片
/// </summary>
/// <param name="path">文件的完整路径</param>
/// <returns>返回结果</returns>
public bool IsImage(string path)
{
    try
    {
        Drawing.Image img = System.Drawing.Image.FromFile(path);
        return true;
    }
    catch (Exception e)
    {
        return false;
    }
}
```

# 图片文件头字节 判断真实图片类型

判断真实的图片类型，通过文件开头的字节内容检查图片实际的类型。不同的图片类型在文件开头就定义好了其存储类型或格式。

这是最正确、最严谨的判断图片类型的方法。




[[C#] Image的Image.GetThumbnailImage(获取缩略图)方法实际是缩放与拉伸](https://blog.csdn.net/m0_46555380/article/details/106346660)

[求C#中快速获取图片缩略图(效率要很快)](https://bbs.csdn.net/topics/390717915)

[【C#】获取任意文件的缩略图](https://blog.csdn.net/catshitone/article/details/78042649)

[【C#】WindowsAPICodePack-Shell使用教程](https://blog.csdn.net/catshitone/article/details/72723927)

[关于C#：如何从文件获取缩略图](https://www.codenong.com/61323700/)