using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace HafizaOyunu
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<BitmapImage> ResimDeposu; //9 elemanlı
        List<BitmapImage> ResimCiftleri; //16 elemanlı
        Image oncekiResim;
        SoundPlayer pop = new SoundPlayer(Properties.Resources.pop);
        SoundPlayer clank = new SoundPlayer(Properties.Resources.clank);
        SoundPlayer blip = new SoundPlayer(Properties.Resources.blip);
        SoundPlayer click = new SoundPlayer(Properties.Resources.click);


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ResimDeposunuDoldur();
            ResimCiftleriniDoldur();

            Random r = new Random();

            for (int i = 0; i < 16; i++)
            {
                Image img = new Image();

                int x = r.Next(ResimCiftleri.Count);
                img.Source = ResimDeposu[0];//?
                img.Tag = ResimCiftleri[x];
                ResimCiftleri.RemoveAt(x);
                img.MouseDown += İmg_MouseDown;
                img.MouseEnter += İmg_MouseEnter;

                ugOyunAlani.Children.Add(img); 
            }
        }

        private void İmg_MouseEnter(object sender, MouseEventArgs e)
        {
            //click.Play();
            Image ilgiliResim = (Image)sender;
            imgIpucu.Source = (BitmapImage)ilgiliResim.Tag;
        }

        private async void İmg_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pop.Play();
            Image ilgiliResim = (Image)sender;
            ilgiliResim.Source = (BitmapImage)ilgiliResim.Tag;
            if (oncekiResim == null) //yani ilk tıklamadaysak
            {
                oncekiResim = ilgiliResim;// şimdiki resmi kaydet, yani yedekle
            }
            else
            {
                if (oncekiResim != ilgiliResim)
                {
                    blip.Play();
                    await Task.Delay(500);
                    if (oncekiResim.Source == ilgiliResim.Source)
                    {
                        oncekiResim.Visibility = Visibility.Hidden;
                        ilgiliResim.Visibility = Visibility.Hidden;
                    }
                    else
                    {
                        clank.Play();
                        oncekiResim.Source = ResimDeposu[0];// ? yükle
                        ilgiliResim.Source = ResimDeposu[0];// ? yükle
                    }
                    oncekiResim = null; 
                }
                else
                {
                    MessageBox.Show("Bir daha uyanıklık yapmayacağıma söz veriyorum.");
                }
            }
        }

        private void ResimCiftleriniDoldur()
        {
            ResimCiftleri = new List<BitmapImage>();
            for (int i = 0; i < 16; i++)
            {
                ResimCiftleri.Add(ResimDeposu[i%8+1]);
            }
        }

        private void ResimDeposunuDoldur()
        {
            ResimDeposu = new List<BitmapImage>();
            string ResimKlasoru = "Resimler/";
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "soruisareti.png", UriKind.Relative)));
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "asagiel.png", UriKind.Relative)));
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "calarsaat.png", UriKind.Relative)));
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "cansimidi.png", UriKind.Relative)));
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "dürbün.png", UriKind.Relative)));
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "kumsaati.png", UriKind.Relative)));
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "makas.png", UriKind.Relative)));
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "yukariel.png", UriKind.Relative)));
            ResimDeposu.Add(new BitmapImage(new Uri(ResimKlasoru + "zil.png", UriKind.Relative)));
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (Image item in ugOyunAlani.Children)
            {
                item.Source = (BitmapImage)item.Tag;
            }
            await Task.Delay(500);
            foreach (Image item in ugOyunAlani.Children)
            {
                item.Source = ResimDeposu[0];
            }
        }
    }
}
