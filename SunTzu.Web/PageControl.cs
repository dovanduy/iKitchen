using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SunTzu.Core.Data;

namespace SunTzu.Web
{
    public class PageControl
    {
        /// <summary>
        /// 是否需要分页
        /// </summary>
        public bool NeedPage(IPagedList PageList)
        {
            if (PageList == null)
                return false;
            else
                return PageList.PageCount > 0;
        }

        [Obsolete]
        public static string Render(IPagedList pageList)
        {
            StringBuilder sb = new StringBuilder();
            //判断是否存在数据
            if (pageList != null && pageList.TotalCount > 0)
            {
                #region 生成分页代码
                /*
	<div id="pager">
		<div class="left">
			<div>显示</div>
			<div>
				<select id="numPerPage">
					<option value="20" <%= ModelList.PageSize == 20 ? "selected='selected'" : "" %>>20</option>
					<option value="50" <%= ModelList.PageSize == 50 ? "selected='selected'" : "" %>>50</option>
					<option value="100" <%= ModelList.PageSize == 100 ? "selected='selected'" : "" %>>100</option>
					<option value="200" <%= ModelList.PageSize == 200 ? "selected='selected'" : "" %>>200</option>
				</select>
			</div>
			<div>条，共<%= ModelList.TotalCount %>条</div>
		</div>
		<div class="right">
			<span>
				<ul>
					<li class="first"><a href="#">首页</a></li>
					<li class="prev"><a href="#">上一页</a></li>
					<li><a href="#" class="active">1</a></li>
					<li><a href="#">2</a></li>
					<li><a href="#">3</a></li>
					<li><a href="#">4</a></li>
					<li><a href="#">5</a></li>
					<li class="next"><a href="#">下一页</a></li>
					<li class="last"><a href="#">末页</a></li>
				</ul>
			</span>
			<div class="divPageNum"><input type="text" id="gotoPage" class="pageNum" maxlength="4" /><input type="button" class="btnGotoPage" value="GO" /></div>
		</div>
	</div>	
                 * */
                sb.Append(@"
	<div id='pager'>
		<div class='left'>
			<div>显示</div>
			<div>
				<select id='numPerPage' class='numPerPage'>");
                if (pageList != null)
                {
                    sb.AppendFormat("<option value='20' {0}>20</option>", pageList.PageSize == 20 ? "selected='selected'" : "")
                        .AppendLine();
                    sb.AppendFormat("<option value='50' {0}>50</option>", pageList.PageSize == 50 ? "selected='selected'" : "")
                        .AppendLine();
                    sb.AppendFormat("<option value='100' {0}>100</option>", pageList.PageSize == 100 ? "selected='selected'" : "")
                        .AppendLine();
                    sb.AppendFormat("<option value='200' {0}>200</option>", pageList.PageSize == 200 ? "selected='selected'" : "")
                        .AppendLine();
                }
                sb.AppendFormat(@"
				</select>
			</div>
			<div>条，共{0}条</div>", pageList.TotalCount)
                    .AppendFormat("<div>{0}页</div>", pageList.PageCount+1)
                    .AppendLine(@"
        </div>
        <div class='right'>
            <span>
                <ul>");
                //存在上一页
                if (pageList.HasPreviousPage)
                {
                    sb.AppendFormat("<li class='first'><a href='{0}'>首页</a></li>", GetUrlString(pageList, 0))
                        .AppendLine();
                    sb.AppendFormat("<li class='prev'><a href='{0}'>上一页</a></li>", GetUrlString(pageList, pageList.PageIndex - 1))
                        .AppendLine();
                }
                else
                {
                    sb.AppendLine(@"
					<li class='first disable'><a>首页</a></li>
					<li class='prev disable'><a>上一页</a></li>");
                }
                //当前页超过3
                if (pageList.PageIndex - 3 > 0)
                {
                    sb.Append("<li>...</li> ");
                }
                for (int i = pageList.PageIndex - 3; i <= pageList.PageIndex + 3; i++)
                {
                    if (i <= pageList.PageCount && i >= 0)
                    {
                        if (i == pageList.PageIndex)
                        {
                            sb.Append("<li><a class='active'>" + (i + 1).ToString() + "</a></li>");//当前页
                        }
                        else
                        {
                            sb.Append("<li><a href='" + GetUrlString(pageList, i) + "'>" + (i + 1).ToString() + "</a></li>");
                        }
                    }
                }
                //当前页与最后一页相差超过3
                if (pageList.PageIndex + 3 < pageList.PageCount)
                {
                    sb.Append("<li>...</li> ");
                }
                if (pageList.HasNextPage)
                {
                    sb.AppendFormat("<li class='next'><a href='{0}'>下一页</a></li>", GetUrlString(pageList, pageList.PageIndex + 1))
                        .AppendLine();
                    sb.AppendFormat("<li class='last'><a href='{0}'>末页</a></li>", GetUrlString(pageList, pageList.PageCount))
                        .AppendLine();
                }
                else
                {
                    sb.AppendLine(@"
					<li class='next disable'><a>下一页</a></li>
					<li class='last disable'><a>末页</a></li>");

                }
                sb.AppendFormat(@"
				</ul>
			</span>
			<div class='divPageNum'><input type='text'id='gotoPage' class='pageNum' maxlength='4' value='{0}' /><input type='button' class='btnGotoPage' onclick='{1}' value='GO' /></div>
		</div>
	</div>",pageList.PageIndex+1, "goto_page(\"" + GetUrlRawString() + "\")");

                #endregion
            }
            else
            {
                sb.AppendLine("<div id='noMsg'><span>暂无相关资料</span></div>");
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取分页 url 字符串
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public static string GetUrlString(IPagedList pageList, int page)
        {
            if (page < 0)
                page = 0;

            if (page > pageList.PageCount)
                page = pageList.PageCount;

            StringBuilder sb = new StringBuilder();
            string uri = HttpContext.Current.Request.Url.AbsoluteUri;

            if (uri.Contains('?'))
            {
                // 已经有参数
                int index = uri.IndexOf("page=");
                if (index > 0)
                    sb.Append(uri.Replace("page=" + HttpContext.Current.Request["page"], "page=" + page.ToString()));   // 已有有分页参数存在，替换
                else
                {
                    sb.Append(uri);
                    sb.Append("&page=" + page.ToString());  // 没有分页参数存在，追加
                }
            }
            else
            {
                sb.Append("?page=" + page.ToString());
            }
            return sb.ToString();
        }
        /// <summary>
        /// 获取分页 url 字符串
        /// </summary>
        /// <returns></returns>
        public static string GetUrlRawString()
        {
            StringBuilder sb = new StringBuilder();
            string uri = HttpContext.Current.Request.Url.AbsoluteUri;

            if (uri.Contains('?'))
            {
                // 已经有参数
                int index = uri.IndexOf("page=");
                if (index > 0)
                    sb.Append(uri.Replace("page=" + HttpContext.Current.Request["page"], "page=###"));   // 已有有分页参数存在，替换
                else
                {
                    sb.Append(uri);
                    sb.Append("&page=###");  // 没有分页参数存在，追加
                }
            }
            else
            {
                sb.Append("?page=###");
            }
            return sb.ToString();
        }
    }
}
