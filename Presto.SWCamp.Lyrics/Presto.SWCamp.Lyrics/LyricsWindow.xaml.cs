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
using System.Text.RegularExpressions;

namespace Presto.SWCamp.Lyrics
{
    /// <summary>
    /// LyricsWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LyricsWindow : Window
    {
        List<string> lyricList = new List<string>(); // lrc 파일에서 시간 뒤의 문구(가사)를 저장하는 리스트
        List<string> timeList = new List<string>(); // lrc 파일에서 시간을 저장하는 list(mm:ss.ff 형식의 시간을 (mm*60 + ss.ff)*1000 로 저장)

        public LyricsWindow()
        {
            InitializeComponent();
            PrestoSDK.PrestoService.Player.StreamChanged += MusicChanged; //음악이 바뀔 때 해당 프로시저 수행
        }

        private void MusicChanged(object sender, EventArgs e) //음악이 바뀔 때 수행되는 프로시저
        {
            lyricList.Clear();
            timeList.Clear();

            var musicFileName = System.IO.Path.GetFileNameWithoutExtension(PrestoSDK.PrestoService.Player.CurrentMusic.Path);
            var lyricFilePath = System.IO.Path.GetDirectoryName(PrestoSDK.PrestoService.Player.CurrentMusic.Path) + @"\" + musicFileName + ".lrc";
            String[] lines = File.ReadAllLines(lyricFilePath, Encoding.Default); // 경로의 파일의 줄을 읽는다. 글자가 깨지는 경우가 있어서 인코딩 입력

            foreach (var line in lines)
            {
                var m = line.Split('[')[1].Split(']')[0];
                if (Regex.IsMatch(m, @"\d\d\:\d\d\.\d\d"))//m의 값이 2자리의 숫자일 경우
                {
                    if(timeList.Count>0 && timeList[timeList.Count-1]==m)
                    {
                        String s = lyricList[lyricList.Count-1];
                        lyricList.RemoveAt(lyricList.Count-1);
                        lyricList.Add(s + "___" +line.Substring(10));
                    }
                    else
                    {
                        timeList.Add(m);
                        lyricList.Add(line.Substring(10));
                    }
                    
                }
            }

            
            String temp = "";
            foreach(var x in lyricList)
            {
                temp += x + " \n";
            }

            tbk1.Text = musicFileName;
            tbk2.Text = lyricFilePath;
            tbk3.Text = temp;
        }
    }
}
