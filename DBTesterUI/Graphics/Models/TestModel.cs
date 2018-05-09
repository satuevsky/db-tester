using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace DBTesterUI.Graphics.Models
{
    class TestModel
    {
        public TestModel()
        {
            this.MyModel = new PlotModel
            {
                Title = "Вставка данных",
                Subtitle = "Вставка 50.000 случайных записей",
                PlotType = PlotType.XY,
                PlotAreaBorderColor = OxyColor.FromRgb(160, 160, 160)
            };


            this.MyModel.Axes.Add(new LinearAxis
            {
                Title = "Машин",
                Position = AxisPosition.Bottom,
                Minimum = 0.8,
                Maximum = 3.2,
                MajorStep = 1,
                MajorGridlineColor = OxyColor.FromRgb(220, 220, 220),
                MajorGridlineStyle = LineStyle.Solid,
                TicklineColor = OxyColor.FromRgb(160, 160, 160)
            });
            this.MyModel.Axes.Add(new LinearAxis
            {
                Title = "Время(сек)",
                Position = AxisPosition.Left,
                Minimum = 0,
                MaximumPadding = 0.5,
                TicklineColor = OxyColor.FromRgb(160, 160, 160)
            });


            var series1 = new LineSeries
            {
                Title = "MongoDB",
                MarkerType = MarkerType.Circle,
                Color = OxyColor.FromRgb(116, 189, 76),
                MarkerFill = OxyColor.FromRgb(116, 189, 76),
            };
            series1.Points.Add(new DataPoint(1, 30));
            series1.Points.Add(new DataPoint(2, 15));
            series1.Points.Add(new DataPoint(3, 5));

            var series2 = new LineSeries
            {
                Title = "MySQL",
                MarkerType = MarkerType.Circle,

                Color = OxyColor.FromRgb(68, 121, 161),
                //MarkerSize = 2,
                MarkerFill = OxyColor.FromRgb(68, 121, 161),
                //MarkerStroke = OxyColor.FromRgb(255,0,0),
                //MarkerStrokeThickness = 3
            };
            series2.Points.Add(new DataPoint(1, 27));
            series2.Points.Add(new DataPoint(2, 17));
            series2.Points.Add(new DataPoint(3, 9));

            var point = new PointAnnotation()
            {
                Fill = OxyColor.FromRgb(255, 0, 0),
                Size = 3,
                X = 3,
                Y = 5,
            };

            this.MyModel.Annotations.Add(point);
            this.MyModel.Series.Add(series1);
            this.MyModel.Series.Add(series2);
        }

        public PlotModel MyModel { get; private set; }
    }
}