namespace iKitchen.Web.Models
{
    public class SystemSet
    {
        #region const

        /// <summary>
        /// 房源发布权限
        /// </summary>
        public const int SaleInfoPublish = 1;
        /// <summary>
        /// 房源协助发布
        /// </summary>
        public const int SaleInfoReview = 2;
        /// <summary>
        /// 房源修改
        /// </summary>
        public const int SaleInfoEdit = 4;
        /// <summary>
        /// 房源删除
        /// </summary>
        public const int SaleInfoDelete = 8;
        /// <summary>
        /// 房源刷新
        /// </summary>
        public const int SaleInfoRefresh = 16;
        /// <summary>
        /// 房源投诉复查
        /// </summary>
        public const int SaleInfoComplaintReview = 32;
        /// <summary>
        /// 房源房源查询
        /// </summary>
        public const int SaleInfoSearch = 64;
        /// <summary>
        /// 楼盘新建
        /// </summary>
        public const int ProjectCreate = 128;
        /// <summary>
        /// 楼盘启用
        /// </summary>
        public const int ProjectApprove = 256;
        /// <summary>
        /// 楼盘删除
        /// </summary>
        public const int ProjectDelete = 512;
        /// <summary>
        /// 楼盘查询
        /// </summary>
        public const int ProjectSearch = 1024;
        /// <summary>
        /// 用户禁用
        /// </summary>
        public const int UserDisable = 2048;
        /// <summary>
        /// 用户删除
        /// </summary>
        public const int UserDelete = 4096;
        /// <summary>
        /// 用户查询
        /// </summary>
        public const int UserSearch = 8192;
        /// <summary>
        /// 刷新申请
        /// </summary>
        public const int RefreshApply = 16384;
        /// <summary>
        /// 刷新启用
        /// </summary>
        public const int RefreshApprove = 32768;
        /// <summary>
        /// 权限设置
        /// </summary>
        public const int SystemAuthorize = 65536;
        /// <summary>
        /// 参数设置
        /// </summary>
        public const int SystemConfigure = 131072;
        /// <summary>
        /// 挂牌量统计
        /// </summary>
        public const int Statistics = 262144;
        /// <summary>
        /// 广告房源审核
        /// </summary>
        public const int Advertisement = 524288;
        /// <summary>
        /// 发布广告房源
        /// </summary>
        public const int AdvertisementApply = 1048576;
        #endregion

        #region Default
        /// <summary>
        /// 获取交易用户默认权限
        /// </summary>
        /// <returns></returns>
        public static int GetDefaultConfigurationOfUser()
        {
            return SaleInfoPublish | SaleInfoEdit | SaleInfoDelete | SaleInfoRefresh 
                | ProjectCreate | RefreshApply;
        }

        /// <summary>
        /// 获取网管用户默认权限
        /// </summary>
        /// <returns></returns>
        public static int GetDefaultConfigurationOfAdmin()
        {
            return SaleInfoReview | SaleInfoEdit | SaleInfoDelete | SaleInfoRefresh | SaleInfoComplaintReview | SaleInfoSearch
                | ProjectCreate | ProjectApprove | ProjectDelete | ProjectSearch
                | UserDisable | UserDelete | UserSearch
                | RefreshApply | RefreshApprove;
        }

        /// <summary>
        /// 获取超管用户默认权限
        /// </summary>
        /// <returns></returns>
        public static int GetDefaultConfigurationOfSystem()
        {
            return int.MaxValue;
        }
        #endregion
    }
}