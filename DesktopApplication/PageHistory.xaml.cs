﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Visifire.Charts;
using Visifire.Commons;
using System.Collections.ObjectModel;

namespace DesktopApplication
{
    /// <summary>
    /// Interaction logic for PageHistory.xaml
    /// </summary>
    public partial class PageHistory : Page
    {

        public static ObservableCollection<History> theWholehistorylist;
        public static ObservableCollection<History> datehistorylist;

        // Create a Chart element
        Chart chart = new Chart(); //static会报子元素错误
        
        //Random rand = new Random();

        //ObservableCollection<KeyValuePair<Double, Double>> values = new ObservableCollection<KeyValuePair<double, double>>();

        static ObservableCollection<string> updateTime;//具体日期
        static ObservableCollection<int> value1;//相应日期的操作次数
        static ObservableCollection<KeyValuePair<String, int>> chartCollection;

   

        public PageHistory()
        {
            InitializeComponent();
            theWholehistorylist = new ObservableCollection<History> { };
            datehistorylist = new ObservableCollection<History> { };
            updateTime = new ObservableCollection<string>();
            value1 = new ObservableCollection<int>();
            chartCollection = new ObservableCollection<KeyValuePair<String, int>>();

            this.mylist.ItemsSource = datehistorylist;

            if (MainWindow.isSigined)
            {
                ObservableCollection<History> historylist = Post.GetAllHistoryList();

                foreach (History item in historylist)
                {
                    PageHistory.theWholehistorylist.Add(item);
                }
            }

            //if (MainWindow.isSigined)
            //{
            //    //ObservableCollection<History> historylist = Post.GetAllHistoryList();
            //     History item = new History() { File_Path = "ddd1", Operate_Date = "2014-03-03" };
            //     History item2 = new History() { File_Path = "ddd2", Operate_Date = "2014-03-04" };
            //     History item3 = new History() { File_Path = "ddd3", Operate_Date = "2014-03-05" };
            //        theWholehistorylist.Add(item);
            //        theWholehistorylist.Add(item2);
            //        theWholehistorylist.Add(item3);

            //}

            refreshchart();
            CreateChart();
   
            //chart.MouseLeftButtonUp += new MouseButtonEventHandler(chart_MouseLeftButtonUp);
        }

        public void refreshchart() //将相应日期和次数存入chartCollection，然后再分别存入updateTime和value1
        {

            if (theWholehistorylist != null & theWholehistorylist.Count > 0)
            {
                foreach (History o in theWholehistorylist)
                {
                    if (chartCollection.Count == 0)
                        chartCollection.Add(new KeyValuePair<String, int>(o.Operate_Date, 1));
                    else
                    {
                        for (int i = 0 ; i < chartCollection.Count ; i++)//KeyValuePair<String, int> item in chartCollection)//使用foreach不能执行删除、修改,这是规定
                        {
                            if (o.Operate_Date != chartCollection[i].Key)
                            {
                                chartCollection.Add(new KeyValuePair<String, int>(o.Operate_Date, 1));
                            }
                            else if (o.Operate_Date == chartCollection[i].Key)
                            {
                                int value = chartCollection[i].Value;
                                chartCollection[i]=new KeyValuePair<String, int>(o.Operate_Date, value++);
                            }
                        }
                    }
                }

                foreach (KeyValuePair<String, int> item in chartCollection)
                {
                    updateTime.Add(item.Key);
                    value1.Add(item.Value);
                }
               
            }
        }

