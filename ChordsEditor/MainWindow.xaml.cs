using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Media;
using Microsoft.VisualBasic;

namespace ChordsEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var lines = new ObservableCollection<Line>();
            var lineNumber = 1;
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<Bar>(GenerateBars(new string[] { "Em", "Em", "Em", "Em" })) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<Bar>(GenerateBars(new string[] { "Em", "B7", "Am / B7", "Em" })) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<Bar>(GenerateBars(new string[] { "Em", "B7", "C7 / B7", "Em" })) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<Bar>(GenerateBars(new string[] { "Em", "B7", "Am / C7", "Em" })) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<Bar>(GenerateBars(new string[] { "C ", "Em", "Am / F# B", "Em" })) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<Bar>(GenerateBars(new string[] { "Em", "B7", "A7 / A#7", "B7" })) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<Bar>(GenerateBars(new string[] { "C7", "Em", "Am", "Em " })) });
            lines.Add(new Line() { LineNumber = lineNumber++, Bars = new ObservableCollection<Bar>(GenerateBars(new string[] { "A7 / A#7", "B7", "Am", "Em" })), Note = "x7" });

            var song = new Song()
            {
                Title = "A very beautiful camel",
                Signature = "4/4",
                Tempo = 150,
                Lines = lines
            };

            DataContext = song;
        }

        private List<Bar> GenerateBars(string[] v)
        {
            return v.Select(content => new Bar() { Content = content, Color = Brushes.Black }).ToList();
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

        private void Always_CanExecute(object sender, CanExecuteRoutedEventArgs e) => e.CanExecute = true;

        private void RemoveBar_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var bar = (Bar)e.Parameter;
            var song = DataContext as Song;
            Line line = song.Lines.First(l => l.Bars.Contains(bar));
            line.Bars.Remove(bar);
        }

        private void AddBarBefore_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var bar = (Bar)e.Parameter;
            var song = DataContext as Song;
            var newBar = new Bar();
            Line line = song.Lines.First(l => l.Bars.Contains(bar));
            line.Bars.Insert(line.Bars.IndexOf(bar), newBar);
        }

        private void AddBarAfter_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            var bar = (Bar)e.Parameter;
            var song = DataContext as Song;
            var newBar = new Bar();
            Line line = song.Lines.First(l => l.Bars.Contains(bar));
            line.Bars.Insert(line.Bars.IndexOf(bar)+1, newBar);
        }

        private void SetColorBlack_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetElementColor(e.Parameter, Brushes.Black);
        }

        private void SetColorRed_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetElementColor(e.Parameter, Brushes.Red);
        }

        private void SetColorGreen_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetElementColor(e.Parameter, Brushes.Green);
        }

        private void SetColorBlue_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            SetElementColor(e.Parameter, Brushes.Blue);
        }

        private void RenumberSongLines(Song song)
        {
            var lineNumber = 1;
            foreach (var line in song.Lines)
            {
                line.LineNumber = lineNumber++;
            }
        }

        private static void SetElementColor(object e, Brush color)
        {
            if (e is Bar)
            {
                var bar = (Bar)e;
                bar.Color = color;
            }
            else if (e is Line)
            {
                var line = (Line)e;
                foreach(var bar in line.Bars)
                {
                    bar.Color = color;
                }
            }
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBox = (TextBlock)e.Source;
            var bar = (Bar)textBox.DataContext;
            ShowInputBox(bar.Content, () => bar.Content = InputTextBox.Text);
        }

        private void ShowInputBox(string DefaultText, Action p)
        {
            InputTextBox.Text = DefaultText;
            InputBoxBehavior = p;
            InputBox.Visibility = System.Windows.Visibility.Visible;
        }

        private Action InputBoxBehavior;

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            InputBox.Visibility = System.Windows.Visibility.Collapsed;
            InputBoxBehavior();
            InputTextBox.Text = String.Empty;
        }
    }

    public class Song
    {
        public string Title { get; set; }
        public string Signature { get; set; }
        public int Tempo { get; set; }
        public ObservableCollection<Line> Lines { get; set; }
    }

    public abstract class AbstractObservableElement : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    public class Line : AbstractObservableElement
    {
        private int _lineNumber;

        public int LineNumber {
            get { return _lineNumber; }
            set {
                _lineNumber = value;
                OnPropertyChanged("LineNumber");
            }
        }

        public ObservableCollection<Bar> Bars { get; set; }

        public string Note { get; set; }

    }

    public class Bar : AbstractObservableElement
    {
        private Brush _color;
        public Brush Color
        {
            get
            {
                return _color;
            }

            set
            {
                _color = value;
                OnPropertyChanged("Color");
            }
        }

        private string _content;
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                OnPropertyChanged("Content");
            }
        }
    }
}
