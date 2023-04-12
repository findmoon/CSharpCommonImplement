using System.Linq;

namespace System.IO
{
    public enum ImgFormat
    {
        JPG = 255216,
        GIF = 7173,
        PNG = 13780,
        SWF = 6787,
        RAR = 8297,
        ZIP = 8075,
        _7Z = 55122,
        INVALIDIMG = 9999999
    }
    /// <summary>
    /// 检查图片类，检查是否为图片、检查图片类型
    /// </summary>
    public class CheckImgFormat
    {
        /// <summary>
        /// 判断文件是否为图片
        /// </summary>
        /// <param name="path">文件的完整路径</param>
        /// <returns>返回结果</returns>
        public static bool IsImage(string path)
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



        public static bool IsImgFormat(string fileName, params ImgFormat[] imgFormat)
        {
            if (imgFormat.Length == 0)
            {
                return false;
            }
            //System.IO.FileNotFoundException:“未能找到文件“E:\privateboolnote\笔记\frontend\Flutter\img\Highly%20Subjective%20Roadmap%20to%20Flutter%20Development.png”。”
            // 字符的url编码解读问题
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                string fileType = string.Empty;

                byte data = br.ReadByte();
                fileType += data.ToString();
                data = br.ReadByte();
                fileType += data.ToString();

                try
                {
                    var format = (ImgFormat)Enum.Parse(typeof(ImgFormat), fileType);
                    if (imgFormat.Contains(format))
                    {
                        return true;
                    }
                }
                catch
                {
                    if (imgFormat.Contains(ImgFormat.INVALIDIMG))
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public static ImgFormat GetImgFormat(string fileName)
        {
            using (var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                BinaryReader br = new BinaryReader(fs);
                string fileType = string.Empty;

                byte data = br.ReadByte();
                fileType += data.ToString();
                data = br.ReadByte();
                fileType += data.ToString();

                try
                {
                    return (ImgFormat)Enum.Parse(typeof(ImgFormat), fileType);
                }
                catch
                {

                    return ImgFormat.INVALIDIMG;
                }
            }
        }
    }

}

