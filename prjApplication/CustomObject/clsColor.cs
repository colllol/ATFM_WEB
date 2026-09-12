using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjApplication
{
    public static class EnumExtensions
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value)
            where TAttribute : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            return type.GetField(name) // I prefer to get attributes this way
                .GetCustomAttributes(false)
                .OfType<TAttribute>()
                .SingleOrDefault();
        }
    }
    public class ColorInfoAttribute : Attribute
    {
        internal ColorInfoAttribute(string _color)
        {
            this.sColor = _color;
        }
        public string sColor { get; private set; }
    }
    public enum clsColor
    {
        [ColorInfo("cssTrChuaThucHien")]cssTrChuaThucHien,
        [ColorInfo("cssTrChuaThucHienCustom")] cssTrChuaThucHienCustom,
        [ColorInfo("cssTrDaCoKeHoachBay")] cssTrDaCoKeHoachBay,
        [ColorInfo("cssTrDaCoKeHoachBayCustom")] cssTrDaCoKeHoachBayCustom,
        [ColorInfo("cssTrDienVanCatCanh")] cssTrDienVanCatCanh,
        [ColorInfo("cssTrDienVanCatCanhCustom")] cssTrDienVanCatCanhCustom,
        [ColorInfo("cssTrDienVanHaCanh")] cssTrDienVanHaCanh,
        [ColorInfo("cssTrDienVanHaCanhCustom")] cssTrDienVanHaCanhCustom,
        [ColorInfo("cssTrHuy")] cssTrHuy,
        [ColorInfo("cssTrHuyCustom")] cssTrHuyCustom,
        [ColorInfo("cssTrThayDoi")] cssTrThayDoi,
        [ColorInfo("cssTrThayDoiCustom")] cssTrThayDoiCustom,
        [ColorInfo("cssTrHoanBay")] cssTrHoanBay,
        [ColorInfo("cssTrHoanBayCustom")] cssTrHoanBayCustom,
        [ColorInfo("cssTrChange")] cssTrChange,
        [ColorInfo("cssTrKhongCoPhep")] cssTrKhongCoPhep,
        [ColorInfo("cssTrKhongCoPhepCustom")] cssTrKhongCoPhepCustom,
        [ColorInfo("cssTdCallSign")] cssTdCallSign,
        [ColorInfo("cssTdCallSignCustom")] cssTdCallSignCustom,
        [ColorInfo("cssTdColChange")] cssTdColChange,
        [ColorInfo("cssTdColChangeCustom")] cssTdColChangeCustom,
        [ColorInfo("cssTextColChange")] cssTextColChange,
        [ColorInfo("cssTextColChangeCustom")] cssTextColChangeCustom,
        [ColorInfo("cssTextBaySom")] cssTextBaySom,
        [ColorInfo("cssTdBaySom")] cssTdBaySom,
    }
    public static class ColorHelper
    {
        public static string GetColorCode(this clsColor p)
        {
            var attr = p.GetAttribute<ColorInfoAttribute>();
            return attr.sColor;
        }
    }
}