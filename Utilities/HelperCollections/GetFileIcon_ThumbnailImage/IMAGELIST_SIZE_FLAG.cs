namespace HelperCollections
{
    /// <summary>
    /// IMAGELIST 的 图标大小标识
    /// </summary>
    public enum IMAGELIST_SIZE_FLAG : int
    {
        /// <summary>
        /// Size(32,32)
        /// </summary>
        SHIL_LARGE = 0x0,
        /// <summary>
        /// Size(16,16)
        /// </summary>
        SHIL_SMALL = 0x1,
        /// <summary>
        /// Size(48,48)
        /// </summary>
        SHIL_EXTRALARGE = 0x2,
        /// <summary>
        /// Size(16,16)
        /// </summary>
        SHIL_SYSSMALL = 0x3,
        /// <summary>
        /// Size(256,256)
        /// </summary>
        SHIL_JUMBO = 0x4,
        /// <summary>
        /// 保留使用：目前测试效果为Size(256,256)
        /// </summary>
        //SHIL_LAST = 0x4,
    }
}
