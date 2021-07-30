namespace NorthwindDemo.Common
{
    public class SystemSettingOptions
    {
        public const string SectionName = "SystemSettings";

        /// <summary>
        /// 服務名稱
        /// </summary>
        public string ServiceName { get; set; } = "Sample.WevApplication";

        /// <summary>
        /// 版本號
        /// </summary>
        public string ServiceVersion { get; set; } = "";

        /// <summary>
        /// 服務描述
        /// </summary>
        public string ServiceDescription { get; set; } = "Sample.WevApplication";

        /// <summary>
        /// Redis InstanceName
        /// </summary>
        public string RedisInstanceName { get; set; } = "Sample.WevApplication";
    }
}