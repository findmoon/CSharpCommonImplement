WPF客户端读取高清图片、缩略图、性能优化


# [WPF 解决Image控件读取高分辨率图片并缩放占用内存过大](https://blog.51cto.com/u_15127505/4518679)

```C#
需求:读取高分辨率图片显示到宽度为200的Image控件上

核心代码:

private BitmapImage ReadImageFiletToBinary(string fileName)
{
using (var stream = new FileStream(fileName, FileMode.Open))
{
var bitmapImage = new BitmapImage();
bitmapImage.BeginInit();
bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

int streamLen = (int)stream.Length;
byte[] imageData = new byte[stream.Length];
stream.Read(imageData, 0, streamLen);

bitmapImage.StreamSource = new MemoryStream(imageData, 0, streamLen);

bitmapImage.DecodePixelWidth = 200; //设置解码后图像的宽度，图像变小，解析变快
bitmapImage.EndInit();
return bitmapImage;
}
}

这样占用的内存很低,原本直接读取10几张图需要占用150M内存,现在只需不到5M
-----------------------------------
WPF 解决Image控件读取高分辨率图片并缩放占用内存过大
https://blog.51cto.com/u_15127505/4518679
```

# [wpf中大图预览用缩略图](https://blog.csdn.net/BeanGo/article/details/124292823)

在很多情况下都需要显示图片，如果图片太多，并且图片还比较大，直接显示很可能很卡，这时用缩略图显示图片预览，然后再点击缩略图时显示大图，下面代码就是解决这种问题。

```C#
imgcount  = imageList.Count;

for (int i = 0; i < imgcount; i++)
{
	imgFileName = imageList[i];
	
	BitmapImage image = new BitmapImage();    
	
	image.BeginInit();   
	
	image.CacheOption = BitmapCacheOption.None;    
	
	image.UriSource = new Uri(imgFileName);         
	
	image.DecodePixelHeight = 210;   //缩略图高度            
	
	image.EndInit();
	
	System.Windows.Controls.Image addimg = new System.Windows.Controls.Image();            
	
	addimg.Source = image;             
	
	addimg.Name = "img" + i.ToString();         
	
	addimg.Stretch = Stretch.Uniform;   //必须加拉伸情况，否则前面缩略图没有定宽度，图片会没有宽度显示不了图片         
	
	addimg.MouseDown += imageMouseDown;  //点击图片时响应函数，也就是显示大图
	
	imgPanel.Children.Add(addimg); 

}

```

点击缩略图响应函数


```C#
private void imageMouseDown(object sender, MouseButtonEventArgs e)
{

   Image img = (Image)e.OriginalSource;
   string no = img.Name.Substring(3, img.Name.Length - 3);
   int i = Convert.ToInt32(no);
   lbCur.Content = (i + 1).ToString();
   image1.Source = new BitmapImage(new Uri(imageList[i], UriKind.RelativeOrAbsolute));

}

```

# [WPF中加载高分辨率图片性能优化](https://www.cnblogs.com/yang-fei/p/4931406.html)

在最近的项目中，遇到一个关于WPF中同时加载多张图片时，内存占用非常高的问题。

问题背景：

在一个ListView中同时加载多张图片，注意：我们需要加载的图片分辨率非常高。

代码：

XAML:

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

    <Grid\>
        <Grid.RowDefinitions\>
            <RowDefinition Height\="Auto"/>
            <RowDefinition Height\="\*"/>
        </Grid.RowDefinitions\>
        
        <Button Content\="Load" Width\="100" Height\="35" Margin\="0,10" Click\="Button\_Click"/>
        
        <ListView Grid.Row\="1" x:Name\="lvImages"\>
            <ListView.ItemTemplate\>
                <DataTemplate\>
                    <Image Source\="{Binding ImageSource}" MaxWidth\="800"/>
                </DataTemplate\>
            </ListView.ItemTemplate\>

            <ListView.Template\>
                <ControlTemplate\>
                    <Grid\>
                        <ScrollViewer VerticalScrollBarVisibility\="Auto" HorizontalScrollBarVisibility\="Hidden"\>
                            <ItemsPresenter />
                        </ScrollViewer\>
                    </Grid\>
                </ControlTemplate\>
            </ListView.Template\>

            <ListView.ItemsPanel\>
                <ItemsPanelTemplate\>
                    <StackPanel IsItemsHost\="True" VirtualizingPanel.VirtualizationMode\="Recycling" VirtualizingPanel.IsVirtualizing\="True"/>
                </ItemsPanelTemplate\>
            </ListView.ItemsPanel\>
        </ListView\>
    </Grid\>

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

