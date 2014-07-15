using Nelios.S1.Tools;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MusicCompositionHelper
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public static Button addTuning;
		public static MainWindow mainWindow;

		private static SolidColorBrush
			hookOn = new SolidColorBrush(Color.FromArgb(255, 77, 216, 58)),
			hookOff = new SolidColorBrush(Color.FromArgb(255, 216, 58, 58));

		private TuningPrompt tuningPrompt = new TuningPrompt();
		private WindowChord windowChord = new WindowChord();
		private WindowScale windowScale = new WindowScale();

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

		private void b_OctaveDown_Click(object sender, RoutedEventArgs e)
		{
			WindowChord.OctaveDown(-1);
			WindowChord.userOffset -= 1;
		}

		private void b_OctaveUp_Click(object sender, RoutedEventArgs e)
		{
			WindowChord.OctaveUp(1);
			WindowChord.userOffset += 1;
		}

		private void b_StudioOne_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (Utils.s1 == IntPtr.Zero)
					Utils.s1 = Utils.s1Controller.GetS1();
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
			catch (Exception ex)
			{
				Console.WriteLine(Utils.tuningList[0].name);
				MessageBox.Show("Error: " + ex.Message);
			}
		}

		private void b_UndoS1_Click(object sender, RoutedEventArgs e)
		{
			Utils.s1Controller.SendKey(S1Controller.VKeys.VK_L);
			for (int k = 0; k < WindowChord.bNotes[0].Length; k++)
			{
				Utils.s1Controller.SendKey(S1Controller.VKeys.VK_DELETE);
				System.Threading.Thread.Sleep(10);
			}
			WindowChord.toggle = false;
			WindowChord.ResetHighlight();
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