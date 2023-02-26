using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;
using System.Diagnostics;
using Nelios.S1.Tools;
using Nelios.Music.Tools;

namespace MusicCompositionHelper
{
	class Utils
	{
		//Misc
		public static void WindowToggle(Window window)
		{
			if (window.IsVisible) window.Hide();
			else window.Show();
		}

		public static void WindowOffset(Window window, double x, double y)
		{
			window.Left = x;
			window.Top = y;
		}

		public static void DragWindow()
		{
			MainWindow.mainWindow.DragMove();
		}

		/*[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);*/
		public static S1Controller s1Controller = new S1Controller();
		public static IntPtr s1 = IntPtr.Zero;
		public static bool s1On = false;
		/*public static void Send(String key) { System.Windows.Forms.SendKeys.SendWait(key); }
		public static void Send(String key, int howManyTime)
		{
			for (int i = 0; i < howManyTime; i++)
				System.Windows.Forms.SendKeys.SendWait(key);
		}*/

		public static void ToggleS1()
		{
			s1On = !s1On;
			if (s1On)
				Console.WriteLine("Hooked!");
			else
				Console.WriteLine("Hooking Off!");
		}

		//Music
		// C  C# D  D# E  F  F# G  G# A  A# B
		// 0  1  2  3  4  5  6  7  8  9  10 11
		public static String[] tones = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
		public static int[] currentScale = new int[0];

		public static List<Tuning> tuningList = new List<Tuning>();
		public static List<Scale> scaleList = new List<Scale>();

		public static int whichTuning;
		private static int[]
			string0 = new int[16],
			string1 = new int[16],
			string2 = new int[16],
			string3 = new int[16],
			string4 = new int[16],
			string5 = new int[16],
			string6 = new int[16];
		private static int[][] strings = { string0, string1, string2, string3, string4, string5, string6 };

		private static int[]
			chord0 = new int[4],
			chord1 = new int[4],
			chord2 = new int[4],
			chord3 = new int[4],
			chord4 = new int[4],
			chord5 = new int[4],
			chord6 = new int[4];
		public static int[][] chords = { chord0, chord1, chord2, chord3, chord4, chord5, chord6 };

		public static int FindTone(string key)
		{
			for (int i = 0; i < tones.GetLength(0); i++)
				if (key == tones[i]) return i;
			return -1;
		}

		public static void StringsSetup()
		{
			for (int i = 0; i < strings.Length; i++)
				strings[i][0] = tuningList[whichTuning].tones[i];

			for (int s = 0; s < strings.Length; s++)
				for (int i = 1; i < 16; i++)
				{
					strings[s][i] = strings[s][i - 1] + 1;
					if (strings[s][i] > 11)
						strings[s][i] -= 12;
				}
		}

		public static void ComputeScale(string key, int[] steps)
		{
			int tonePosition = 0;
			int startTone;

			Array.Resize(ref currentScale, steps.Length);
			startTone = FindTone(key);
			if (startTone < 0)
				return;
			if (steps.GetLength(0) != currentScale.GetLength(0))
			{
				Array.Resize(ref currentScale, steps.Length);
				startTone = FindTone(key);
				return;
			}
			tonePosition = startTone;
			for (int i = 0; i < steps.GetLength(0); i++)
			{
				Array.Resize(ref currentScale, steps.Length);
				currentScale[i] = tonePosition % tones.GetLength(0);
				tonePosition += steps[i];
			}
		}

		public static void WriteScale(int[] scale)
		{
			foreach (int i in scale)
				Console.WriteLine(tones[i]);
		}

		public static int IntervalFromC(String key) { return FindTone(key); }
		public static int IntervalFromC(String key, int offset) { return FindTone(key) + offset; }

		public static void TuningSetup()
		{
			for (int i = 0; i < tuningList.Count; i++)
				Array.Reverse(tuningList[i].tones);
		}

		public static void SetTuning(int index)
		{
			//Console.WriteLine(tones[tuningList[index][0]]);		//Print lowest string's Note (G/F# etc)
			whichTuning = index;
			Utils.StringsSetup();
		}

		public static void PlaceNoteBackground(int whichString, int whichFret, int[] scale)
		{
			if (WindowScale.noteBackgrounds[0][0] == null) return;
			for (int i = 0; i < scale.Length; i++)
				if (strings[whichString][whichFret] == currentScale[i])
				{
					if (FindTone(tones[i]) == 0)
						WindowScale.noteBackgrounds[whichString][whichFret].Source = WindowScale.imgRoot;
					else
						WindowScale.noteBackgrounds[whichString][whichFret].Source = WindowScale.imgNote;


					for (int a = 0; a < chords[WindowChord.clickedChordRow].Length; a++)
						if (currentScale[i] == chords[WindowChord.clickedChordRow][a] && WindowChord.clickedChord != null && WindowChord.toggle)
							WindowScale.noteBackgrounds[whichString][whichFret].Source = WindowScale.imgHighlight;

					WindowScale.noteBackgrounds[whichString][whichFret].Visibility = Visibility.Visible;
					//Console.WriteLine("String " + whichString + " Fret " + strings[whichString][whichFret]);
				}
		}

		public static void PlaceTones(int whichString, int whichFret, int[] scale)
		{
			if (WindowScale.noteTones[0][0] == null) return;
			for (int i = 0; i < scale.Length; i++)
				if (strings[whichString][whichFret] == currentScale[i])
				{
					WindowScale.noteTones[whichString][whichFret].Source = WindowScale.imgTone[currentScale[i]];
					WindowScale.noteTones[whichString][whichFret].Visibility = Visibility.Visible;
				}
		}

		public static void ClearNotes()
		{
			if (WindowScale.noteBackgrounds[0][0] == null) return;
			for (int i = 0; i < 7; i++)
				for (int a = 0; a < 16; a++)
				{
					WindowScale.noteBackgrounds[i][a].Visibility = Visibility.Hidden;
					WindowScale.noteTones[i][a].Visibility = Visibility.Hidden;
				}
		}

		public static void ClearChords()
		{
			for (int i = 0; i < 7; i++)
				for (int a = 0; a < chords[i].Length; a++)
				{
					WindowChord.bNotes[i][a].Content = "";
					WindowChord.bNotes[i][a].IsEnabled = false;
				}
		}

		public static void PlaceChords()
		{
			if (WindowChord.bNotes[0][0] == null) return;
			ClearChords();

			for (int i = 0; i < currentScale.Length; i++)
				for (int a = 0; a < chords[i].Length; a++)
				{
					if (a * 2 + i < currentScale.Length)
						chords[i][a] = currentScale[(a * 2 + i)];
					else if (a * 2 + i < currentScale.Length * 2)
						chords[i][a] = currentScale[(a * 2 + i) - currentScale.Length];
					else
						chords[i][a] = currentScale[(a * 2 + i) - currentScale.Length * 2];

					WindowChord.bNotes[i][a].Content = tones[chords[i][a]];
					WindowChord.bNotes[i][a].IsEnabled = true;
				}
		}
	}
}
