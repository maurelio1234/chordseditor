using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChordsEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var lines = new ObservableCollection<Line>();
            var lineNumber = 1;
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<string>(new string[] { "Em", "Em", "Em", "Em" }) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<string>(new string[] { "Em", "B7", "Am / B7", "Em" }) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<string>(new string[] { "Em", "B7", "C7 / B7", "Em" }) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<string>(new string[] { "Em", "B7", "Am / C7", "Em" }) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<string>(new string[] { "C ", "Em", "Am / F# B", "Em" }) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<string>(new string[] { "Em", "B7", "A7 / A#7", "B7" }) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<string>(new string[] { "C7", "Em", "Am", "Em " }) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<string>(new string[] { "A7 / A#7", "B7", "Am", "Em" }), Note = "x7" });

            var song = new Song()
            {
                Title = "A very beautiful camel",
                Signature = "4/4",
                Tempo = 150,
                Lines = lines
            };

            DataContext = song;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == true)
            {
                PrintButton.Visibility = Visibility.Hidden;
                printDialog.PrintVisual(this, ((Song)DataContext).Title);
                PrintButton.Visibility = Visibility.Visible;
            }
        }

        private void RemoveLine_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var line = (Line)e.Parameter;
            var song = DataContext as Song;
            song.Lines.Remove(line);
            RenumberSongLines(song);
        }

        private void AddLineBefore_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var line = (Line)e.Parameter;
            var song = DataContext as Song;
            var newLine = new Line();
            song.Lines.Insert(song.Lines.IndexOf(line), newLine);
            RenumberSongLines(song);
        }

        private void AddLineAfter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var line = (Line)e.Parameter;
            var song = DataContext as Song;
            var newLine = new Line();
            song.Lines.Insert(song.Lines.IndexOf(line)+1, newLine);
            RenumberSongLines(song);
        }

        private void RenumberSongLines(Song song)
        {
            var lineNumber = 1;
            foreach (var line in song.Lines)
            {
                line.LineNumber = lineNumber++;
            }
        }

        private void Always_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;
    }

    public class Song
    {
        public string Title { get; set; }
        public string Signature { get; set; }
        public int Tempo { get; set; }
        public ObservableCollection<Line> Lines { get; set; }
    }

    public class Line : INotifyPropertyChanged
    {
        private int _lineNumber;

        public int LineNumber {
            get { return _lineNumber; }
            set {
                _lineNumber = value;
                OnPropertyChanged("LineNumber");
            }
        }

        public ObservableCollection<string> Bars { get; set; }

        public string Note { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
