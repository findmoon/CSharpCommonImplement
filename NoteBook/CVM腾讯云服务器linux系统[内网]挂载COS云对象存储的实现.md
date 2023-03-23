**CVM腾讯云服务器linux系统[内网]挂载COS云对象存储的实现**

[toc]

COS 是否有内网域名？

对象存储（Cloud Object Storage，COS）的默认源站域名格式为：<BucketName-APPID>.cos.<Region>.myqcloud.com，默认已支持公网访问和同地域的内网访问。例如examplebucket-1250000000.cos.ap-guangzhou.myqcloud.com，更多域名相关介绍，请参见 [地域和访问域名](https://cloud.tencent.com/document/product/436/6224)。

您在内网环境下通过该域名访问 COS 时，COS 会智能解析到内网 IP 上。

[CVM 挂载 COS](https://console.cloud.tencent.com/cos/bucket?bucket=pan-storage-bucket-1256319690&region=ap-beijing&type=csg) 