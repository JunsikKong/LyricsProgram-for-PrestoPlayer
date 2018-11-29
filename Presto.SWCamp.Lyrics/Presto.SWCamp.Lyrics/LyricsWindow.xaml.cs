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

namespace Presto.SWCamp.Lyrics
{
    /* 가사 싱크 프로그램
     * 1. 싱크 가사 출력
     * 2. [] 시간 부분만 파싱
     * 3. 2줄 이상의 가사 출력
     * 4. 한국어, 영어 이외의 언어도 출력
     * 5. 재생중인 음악이 바뀔 경우, 전역변수 리스트에 시간(단위 : ms, 데이터형식 : int)과 문자열(데이터형식 : String)저장
     * 6. timer를 통해 재생중인 가사의 리스트를 이진탐색으로 범위 탐색 후 UI에 가사 업데이트
         */

    public partial class LyricsWindow : Window
    {
        private List<string> _lyricList = new List<string>(); // lrc 파일에서 시간 뒤의 문구(가사)를 저장하는 리스트
        private List<int> _timeList = new List<int>(); // lrc 파일에서 시간을 저장하는 list(mm:ss.ff 형식의 시간을 mm*60000 + ss*1000 + ff*10 로 저장)
        

        public LyricsWindow()
        {
            InitializeComponent();
            PrestoSDK.PrestoService.Player.StreamChanged += Stream_Changed; //음악이 바뀔 때 해당 이벤트 수행
            var dt = new DispatcherTimer(); // 타이머 할당
            dt.Interval = TimeSpan.FromMilliseconds(10); // 타이머 주기
            dt.Tick += Timer_Tick; // 이벤트 지정
            dt.Start(); // 타이머 시작
        }

        private void Stream_Changed(object sender, EventArgs e) //음악이 바뀔 때 수행되는 이벤트
        {
            try
            {
                _lyricList.Clear();
                _timeList.Clear();

                var musicFileName = System.IO.Path.GetFileNameWithoutExtension(PrestoSDK.PrestoService.Player.CurrentMusic.Path);
                var lyricFilePath = System.IO.Path.GetDirectoryName(PrestoSDK.PrestoService.Player.CurrentMusic.Path) + @"\" + musicFileName + ".lrc";
                string[] lines = File.ReadAllLines(lyricFilePath, Encoding.Default); // 경로의 파일의 줄을 읽는다. 글자가 깨지는 경우가 있어서 인코딩 입력

                foreach (var line in lines)
                {
                    var m = line.Split('[')[1].Split(']')[0];
                    if (Regex.IsMatch(m, @"\d\d\:\d\d\.\d\d"))//m의 값이 2자리의 숫자일 경우
                    {
                        int cnt = _timeList.Count;
                        int time = int.Parse(m.Split(':')[0]) * 60000 // mm
                            + int.Parse(m.Split(':')[1].Split('.')[0]) * 1000 // ss
                            + int.Parse(m.Split(':')[1].Split('.')[1]) * 10; // ff
                        string lrc = line.Substring(10);

                        if (cnt > 0 && _timeList[cnt - 1] == time)
                        {
                            string s = _lyricList[cnt - 1];
                            _lyricList.RemoveAt(cnt - 1);
                            _lyricList.Add(s + "\n" + lrc);
                        }
                        else
                        {
                            _timeList.Add(time);
                            _lyricList.Add(lrc);
                        }
                    }
                }
            }
            catch
            {
                tbk2.Text = "가사 파일을 불러올 수 없습니다.";
            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            int cnt = _timeList.Count;
            if(cnt != 0)
            {
                Double t = PrestoSDK.PrestoService.Player.Position;
                int index;
                if (t < _timeList[0]) index = -1; // 현재재생시간 < 가사[0] 일 때
                else if (t >= _timeList[cnt - 1]) index = cnt - 1; // 현재재생시간 >= 가사[0] 일 때
                else index = bSearch(_timeList, t, 0, cnt - 1); // 이외의 경우 이진 탐색

                if (index <= 0) tbk1.Text = "";
                else tbk1.Text = _lyricList[index - 1].ToString();

                if (index <= -1) tbk2.Text = "";
                else tbk2.Text = _lyricList[index].ToString();

                if (index >=cnt-1) tbk2.Text = "";
                else tbk3.Text = _lyricList[index + 1].ToString();
            }
        }

        private static int bSearch(List<int> lst ,Double val, int first, int last)
        {
            int mid = (first + last) / 2;
            if (val >= lst[mid] && val < lst[mid + 1])
                return mid;
            else if (val < lst[mid])
                return bSearch(lst, val, first, mid - 1);
            else
                return bSearch(lst, val, mid + 1, last);
        }
    }
}
