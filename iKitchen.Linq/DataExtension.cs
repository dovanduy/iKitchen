using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunTzu.Core.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;

namespace iKitchen.Linq
{
    /// <summary>
    /// 数据对象扩展
    /// </summary>
    public static class DataExtension
    {
        /// <summary>
        /// 保存或更新
        /// </summary>
        /// <param name="data">要保存或更新的数据</param>
        /// <returns>操作结果</returns>
        public static bool SaveOrUpdate(this IEntity data)
        {
            bool success = false;
            if (data != null)
            {
                var db = DataContextManager.GetContext();

                if (data.Id < 1)
                {
                    if (data.CreateOn == DateTime.MinValue)
                        data.CreateOn = DateTime.Now;
                    if (data.UpdateOn == DateTime.MinValue)
                        data.UpdateOn = DateTime.Now;
                    db.Set(data.GetType()).Add(data);
                }
                else
                {
                    data.UpdateOn = DateTime.Now;
                    db.Entry(data).State = EntityState.Modified;
                }

                try
                {
                    db.SaveChanges();
                    success = true;
                }
                catch (Exception e)
                {
                    success = false;
                }
            }
            return success;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="data">要删除的数据</param>
        /// <param name="isLogicDelete">是否逻辑删除</param>
        /// <returns>操作结果</returns>
        public static bool Delete(this IEntity data, bool isLogicDelete)
        {
            bool success = false;
            var db = DataContextManager.GetContext();
            if (isLogicDelete)
            {
                db.Set(data.GetType()).Remove(data);
            }
            else
            {
                data.State = -1; // state = -1 时，逻辑删除
            }
            try
            {
                db.SaveChanges();
                success = true;
            }
            catch (Exception)
            {
                success = false;
            }
            return success;
        }

        /// <summary>
        /// 逻辑删除数据
        /// </summary>
        /// <param name="data">要删除的数据</param>
        /// <returns>操作结果</returns>
        public static bool Delete(this IEntity data)
        {
            return Delete(data, true);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="dataList">要删除的数据</param>
        /// <param name="isLogicDelete">是否逻辑删除</param>
        /// <returns>操作结果</returns>
        public static bool Delete(this List<IEntity> dataList, bool isLogicDelete)
        {
            bool success = false;
            if (dataList != null && dataList.Count > 0)
            {
                var db = DataContextManager.GetContext();
                if (isLogicDelete)
                {
                    dataList.ForEach(c => c.State = -1);
                }
                else
                {
                    var dbSet = db.Set(dataList[0].GetType());
                    dataList.ForEach(c => dbSet.Remove(c));
                }
                try
                {
                    db.SaveChanges();
                    success = true;
                }
                catch (Exception)
                {
                    success = false;
                }
            }
            return success;
        }

        /// <summary>
        /// 逻辑删除数据
        /// </summary>
        /// <param name="dataList">要删除的数据</param>
        /// <returns>操作结果</returns>
        public static bool Delete(this List<IEntity> dataList)
        {
            return Delete(dataList, true);
        }

        public static int ExecuteCommand(this DbContext db, string sql, params object[] parameters)
        {
            return db.Database.ExecuteSqlCommand(sql, parameters);
        }
    }
}
