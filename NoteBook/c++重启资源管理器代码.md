**c++重启资源管理器代码**

[toc]

//重启资源管理器
system("taskkill /im explorer.exe /f");

Sleep(600);
//打开资源管理器
TCHAR szPath[255] = {0};
if ((SHGetFolderPath(NULL, CSIDL_WINDOWS, NULL, SHGFP_TYPE_CURRENT, szPath))!=S_OK)
{
wsprintf(szPath,L"c:\\windows");
}
ShellExecute(NULL,L"open",L"explorer.exe",0, szPath,SW_NORMAL);





[c++重启资源管理器代码](https://blog.csdn.net/dep36/article/details/52385143)