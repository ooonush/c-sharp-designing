using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delegates.Reports
{
	public abstract class ReportMaker
	{
		protected abstract string MakeCaption(string caption);
		protected abstract string BeginList();
		protected abstract string MakeItem(string valueType, string entry);
		protected abstract string EndList();
		public string MakeReport<T>(IEnumerable<Measurement> measurements, IStatisticMaker<T> statisticMaker)
		{
			var data = measurements.ToList();
			var result = new StringBuilder();
			result.Append(MakeCaption(statisticMaker.Caption));
			result.Append(BeginList());
			result.Append(MakeItem(
				"Temperature", 
				statisticMaker.MakeStatistics(data.Select(z => z.Temperature)).ToString()));
			result.Append(MakeItem(
				"Humidity", 
				statisticMaker.MakeStatistics(data.Select(z => z.Humidity)).ToString()));
			result.Append(EndList());
			return result.ToString();
		}
	}

	public class HtmlReportMaker : ReportMaker
	{
		protected override string MakeCaption(string caption)
		{
			return $"<h1>{caption}</h1>";
		}

		protected override string BeginList()
		{
			return "<ul>";
		}

		protected override string EndList()
		{
			return "</ul>";
		}

		protected override string MakeItem(string valueType, string entry)
		{
			return $"<li><b>{valueType}</b>: {entry}";
		}
	}

	public class MarkdownReportMaker : ReportMaker
	{
		protected override string BeginList()
		{
			return "";
		}

		protected override string EndList()
		{
			return "";
		}

		protected override string MakeCaption(string caption)
		{
			return $"## {caption}\n\n";
		}

		protected override string MakeItem(string valueType, string entry)
		{
			return $" * **{valueType}**: {entry}\n\n";
		}
	}

	public static class ReportMakerHelper
	{
		public static string MeanAndStdHtmlReport(IEnumerable<Measurement> measurements)
		{
			return new HtmlReportMaker().MakeReport(measurements, new MeanAndStdStatisticMaker());
		}

		public static string MedianMarkdownReport(IEnumerable<Measurement> measurements)
		{
			return new MarkdownReportMaker().MakeReport(measurements, new MedianStatisticMaker());
		}

		public static string MeanAndStdMarkdownReport(IEnumerable<Measurement> measurements)
		{
			return new MarkdownReportMaker().MakeReport(measurements, new MeanAndStdStatisticMaker());
		}

		public static string MedianHtmlReport(IEnumerable<Measurement> measurements)
		{
			return new HtmlReportMaker().MakeReport(measurements, new MedianStatisticMaker());
		}
	}

	public interface IStatisticMaker<out T>
	{
		string Caption { get; }
		T MakeStatistics(IEnumerable<double> data);
	}

	public class MeanAndStdStatisticMaker : IStatisticMaker<MeanAndStd>
	{
		public string Caption => "Mean and Std";

		public MeanAndStd MakeStatistics(IEnumerable<double> data)
		{
			var dataList = data.ToList();
			double mean = dataList.Average();
			double std = Math.Sqrt(dataList.Select(z => Math.Pow(z - mean, 2)).Sum() / (dataList.Count - 1));

			return new MeanAndStd
			{
				Mean = mean,
				Std = std
			};
		}
	}

	public class MedianStatisticMaker : IStatisticMaker<double>
	{
		public string Caption => "Median";

		public double MakeStatistics(IEnumerable<double> data)
		{
			var list = data.OrderBy(z => z).ToList();
			if (list.Count % 2 == 0)
			{
				return (list[list.Count / 2] + list[list.Count / 2 - 1]) / 2;
			}
			
			return list[list.Count / 2];
		}
	}
}