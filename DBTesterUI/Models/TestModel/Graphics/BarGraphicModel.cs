using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Timers;
using DBTesterLib.Db;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DBTesterUI.Models.TestModel.Graphics
{
    class BarGraphicModel : PlotModel, IGraphicModel
    {
        private CategoryAxis MachinesAxis { get; set; }
        private Axis DurationAxis { get; set; }

        public DbTestItem TestItem { get; set; }

        private Random _rand;


        public BarGraphicModel(DbTestItem test)
        {
            _rand = new Random();
            TestItem = test;
            PlotType = PlotType.XY;
            PlotAreaBorderColor = OxyColor.FromRgb(160, 160, 160);

            // Ось количества машин
            MachinesAxis = new CategoryAxis
            {
                Title = "Машин",
                Position = AxisPosition.Bottom,
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
                var series = new ColumnSeries()
                {
                    Title = item.Db.Name,
                };

                if (!ColorIsBusy(color))
                {
                    series.FillColor = color;
                }

                Series.Add(series);
            });

            foreach (var t in test.DbShardGroups)
            {
                MachinesAxis.Labels.Add(t.MachinesCount.ToString());
            }

            Axes.Add(MachinesAxis);
            Axes.Add(DurationAxis);

            Update();

            var timer = new Timer(100);
            timer.Elapsed += (sender, args) => { Update(); };
            timer.Start();
        }

        public void Update()
        {
            lock (this)
            {
                Title = TestItem.Name;
                double maxDuration = 0;
                for (int groupIndex = 0; groupIndex < TestItem.Testers.GetLength(0); groupIndex++)
                {
                    for (int dbIndex = 0; dbIndex < TestItem.Testers.GetLength(1); dbIndex++)
                    {
                        var tester = TestItem.Testers[groupIndex, dbIndex];
                        ColumnSeries series = Series[dbIndex] as ColumnSeries;
                        if (groupIndex == 0)
                        {
                            series?.Items.Clear();
                        }

                        double duration = tester?.Duration.TotalSeconds ?? 0;
                        maxDuration = duration > maxDuration ? duration : maxDuration;

                        series?.Items.Add(new ColumnItem(duration));
                    }
                }

                DurationAxis.Maximum = Math.Ceiling(maxDuration / 5) * 5;
            }
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
            return Series.Any(series => ((ColumnSeries) series).FillColor.Equals(color));
        }
    }
}