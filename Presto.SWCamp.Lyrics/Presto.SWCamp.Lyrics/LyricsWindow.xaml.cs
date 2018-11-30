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
using System.Windows.Threading;
using System.Windows.Media.Animation;
using Microsoft.Win32;

namespace Presto.SWCamp.Lyrics
{
    /* 가사 싱크 프로그램
     * 1. 싱크 가사 출력
     * 2. [] 시간 부분만 파싱
     * 3. 2줄 이상의 가사 출력
     * 4. 한국어, 영어 이외의 언어도 출력
     * 5. 재생중인 음악이 바뀔 경우, 전역변수 리스트에 시간(단위 : ms, 데이터형식 : int)과 문자열(데이터형식 : String)저장
     * 6. timer를 통해 재생중인 가사의 리스트를 이진탐색으로 범위 탐색 후 UI에 가사 업데이트
     * 7. 전,후 가사 동시 출력
     * 8. 가끔 가사들을 보면 싱크가 조금 안맞는 파일들이 있기에, 싱크 조절기능이 필요할 것 같아서 추가
     * 9. 가사 파일이 없을 경우 직접 불러올 수 있도록 OpenFileDialog 버튼 추가
         */

    public partial class LyricsWindow : Window
    {
        private List<string> _lrc = new List<string>(); // lrc 파일에서 시간 뒤의 문구(가사)를 저장하는 리스트
        private List<int> _tm = new List<int>(); // lrc 파일에서 시간을 저장하는 list(mm:ss.ff 형식의 시간을 mm*60000 + ss*1000 + ff*10 로 저장)
        private int syncDelay = 0;

        public LyricsWindow()
        {
            InitializeComponent();
            DoubleAnimation dblani = new DoubleAnimation(); // 애니메이션 추가
            dblani.From = 0;
            dblani.To = 1;
            dblani.Duration = TimeSpan.FromMilliseconds(500);
            dblani.EasingFunction = new QuarticEase();
            this.BeginAnimation(OpacityProperty, dblani);

            PrestoSDK.PrestoService.Player.StreamChanged += Stream_Changed; //음악이 바뀔 때 해당 이벤트 수행
            var dt = new DispatcherTimer(); // 타이머 할당
            dt.Interval = TimeSpan.FromMilliseconds(10); // 타이머 주기
            dt.Tick += Timer_Tick; // 이벤트 지정
            dt.Start(); // 타이머 시작
        }

        private void Stream_Changed(object sender, EventArgs e) //음악이 바뀔 때 수행되는 이벤트
        {
            var musicFileName = System.IO.Path.GetFileNameWithoutExtension(PrestoSDK.PrestoService.Player.CurrentMusic.Path);
            var lyricFilePath = System.IO.Path.GetDirectoryName(PrestoSDK.PrestoService.Player.CurrentMusic.Path) + @"\" + musicFileName + ".lrc";
            PathLrcLink(lyricFilePath);
        }

