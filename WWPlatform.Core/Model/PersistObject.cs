using WWPlatform.Core.Utilities;
using System.ComponentModel.DataAnnotations;

namespace WWPlatform.Core.Model
{
    //table splitting: split database table into 2 parts:key+columns
    public abstract class PersistObject
    {
        /// <summary>
        /// 属性的set方法必须设置为protected/public/internal之一，而不能设置为private
        /// </summary>
        public virtual int Idkey { get; protected set; }

        /// <summary>
        /// 将IdKey转化为base64字符串
        /// </summary>
        public string Base64Key
        {
            get
            {
                return Base64.Encode(this.Idkey.ToString());
            }
        }

        #region 重写object的7大基础方法中的2个
        public override int GetHashCode()
        {
            //对于相同的Idkey,认为是同一个Catalog对象
            return Idkey.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            { return false; }

            PersistObject po = obj as PersistObject;
            if (po == null)
            { return false; }

            return po.Idkey == this.Idkey;
        }
        #endregion
    }
}