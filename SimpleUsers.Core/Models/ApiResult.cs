using System;

namespace SimpleUsers.Core.Models
{
    public class ApiResult
    {
        public bool Ok {get;set;}

        public string Err {get;set;}

    }

    public class ApiResult<T> : ApiResult
    {
        public T Data {get;set;}
    }

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