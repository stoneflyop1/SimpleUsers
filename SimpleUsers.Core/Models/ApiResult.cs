using System;

namespace SimpleUsers.Core.Models
{
    /// <summary>
    /// API的返回信息
    /// </summary>
    public class ApiResult
    {
        /// <summary>
        /// 是否成功
        /// </summary>
        /// <returns></returns>
        public bool Ok {get;set;}
        /// <summary>
        /// 出错信息
        /// </summary>
        /// <returns></returns>
        public string Err {get;set;}

    }
    /// <summary>
    /// API的返回信息
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// API返回的数据
        /// </summary>
        /// <returns></returns>
        public T Data {get;set;}
    }

    #pragma warning disable 1591

    public static class ApiResultUtil
    {
        public static ApiResult Success()
        {
            return new ApiResult{Ok = true};
        }

        public static ApiResult<T> Success<T>(T data)
        {
            return new ApiResult<T>{Ok = true, Data = data};
        }

        public static ApiResult Fail(string err)
        {
            return new ApiResult{Err = err};
        }

        public static ApiResult<T> Fail<T>(string err)
        {
            return new ApiResult<T>{Err = err};
        }

        public static string GetMessage(Exception ex)
        {
            while(ex.InnerException != null)
            {
                ex = ex.InnerException;
            }
            return ex.Message;
        }

        public static ApiResult Fail(Exception ex)
        {
            return new ApiResult{Err = GetMessage(ex)};
        }

        public static ApiResult<T> Fail<T>(Exception ex)
        {
            return new ApiResult<T>{Err = GetMessage(ex)};
        }
    }
}