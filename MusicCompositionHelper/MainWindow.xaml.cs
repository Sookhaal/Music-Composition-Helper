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

namespace MusicCompositionHelper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		WindowScale windowScale = new WindowScale();
		WindowChord windowChord = new WindowChord();
		TuningPrompt tuningPrompt = new TuningPrompt();

		public static MainWindow mainWindow;
		public static Button addTuning;
		static SolidColorBrush
			hookOn = new SolidColorBrush(Color.FromArgb(255, 77, 216, 58)),
			hookOff = new SolidColorBrush(Color.FromArgb(255, 216, 58, 58));
		public MainWindow()
		{
			InitializeComponent();
			mainWindow = this;
			Utils.WindowOffset(windowChord, this.Left - windowChord.Width, this.Top);
			Utils.WindowToggle(windowChord);

			Utils.WindowOffset(windowScale, this.Left, this.Top + this.Height);
			Utils.WindowToggle(windowScale);
		}

		private void b_Exit_Click(object sender, RoutedEventArgs e)
		{
			Application.Current.Shutdown();
		}

		private void b_StudioOne_Click(object sender, RoutedEventArgs e)
		{
			if (Utils.s1 == IntPtr.Zero)
				Utils.HookToS1();
			if (Utils.s1 != IntPtr.Zero)
				Utils.ToggleS1();
			if (Utils.s1On)
			{
				b_StudioOne.Foreground = hookOn;
				b_UndoS1.IsEnabled = true;
				b_OctaveUp.IsEnabled = true;
				b_OctaveDown.IsEnabled = true;
			}
			else
			{
				b_StudioOne.Foreground = hookOff;
				b_UndoS1.IsEnabled = false;
				b_OctaveUp.IsEnabled = false;
				b_OctaveDown.IsEnabled = false;
			}
		}

		private void b_UndoS1_Click(object sender, RoutedEventArgs e)
		{
			Utils.SetForegroundWindow(Utils.s1);
			Utils.Send("l");
			for (int k = 0; k < WindowChord.bNotes[0].Length; k++)
				Utils.Send("{DEL}");
			WindowChord.toggle = false;
			WindowChord.ResetHighlight();
		}

		private void b_OctaveUp_Click(object sender, RoutedEventArgs e)
		{
			Utils.SetForegroundWindow(Utils.s1);
			Utils.Send("l");
			for (int k = 0; k < WindowChord.bNotes[0].Length; k++)
				Utils.Send("{DEL}");
			WindowChord.userOffset += 12;
			WindowChord.ChordCompute();
		}

		private void b_OctaveDown_Click(object sender, RoutedEventArgs e)
		{
			Utils.SetForegroundWindow(Utils.s1);
			Utils.Send("l");
			for (int k = 0; k < WindowChord.bNotes[0].Length; k++)
				Utils.Send("{DEL}");
			WindowChord.userOffset -= 12;
			WindowChord.ChordCompute();
		}

		private void Window_LocationChanged(object sender, EventArgs e)
		{
			Utils.WindowOffset(windowScale, this.Left, this.Top + this.Height);
			Utils.WindowOffset(windowChord, this.Left - windowChord.Width, this.Top);
			Utils.WindowOffset(tuningPrompt, this.Left, this.Top - tuningPrompt.Height);
		}

		private void Window_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Utils.DragWindow();
		}

		private void Window_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}
	}
}
