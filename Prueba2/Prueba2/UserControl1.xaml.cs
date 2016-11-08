using System;
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

namespace Prueba2
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public bool Expandir = false;// Flag to control the grew of the rectangle
        private Point LastPoint;// Its the las point where grew the rectanglu

        /// <summary>
        /// Indicate if the grew is in the right or left side , or not grew
        /// </summary>
        private enum HitType
        {
            None, L, R,
        };
        public UserControl1()
        {
            InitializeComponent();
         
          
        }
        HitType MouseHitType = HitType.None;//its a enum object
        /// <summary>
        /// It is an event when click over the right button 
        /// Move the rectangle a period to the rigth side
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Right_Click(object sender, RoutedEventArgs e)
        {
            if ((Canvas.GetLeft(rectangle1) + rectangle1.Width)+5 < canvas1.ActualWidth)
                Canvas.SetLeft(rectangle1, Canvas.GetLeft(rectangle1) + 10);

        }

        /// <summary>
        /// It is an event when click over the left button 
        /// Move the rectangle a period to the left side
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Left_Click(object sender, RoutedEventArgs e)
        {
            if (Canvas.GetLeft(rectangle1)>5)
            Canvas.SetLeft(rectangle1, Canvas.GetLeft(rectangle1) - 10);
         /*   rectangle1.Width += 10;
              Canvas.SetLeft(rectangle1, 10);*/
        }

        /// <summary>
        /// Identifies the type of movement to place the corresponding mouse animation
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        private HitType SetHitType(Rectangle rect, Point point)
        {
            double left = Canvas.GetLeft(rectangle1);
            double top = Canvas.GetTop(rectangle1);
            double right = left + rectangle1.Width;
        
            if (point.X < left) return HitType.None;
            if (point.X > right) return HitType.None;
       

            const double GAP = 8;
            if (point.X - left < GAP)
            {
                return HitType.L;
            }
            if (right - point.X < GAP)
            {

                return HitType.R;
            }

            return HitType.None;
        }

        /// <summary>
        /// Make the mouse animation
        /// </summary>
        private void SetMouseCursor()
        {
            // See what cursor we should display.
            Cursor desired_cursor = Cursors.Arrow;
            switch (MouseHitType)
            {
                case HitType.None:
                    desired_cursor = Cursors.Arrow;
                    break;

                case HitType.L:
                    desired_cursor = Cursors.SizeWE;
                    break;
                case HitType.R:
                    desired_cursor = Cursors.SizeWE;
                    break;
            }

            // Display the desired cursor.
            if (Cursor != desired_cursor) Cursor = desired_cursor;
        }

        // Start dragging.
        /// <summary>
        /// selec the point to where will grow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MouseHitType = SetHitType(rectangle1, Mouse.GetPosition(canvas1));
            SetMouseCursor();
            if (MouseHitType == HitType.None) return;

            LastPoint = Mouse.GetPosition(canvas1);
            Expandir = true;
        }
        /// <summary>
        /// Handles the rectangle moves
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void canvas1_MouseMove(object sender, MouseEventArgs e)
        {
            double x = canvas1.ActualWidth;
            if (!Expandir)
            {
                MouseHitType = SetHitType(rectangle1, Mouse.GetPosition(canvas1));
                SetMouseCursor();
            }
            else
            {
                // See how much the mouse has moved.
                Point point = Mouse.GetPosition(canvas1);

                double offset_x = point.X - LastPoint.X;
                double offset_y = point.Y - LastPoint.Y;
                  
                // Get the rectangle's current position.
                double new_x = Canvas.GetLeft(rectangle1);


                double new_width = rectangle1.Width;

               
                // Update the rectangle.
                switch (MouseHitType)
                {

                    case HitType.L:
                        if (new_x > 0)
                        {
                            new_x += offset_x;
                            new_width -= offset_x;
                        }
                        else
                        {
                            if (point.X > LastPoint.X)
                            {
                                new_x += offset_x;
                                new_width -= offset_x;
                            }
                        }
                        break;
                    case HitType.R:
                        if ((new_x + rectangle1.Width) < canvas1.ActualWidth)
                        {
                            new_width += offset_x;
                        }
                        else
                        {
                            if (point.X < LastPoint.X)
                            {
                                new_width += offset_x;
                            }

                        }
                        break;

                }

                // Don't use negative width or height.
                if (new_width > 0)
                {
                    // Update the rectangle.

                   
                    Canvas.SetLeft(rectangle1, new_x);

                    rectangle1.Width = new_width;


                    // Save the mouse's new location.
                    LastPoint = point;
                }
            }
        }


        /// <summary>
        /// when you click up on canvas  not allow growth of the rectnagle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas1_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Expandir = false;
         

        }

        /// <summary>
        /// when the mouse is over button not allow growth of the rectnagle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Left_MouseMove_1(object sender, MouseEventArgs e)
        {
            Expandir = false;
        }

        /// <summary>
        /// when the mouse is over button not allow growth of the rectnagle
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Rigth_MouseMove(object sender, MouseEventArgs e)
        {
            Expandir = false;
        }
        

        /// <summary>
        /// Draw upper limit  in te Axis x
        /// </summary>
        /// <param name="nodos"></param>
        public  void MaximoNodo(Nodo nodos)
        {
            label.Content = nodos.ts;
        }

        /// <summary>
        /// Draw the lower limit in the Axis x
        /// </summary>
        /// <param name="nodos"></param>
        public  void MinNodo(Nodo nodos)
        {
            Etiqueta.Content = nodos.ts;
        }
    }
}

