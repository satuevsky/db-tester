using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OxyPlot;
using OxyPlot.Axes;

namespace DBTesterUI.Models.Config.TestModel
{
    class GraphicModel : PlotModel
    {
        private Axis MachinesAxis { get; set; }
        private Axis DurationAxis { get; set; }

        public GraphicModel(DbTestItem test)
        {
            PlotType = PlotType.XY;
            PlotAreaBorderColor = OxyColor.FromRgb(160, 160, 160);

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

            DurationAxis = new LinearAxis
            {
                Title = "Время(сек)",
                Position = AxisPosition.Left,
                Minimum = 0,
                MaximumPadding = 0.5,
                TicklineColor = OxyColor.FromRgb(160, 160, 160)
            };

            Axes.Add(MachinesAxis);
            Axes.Add(DurationAxis);
        }
    }
}