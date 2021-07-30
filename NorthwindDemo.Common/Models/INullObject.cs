namespace NorthwindDemo.Common.Models
{
    public interface INullObject
    {
        /// <summary>
        /// 確認此執行個體是否為 Null。
        /// </summary>
        /// <returns>此執行個體是 Null 則為 <c>true</c>，否則為 <c>false</c>。</returns>
        bool IsNull();
    }
}