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
	/// Interaction logic for TuningPrompt.xaml
	/// </summary>
	public partial class TuningPrompt : Window
	{
		static ComboBox[] tuningNotes = new ComboBox[7];
		public TuningPrompt()
		{
			InitializeComponent();
			for (int i = 0; i < 7; i++)
			{
				tuningNotes[i] = new ComboBox();
				gridTuning.Children.Add(tuningNotes[i]);
				Grid.SetColumn(tuningNotes[i], i);
				Grid.SetRow(tuningNotes[i], 0);
			}
		}

		private void DisableRightMouseButton(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}

		private void DragWindow(object sender, MouseButtonEventArgs e)
		{
			this.DragMove();
		}

		private void Add(object sender, RoutedEventArgs e)
		{
			Utils.WindowToggle(this);
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			MainWindow.addTuning.IsEnabled = true;
			Utils.WindowToggle(this);
		}
	}
}
