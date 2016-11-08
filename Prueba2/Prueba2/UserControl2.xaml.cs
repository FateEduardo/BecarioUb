
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
using System.Drawing;
using System.Collections;
using System.Timers;
using System.Windows.Threading;

namespace Prueba2
{
    /// <summary>
    /// Interaction logic for UserControl2.xaml
    /// </summary>
    /// 


    public partial class UserControl2 : UserControl
    {
        private Deserializate ds;// contains the data of input json
        public double Canvas_Widht;
        public double Canvas_Height;
        private static DispatcherTimer timer;
        private DateTime time;
        private Nodo inicio;//start reference point
        private int intervalos = 1;
        private int ticks;
        private int meta = 1;
        private int Base=0;
        private Dictionary<string, Dictionary<DateTime,int>> L_N = new Dictionary<string, Dictionary<DateTime, int>>();
        //structure handler data
        private int indice = 0;

        public UserControl2()
        {
            InitializeComponent();


        }

        private void timer_Tick(object sender, EventArgs e)
        {
            //Event triggered every second
            Etiquetas();
            LlenarNombres();
            ControlGrid();
            EjeY();
        

        }

///Its a handler of timeslots in RunTime       
        private void ControlGrid()
        {
            ticks++;
            if (intervalos + 1 > 10)
            {
               
                intervalos = 1;
                
            }
            SeleccionadorPeriodo();
         
            Cont.Children.Clear();
            if(intervalos!=1)Cuadricula();
            onPaintGrafica(Canvas_Widht, inicio.ts, Base);
            if (ticks==meta)
            {
                meta += Base;
                Cuadros.Content = Base + " segundos por division";
                intervalos++;
            }
 
           
        }


        /// <summary>
        ///charge the data in the structure
        ///Key->source
        ///value->structure2
        ///Structure2
        ///key->DateTime
        ///value->frecuency
        /// </summary>
        private void LlenarNombres()
        {
       
            for (int pos = indice; pos < ds.list.Count; pos++)
            {
                Nodo nodo = ds.list.ElementAt(pos);
                if (nodo.ts.ToLongTimeString() == time.ToLongTimeString())
                {
                    if (L_N.ContainsKey(nodo.source))
                    {
                        //nodo existe
                        Dictionary<DateTime,int> aux = (Dictionary<DateTime, int>)L_N[nodo.source];
                        if (aux.ContainsKey(nodo.ts))
                        {
                            //tiempo existe
                            TiempoExistente(nodo.source, nodo.ts);
                        }else
                        {
                            //tiempo no existe
                            TiempoNoExistente(nodo.source, nodo.ts);
                        }
                     
                    } else
                    {
                        //nodo no existe
                        var aux = new Dictionary<DateTime,int>();
                        aux.Add(nodo.ts, 1);
                        L_N.Add(nodo.source, aux);
                    }

                }else
                {
                    break;
                }
                indice++;
            }
        }
        /// <summary>
        /// It is a complementary function validate if the source exist in the structure and has more than one menssagge in the time
        /// </summary>
        /// <param name="nodo"></param>
        /// <param name="time"></param>
          private void TiempoExistente(string nodo,DateTime time)
        {
            Dictionary<DateTime,int> aux= (Dictionary<DateTime, int>)L_N[nodo];
            int var = (int)aux[time];
            aux[time] = var + 1;
            L_N[nodo] = aux;
        }

         /// <summary>
        /// It is a complementary function validate if the source exist in the structure and is the first time that the node send a packet
        /// </summary>
        /// <param name="nodo"></param>
        /// <param name="time"></param>
        private void TiempoNoExistente(string nodo, DateTime time)
        {
            Dictionary<DateTime, int> aux = (Dictionary<DateTime, int>)L_N[nodo];
            aux.Add(time, 1);
            L_N[nodo] = aux;
        }
        /// <summary>
        /// It is a complementary function , and helps to select the time division. when the time corrent is over the base second the slot value is the nexts base second
        /// </summary>
          private void SeleccionadorPeriodo()
        {
            switch (ticks)
            {
                case 1:
                    Base= 1;
                    break;
                case 10:
                    Base= 10;
                    break;
                case 100:
                    Base= 100;
                    break;

            }
        }
        /// <summary>
        /// Configure the dimensions of the canvas
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       
        private void Load(object sender, RoutedEventArgs e)
        {
            double x = Componente.canvas1.ActualWidth;
            Canvas.SetLeft(Componente.rectangle1, 0);
            Componente.rectangle1.Width = x;
        }
        
