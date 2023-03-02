using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelperCollections.ArgumentGuardHandle
{
    /// <summary>
    /// 确保参数的类，检查参数不为null、empty
    /// </summary>
    public static class ArgumentGuard
    {
        /// <summary>
        /// param、otherParams用于确保不为Null
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        /// <param name="otherParams"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void NotNull(object param, string paramName, params string[] otherParams)
        {
            if (param is null)
            {
                throw new ArgumentNullException(paramName);
            }
            foreach (var otherParam in otherParams)
            {
                if (otherParam is null)
                {
                    throw new ArgumentNullException();
                }
            }
        }

        public static void NotNullOrEmpty(string param, string paramName, string specifyexMsg = null)
        {
            if (string.IsNullOrEmpty(param))
            {
                throw new ArgumentException(specifyexMsg ?? "The string can not be empty.", paramName);
            }
        }
        /// <summary>
        /// param、otherParams用于确保不为NullEmptyOrWhiteSpace
        /// </summary>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        /// <param name="specifyexMsg">指定的特殊错误内容</param>
        /// <param name="otherParams"></param>
        /// <exception cref="ArgumentException"></exception>
        public static void NotNullEmptyOrWhiteSpace(string param, string paramName, string specifyexMsg = null,params string[] otherParams)
        {
            if (string.IsNullOrWhiteSpace(param))
            {
                throw new ArgumentException(specifyexMsg ?? "The string can not be empty or WhiteSpace.", paramName);
            }
            foreach (var otherParam in otherParams)
            {
                if (string.IsNullOrWhiteSpace(otherParam))
                {
                    throw new ArgumentException(specifyexMsg ?? "The string can not be empty or WhiteSpace.");
                }
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="param"></param>
        /// <param name="paramName"></param>
        /// <param name="specifyexMsg">指定的特殊错误内容</param>
        /// <exception cref="ArgumentException"></exception>
        public static void NotNullOrEmpty<T>(IEnumerable<T> param, string paramName,string specifyexMsg=null)
        {
            if (param is null || param.Count() == 0)
            {
                throw new ArgumentException(specifyexMsg??"The collection can not be null or empty.", paramName);
            }
        }
    }
}
