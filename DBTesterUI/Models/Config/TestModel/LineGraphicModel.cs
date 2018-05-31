using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using DBTesterLib.Db;
using DBTesterLib.Tester;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DBTesterUI.Models.Config.TestModel
{
    class LineGraphicModel : PlotModel, IGraphicModel
    {
        private Axis MachinesAxis { get; set; }
        private Axis DurationAxis { get; set; }

        public DbTestItem TestItem { get; set; }

        private Random _rand;


        public LineGraphicModel(DbTestItem test)
        {
            _rand = new Random();
            TestItem = test;
            PlotType = PlotType.XY;
            PlotAreaBorderColor = OxyColor.FromRgb(160, 160, 160);

            // Ось количества машин
            MachinesAxis = new LinearAxis
            {
                Title = "Машин",
                Position = AxisPosition.Bottom,
                Minimum = 0.8,
                Maximum = test.Testers.GetLength(0) + 0.8,
                MajorStep = 1,
                MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                MajorGridlineStyle = LineStyle.Solid,
                TicklineColor = OxyColor.FromRgb(160, 160, 160)
            };

            // Ось времени
            DurationAxis = new LinearAxis
            {
                Title = "Время(сек)",
                Position = AxisPosition.Left,
                Minimum = 0,
                Maximum = 10,
                MaximumPadding = 0.5,
                TicklineColor = OxyColor.FromRgb(160, 160, 160)
            };

            test.DbShardGroups[0].ShardGroupItems.ForEach(item =>
            {
                var color = GetColor(item.Db);
                var series = new LineSeries
                {
                    Title = item.Db.Name,
                    MarkerType = MarkerType.Circle,
                    CanTrackerInterpolatePoints = false,
                    Smooth = true,
                };

                if (!ColorIsBusy(color))
                {
                    series.Color = color;
                    series.MarkerFill = color;
                }

                Series.Add(series);
            });

            Axes.Add(MachinesAxis);
            Axes.Add(DurationAxis);

            Update();
        }

        public void Update()
        {
            Title = TestItem.Name;
            double maxDuration = 0;
            for (int groupIndex = 0; groupIndex < TestItem.Testers.GetLength(0); groupIndex++)
            {
                for (int dbIndex = 0; dbIndex < TestItem.Testers.GetLength(1); dbIndex++)
                {
                    var tester = TestItem.Testers[groupIndex, dbIndex];
                    LineSeries series = Series[dbIndex] as LineSeries;
                    if (groupIndex == 0)
                    {
                        series?.Points.Clear();
                    }

                    double duration = tester?.Duration.TotalSeconds ?? 0;
                    maxDuration = duration > maxDuration ? duration : maxDuration;

                    series?.Points.Add(new DataPoint(groupIndex + 1, duration));
                }
            }

            DurationAxis.Maximum = Math.Ceiling(maxDuration / 10) * 10;
        }

        private OxyColor GetColor(IDb db)
        {
            string name = db.Name;
            if (Regex.IsMatch(name, "mongo", RegexOptions.IgnoreCase))
                return OxyColor.FromRgb(116, 189, 76);
            if (Regex.IsMatch(name, "mysql", RegexOptions.IgnoreCase))
                return OxyColor.FromRgb(68, 121, 161);

            var randomColors = new[]
            {
                OxyColor.Parse("#009688"),
                OxyColor.Parse("#3f51b5"),
                OxyColor.Parse("#607d8b"),
                OxyColor.Parse("#ff9800"),
            };

            return randomColors[_rand.Next(randomColors.Length)];
        }

        private bool ColorIsBusy(OxyColor color)
        {
            return Series.Any(series => ((LineSeries) series).Color.Equals(color));
        }
    }
}