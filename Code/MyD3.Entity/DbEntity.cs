using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyD3.Data.Entities
{
    public class DbEntity
    {
        /// <summary>
        /// 获取或修改实体标识
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 重写对象比较
        /// </summary>
        /// <param name="obj">需要比较的对象</param>
        /// <returns>是否一致</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as DbEntity);
        }

        /// <summary>
        /// 是否为临时实体
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static bool IsTransient(DbEntity obj)
        {
            return obj != null && string.IsNullOrEmpty(obj.ID);
        }

        /// <summary>
        /// 获取实际类型
        /// </summary>
        /// <returns></returns>
        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(DbEntity other)
        {
            if (other == null)
            {
                //如果第二比较对象为空则返回不等
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                //如果引用一致，则返回等
                return true;
            }

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(ID, other.ID))
            {
                //如果不是临时对象且ID相等
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                //如果两者类型引用一致则返回相等
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(ID, default(string)))
                return base.GetHashCode();
            return ID.GetHashCode();
        }

        public static bool operator ==(DbEntity x, DbEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(DbEntity x, DbEntity y)
        {
            return !(x == y);
        }
    }
}
