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
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Presto.SWCamp.Lyrics
{
    /**************** 가사 싱크 프로그램 기능 ****************
     * 1. 싱크 가사 출력
     * 2. [] 시간 부분만 파싱
     * 3. 2줄 이상의 가사 출력
     * 4. 한국어, 영어 이외의 언어도 출력
     * 5. 재생중인 음악이 바뀔 경우, 전역변수 리스트에 시간(단위 : ms, 데이터형식 : int)과 문자열(데이터형식 : String)저장
     * 6. timer를 통해 재생중인 가사의 리스트를 이진탐색으로 범위 탐색 후 UI에 가사 업데이트
     * 7. 전,후 가사 동시 출력
     * 8. 가끔 가사들을 보면 싱크가 조금 안맞는 파일들이 있기에, 싱크 조절기능이 필요할 것 같아서 추가
     * 9. 가사 파일이 없을 경우 직접 불러올 수 있도록 OpenFileDialog 버튼 추가
     * 
     */

    public partial class LyricsWindow : Window
    {
        // 리스트 선언, 가사와 시간을 저장, 시간은 int 형식의 ms 단위로 저장
        // 현재 싱크의 속도를 저장하는 int 형식의 변수 선언, 양수면 싱크가 느리고 음수면 싱크가 빠르다.
        private List<string> _lrc = new List<string>();
        private List<int> _tm = new List<int>();
        private int _sD = 0;

        public LyricsWindow()
        {
            InitializeComponent();
            // 폼이 시작될 때 보여주는 애니메이션
            DoubleAnimation doubleAni = new DoubleAnimation(); 
            doubleAni.From = 0;
            doubleAni.To = 1;
            doubleAni.Duration = TimeSpan.FromMilliseconds(500);
            doubleAni.EasingFunction = new QuarticEase();
            this.BeginAnimation(OpacityProperty, doubleAni);

            // 재생중인 음악이 바뀔 때 수행될 이벤트
            PrestoSDK.PrestoService.Player.StreamChanged += Stream_Changed;

            // 실시간으로 재생시간에 맞는 가사를 가져올 수 있도록 하는 타이머
            var dt = new DispatcherTimer();
            dt.Interval = TimeSpan.FromMilliseconds(10);
            dt.Tick += Timer_Tick;
            dt.Start();
        }

        private void Stream_Changed(object sender, EventArgs e) //음악이 바뀔 때 수행되는 이벤트
        {
            var path = System.IO.Path.GetDirectoryName
                (PrestoSDK.PrestoService.Player.CurrentMusic.Path) + @"\" +
                System.IO.Path.GetFileNameWithoutExtension(PrestoSDK.PrestoService.Player.CurrentMusic.Path) + ".lrc";
            PathLrcLink(path);
        }

        private void PathLrcLink(string p)
        {
            try
            {
                // 리스트 초기화
                _lrc.Clear();
                _tm.Clear();

                // 경로의 파일의 모든줄을 배열에 할당한다. 인코딩을 안쓰면 다른 나라의 언어가 깨지므로 Encoding 속성을 추가해줬다.
                string[] lines = File.ReadAllLines(p, Encoding.Default);

                // 재생한 음악 태그의 타이틀이 없을 경우 파일명으로 가져오기.
                if (PrestoSDK.PrestoService.Player.CurrentMusic.Title is null)
                    _lrc.Add(System.IO.Path.GetFileNameWithoutExtension(PrestoSDK.PrestoService.Player.CurrentMusic.Path));
                else
                    _lrc.Add(PrestoSDK.PrestoService.Player.CurrentMusic.Title);
                _tm.Add(0);

                // 리스트에 시간과 가사를 각각 저장하는 for문
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
            catch // 위의 상황에서 예외 발생시(가사를 불러오지 못했을 경우)
            {
                tbk1.Text = "";
                tbk2.Text = "가사 파일을 불러올 수 없습니다.";
                tbk3.Text = "";
            }

            // 가사의 싱크값 초기화
            _sD = 0;
            setSyncText();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int cnt = _tm.Count;

            // 가사가 리스트에 저장되 있을 경우 가사 탐색
            if (cnt != 0)
            {
                double t = PrestoSDK.PrestoService.Player.Position;
                int index;

                if (t < _tm[1]) index = 0;
                else if (t >= _tm[cnt - 1]) index = cnt - 1;
                else index = bSearch(_tm, t, 1, cnt - 1);


                if (index == 0) tbk1.Text = "";
                else tbk1.Text = _lrc[index - 1].ToString();

                tbk2.Text = _lrc[index].ToString();

                if (index >= cnt - 1) tbk3.Text = "";
                else tbk3.Text = _lrc[index + 1].ToString();
            }
        }

        // 범위 이진탐색
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

        // 텍스트블럭에 싱크속도 보여주기
        private void setSyncText()
        {
            if (_tm.Count > 0)
            {
                if (_sD > 0) tbkSync.Text = "가사 싱크 : " + (Convert.ToDouble(_sD) / 1000).ToString("F1") + " 초 느림";
                else if (_sD == 0) tbkSync.Text = "가사 싱크 : 정상";
                else tbkSync.Text = "가사 싱크 : " + (Convert.ToDouble(-_sD) / 1000).ToString("F1") + " 초 빠름";
            }
            else
            {
                tbkSync.Text = "가사 파일을 불러올 수 없습니다.";
            }
        }

        // 싱크 리셋버튼
        private void btnSyncRst_Click(object sender, RoutedEventArgs e)
        {
            // 가사가 리스트에 있을 경우만 수행
            if (_tm.Count > 0)
            {
                for (int i = 0; i < _tm.Count; i++)
                {
                    int t;
                    t = _tm[i] - _sD;
                    _tm.RemoveAt(i);
                    _tm.Insert(i, t);
                }
                _sD = 0;
                setSyncText();
            }
        }

        // 싱크 느리게하는 버튼
        private void btnSyncDn_Click(object sender, RoutedEventArgs e)
        {
            // 가사가 리스트에 있을 경우만 수행
            if (_tm.Count > 0)
            {
                for (int i = 0; i < _tm.Count; i++)
                {
                    int t;
                    t = _tm[i] + 100;
                    _tm.RemoveAt(i);
                    _tm.Insert(i, t);
                }
                _sD += 100;
                setSyncText();
            }
        }

        // 싱크 빠르게하는 버튼
        private void btnSyncUp_Click(object sender, RoutedEventArgs e)
        {
            // 가사가 리스트에 있을 경우만 수행
            if (_tm.Count > 0)
            {
                for (int i = 0; i < _tm.Count; i++)
                {
                    int t;
                    t = _tm[i] - 100;
                    _tm.RemoveAt(i);
                    _tm.Insert(i, t);
                }
                _sD -= 100;
                setSyncText();
            }
        }

        // 가사가 없거나, 다른 경로에 있거나 등등 새로 가사파일을 불러오고 싶을 때
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
