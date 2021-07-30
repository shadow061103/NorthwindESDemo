namespace NorthwindDemo.Repository.Models.Cache
{
    public class CacheKey
    {
        public static string NorthwindExist => "northwind::exist::{0}";

        public static string NorthwindGet => "northwind::get::{0}";
    }
}