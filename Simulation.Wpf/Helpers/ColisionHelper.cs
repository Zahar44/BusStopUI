using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Simulation.Wpf.Helpers
{
    class ColisionHelper
    {
        private const double pading = 10;
        private UserControl view;
        private Point placeCoordinat;
        private Canvas canvas;
        private ICollection<UserControl> posibleImposition;

        public ColisionHelper(UserControl _view, Point _placeCoordinat, Canvas _canvas, ICollection<UserControl> _posibleImposition)
        {
            view = _view;
            placeCoordinat = _placeCoordinat;
            canvas = _canvas;
            posibleImposition = _posibleImposition;
        }

        public static void CalculateRoadPosition(Line road, UserControl first, UserControl second)
        {
            double weight = first.ActualWidth;
            double height = first.ActualHeight;
            double leftX, leftY, rightX, rightY;

            Point f = new Point(Canvas.GetLeft(first), Canvas.GetTop(first));
            Point s = new Point(Canvas.GetLeft(second), Canvas.GetTop(second));
            Point min = MinPointByX(f, s);
            Point max = MaxPointByX(f, s);

            if (Math.Abs(f.X - s.X) > Math.Abs(f.Y - s.Y))
            {
                leftX = min.X + weight + pading;
                leftY = min.Y + height / 2;
                rightX = max.X - pading;
                rightY = max.Y + height / 2;
            }
            else
            {
                min = MinPointByY(f, s);
                max = MaxPointByY(f, s);
                leftX = min.X + weight / 2;
                leftY = min.Y + height + pading;
                rightX = max.X + weight / 2;
                rightY = max.Y - pading;
            }
            
            road.X1 = leftX;
            road.X2 = rightX;
            road.Y1 = leftY;
            road.Y2 = rightY;
        }

        public bool CanMove()
        {
            return !EndOfField(
                placeCoordinat.X, placeCoordinat.Y,
                view.ActualHeight, view.ActualWidth,
                canvas.ActualHeight, canvas.ActualWidth
                );
        }

        public Point CalculateOffset()
        {
            Point res = new Point(0, 0);
            foreach (var control in posibleImposition)
            {
                if (control == view) continue;

                double fromX = Canvas.GetLeft(control);
                double fromY = Canvas.GetTop(control);
                var response = Overlap(
                    placeCoordinat.X, placeCoordinat.Y,
                    view.ActualWidth, view.ActualHeight,
                    fromX, fromY,
                    control.ActualWidth, control.ActualHeight
                    );

                res.X += response.X;
                res.Y += response.Y;
            }
            return res;
        }

        private bool EndOfField(double toX, double toY, double height, double weight, double maxHeight, double maxWeight)
        {
            bool _x = toX - weight / 2 < 1 || toX + weight / 2 > maxWeight;
            bool _y = toY - height / 2 < 1 || toY + height / 2 > maxHeight;

            return _x || _y;
        }

        private Point Overlap(double toX, double toY, double toWeight, double toHeight, double fromX, double fromY, double fromWeight, double fromHeight)
        {
            double _xToLLocation = toX - toWeight / 2;
            double _xToRLocation = toX + toWeight / 2;
            double _yToULocation = toY - toHeight / 2;
            double _yToDLocation = toY + toHeight / 2;

            double _xFromLLocation = fromX;
            double _xFromRLocation = fromX + fromWeight;
            double _yFromULocation = fromY;
            double _yFromDLocation = fromY + fromHeight;

            double _xLC = _xToLLocation - _xFromRLocation; // someone left
            double _xRC = _xFromLLocation - _xToRLocation; // someone right
            bool _x = _xLC > pading && !(_xRC < pading) || !(_xLC > pading) && _xRC < pading; // XOR

            double _yUC = _yToULocation - _yFromDLocation; // someone up
            double _yDC = _yFromULocation - _yToDLocation; // someone down
            bool _y = _yUC > pading && !(_yDC < pading) || !(_yUC > pading) && _yDC < pading; // XOR

            if (_x && _y)
            {
                return GetOffset(-_xLC, _xRC, -_yUC, _yDC);
            }
            return new Point(0, 0);
        }

        private Point GetOffset(double left, double right, double up, double down)
        {
            double resX = Math.MinMagnitude(left, right);
            double resY = Math.MinMagnitude(up, down);
            double min = Math.MinMagnitude(resX, resY);

            if(min == resX)
            {
                return new Point(resX, 0);
            }
            else
            {
                return new Point(0, resY);
            }
        }

        private static Point MinPointByX(Point x1, Point x2) => x1.X < x2.X ? x1 : x2;
        private static Point MinPointByY(Point x1, Point x2) => x1.Y < x2.Y ? x1 : x2;
        private static Point MaxPointByX(Point x1, Point x2) => x1.X > x2.X ? x1 : x2;
        private static Point MaxPointByY(Point x1, Point x2) => x1.Y > x2.Y ? x1 : x2;
    }
}
