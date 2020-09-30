using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ParaKap
{
    class Para:Image
    {
        Random r = new Random();
        DispatcherTimer timer = new DispatcherTimer();
        public Para(MainWindow w)//Yapıcı method
        {
            this.Source = new BitmapImage(new Uri("Resimler/para.png",UriKind.Relative));
            this.Height = 50;
            this.Width = 50;
            Canvas.SetTop(this, -this.Height);
            Canvas.SetLeft(this, r.Next((int)(w.cOyunAlani.ActualWidth - this.Width)));
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            AsagiDus();
        }

        private void AsagiDus()
        {
            double mevcut = Canvas.GetTop(this);
            Canvas.SetTop(this, mevcut + 5);
        }
    }
}