C#:

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

    public partial class MainWindow : Window
    { public MainWindow()
        {
            InitializeComponent();
        } private void Button\_Click(object sender, RoutedEventArgs e)
        {
            lvImages.Items.Clear(); // Image folder location: D:\\Pics

            string\[\] files = System.IO.Directory.GetFiles(@"D:\\Pics");

            List<ImageSourceModel> models = new List<ImageSourceModel>(); foreach(var path in files)
            {
                BitmapImage image \= new BitmapImage();

                image.BeginInit();

                image.UriSource \= new System.Uri(path);

                image.EndInit();

                image.Freeze();

                models.Add(new ImageSourceModel() { ImageSource = image });
            }

            lvImages.ItemsSource \= models;
        }
    } public class ImageSourceModel
    { public ImageSource ImageSource { get; set; }
    }

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

内存占用情况(此时只加载了20张图片，内存占用>1G)：

![](https://images2015.cnblogs.com/blog/622438/201511/622438-20151102211532383-1435038574.png)

优化方案：

1\. 初始加载时，只加载部分图片并显示。当ScrollViewer滚动到底部时，再加载一部分。关于这个方案，可以参考 [WPF MVVM模式下实现ListView下拉显示更多内容](http://www.cnblogs.com/yang-fei/p/4718325.html)；

但是这并不能解决最终内存占用过高的情况。

2\. 给图片设置DecodePixelWidth属性，

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

    BitmapImage image = new BitmapImage();

    image.BeginInit();

    image.UriSource \= new System.Uri(path);

    **image.DecodePixelWidth** **\= 800****;**

    image.EndInit();

    image.Freeze();

    models.Add(new ImageSourceModel() { ImageSource = image });

[![复制代码](//common.cnblogs.com/images/copycode.gif)](javascript:void(0); "复制代码")

此时的内存占用如图

![](https://images2015.cnblogs.com/blog/622438/201511/622438-20151102211543164-1942950521.png)

内存降低的非常显著，此时同样多的图片内存占用只有40M左右。

最终我们可以把优化方案1和优化方案2结合起来。这样在加载多张图片时不会出现卡顿的现象。另外从用户体验的角度我们可以在图片显示出来前，先用一个Loading的动画效果过渡下。

感谢您的阅读。代码和测试图片请点击[这里](http://pan.baidu.com/s/1qW9sw9Y)下载。

# 一

[WPF客户端读取高清图片很卡，缩略图解决方案](https://blog.csdn.net/educast/article/details/7640661)

在Ftp上传上，有人上传了高清图片，每张图片大约2M。

如果使用传统的BitmapImage类，然后绑定 Source 属性的方法，有些电脑在首次会比较卡，一张电脑10秒，4张大约会卡40秒。

 

所以我先异步的下载图片，得到downloadFileStream对象，然后绑定到BitmapImage类上。例如：

System.Windows.Controls.Image photo = new Image

{

    Width = 100,

    Height = 100,

    Margin = new Thickness(2),

    Stretch = Stretch.Uniform

};

 

BitmapImage bitmap = new BitmapImage();

bitmap.BeginInit();

bitmap.StreamSource = downloadFileStream;

bitmap.EndInit();

 

photo.Source = bitmap;

 

ListBoxItem lbi = new ListBoxItem()

{

    DataContext = pvo,

    Content = photo

};

 

this.lbPhotoes.Items.Add(lbi);

 

因为bitmap的StreamSource比较大，造成lbi对象比较大，所以lbPhotoes.Items.Add 方法在添加了两张图片之后就会卡大约30秒的时间。

 

所以尝试使用缩略图的方式来使BitmapImage的对象变小，在这里采用缩略图是因为客户端需要图片大小大致是

(100，100)。

 

完整的代码如下：

System.Windows.Controls.Image photo = new Image

{

    Width = 100,

    Height = 100,

    Margin = new Thickness(2),

    Stretch = Stretch.Uniform

};

 

using (System.Drawing.Image drawingImage = System.Drawing.Image.FromStream(downloadFileStream))

{

using (System.Drawing.Image thumbImage =

drawingImage.GetThumbnailImage(100, 100, () => { return true; }, IntPtr.Zero))

    {

        MemoryStream ms = new MemoryStream();

        thumbImage.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

 

        BitmapFrame bf = BitmapFrame.Create(ms);

        photo.Source = bf;

    }

}

 

ListBoxItem lbi = new ListBoxItem()

{

    DataContext = pvo,

    Content = photo

};

 

this.lbPhotoes.Items.Add(lbi);

 

在这里，要引用System.Drawing.dll.使用System.Drawing.Image 类的GetThumbnailImage 方法来获取thumbImage，接着使用MemoryStream来保存缩略图的stream，接着用缩略图的stream来生成图片了。

 

 没那么复杂，只要直接设置DecodeWidth即可
其实Thumbnail内部也是这样实现的

最后说一句：虽然解决了这个问题，不过每次都要下载高清图片，生成缩略图，这是很耗时的，所以在上传图片的时候就应该生成缩略图了，将缩略图保存起来了。因为在局域网中，网速比较快，这种方式基本也可以满足要求了。

# 其他

https://bbs.csdn.net/topics/393469868

# [解决WPF图片占用问题(WPF读取图片)](https://blog.csdn.net/weixin_43563837/article/details/105556790)

WPF调用图片过程中,最大问题就是图片占用,用这个方法将图片读取方式改为放到内存中,可以有效避免图片占用：

```C#
string imgpath="D:\\files..";
BitmapImage bitmap = new BitmapImage();
using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(imgpath)))
                {
                    bitmap = new BitmapImage();
                    bitmap.DecodePixelHeight = 100;
                    bitmap.BeginInit();
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;//设置缓存模式
                    bitmap.StreamSource = ms;//通过StreamSource加载图片
                    bitmap.EndInit();
                    bitmap.Freeze();
                }

Image img = new Image
            {
                Height = 98,
                Source = bitmap
            };
```


# [image图片解除占用 wpf_wpf4.5 关于 BitmapImage设置图片后 资源占用的问题](https://blog.csdn.net/weixin_39912984/article/details/111915913?spm=1001.2101.3001.6650.4&utm_medium=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-4-111915913-blog-105556790.235%5Ev27%5Epc_relevant_3mothn_strategy_and_data_recovery&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-4-111915913-blog-105556790.235%5Ev27%5Epc_relevant_3mothn_strategy_and_data_recovery&utm_relevant_index=5)

wpf4.5 关于 BitmapImage设置图片后 资源占用的问题问题出现： 设计背景图后，可以删除背景图，发现图片被占用

解决办法 ， 最后一步  调用 BitmapImage对象的clone方法， 不支持packuri

也可以设置

BitmapImage bitmapImage = new BitmapImage(); //初始化BitmapImage类的一个新实例Image image1 = new Image(); //定义一个Image控件string strPath = "D:\\mImage.png";//图片所在的位置bitmapImage.BeginInit(); //表示BitmapImage初始化开始bitmapImage.CacheOption = BitmapCacheOption.Onload;bitmapImage.UriSource = new Uri(strPath);//获取或设置BitmapImage的Uri源bitmapImage.EndInit();//表示BitmapImage初始化结束image1.Source = bitmapImage;//将image1控件的源指定为bitmapImage

或者

读取文件到流，然后到内存去private void InitImage()

{

using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))

{

FileInfo fi = new FileInfo(filePath);

byte[] bytes = reader.ReadBytes((int)fi.Length);

reader.Close();

image = new Image();

bitmapImage = new BitmapImage();

bitmapImage.BeginInit();

bitmapImage.StreamSource = new MemoryStream(bytes);

bitmapImage.EndInit();

image.Source = bitmapImage;

bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

this.LayoutRoot.Children.Add(image);

}

}

推荐您阅读更多有关于“WPF4.5，”的文章


# [WPF 设置图片的分辨率DPI](https://blog.csdn.net/YouyoMei/article/details/104598426)

WPF 修改图片的分辨率/DPI
在WPF中，当使用到PNG之类的图片作为背景时。我发现一个问题：图片属性（Windows）的宽、高相同的两张图片在WPF界面上显示却大小不一。如下图所示。


![https://img-blog.csdnimg.cn/2020030119412565.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L1lvdXlvTWVp,size_16,color_FFFFFF,t_70](https://img-blog.csdnimg.cn/2020030119412565.png?x-oss-process=image/watermark,type_ZmFuZ3poZW5naGVpdGk,shadow_10,text_aHR0cHM6Ly9ibG9nLmNzZG4ubmV0L1lvdXlvTWVp,size_16,color_FFFFFF,t_70)

在后台应用程序调试时发现，两个图片的DPI不一致。

2.png

![在这里插入图片描述](https://img-blog.csdnimg.cn/20200301194204559.png)

3.png

![在这里插入图片描述](https://img-blog.csdnimg.cn/20200301194215268.png)

百度了下，网友提供了三种解决方法：

- 创建 BitmapImage 对象，根据当前屏幕的 DPI 值计算 DecodePixelWidth 和 DecodePixelHeight ；
    
- 创建 DrawingImage 对象，直接按照 WPF 的坐标单位绘制图片原始像素大小的图片；
    
- 创建 [Bitmap](https://so.csdn.net/so/search?q=Bitmap&spm=1001.2101.3001.7020) / WriteableBitmap 对象，重新创建一张 96 DPI 的图片。
    

尝试了下，没走通，于是另辟蹊径。

在调试的时候，发现Biamap生成的时候DPI已经是299了，因此将目光转到了修改Biamap的DPI。

## 方法1：

1.将图片加载成bitmap格式，然后转换成BitmapImage格式

```C#
		/// <summary>
        /// 图片转换
        /// </summary>
        /// <param name="bitmap">bitmap格式图片</param>
        /// <returns></returns>
        private static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            // 直接设置DPI
            bitmap.SetResolution(96, 96);
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

```

2.使用

```C#
            ib2.ImageSource = BitmapToBitmapImage(new System.Drawing.Bitmap(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "3.png")));

```

说明：ib2是一个画刷（ImageBrush）。

但是好像在Win7下面这个方法失效了。

**方法2：**

思路：重新生成一个Bitmap，将原来的从文件资源加载上来的Bitmap绘制到新的Bitmap上。然后再用新的Bitmap去转换成BitmapImage格式。

Bitmap转换方法：

```C#
        /// <summary>
        /// 转换Bitmap类型，通过GDI重新获取一个新的Bitmap。
        /// </summary>
        /// <param name="imagePath">原图片的路径</param>
        /// <returns></returns>
        private Bitmap TranslateBitmap(string imagePath)
        {
            // 返回的
            Bitmap result = null;
            using (FileStream fs = new FileStream(imagePath, FileMode.Open))
            {
                // 原图片信息
                Bitmap orignal = new Bitmap(fs);

                // 注意：如果是要透明的图片就需要使用 Format32bppPArgb 格式，具有Alpha透明度。
                result = new Bitmap(orignal.Width, orignal.Height, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
                // 设置 DPI 信息，后来测试发现不用设置
                //result.SetResolution(96.0F, 96.0F);
                // 使用GDI画图
                using (Graphics g = Graphics.FromImage(result))
                {
                    g.Clear(System.Drawing.Color.Transparent);
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(orignal, new System.Drawing.Rectangle(0, 0, result.Width, result.Height), 0, 0, orignal.Width, orignal.Height, GraphicsUnit.Pixel);
                    g.Dispose();
                }
            }
            return result;
        }

```

然后稍微修改下BitmapToBitmapImage方法：

```C#
		/// <summary>
        /// 图片转换
        /// </summary>
        /// <param name="bitmap">bitmap格式图片</param>
        /// <returns></returns>
        private static BitmapImage BitmapToBitmapImage(System.Drawing.Bitmap bitmap)
        {
            BitmapImage bitmapImage = new BitmapImage();
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

```

问题解决了。

Over



# [WPF 加载大图片的问题](https://q.cnblogs.com/q/21770/)

我用wpf中的Image控件来显示大图片的时候，当图片超过12M，经常出现不显示的现象。其他的图片大小为6M左右的时候，基本都可以正常的显示。

如果图片能够正常显示的时候，中间会有个几秒钟的停顿，而没有正常显示的时候，就不会出现停顿现象。

代码如下：

   BinaryReader binReader = new BinaryReader(File.Open(picPath, FileMode.Open));
   FileInfo fileInfo = new FileInfo(picPath);
   byte[] bytes = binReader.ReadBytes((int)fileInfo.Length);
   binReader.Close();

   BitmapImage bitmap = new BitmapImage();
   bitmap.BeginInit();
   bitmap.StreamSource = new MemoryStream(bytes);
   bitmap.EndInit();

   image.Source = bitmap;

picPath 是图片的路径，image 就是Image控件。

希望各位高手能帮忙解决一下。非常感谢。

 

.NET技术WPF
问题补充： 在使用ImageFailed事件的时候，图片不加载提示是：“没有足够的内存继续执行程序。”，怎么解决这个问题？


使用多线程：

var source = await Task.Run<ImageSource>(() =>
                    {                       
                        var s = new BitmapImage();
                        s.BeginInit();
                        s.CacheOption = BitmapCacheOption.OnLoad;
                        using (var stream = File.OpenRead(path))//图片路径
                        {
                            s.StreamSource = stream;
                            s.EndInit();
                            s.Freeze();//冻结 否则不能被UI线程调用
                        }
                        return s;

　　　　　　  });

bigImage.Source=source;

//另外，可以先显示低分辨率的缩略图，添加过渡动画……

source.DecodePixelWidth =100；//设置一个较小的值即可“瞬间”加载……

以上代码测试过20多Ｍ的图片，耗时２～３秒吧。


# [C#:涉及DPI的高分辨率下的显示问题](https://www.cnblogs.com/shenchao/p/5594831.html)【好文】

一、背景

　　在PC机上显示正常，在高分辨率下的Pad上，显示出现问题：

　　　　1、显示在屏幕最右端的窗体（控件）显示不出来；

　　　　2、截图时，被截图的界面字体文字变大，界面因此显示不全。

二、解决方法：

　　方法一：WPF上使用WPF方式获取屏幕大小，而不是Winform的获取屏幕大小的方式。

```C#
                //Size primarySize = Screen.PrimaryScreen.Bounds.Size;
                double dWidth = System.Windows.SystemParameters.PrimaryScreenWidth;
                double dHeight = System.Windows.SystemParameters.PrimaryScreenHeight;
```


方法二：Winform解决方法：

　　1、设置窗体的背景图片方式改为可缩放方式（Zoom）： BackgroundImageLayout = ImageLayout.Zoom;

　　2、依据DPI扩展拷贝图片的大小,设置拷贝的图片的DPI（bmp的SetResolution方法）

```C#
BackgroundImage = GetDestopImage();
BackgroundImageLayout = ImageLayout.Zoom;



        private Image GetDestopImage()
        {
            float rate = dpi / 96;
            Rectangle rect = Screen.GetBounds(this);
            Size sz = new System.Drawing.Size();
            sz.Width = (int)(rect.Size.Width * rate);
            sz.Height = (int)(rect.Size.Height * rate);
            Bitmap bmp = new Bitmap(
                sz.Width, sz.Height, PixelFormat.Format32bppArgb);
            bmp.SetResolution(dpi, dpi);
            Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(0, 0, 0, 0, sz);
            //IntPtr gHdc = g.GetHdc();
            //IntPtr deskHandle = NativeMethods.GetDesktopWindow();

            //IntPtr dHdc = NativeMethods.GetDC(deskHandle);
            //NativeMethods.BitBlt(
            //    gHdc,
            //    0,
            //    0,
            //    Width ,
            //    Height,
            //    dHdc,
            //    0,
            //    0,
            //    NativeMethods.TernaryRasterOperations.SRCCOPY);
            //NativeMethods.ReleaseDC(deskHandle, dHdc);
            //g.ReleaseHdc(gHdc);
            //bmp.Save("test.png");
            return bmp;
        }
```

　3、修改拷贝内容位置信息

```C#
private void DrawLastImage()
        {
            float rate = dpi / 96;
            int reWidth = (int)(Width * rate);
            int reHeight = (int)(Height * rate);
            using (Bitmap allBmp = new Bitmap(
                reWidth, reHeight, PixelFormat.Format32bppArgb))
            {
                allBmp.SetResolution(dpi,dpi);
                using (Graphics allGraphics = Graphics.FromImage(allBmp))
                {
                    allGraphics.InterpolationMode = 
                        InterpolationMode.HighQualityBicubic;
                    allGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    allGraphics.DrawImage(
                        BackgroundImage,
                        Point.Empty);
                    DrawOperate(allGraphics);
                    allGraphics.Flush();

                    Rectangle reSelectImageRect = new Rectangle();
                    reSelectImageRect.X = (int)(SelectImageRect.X * rate);
                    reSelectImageRect.Y = (int)(SelectImageRect.Y * rate);
                    reSelectImageRect.Width = (int)(SelectImageRect.Width * rate);
                    reSelectImageRect.Height = (int)(SelectImageRect.Height * rate);
                    Bitmap bmp = new Bitmap(
                       reSelectImageRect.Width,
                       reSelectImageRect.Height,
                       PixelFormat.Format32bppArgb);
                    bmp.SetResolution(dpi, dpi);
                    Graphics g = Graphics.FromImage(bmp);
                    g.DrawImage(
                        allBmp,
                        0,
                        0,
                        reSelectImageRect,
                        GraphicsUnit.Pixel);

                    g.Flush();
                    g.Dispose();
                    _image = bmp;
                }
            }
        }
```

　　4、获取DPI代码：

```C#
public static float getLogPiex()
        {
            float returnValue = 96;
            try
            {
            RegistryKey key = Registry.CurrentUser;
            RegistryKey pixeKey = key.OpenSubKey("Control Panel\\Desktop");
            if (pixeKey != null)
            {
                var pixels = pixeKey.GetValue("LogPixels");
                if (pixels != null)
                {
                    returnValue = float.Parse(pixels.ToString());
                }
                pixeKey.Close();
            }
            else
            {
                pixeKey = key.OpenSubKey("Control Panel\\Desktop\\WindowMetrics");
                if (pixeKey != null)
                {
                    var pixels = pixeKey.GetValue("AppliedDPI");
                    if (pixels != null)
                    {
                        returnValue = float.Parse(pixels.ToString());
                    }
                    pixeKey.Close();
                }
            }
            }
            catch(Exception ex)
            {
                returnValue = 96;
            }
            return returnValue;
        }
```

# [WPF：图像处理（一）图像文件获取与预览](https://blog.csdn.net/jhqin/article/details/7472190)

```C#
/* ----------------------------------------------------------
文件名称：Folder.cs
作者：秦建辉
MSN：splashcn@msn.com
QQ：36748897
博客：http://blog.csdn.net/jhqin
开发环境：
    Visual Studio V2010
    .NET Framework 4 Client Profile
版本历史：
    V1.0    2012年04月16日
            图像文件获取与预览
------------------------------------------------------------ */
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
 
namespace Splash.Imaging
{
    public static class Folder
    {   
        /// <summary>
        /// 委托声明：鼠标点击事件处理器
        /// </summary>
        /// <param name="sender">附加事件处理程序的对象</param>
        /// <param name="e">事件数据</param>
        public delegate void MyMouseDownEventHandler(object sender, System.Windows.Input.MouseButtonEventArgs e);
 
        /// <summary>
        /// 返回指定目录中与指定的搜索模式匹配的文件的名称（包含它们的路径），并使用一个值以确定是否搜索子目录
        /// </summary>
        /// <param name="folderPath">将从其检索文件的目录</param>
        /// <param name="filter">搜索模式匹配</param>
        /// <param name="searchOption">指定搜索操作应包括所有子目录还是仅包括当前目录</param>
        /// <returns>包含指定目录中与指定搜索模式匹配的文件的名称列表</returns>
        public static String[] GetFiles(String folderPath, String filter, SearchOption searchOption)
        {   // 获取文件列表
            String[] FileEntries = Directory.GetFiles(folderPath, "*", searchOption);
            if (FileEntries.Length == 0) 
                return null;
            else if (String.IsNullOrEmpty(filter)) 
                return FileEntries;
 
            // 建立正则表达式
            Regex rx = new Regex(filter, RegexOptions.IgnoreCase);
 
            // 过滤文件
            List<String> FilterFileEntries = new List<String>(FileEntries.Length);            
            foreach (String FileName in FileEntries)
            {
                if (rx.IsMatch(FileName))
                {
                    FilterFileEntries.Add(FileName);
                }
            }
 
            return (FilterFileEntries.Count == 0) ? null : FilterFileEntries.ToArray();            
        }
 
        /// <summary>
        /// 返回指定目录中图像文件列表，并使用一个值以确定是否搜索子目录
        /// </summary>
        /// <param name="folderPath">将从其检索文件的目录</param>
        /// <param name="searchOption">指定搜索操作应包括所有子目录还是仅包括当前目录</param>
        /// <returns>图像文件列表</returns>
        public static String[] GetImages(String folderPath, SearchOption searchOption)
        {
            String filter = "\\.(bmp|gif|jpg|jpe|png|tiff|tif)$";
            return GetFiles(folderPath, filter, searchOption);
        }
 
        /// <summary>
        /// 显示图像
        /// </summary>
        /// <param name="panel">放置图像控件的容器</param>
        /// <param name="images">要显示的图像文件集合</param>
        /// <param name="handler">图像控件点击事件处理器</param>
        public static void DisplayImages(StackPanel panel, String[] images, MyMouseDownEventHandler handler)
        {   // 参数检测
            if ((panel == null) || (images == null)) return;
 
            // 清空所有图像控件
            panel.Children.Clear();
 
            // 增加新的图像控件
            Double Stride = (panel.Orientation == Orientation.Horizontal) ? panel.Height : panel.Width;
            foreach (String FileName in images)
            {   // 设置边框                           
                Border border = new Border();
                border.Margin = new System.Windows.Thickness(4);            // 边框外边距
                border.BorderThickness = new System.Windows.Thickness(4);   // 边框厚度
                border.BorderBrush = new SolidColorBrush(Colors.DarkGreen); // 边框颜色
 
                // 边框大小
                if (panel.Orientation == Orientation.Horizontal)
                {   
                    border.Height = border.Width = Stride - border.Margin.Top - border.Margin.Bottom; 
                }
                else
                {
                    border.Height = border.Width = Stride - border.Margin.Left - border.Margin.Right;
                }
                
                // 设置图像控件
                Image image = new Image();
                image.Width = border.Width - border.BorderThickness.Left - border.BorderThickness.Right;   // 图像控件宽度
                image.Height = border.Height - border.BorderThickness.Top - border.BorderThickness.Bottom;  // 图像控件高度
                image.Stretch = System.Windows.Media.Stretch.Uniform;
                image.Source = new BitmapImage(new Uri(FileName));
 
                // 图片点击事件
                image.MouseDown += new System.Windows.Input.MouseButtonEventHandler(handler);
 
                // 设置控件布局
                border.Child = image;
                panel.Children.Add(border);
            }
        }
    }
}
```


# [WPF图片浏览器（显示大图、小图等）](https://blog.csdn.net/wangshubo1989/article/details/46784601)

**1.概述  **  

        最近利用WPF做了一个图片浏览器，能够将文件夹中的所有图片以小图的形式显示，并将选中的图片以512\*512大小显示。显示大图当然用的是WPF自带的Image控件，而显示小图则需要将所有的图片放入ListBox控件中，ListBox里的每一个Item为Image控件。为了方便显示多张小图，在ListBox控件外加入了ScrollViewer控件，以便可以拖动显示ListBox中的Item。并实现了对图片的一系列操作，如放大、缩小、恢复1:1的比例、上一张、下一张等功能。效果图如下：

![https://img-blog.csdn.net/20150707085559995?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQv/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/Center](https://img-blog.csdn.net/20150707085559995?watermark/2/text/aHR0cDovL2Jsb2cuY3Nkbi5uZXQv/font/5a6L5L2T/fontsize/400/fill/I0JBQkFCMA==/dissolve/70/gravity/Center)

2.XAML设计

2.1 ScrollViewer样式

即为滚动条添加一个动画，只有鼠标移到滚动条或是通过滑轮切换图片，或是连续点击上一张、下一张按钮时，滚动条才会显示，其他状态下隐藏。

2.2 ListBox样式

<Style TargetType="{x:Type ListBox}" x:Key="PhotoListBoxStyle"> 

    <Setter Property="Foreground" Value="White" /> 

<Setter Property="Template"> 

<Setter.Value> 

<ControlTemplate TargetType="{x:Type ListBox}" > 

<WrapPanel Margin="5" IsItemsHost="True" Orientation="Horizontal" ItemHeight="{Binding ElementName=ZoomSlider, Path='Value'}" ItemWidth="{Binding ElementName=ZoomSlider, Path='Value'}" VerticalAlignment="Top" HorizontalAlignment="Stretch" /> 

</ControlTemplate> 

</Setter.Value> 

</Setter>

</Style>

<ScrollViewer Panel.ZIndex="1" Name="scrolls" Style="{StaticResource for_scrollviewer}" VerticalScrollBarVisibility="Disabled" HorizontalScrollBarVisibility="Visible" Margin="181,561,1107,0" MaxWidth="560" Height="174" VerticalAlignment="Top" Canvas.Left="463" Canvas.Top="158" Width="516" HorizontalAlignment="Center" HorizontalContentAlignment="Center"> 

    <ListBox IsSynchronizedWithCurrentItem="True" Name="list1" Style="{StaticResource PhotoListBoxStyle}" Margin="5" SelectionMode="Extended" SelectedIndex="-1" Height="150" SelectionChanged="list1_SelectionChanged" UIElement.MouseWheel="ImageList_MouseWheel"> 

        <ListBox.Resources> 

              <Style TargetType="ListBoxItem"> 

                    <Style.Resources> 

                     <SolidColorBrush x:Key="{x:Static SystemColors.HighlightBrushKey}" Color="Blue" /> 

                     <SolidColorBrush x:Key="{x:Static SystemColors.ControlBrushKey}" Color="Blue" Opacity=".3"/> 

                     <SolidColorBrush x:Key="{x:Static SystemColors.HighlightTextBrushKey}" Color="White" /> 

                     <SolidColorBrush x:Key="{x:Static SystemColors.ControlTextBrushKey}" Color="White" /> 

                    </Style.Resources> 

              </Style>

        </ListBox.Resources> 

    </ListBox> 

</ScrollViewer>

2.3 Button样式

操作按钮放在一个StackPanel中，当鼠标移入的时候按钮的透明度发生变化，以1:1按钮的样式为例 如下：


<Style x:Key = "ImgButton" TargetType = "P{x:Type Button}">

    <Setter Property = "Cursor" Value = "Hand"></Setter>

    <Setter Property = "Width" Value = "35"></Setter>
    <Setter Property = "Height" Value = "30"></Setter>

</Style>

<Style x:Key = "UndoStyle" TargetType = "{x:Type Button}" BasedOn = "{StaticResource ImgButton}">

  <Setter Property = "Template">

      <Setter.Value>

      <ControlTemplate TargetType="{x:Type Button}"> 

        <Border> 

         <Image Name="img" Source="/DoctorOld;component/Images/undo.png">

        </Image> 

        </Border> 

      <ControlTemplate.Triggers> 

      <Trigger Property="IsMouseOver" Value="True"> 

          <Setter Property="Opacity" Value="0.7">

          </Setter> 

      </Trigger> 

       </ControlTemplate.Triggers> 

      </ControlTemplate>
      </Setter.Value>
  </Setter>


3.后台代码

3.1 初始化

首先定义一个ArrayList用于存放图片的名称，即 ArrayList myArrayList = new ArrayList();

然后将指定文件夹中的所有图像的名称信息放到myArrayList中

DirectoryInfo Dir = new DirectoryInfo("D:\\myPic); 

foreach (FileInfo file in Dir.GetFiles("*.png", System.IO.SearchOption.AllDirectories)) 

{ 

str = file.ToString();

myArrayList.Add(str); 

} 

如果图片的数量大于0，就执行AddImg（）函数

if (newArrayList.Count > 0) 

{ 

    for (int i = 0; i < newArrayList.Count; i++) 

    { 

        AddImg("D:\\myPic + "\\" + newArrayList[i].ToString()); 

    } 

}

将图片添加到ListBox中的函数如下：

public void AddImg(string szPath) 

{ 

string[] s = szPath.Split(new char[] { '\\' }); //按照“\\”截取字符串，即为了获取图片的名称

System.Windows.Controls.Image listImage = new System.Windows.Controls.Image(); //创建一个Image控件

Border br = new Border(); 

BitmapImage bitmapImage = new BitmapImage(); 

ListBoxItem newListBox = new ListBoxItem(); //创建一个ListBoxItem，作为ListBox的元素

//将指定路径的图片加载到名为listImage的Image控件中，请参考http://blog.csdn.net/wangshubo1989/article/details/46710411

bitmapImage.BeginInit(); 

bitmapImage.CacheOption = BitmapCacheOption.OnLoad;

bitmapImage.UriSource = new Uri(szPath); 

bitmapImage.EndInit(); 

bitmapImage.Freeze();

listImage.Source = bitmapImage;

//设置border的高、宽、圆角

br.Width = 106; 

br.Height = 106; 

br.CornerRadius = new CornerRadius(10); 



Label PicLabel = new Label();//鼠标移到图片上显示图片的名称 

listImage.ToolTip = s[2]; //获得图片的名称，例如全路径为 D：\\myPic\\123.png，即将提示的字符串赋值为123.png



//Image添加到Border中 

br.Child = listImage; 

br.Padding = new System.Windows.Thickness((float)1.1f); 

br.Background = System.Windows.Media.Brushes.White; 

br.BorderThickness = new System.Windows.Thickness((int)3); 

//阴影效果 

DropShadowEffect myDropShadowEffect = new DropShadowEffect(); 

// Set the color of the shadow to Black. 

System.Windows.Media.Color myShadowColor = new System.Windows.Media.Color(); 

myShadowColor.A = 255; 

// Note that the alpha value is ignored by Color property. 

// The Opacity property is used to control the alpha.

myShadowColor.B = 50; 

myShadowColor.G = 50; 

myShadowColor.R = 50; 

myDropShadowEffect.Color = myShadowColor; 

// Set the direction of where the shadow is cast to 320 degrees. 

myDropShadowEffect.Direction = 310; 

// Set the depth of the shadow being cast. 

myDropShadowEffect.ShadowDepth = 20; 

// Set the shadow softness to the maximum (range of 0-1). 

myDropShadowEffect.BlurRadius = 10; 

// Set the shadow opacity to half opaque or in other words - half transparent. 

// The range is 0-1. 

myDropShadowEffect.Opacity = 0.4; 

// Apply the effect to the Button.

newListBox.Effect = myDropShadowEffect;

newListBox.Content = br; 

newListBox.Margin = new System.Windows.Thickness((int)10); 

newListBox.DataContext = szPath; 

list1.Items.Add(newListBox); //list1为界面上ListBox控件的名称

list1.SelectedIndex = list1.Items.Count - 1;

scrolls.ScrollToRightEnd(); //使得滚动条 滚到最后， scrolls为ScrollViewer控件的名称

}

3.2 放大
首先写一个图片大小变化的函数，参数为布尔型，用于判断是放大还是缩小，其中image1为显示大图的Image控件的名称

private void ChangImgSize(bool big)

{ 

    Matrix m = image1.RenderTransform.Value; 

    System.Windows.Point p = new System.Windows.Point((image1.ActualWidth) / 2, (image1.ActualHeight) / 2); 

    if (big) 

    { 

        m.ScaleAtPrepend(1.1, 1.1, p.X, p.Y); 

    } 

    else 

    { 

        m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, p.X, p.Y); 

    } 

    image1.RenderTransform = new MatrixTransform(m);

}

所以对于点击放大按钮函数如下：

private void btnBig_Click(object sender, RoutedEventArgs e)

{ 

    ChangImgSize(true); 

}



同时，可以通过滚动鼠标滑轮来进行放大缩小，通过判断e.Delta的正负来判读是向上还是向下滚动鼠标换轮

private void Image_MouseWheel(object sender, RoutedEventArgs e)

{

    ChangImgSize(e.Delta>0);

}

3.3 缩小

原理如上，顾点击缩小按钮的函数如下

private void btnSmall_Click(object sender, RoutedEventArgs e)

{ 

    ChangImgSize(false);

}



同时，可以通过滚动鼠标滑轮来进行缩小 

3.4 恢复1:1

点击1:1按钮 即恢复512*512的大小的图像

private void btnUndo_Click(object sender, RoutedEventArgs e)//恢复图像512*512大小 

{ 

    image1.RenderTransform = new MatrixTransform(new Matrix(1, 0, 0, 1, 0, 0)); 

}



3.5 下一张


private void nextStyle_Click(object sender, RoutedEventArgs e)//点击下一张按钮 

{ 

m_nImgCount = list1.SelectedIndex + 1; //m_nImgCount 为静态全局变量，用于存储当前小图的数量

if (m_nImgCount < m_nPicNum) 

{ 

list1.SelectedIndex++; 

BitmapImage bitmapImage1 = new BitmapImage(); 

bitmapImage1.BeginInit(); 

bitmapImage1.CacheOption = BitmapCacheOption.OnLoad; 

bitmapImage1.UriSource = new Uri("D:\\myPic" + newArrayList[list1.SelectedIndex].ToString()); 

bitmapImage1.EndInit(); 

bitmapImage1.Freeze(); 

image1.Source = bitmapImage1;

scrolls.ScrollToHorizontalOffset(list1.SelectedIndex * 130); 

} 

else 

{ 

      MessageBox.Show("No Image"); 

} 

}

3.6 上一张

原理和下一张操作相同，不在赘述。

3.7 删除

请参照文章 http://blog.csdn.net/wangshubo1989/article/details/46710411

4.总结
一个图片查看器就此呈现在你的眼前，可以进一步的丰富界面效果。如果有疑问、错误、代码优化、需要更多源码等问题，欢迎留言。

邮箱：wangshubo1989@126.com
————————————————
版权声明：本文为CSDN博主「一苇渡江694」的原创文章，遵循CC 4.0 BY-SA版权协议，转载请附上原文出处链接及本声明。
原文链接：https://blog.csdn.net/wangshubo1989/article/details/46784601