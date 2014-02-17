using ESRI.ArcGIS.Client;
using ESRI.ArcGIS.Client.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MapControl
{
    public partial class MapControl : UserControl
    {

        Envelope INIT_EXTENT = new Envelope(13449261, 2822872, 13571592, 2914860); 
        public MapControl()
        {
            InitializeComponent();

            ArcGISLib.GoogleMap map1 = new ArcGISLib.GoogleMap() { ID = "BaseMap" };
            map1.SetValue(Map.NameProperty, "BaseMap");
            ElementLayer lyr = new ElementLayer() { ID = "ElementLyr" };
          
            this.map1.Layers.Add(map1);
            this.map1.Layers.Add(lyr);
            overviewMap1.Layer  = new ArcGISLib.GoogleMap() ;
            
        }

        public void AddControllRoomPin(Common.Info.ControlRoomInfo info)
        {
            Pins.CotrolRoomPin pin = new Pins.CotrolRoomPin() { Width = 24, Height = 24 };
            pin.DataContext = info;
         ElementLayer.SetEnvelope(pin, new Envelope(new MapPoint(info.X, info.Y), new MapPoint(info.X, info.Y)));
        (this.map1.Layers["ElementLyr"] as ElementLayer).Children.Add(pin);
        }

     public   void ZoomToLevel(int level)
        {
            double resolution = (this.map1.Layers["BaseMap"] as ESRI.ArcGIS.Client.TiledLayer).TileInfo.Lods[level].Resolution;
            this.map1.ZoomToResolution(resolution);
        }
     public    void ZoomToLevel(int level, MapPoint point)
        {
            bool zoomentry = false;
            double resolution;
            if (level == -1)
                resolution = map1.Resolution;
            else
                resolution = (this.map1.Layers["BaseMap"] as TiledLayer).TileInfo.Lods[level].Resolution;


            if (Math.Abs(map1.Resolution - resolution) < 0.05)
            {
                this.map1.PanTo(point);
                return;
            }
            zoomentry = false;
            this.map1.ZoomToResolution(resolution);

            map1.ExtentChanged += (s, a) =>
            {
                if (!zoomentry)
                    this.map1.PanTo(point);

                zoomentry = true;

                //   SwitchLayerVisibility();
            };



        }

     public   int GetCurrentLevel()
        {
            Lod[] Lods = (this.map1.Layers["BaseMap"] as TiledLayer).TileInfo.Lods;
            for (int i = 0; i < Lods.Length; i++)
            {
                if (Math.Abs(this.map1.Resolution - Lods[i].Resolution) < 1 || this.map1.Resolution >= Lods[i].Resolution)
                {
                    return i;
                }

            }
            return 0;
        }

        private void map1_ExtentChanged(object sender, ExtentEventArgs e)
        {
            this.txtLevel.Text = this.GetCurrentLevel().ToString();
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
       //     PowerLostInfo info = (sender as Control).DataContext as PowerLostInfo;

         //   ESRI.ArcGIS.Client.Projection.WebMercator wm = new ESRI.ArcGIS.Client.Projection.WebMercator();

          //  MapPoint mp = wm.FromGeographic(new MapPoint(info.lon, info.lat)) as MapPoint;
        //    this.ZoomToLevel(15, mp);


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.map1.ZoomTo(this.INIT_EXTENT);
        }
    }
}
