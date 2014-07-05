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
using System.Windows.Shapes;

namespace MusicCompositionHelper
{
	/// <summary>
	/// Interaction logic for Chord.xaml
	/// </summary>
	public partial class WindowChord : Window
	{
		public static Button[] bNotes0 = new Button[4];
		public static Button[] bNotes1 = new Button[4];
		public static Button[] bNotes2 = new Button[4];
		public static Button[] bNotes3 = new Button[4];
		public static Button[] bNotes4 = new Button[4];
		public static Button[] bNotes5 = new Button[4];
		public static Button[] bNotes6 = new Button[4];
		public static Button[][] bNotes = { bNotes0, bNotes1, bNotes2, bNotes3, bNotes4, bNotes5, bNotes6 };
		public static Button clickedChord;
		public static int clickedChordRow, oldClickedChordRow, offset = 0, userOffset = 0;
		public static bool toggle = false;

		static SolidColorBrush
			normal = new SolidColorBrush(Color.FromArgb(255, 186, 186, 186)),
			highlight = new SolidColorBrush(Color.FromArgb(255, 104, 223, 52));

		public WindowChord()
		{
			InitializeComponent();

			for (int i = 0; i < bNotes.Length; i++)
			{
				for (int a = 0; a < bNotes[i].Length; a++)
				{
					bNotes[i][a] = new Button();
					bNotes[i][a].Content = i.ToString() + a.ToString();
					bNotes[i][a].Name = "c" + i;
					gButtonGrid.Children.Add(bNotes[i][a]);
					Grid.SetColumn(bNotes[i][a], a);
					Grid.SetRow(bNotes[i][a], i);
					bNotes[i][a].Click += ChordClick;
					bNotes[i][a].FontSize = 20;
				}
			}
			Utils.PlaceChords();
		}

		private void DragMainWindow(object sender, MouseButtonEventArgs e)
		{
			Utils.DragWindow();
		}

		private void Window_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void ChordClick(object sender, RoutedEventArgs e)
		{
			Utils.Send("^(+(l))");
			userOffset = 0;
			clickedChord = (Button)sender;
			clickedChordRow = (int)Char.GetNumericValue(clickedChord.Name[clickedChord.Name.Length - 1]);
			if (clickedChordRow == oldClickedChordRow) toggle = !toggle;
			else
			{
				oldClickedChordRow = clickedChordRow;
				toggle = true;
			}
			ChordCompute();
			userOffset = 0;
		}

		public static void ResetHighlight()
		{
			for (int a = 0; a < bNotes[0].Length; a++)
			{
				for (int i = 0; i < bNotes.Length; i++)
				{
					bNotes[i][a].Foreground = normal;
				}
			}
			WindowScale.windowScale.Do();
		}

		public static void ChordCompute()
		{
			for (int a = 0; a < bNotes[0].Length; a++)
			{
				for (int i = 0; i < bNotes.Length; i++)
				{
					bNotes[i][a].Foreground = normal;
				}
				if (toggle)
				{
					bNotes[clickedChordRow][a].Foreground = highlight;
					if (Utils.s1On && Utils.s1 != IntPtr.Zero)
					{
						Utils.SetForegroundWindow(Utils.s1);
						Utils.Send("^v");
						if (a > 0)
							if (Utils.IntervalFromC(bNotes[clickedChordRow][a].Content.ToString()) < Utils.IntervalFromC(bNotes[clickedChordRow][a - 1].Content.ToString()))
								offset = 12;
						if (offset + userOffset < 0)
							Utils.Send("{DOWN}", Math.Abs(Utils.IntervalFromC(bNotes[clickedChordRow][a].Content.ToString(), offset + userOffset)));
						else
							Utils.Send("{UP}", Utils.IntervalFromC(bNotes[clickedChordRow][a].Content.ToString(), offset + userOffset));
					}
				}
				else bNotes[clickedChordRow][a].Foreground = normal;
			}
			Console.WriteLine("");
			offset = 0;
			WindowScale.windowScale.Do();
			Utils.Send("^(+(l))");
		}
	}
}
