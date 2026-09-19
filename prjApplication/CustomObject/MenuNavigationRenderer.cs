using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using prjComponents;

namespace prjApplication
{
    internal static class MenuNavigationRenderer
    {
        private sealed class MenuItem
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Icon { get; set; }
            public string Url { get; set; }
            public List<MenuItem> Children { get; set; }
        }

        public static string Render(DataTable rows, string applicationPath, int requestedMenuID)
        {
            List<MenuItem> roots = BuildTree(rows);
            StringBuilder html = new StringBuilder();
            int activeParentID = FindActiveParentID(roots, requestedMenuID);

            for (int i = 0; i < roots.Count; i++)
            {
                MenuItem parent = roots[i];
                bool isActive = activeParentID > 0 ? parent.ID == activeParentID : i == 0;
                html.Append(isActive ? "<li class=\"active open hover\">" : "<li class=\"hover\">");
                html.Append("<a href=\"#\" class=\"dropdown-toggle\" data-toggle=\"dropdown\">");

                if (!string.IsNullOrWhiteSpace(parent.Icon))
                {
                    html.Append("<i class=\"menu-icon\"><img src=\"")
                        .Append(HttpUtility.HtmlAttributeEncode(applicationPath + parent.Icon))
                        .Append("\" style=\"width:28px; height:28px;\" /></i>");
                }
                else
                {
                    html.Append("<i class=\"menu-icon fa fa-list-alt\"></i>");
                }

                html.Append("<span class=\"menu-text\">")
                    .Append(HttpUtility.HtmlEncode(parent.Name))
                    .Append("</span><b class=\"arrow fa fa-angle-down\"></b></a>");

                if (parent.Children.Count > 0)
                {
                    html.Append("<b class=\"arrow\"></b><ul class=\"submenu\">");
                    foreach (MenuItem child in parent.Children)
                    {
                        string childUrl = child.Url ?? string.Empty;
                        string separator = childUrl.Contains("?") ? "&" : "?";
                        html.Append("<li class=\"hover\"><a href=\"")
                            .Append(HttpUtility.HtmlAttributeEncode(
                                applicationPath + "/" + childUrl + separator + "Menu_ID=" + child.ID))
                            .Append("\"><i class=\"menu-icon fa fa-caret-right\"></i>")
                            .Append(HttpUtility.HtmlEncode(child.Name))
                            .Append("</a><b class=\"arrow\"></b></li>");
                    }
                    html.Append("</ul>");
                }

                html.Append("</li>");
            }

            return html.ToString();
        }

        public static string RenderTitle(DataTable rows, int menuID)
        {
            string menuName;
            string parentName;
            if (!MenuCache.TryGetMenuNames(rows, menuID, out menuName, out parentName))
                return string.Empty;

            return "&nbsp;<h1 style=\"font-family: 'Tahoma';\">"
                + HttpUtility.HtmlEncode(parentName)
                + "<small><i class=\"ace-icon fa fa-angle-double-right\"></i>"
                + HttpUtility.HtmlEncode(menuName)
                + "</small><h1>";
        }

        private static List<MenuItem> BuildTree(DataTable rows)
        {
            List<MenuItem> roots = new List<MenuItem>();
            Dictionary<int, MenuItem> items = new Dictionary<int, MenuItem>();
            if (rows == null)
                return roots;

            foreach (DataRow row in rows.Rows)
            {
                MenuItem item = new MenuItem
                {
                    ID = CommonLib.CheckNullInt(row["ID"]),
                    Name = CommonLib.CheckNullStr(row["MENUNAME"]),
                    Icon = CommonLib.CheckNullStr(row["MENUICON"]),
                    Url = CommonLib.CheckNullStr(row["MENUURL"]),
                    Children = new List<MenuItem>()
                };
                items[item.ID] = item;
            }

            foreach (DataRow row in rows.Rows)
            {
                int id = CommonLib.CheckNullInt(row["ID"]);
                int parentID = CommonLib.CheckNullInt(row["PARRENTID"]);
                MenuItem item;
                if (!items.TryGetValue(id, out item))
                    continue;

                MenuItem parent;
                if (parentID > 0 && items.TryGetValue(parentID, out parent))
                    parent.Children.Add(item);
                else if (parentID == 0)
                    roots.Add(item);
            }

            return roots;
        }

        private static int FindActiveParentID(List<MenuItem> roots, int requestedMenuID)
        {
            if (requestedMenuID <= 0)
                return 0;

            foreach (MenuItem parent in roots)
            {
                if (parent.ID == requestedMenuID
                    || parent.Children.Exists(child => child.ID == requestedMenuID))
                    return parent.ID;
            }

            return 0;
        }
    }
}