        /// <summary>
        /// Draw the limit labels with his respective time
        /// </summary>
        private void Etiquetas()
        {
            time = ValidarFecha();
            Componente.label.Content = time.ToLongTimeString();
        }


 
        /// <summary>
        /// It helps validate the amount of time plus a second
        /// </summary>
        /// <returns>DateTiem</returns>
        private DateTime ValidarFecha()
        {
            if (time.Second + 1 < 59)
            {
                DateTime fecha = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, time.Hour, time.Minute, time.Second + 1);
                return fecha;
            }
            else
            {
                if (time.Minute + 1 < 59)
                {
                    DateTime fecha = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, time.Hour, time.Minute+1, 0);
                    return fecha;
                }
                else
                {
                    DateTime fecha = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, time.Hour+1, 0, 0);
                    return fecha;
                }
            }

        }


       
        /// <summary>
        /// Overcharge of the function, this function. It allow time jumps of 1 and 10 seconds   
        /// </summary>
        /// <param name="ti"></param>
        /// <param name="SALTO"></param>
        /// <returns></returns>
        private DateTime ValidarFecha(DateTime ti, int SALTO)
        {
            if (ti.Second + SALTO < 59)
            {
                DateTime fecha = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, ti.Hour, ti.Minute, ti.Second + SALTO,998);
                return fecha;
            }
            else
            {
                if (ti.Minute + 1 < 59)
                {

                    int sec = SALTO - (60 - ti.Second);
                    DateTime fecha = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, ti.Hour, ti.Minute + 1, sec,998);
                    return fecha;
                }
                else
                {
                    int sec = SALTO - (60 - ti.Second);
                    DateTime fecha = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, ti.Hour + 1, 0, sec,998);
                    return fecha;
                }
            }

        }

         /// <summary>
        /// Draw the line division
        /// </summary>
        public void Cuadricula()
        {

            double paso = Canvas_Widht / intervalos;
            double pos = paso;
           
            for (int i = 0; i < intervalos-1; i++)
            {
                Line line = new Line();
                line.Visibility = System.Windows.Visibility.Visible;
                line.StrokeThickness = 0.2;
                line.Stroke = System.Windows.Media.Brushes.Gray;
                line.X1 =pos;
                line.X2 = pos; 
                line.Y1 = Canvas_Height;
                line.Y2 = 0;
                Cont.Children.Add(line);
                pos += paso;
            }
         //   onPaintGrafica(paso);
        }

        /// <summary>
        /// Chargs the node when send a packet in the AxisY  
        /// </summary>
        
        private void EjeY(){
           
            Stack.Children.Clear();
            foreach (string str in L_N.Keys) {
                Label lb = new Label();
                lb.Content = str;
            Stack.Children.Add(lb);
            }   
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="inicio"></param>
        /// <param name="SALTO"></param>
        private void onPaintGrafica(double pos,DateTime inicio,int SALTO)
        {
            double posH = 8;
            double auxA = posH;
            double inter = pos / (intervalos + intervalos );
            if (intervalos == 1) inter = pos / 2;
            double auxP = inter;
   
            for (int i = 0; i < intervalos; i++)
            {
                DateTime auxTime = ValidarFecha(inicio, SALTO);
                auxA = posH;

                foreach (Dictionary<DateTime,int> info in L_N.Values)
                {
                    bool band = false;
                    int Grosor = 0;
                    var li = info.Keys.ToList();
                    li.Sort();
                    foreach (DateTime tic in li)
                    {
                   
                        if (tic.TimeOfDay >= inicio.TimeOfDay && tic.TimeOfDay <=auxTime.TimeOfDay)
                        {
                            band = true;
                            Grosor++;
                        }else
                        {
                            break;
                        }
                    }
                    if (band)
                    {
                        Ellipse elipse = DibujarElipse(Grosor, Grosor);
                   
                        Canvas.SetLeft(elipse, auxP);
                        Canvas.SetTop(elipse, auxA);
                        Cont.Children.Add(elipse);
                    }
                    ///aqui se cambian las variables de control de dibujo
                    auxA += posH*2;

                }
                auxP += inter*2;
                inicio = auxTime;

            }
        }
        /// <summary>
        /// Return a elipse with the dimensions according to node frequency        
        /// </summary>
        /// <param name="WH"></param>
        /// <param name="HH"></param>
        /// <returns></returns>
        
        private Ellipse DibujarElipse(int WH,int HH)
        {
            Ellipse elipse = new Ellipse();
            elipse.Fill = new SolidColorBrush(Colors.Red);
            elipse.Stroke = Brushes.Blue;
            elipse.StrokeThickness = 2;
      
            elipse.Width = WH * 10;
            elipse.Height = HH * 10;
            return elipse;
        }

        
        /// <summary>
        /// Load the initial conditions of the estrucutra and reading data. Draw the graphic for fist time
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private void Grafica_Loaded(object sender, RoutedEventArgs e)
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(100);
            timer.Tick += timer_Tick;
            timer.Start();
            ds = new Deserializate();
            ds.Leer();


            inicio = ds.list.ElementAt(0);
            Dictionary<DateTime,int> aux = new  Dictionary<DateTime,int>();
            aux.Add(inicio.ts, 1);
            L_N.Add(inicio.source, aux);
            time = inicio.ts;
            indice = 1;
            string STR_inicio = inicio.ts.ToLongTimeString();
            Componente.Etiqueta.Content = STR_inicio;
            Componente.label.Content = STR_inicio;
            EjeY();
           
        }
    }
}