        private void PathLrcLink(string p)
        {
            try
            {
                _lrc.Clear(); // 가사 리스트 초기화
                _tm.Clear(); // 시간 리스트 초기화

                string[] lines = File.ReadAllLines(p, Encoding.Default); // 경로의 파일의 줄을 읽는다. 글자가 깨지는 경우가 있어서 인코딩 입력

                if (PrestoSDK.PrestoService.Player.CurrentMusic.Title is null) // 음악의 타이틀이 존재하지 않을 경우 파일명으로 
                    _lrc.Add(System.IO.Path.GetFileNameWithoutExtension(PrestoSDK.PrestoService.Player.CurrentMusic.Path));
                else
                    _lrc.Add(PrestoSDK.PrestoService.Player.CurrentMusic.Title);
                _tm.Add(0);
                foreach (var line in lines)
                {
                    var m = line.Split('[')[1].Split(']')[0];
                    if (Regex.IsMatch(m, @"\d\d\:\d\d\.\d\d"))//m의 값이 2자리의 숫자일 경우
                    {
                        int cnt = _tm.Count;
                        int time = int.Parse(m.Split(':')[0]) * 60000 // mm
                            + int.Parse(m.Split(':')[1].Split('.')[0]) * 1000 // ss
                            + int.Parse(m.Split(':')[1].Split('.')[1]) * 10; // ff
                        string lrc = line.Substring(10);

                        if (cnt > 1 && _tm[cnt - 1] == time && time > 0)
                        {
                            string s = _lrc[cnt - 1];
                            _lrc.RemoveAt(cnt - 1);
                            _lrc.Add(s + "\n" + lrc);
                        }
                        else
                        {
                            _tm.Add(time);
                            _lrc.Add(lrc);
                        }
                    }
                }
            }
            catch
            {
                tbk1.Text = "";
                tbk2.Text = "가사 파일을 불러올 수 없습니다.";
                tbk3.Text = "";
            }
            syncDelay = 0;
            setSyncText();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int cnt = _tm.Count;
            if (cnt != 0)
            {
                double t = PrestoSDK.PrestoService.Player.Position;
                int index;
                if (t < _tm[1]) index = 0; // 현재재생시간 < 가사시간[1] 일 때
                else if (t >= _tm[cnt - 1]) index = cnt - 1; // 현재재생시간 >= 가사[0] 일 때
                else index = bSearch(_tm, t, 1, cnt - 1); // 이외의 경우 이진 탐색

                if (index == 0) tbk1.Text = "";
                else tbk1.Text = _lrc[index - 1].ToString();

                tbk2.Text = _lrc[index].ToString();

                if (index >= cnt - 1) tbk3.Text = "";
                else tbk3.Text = _lrc[index + 1].ToString();
            }
        }

        private static int bSearch(List<int> lst, Double val, int first, int last)
        {
            int mid = (first + last) / 2;
            if (val >= lst[mid] && val < lst[mid + 1])
                return mid;
            else if (val < lst[mid])
                return bSearch(lst, val, first, mid - 1);
            else
                return bSearch(lst, val, mid + 1, last);
        }

        private void setSyncText()
        {
            if (_tm.Count > 0)
            {
                if (syncDelay > 0) tbkSync.Text = "가사 싱크 : " + (Convert.ToDouble(syncDelay) / 1000).ToString("F1") + " 초 느림";
                else if (syncDelay == 0) tbkSync.Text = "가사 싱크 : 정상";
                else tbkSync.Text = "가사 싱크 : " + (Convert.ToDouble(-syncDelay) / 1000).ToString("F1") + " 초 빠름";
            }
            else
            {
                tbkSync.Text = "가사 파일을 불러올 수 없습니다.";
            }
        }

        private void btnSyncRst_Click(object sender, RoutedEventArgs e)
        {
            if (_tm.Count > 0)
            {
                for (int i = 0; i < _tm.Count; i++)
                {
                    int t;
                    t = _tm[i] - syncDelay;
                    _tm.RemoveAt(i);
                    _tm.Insert(i, t);
                }
                syncDelay = 0;
                setSyncText();
            }
        }

        private void btnSyncDn_Click(object sender, RoutedEventArgs e)
        {
            if (_tm.Count > 0)
            {
                for (int i = 0; i < _tm.Count; i++)
                {
                    int t;
                    t = _tm[i] + 100;
                    _tm.RemoveAt(i);
                    _tm.Insert(i, t);
                }
                syncDelay += 100;
                setSyncText();
            }
        }

        private void btnSyncUp_Click(object sender, RoutedEventArgs e)
        {
            if (_tm.Count > 0)
            {
                for (int i = 0; i < _tm.Count; i++)
                {
                    int t;
                    t = _tm[i] - 100;
                    _tm.RemoveAt(i);
                    _tm.Insert(i, t);
                }
                syncDelay -= 100;
                setSyncText();
            }
        }

        private void btnLoadLrc_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fldlg = new OpenFileDialog();
            fldlg.Filter = "가사 파일|*.lrc|모든 파일|*.*";
            fldlg.DefaultExt = ".lrc";
            Nullable<bool> dialogOK = fldlg.ShowDialog();

            if (dialogOK == true)
            {
                string f = fldlg.FileName;
                PathLrcLink(f);
            }
        }
    }
}
