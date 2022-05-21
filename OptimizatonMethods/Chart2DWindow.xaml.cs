using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using ChartDirector;

using OptimizatonMethods.Models;


namespace OptimizatonMethods
{
    /// <summary>
    ///     Логика взаимодействия для Chart2DWindow.xaml
    /// </summary>
    public partial class Chart2DWindow : Window
    {
        private readonly List<Point3D> _dataList = new();
        private readonly Task _task;
        private readonly MathModelTest mathModel;
        private ContourLayer contourLayer;


        public Chart2DWindow(MathModelTest mathModel)
        {
            this.mathModel = mathModel;
            InitializeComponent();
        }

        private void drawChart(WPFChartViewer viewer)
        {
            var dataX = new List<double>();
            var dataY = new List<double>();
            var step = 1;

            for (double i = mathModel.p1.min-step; i < mathModel.p1.max+step; i+=step)
            {
                dataX.Add(i);
            }
            for (double i = mathModel.p2.min - step; i < mathModel.p2.max + step; i += step)
            {
                dataY.Add(i);
            }
            var dataZ = new List<double>();

            for (int i = 0; i < dataX.Count; i++)
            {
                for (int j = 0; j < dataY.Count; j++)
                {
                    dataZ.Add(0);
                }
            }
            var k = 0;

            for (int i = 0; i < dataX.Count; i++)
            {
                for (int j = 0; j < dataY.Count; j++)
                {
                    mathModel.p1.parameter.Value = dataX[i];
                    mathModel.p2.parameter.Value = dataY[j];
                    dataZ[j*dataX.Count+i]= mathModel.Function();
                }
            }


            // Create a XYChart object of size 575 x 525 pixels
            var c = new XYChart(575, 525);

            // Set the plotarea at (75, 30) and of size 450 x 450 pixels. Use semi-transparent black
            // (80000000) dotted lines for both horizontal and vertical grid lines
            var p = c.setPlotArea(75, 30, 450, 450, -1, -1, -1, c.dashLineColor(unchecked((int) 0xaf000000), Chart.DotLine), -1);

            // Set the chart and axis titles
            c.addTitle("     <*block,bgcolor=FFFF00*> *** Drag Crosshair to Move Cross Section *** <*/*>",
                       "Arial Bold", 15);

            c.xAxis().setTitle(mathModel.p1.parameter.ToString(), "Arial Bold Italic", 10);
            c.yAxis().setTitle(mathModel.p2.parameter.ToString(), "Arial Bold Italic", 10);

            // Put the y-axis on the right side of the chart
            c.setYAxisOnRight();

            // Set x-axis and y-axis labels to use Arial Bold font
            c.xAxis().setLabelStyle("Arial", 10);
            c.yAxis().setLabelStyle("Arial", 10);

            // When auto-scaling, use tick spacing of 40 pixels as a guideline
            c.xAxis().setLinearScale(dataX.Min(), dataX.Max(), 1);
            c.yAxis().setLinearScale(dataY.Min(), dataY.Max(), 1);

            // Add a contour layer using the given data
            contourLayer = c.addContourLayer(dataX.ToArray(), dataY.ToArray(), dataZ.ToArray());
            contourLayer.setContourLabelFormat("<*font=Arial Bold,size=10*>{value}<*/font*>");

            contourLayer.setZBounds(0);

            // Move the grid lines in front of the contour layer
            c.getPlotArea().moveGridBefore(contourLayer);

            // Add a vertical color axis at x = 0 at the same y-position as the plot area.
            var cAxis = contourLayer.setColorAxis(0, p.getTopY(), Chart.TopLeft,
                                                  p.getHeight(), Chart.Right);

            // Use continuous gradient coloring (as opposed to step colors)
            cAxis.setColorGradient(true);

            // Add a title to the color axis using 12 points Arial Bold Italic font
            cAxis.setTitle("Color Legend Title Place Holder", "Arial Bold Italic", 10);

            // Set color axis labels to use Arial Bold font
            cAxis.setLabelStyle("Arial", 10);

            // Set the chart image to the WinChartViewer
            viewer.Chart = c;

            // Tooltip for the contour chart
            viewer.ImageMap = c.getHTMLImageMap("", "",
                                                "title='<*cdml*><*font=Arial Bold*>X={x|2}<*br*>Y={y|2}<*br*>Z={z|2}'");

            // Initializse the crosshair position to the center of the chart


            // Draw the cross section and crosshair
        }


        private void Chart2DWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            drawChart(WPFChartViewer1);

            // Extended the plot area mouse event region to make it easier to drag the crosshair
            //WPFChartViewer1.setPlotAreaMouseMargin(100);
        }
    }
}
