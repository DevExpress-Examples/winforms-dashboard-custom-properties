Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports DevExpress.DashboardCommon

Namespace WindowsFormsAppCustomProperties
	Public Module CustomPropertyExtensions
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetValue(Of T As Structure)(ByVal [property] As CustomProperties, ByVal name As String) As T
			Dim value = [property].GetValue(name)
			If value Is Nothing Then
				Return CType(Nothing, T)
			End If
			Return DirectCast(Convert.ChangeType(value, GetType(T)), T)
		End Function
		<System.Runtime.CompilerServices.Extension> _
		Public Function ConvertToEnum(Of EnumType)(ByVal enumValue As String) As EnumType
			Return DirectCast(System.Enum.Parse(GetType(EnumType), enumValue), EnumType)
		End Function
		<System.Runtime.CompilerServices.Extension> _
		Public Function ConvertToString(ByVal enumValue As System.Enum) As String
			Return System.Enum.GetName(enumValue.GetType(), enumValue)
		End Function
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetAllSeries(ByVal chartDashboardItem As ChartDashboardItem) As List(Of ChartSeries)
			Dim allSeriesNames = chartDashboardItem.Panes.SelectMany(Function(p) p.Series.ToList()).ToList()
			Return allSeriesNames
		End Function
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetUniqueSeriesID(ByVal chartSeries As ChartSeries) As String
			If TypeOf chartSeries Is SimpleSeries Then
				Return CType(chartSeries, SimpleSeries).Value.UniqueId
			End If
			If TypeOf chartSeries Is RangeSeries Then
				Return CType(chartSeries, RangeSeries).Value1.UniqueId
			End If
			If TypeOf chartSeries Is WeightedSeries Then
				Return CType(chartSeries, WeightedSeries).Value.UniqueId
			End If
			If TypeOf chartSeries Is OpenHighLowCloseSeries Then
				Return CType(chartSeries, OpenHighLowCloseSeries).Low.UniqueId
			End If
			If TypeOf chartSeries Is HighLowCloseSeries Then
				Return CType(chartSeries, HighLowCloseSeries).Low.UniqueId
			End If
			Return Nothing
		End Function
		<System.Runtime.CompilerServices.Extension> _
		Public Function GetRelatedSeries(ByVal chartItem As ChartDashboardItem, ByVal propertyName As String) As List(Of ChartSeries)
			Return chartItem.GetAllSeries().Where(Function(s) s.CustomProperties.GetValue(propertyName) IsNot Nothing).ToList()
		End Function
	End Module
End Namespace
