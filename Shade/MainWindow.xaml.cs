using Shade.Objects;
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

namespace Shade
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string shadetext = ShadeStream.GetShadeUnicodeText(@"C:\Development\CSharp\Shade\shadeimage.png");

            ShadeStream.SetShadeUnicodeText(@"C:\Development\CSharp\Shade\shadeimage.png", "Simon Merlung");
        }
    }
}
