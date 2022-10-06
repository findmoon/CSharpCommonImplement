using System.Runtime.InteropServices;

namespace CMCom
{
    [Guid("D16E9145-733E-48DA-87FB-306703E4F776")]
    public interface CMComInterface
    {
        [DispId(1)]
        int Add(int a, int b);
    }
}