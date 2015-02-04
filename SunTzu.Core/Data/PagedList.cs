using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SunTzu.Core.Data
{
    /// <summary>
    /// 分页接口
    /// </summary>
    public interface IPagedList
    {
        int TotalCount
        {
            get;
            set;
        }

        int PageIndex
        {
            get;
            set;
        }

        int PageSize
        {
            get;
            set;
        }

        int PageCount
        {
            get;
        }

        bool HasPreviousPage
        {
            get;
        }

        bool HasNextPage
        {
            get;
        }
    }

    /// <summary>
    /// 分页列表类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedList<T> : List<T>, IPagedList
    {
        public PagedList(IQueryable<T> source, int index, int pageSize)
        {
            try
            {
                if (source != null)
                {
                    this.TotalCount = source.Count();
                    this.PageSize = pageSize != 0 ? pageSize : 20;
                    this.PageIndex = index;
                    this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
                }
            }
            catch
            {
                throw;
            }
        }


        public PagedList(IEnumerable<T> source, int index, int pageSize)
        {
            try
            {
                if (source != null)
                {
                    this.TotalCount = source.Count();
                    this.PageSize = pageSize != 0 ? pageSize : 20;
                    this.PageIndex = index;
                    this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
                }
            }
            catch
            {
            }
        }

        public PagedList(List<T> source, int index, int pageSize)
        {
            try
            {
                if (source != null)
                {
                    this.TotalCount = source.Count();
                    this.PageSize = pageSize != 0 ? pageSize : 20;
                    this.PageIndex = index;
                    this.AddRange(source.Skip(index * pageSize).Take(pageSize).ToList());
                }
            }
            catch
            {
            }
        }

        public int TotalCount
        {
            get;
            set;
        }

        public int PageIndex
        {
            get;
            set;
        }

        public int PageSize
        {
            get;
            set;
        }

        public int PageCount
        {
            get
            {
                return TotalCount < 1 ? 0 : (TotalCount - 1) / PageSize;
            }
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 0);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return ((PageIndex + 1) * PageSize) < TotalCount;
            }
        }
    }

    /// <summary>
    /// 扩展方法：IQueryable.ToPagedList(int );
    /// </summary>
    public static class Pagination
    {
        /// <summary>
        /// 扩展方法，返回分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index">页号</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index, int pageSize)
        {
            if (pageSize == 0)
                pageSize = 20;
            return new PagedList<T>(source, index, pageSize);
        }

        /// <summary>
        /// 扩展方法，返回分页列表
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index">页号</param>
        /// <param name="pageSize">每页大小</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index, int pageSize)
        {
            if (pageSize == 0)
                pageSize = 20;
            return new PagedList<T>(source, index, pageSize);
        }
        /// <summary>
        /// 扩展方法，返回分页列表，默认每页20个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index">页号</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> source, int index)
        {
            return new PagedList<T>(source, index, 20);
        }

        /// <summary>
        /// 扩展方法，返回分页列表，默认每页20个数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="index">页号</param>
        /// <returns></returns>
        public static PagedList<T> ToPagedList<T>(this IEnumerable<T> source, int index)
        {
            return new PagedList<T>(source, index, 20);
        }
    }
}
