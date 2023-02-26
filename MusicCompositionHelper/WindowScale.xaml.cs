using Nelios.Music.Tools;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace MusicCompositionHelper
{
	/// <summary>
	/// Interaction logic for WindowScale.xaml
	/// </summary>
	public partial class WindowScale : Window
	{
		public Grid gridNeck;
		public static BitmapImage imgNote, imgRoot, imgHighlight;
		public static BitmapImage[] imgTone = new BitmapImage[12];
		private static Image[] noteTone = new Image[12];
		public static WindowScale windowScale;

		private static Image[] noteBackground0 = new Image[16];
		private static Image[] noteBackground1 = new Image[16];
		private static Image[] noteBackground2 = new Image[16];
		private static Image[] noteBackground3 = new Image[16];
		private static Image[] noteBackground4 = new Image[16];
		private static Image[] noteBackground5 = new Image[16];
		private static Image[] noteBackground6 = new Image[16];
		public static Image[][] noteBackgrounds = { noteBackground0, noteBackground1, noteBackground2, noteBackground3, noteBackground4, noteBackground5, noteBackground6 };

		private static Image[] noteTone0 = new Image[16];
		private static Image[] noteTone1 = new Image[16];
		private static Image[] noteTone2 = new Image[16];
		private static Image[] noteTone3 = new Image[16];
		private static Image[] noteTone4 = new Image[16];
		private static Image[] noteTone5 = new Image[16];
		private static Image[] noteTone6 = new Image[16];
		public static Image[][] noteTones = { noteTone0, noteTone1, noteTone2, noteTone3, noteTone4, noteTone5, noteTone6 };
		private XmlSerializer xs;

		public WindowScale()
		{
			InitializeComponent();
			windowScale = this;
			imgNote = new BitmapImage(new Uri(@"pack://application:,,,/images/Note.png"));
			imgRoot = new BitmapImage(new Uri(@"pack://application:,,,/images/Root.png"));
			imgHighlight = new BitmapImage(new Uri(@"pack://application:,,,/images/Highlight.png"));
			for (int i = 0; i < Utils.tones.Length; i++)
			{
				imgTone[i] = new BitmapImage(new Uri(@"pack://application:,,,/images/" + i + ".png"));
				noteTone[i] = new Image();
				noteTone[i].Source = imgTone[i];
			}

			for (int i = 0; i < noteBackgrounds.Length; i++)
			{
				for (int a = 0; a < noteBackgrounds[i].Length; a++)
				{
					noteBackgrounds[i][a] = new Image();
					gNeck.Children.Add(noteBackgrounds[i][a]);
					Grid.SetColumn(noteBackgrounds[i][a], a);
					Grid.SetRow(noteBackgrounds[i][a], i + 1);
					noteBackgrounds[i][a].Visibility = Visibility.Hidden;

					noteTones[i][a] = new Image();
					gNeck.Children.Add(noteTones[i][a]);
					Grid.SetColumn(noteTones[i][a], a);
					Grid.SetRow(noteTones[i][a], i + 1);
					noteTones[i][a].Visibility = Visibility.Hidden;
				}
			}

			xs = new XmlSerializer(typeof(List<Tuning>));
			using (StreamReader rd = new StreamReader("Settings\\Tunings.xml"))
			{
				try
				{
					Utils.tuningList = xs.Deserialize(rd) as List<Tuning>;
				}
				catch
				{
					MessageBox.Show("Error in Tunings.xml");
				}
			}
			for (int i = 0; i < Utils.tuningList.Count; i++)
			{
				choseTuning.Items.Add(Utils.tuningList[i].name);
			}

			xs = new XmlSerializer(typeof(List<Scale>));
			using (StreamReader rd = new StreamReader("Settings\\Scales.xml"))
			{
				try
				{
					Utils.scaleList = xs.Deserialize(rd) as List<Scale>;
					//Console.WriteLine(Utils.scaleList.Count);
				}
				catch
				{
					MessageBox.Show("Error in Tunings.xml");
				}
			}
			for (int i = 0; i < Utils.scaleList.Count; i++)
			{
				if (Utils.scaleList[i].name == "none")
					choseScale.Items.Add(new Separator());
				else
					choseScale.Items.Add(Utils.scaleList[i].name);
			}
			Utils.TuningSetup();
			Utils.SetTuning(choseTuning.SelectedIndex);
			Do();
			Utils.PlaceChords();
		}

		private void PlaceNoteBackground(string key, int scale)
		{
			//Utils.ComputeScale(key, Utils.tempScales[scale]);
			Utils.ComputeScale(key, Utils.scaleList[scale].tones);
			Utils.ClearNotes();
			for (int i = 0; i < 7; i++)
			{
				for (int a = 0; a < 16; a++)
				{
					//Utils.PlaceNoteBackground(i, a, Utils.tempScales[scale]);
					Utils.PlaceNoteBackground(i, a, Utils.scaleList[scale].tones);
					//Utils.PlaceTones(i, a, Utils.tempScales[scale]);
					Utils.PlaceTones(i, a, Utils.scaleList[scale].tones);
					//noteBackgrounds[0][Utils.WhichFret(note)[i]].Source = imgNote;
					//noteBackgrounds[0][Utils.WhichFret(note)[i]].Visibility = Visibility.Visible;
				}
			}
		}

		public void Do()
		{
			Utils.ClearNotes();
			Utils.SetTuning(choseTuning.SelectedIndex);
			Utils.PlaceChords();
			PlaceNoteBackground(choseKey.Text, choseScale.SelectedIndex);
		}

		//Scale
		private void choseScale_DropDownClosed(object sender, EventArgs e)
		{
			Do();
		}

		private void choseScale_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			if (!choseScale.IsDropDownOpen)
				e.Handled = true;
			else e.Handled = false;
		}

		private void choseScale_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		private void choseScale_MouseMove(object sender, MouseEventArgs e)
		{
			Do();
		}

		//Tuning
		private void choseTuning_DropDownClosed(object sender, EventArgs e)
		{
			Do();
		}

		private void choseTuning_MouseMove(object sender, MouseEventArgs e)
		{
			Do();
		}

		private void choseTuning_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			e.Handled = true;
		}

		private void choseTuning_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		//Key
		private void choseKey_DropDownClosed(object sender, EventArgs e)
		{
			Do();
		}

		private void choseKey_MouseMove(object sender, MouseEventArgs e)
		{
			Do();
		}

		private void choseKey_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
		{
			e.Handled = true;
		}

		private void choseKey_PreviewKeyDown(object sender, KeyEventArgs e)
		{
			e.Handled = true;
		}

		//Neck Image
		private void Image_MouseMove(object sender, MouseEventArgs e)
		{
			Do();
		}

		private void DragMainWindow(object sender, MouseButtonEventArgs e)
		{
			Utils.DragWindow();
		}

		private void DisableRightMouseButton(object sender, MouseButtonEventArgs e)
		{
			e.Handled = true;
		}
	}
}