        private void CreateChart()
        {

            ColorSet cs = new ColorSet();
            cs.Id = "colorset1"; // 设置ColorSet 的 Id 为 colorset1
            cs.Brushes.Add(new SolidColorBrush(Colors.Green));
            cs.Brushes.Add(new SolidColorBrush(Colors.Red));
            cs.Brushes.Add(new SolidColorBrush(Colors.Pink));
            cs.Brushes.Add(new SolidColorBrush(Colors.Yellow));
            cs.Brushes.Add(new SolidColorBrush(Colors.Orange));

            chart.ColorSets.Add(cs);
            chart.ColorSet = "colorset1";  // 设置 Chart 使用自定义的颜色集合 colorset1

            chart.BorderThickness = new Thickness(0, 0, 0, 0);
            // Set chart properties
            chart.Theme = "Theme1";

            chart.Background = new SolidColorBrush(Colors.LightGray);

            chart.LightingEnabled = true;
            chart.ToolBarEnabled = true;
            //chart.View3D = true;
            chart.AnimationEnabled = true;

            // Set chart width and height
            chart.Width = 1000;
            chart.Height = 300;

            //Create a new Title element
            Title title = new Title();
            //Set the Title Text property
            title.Text = "历史记录";
            //Add Title to chart
            chart.Titles.Add(title);

            // Create new Axis
            Axis axisX = new Axis();
            Axis axisY = new Axis();
            // Set properties of Axis
            axisX.Title = "日期";
           
            axisY.Title = "文件访问次数";
            axisX.IntervalType = IntervalTypes.Days;//图表的X轴坐标按什么来分类，如时分秒
            //axisX.Interval = 1; //图表中的X轴坐标间隔如2，3，20等，单位为xAxis.IntervalType设置的时分秒。
            axisY.Interval = 1;
            axisX.ValueFormatString = "yyyy/MMM/d";//横坐标格式
            //设置图标中Y轴的最小值永远为0
            axisY.AxisMinimum = 0;
            //设置图表中Y轴的后缀
            //axisY.Suffix = "";
            // Add Axis to AxesX collection
            chart.AxesX.Add(axisX);
            chart.AxesY.Add(axisY);

            // Create new DataSeries 创建一个新的数据线。
            DataSeries dataSeries = new DataSeries();         
            dataSeries.ShadowEnabled = true;
            dataSeries.LineThickness = 2;            
            //设置为线
            dataSeries.RenderAs = RenderAs.Line;
            dataSeries.Opacity = 0.5;
            dataSeries.Bevel = false;
            //dataSeries.Color = new SolidColorBrush(Colors.LightGreen);
            // Set property of DataSeries
            //dataSeries.XValueType = ChartValueTypes.DateTime;
//            dataSeries.AxisXType = DateTime;
            //dataSeries.XValueFormatString = "yyyy/MMM/d";//点开格式
           
            // 设置数据点
            DataPoint dataPoint;
            for (int i = 0; i < updateTime.Count; i++)
            {
                // 创建一个数据点的实例。
                dataPoint = new DataPoint();

                // 设置X轴点
                //dataPoint.XValue = updateTime[i];
                dataPoint.AxisXLabel = updateTime[i];

                //设置Y轴点
                dataPoint.YValue = value1[i];
                dataPoint.MarkerSize = 18;
                //dataPoint.Tag = tableName.Split('(')[0];

                //设置数据点颜色
                //dataPoint.Color = new SolidColorBrush(Color.FromArgb((byte)new Random().Next(1, 255), (byte)new Random().Next(1, 255), (byte)new Random().Next(1, 255), 1));
                dataPoint.Color = cs.Brushes[i];
                dataPoint.MouseLeftButtonDown += new MouseButtonEventHandler(dataPoint_MouseLeftButtonDown);

                //添加数据点
                dataSeries.DataPoints.Add(dataPoint);
            }
            // Add DataSeries to Series collection in chart 添加数据线到数据序列。
                chart.Series.Add(dataSeries);

            // Add chart to the LayoutRoot for display
            //LayoutRoot.Children.Clear();
            //DependencyObject parent = chart.Parent;
            //if (parent != null)
            //{
            //    parent.SetValue(ContentPresenter.ContentProperty, null);
            //}

                LayoutRoot.Children.Add(chart);
        }

        void dataPoint_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DataPoint dp = sender as DataPoint;
            //string datetime = (string)dp.XValue;
            string datetime = (string)dp.AxisXLabel;
            datehistorylist.Clear();
            foreach(History item in Post.GetHistoryList(datetime))
            {
                datehistorylist.Add(item);
            }

            //TimeTextBlock.Text = datetime.ToString();
            //MessageBox.Show(datetime.ToString());
        }
    
    }
}
