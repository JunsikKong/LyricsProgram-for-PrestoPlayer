using Presto.SDK;
using System;
using System.IO;
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
using System.Windows.Shapes;

namespace Presto.SWCamp.Lyrics
{
    /// <summary>
    /// LyricsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LyricsWindow : Window
    {
        List<string> lyricList = new List<string>();
        List<int> timeList = new List<int>();

        public LyricsWindow()
        {
            InitializeComponent();
            PrestoSDK.PrestoService.Player.StreamChanged += MusicChanged; //음악이 바뀔 때 해당 프로시저 수행
        }

        private void MusicChanged(object sender, EventArgs e) //음악이 바뀔 때 수행되는 프로시저
        {
            var musicFileName = System.IO.Path.GetFileNameWithoutExtension(PrestoSDK.PrestoService.Player.CurrentMusic.Path);
            var lyricFilePath = System.IO.Path.GetDirectoryName(PrestoSDK.PrestoService.Player.CurrentMusic.Path) + "\\" + musicFileName + ".lrc";
            tbk1.Text = musicFileName;
            tbk2.Text = lyricFilePath;
        }
    }
}
