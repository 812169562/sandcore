using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sand.Extensions
{
    /// <summary>
    /// DataAnnotations验证操作
    /// </summary>
    public static class DataAnnotationValidation
    {
        /// <summary>
        /// 验证
        /// </summary>
        /// <param name="target">验证目标</param>
        public static ValidationResultCollection Validate(object target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            var result = new ValidationResultCollection();
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(target, null, null);
            var isValid = Validator.TryValidateObject(target, context, validationResults, true);
            if (!isValid)
                result.AddList(validationResults);
            return result;
        }
    }

    /// <summary>
    /// 验证结果集合
    /// </summary>
    public class ValidationResultCollection : IEnumerable<ValidationResult>
    {
        /// <summary>
        /// 初始化验证结果集合
        /// </summary>
        public ValidationResultCollection()
        {
            _results = new List<ValidationResult>();
        }
        /// <summary>
        /// 验证结果
        /// </summary>
        private readonly List<ValidationResult> _results;

        /// <summary>
        /// 成功验证结果集合
        /// </summary>
        public static readonly ValidationResultCollection Success = new ValidationResultCollection();

        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsValid => _results.Count == 0;

        /// <summary>
        /// 验证结果个数
        /// </summary>
        public int Count => _results.Count;

        /// <summary>
        /// 添加验证结果
        /// </summary>
        /// <param name="result">验证结果</param>
        public void Add(ValidationResult result)
        {
            if (result == null)
                return;
            _results.Add(result);
        }

        /// <summary>
        /// 添加验证结果集合
        /// </summary>
        /// <param name="results">验证结果集合</param>
        public void AddList(IEnumerable<ValidationResult> results)
        {
            if (results == null)
                return;
            foreach (var result in results)
                Add(result);
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        IEnumerator<ValidationResult> IEnumerable<ValidationResult>.GetEnumerator()
        {
            return _results.GetEnumerator();
        }

        /// <summary>
        /// 获取迭代器
        /// </summary>
        public IEnumerator GetEnumerator()
        {
            return _results.GetEnumerator();
        }
    }
}
