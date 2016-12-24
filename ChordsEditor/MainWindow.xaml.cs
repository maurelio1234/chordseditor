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

namespace ChordsEditor
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var lines = new List<Line>();
            var lineNumber = 1;
            lines.Add(new Line() { LineNumber = lineNumber++, Chords = "Em | Em | Em | Em " });
            lines.Add(new Line() { LineNumber = lineNumber++, Chords = "Em | B7 | Am / B7 | Em" });
            lines.Add(new Line() { LineNumber = lineNumber++, Chords = "Em | B7 | C7 / B7 | Em" });
            lines.Add(new Line() { LineNumber = lineNumber++, Chords = "Em | B7 | Am / C7 | Em" });
            lines.Add(new Line() { LineNumber = lineNumber++, Chords = "C  | Em | Am / F# B | Em" });
            lines.Add(new Line() { LineNumber = lineNumber++, Chords = "Em | B7 | A7 / A#7 | B7" });
            lines.Add(new Line() { LineNumber = lineNumber++, Chords = "C7 | Em | Am | Em " });
            lines.Add(new Line() { LineNumber = lineNumber++, Chords = "A7 / A#7 | B7 | Am | Em", Note = "x7" });

            var song = new Song() {
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
    }

    public class Song
    {
        public string Title { get; set;  }
        public string Signature { get; set;  }
        public int    Tempo { get; set;  }
        public List<Line> Lines { get; set; }
    }

    public class Line
    {
        public int LineNumber { get; set; }
        public string Chords { get; set;  }
        public string Note { get; set;  }
    }
}
