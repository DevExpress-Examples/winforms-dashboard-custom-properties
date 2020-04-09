using System;
using System.Collections.Generic;
using System.Linq;
using DevExpress.DashboardCommon;

namespace WindowsFormsAppCustomProperties {
    public static class CustomPropertyExtensions {
        public static T GetValue<T>(this CustomProperties property, string name) where T : struct {
            var value = property.GetValue(name);
            if(value == null) return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }
        public static EnumType ConvertToEnum<EnumType>(this String enumValue) {
            return (EnumType)Enum.Parse(typeof(EnumType), enumValue);
        }
        public static string ConvertToString(this Enum enumValue) {
            return Enum.GetName(enumValue.GetType(), enumValue);
        }
        public static List<ChartSeries> GetAllSeries(this ChartDashboardItem chartDashboardItem) {
            var allSeriesNames = chartDashboardItem.Panes.SelectMany(p => p.Series.ToList()).ToList();
            return allSeriesNames;
        }
        public static string GetUniqueSeriesID(this ChartSeries chartSeries) {
            if(chartSeries is SimpleSeries) {
                return ((SimpleSeries)chartSeries).Value.UniqueId;
            }
            if(chartSeries is RangeSeries) {
                return ((RangeSeries)chartSeries).Value1.UniqueId;
            }
            if(chartSeries is WeightedSeries) {
                return ((WeightedSeries)chartSeries).Value.UniqueId;
            }
            if(chartSeries is OpenHighLowCloseSeries) {
                return ((OpenHighLowCloseSeries)chartSeries).Low.UniqueId;
            }
            if(chartSeries is HighLowCloseSeries) {
                return ((HighLowCloseSeries)chartSeries).Low.UniqueId;
            }
            return null;
        }
        public static List<ChartSeries> GetRelatedSeries(this ChartDashboardItem chartItem, string propertyName) {
            return chartItem.GetAllSeries().Where(s => s.CustomProperties.GetValue(propertyName) != null).ToList();
        }
    }
}
