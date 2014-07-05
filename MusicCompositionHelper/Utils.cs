using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Runtime.InteropServices;

namespace MusicCompositionHelper
{
	class Utils
	{
		//Programming utils
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

		[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("USER32.DLL")]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
		public static IntPtr s1 = IntPtr.Zero;
		public static bool s1On = false;
		public static void Send(String key) { System.Windows.Forms.SendKeys.SendWait(key); }
		public static void Send(String key, int howManyTime)
		{
			for (int i = 0; i < howManyTime; i++)
				System.Windows.Forms.SendKeys.SendWait(key);
		}
		public static void HookToS1()
		{
			s1 = FindWindow("CCLWindowClass", null);

			if (s1 == IntPtr.Zero)
			{
				MessageBox.Show("Studio One is not running.");
				return;
			}
		}

		public static void ToggleS1()
		{
			s1On = !s1On;
			if (s1On)
				Console.WriteLine("Hooked!");
			else
				Console.WriteLine("Hooking Off!");
		}


		//Music utils

		public static String[] tones = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
		public static int[] currentScale = new int[0];
		// C  C# D  D# E  F  F# G  G# A  A# B
		// 0  1  2  3  4  5  6  7  8  9  10 11
		public static Tuning[] tuningCreator = 
		{
			new Tuning(new int[] { 7, 0, 7, 0, 7, 0, 4 }, "Open C"),
			new Tuning(new int[] { 6, 11, 6, 11, 6, 11, 3 }, "Open B"),
			new Tuning(new int[] { 11, 4, 9, 2, 7, 11, 4 }, "Standard"),
			new Tuning(new int[] { 10, 3, 8, 1, 6, 10, 3 }, "1/2 Down"),
			new Tuning(new int[] { 9, 2, 7, 0, 5, 9, 2 }, "Full Down"),
			new Tuning(new int[] { 9, 2, 9, 2, 7, 11, 4 }, "Drop D"),
			new Tuning(new int[] { 7, 0, 7, 0, 5, 9, 2 }, "Drop C"),
			new Tuning(new int[] { 6, 11, 6, 11, 4, 8, 1 }, "Drop B"),
			new Tuning(new int[] { 4, 9, 4, 9, 2, 6, 11 }, "Drop A")
		};
		//public static int[][] tuningList = new int[tuningCreator.Length][];
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

		//Standard modes
		public static int[]
			scaleIonian = { 2, 2, 1, 2, 2, 2, 1 },			//AKA Major
			scaleDorian = { 2, 1, 2, 2, 2, 1, 2 },
			scalePhrygian = { 1, 2, 2, 2, 1, 2, 2 },
			scaleLydian = { 2, 2, 2, 1, 2, 2, 1 },
			scaleMixolydian = { 2, 2, 1, 2, 2, 1, 2 },
			scaleAeolian = { 2, 1, 2, 2, 1, 2, 2 },			//AKA Minor
			scaleLocrian = { 1, 2, 2, 1, 2, 2, 2 },

		//Harmonic modes
			scaleIonianHarmonic = { 2, 1, 2, 2, 1, 3, 1 },	//AKA Harmonic Minor
			scaleDorianHarmonic = { 1, 2, 2, 1, 3, 1, 2 },
			scalePhrygianHarmonic = { 2, 2, 1, 3, 1, 2, 1 },
			scaleLydianHarmonic = { 2, 1, 3, 1, 2, 1, 2 },
			scaleMixolydianHarmonic = { 1, 3, 1, 2, 1, 2, 2 },
			scaleAeolianHarmonic = { 3, 1, 2, 1, 2, 2, 1 },
			scaleLocrianHarmonic = { 1, 2, 1, 2, 2, 1, 3 },

		//Melodic modes
			scaleIonianMelodic = { 2, 1, 2, 2, 2, 2, 1 },	//AKA Melodic Minor
			scaleDorianMelodic = { 1, 2, 2, 2, 2, 2, 1 },
			scaleLydianAugmentedMelodic = { 2, 2, 2, 2, 1, 2, 1 },
			scaleLydianMelodic = { 2, 2, 2, 1, 2, 1, 2 },
			scaleMixolydianMelodic = { 2, 2, 1, 2, 1, 2, 2 },
			scaleLocrianMelodic = { 2, 1, 2, 1, 2, 2, 2 },
			scaleSuperlocrianMelodic = { 1, 2, 1, 2, 2, 2, 2 },

		//Pentatonics
			scalePentatonicMajor = { 2, 2, 3, 2, 3 },
			scalePentatonicMinor = { 3, 2, 2, 3, 2 },
			none = { };


		public static int[][] scales = {scaleIonian, scaleDorian, scalePhrygian, scaleLydian, scaleMixolydian, scaleAeolian, scaleLocrian, none,

									   scaleIonianHarmonic, scaleDorianHarmonic, scalePhrygianHarmonic, scaleLydianHarmonic,
									   scaleMixolydianHarmonic, scaleAeolianHarmonic, scaleLocrianHarmonic, none,

									   scaleIonianMelodic, scaleDorianMelodic,scaleLydianAugmentedMelodic, scaleLydianMelodic,
									   scaleMixolydianMelodic, scaleLocrianMelodic, scaleSuperlocrianMelodic, none,

									   scalePentatonicMajor,scalePentatonicMinor};

		public static int FindTone(string key)
		{
			for (int i = 0; i < tones.GetLength(0); i++)
			{
				if (key == tones[i]) return i;
			}
			return -1;
		}

		public static void StringsSetup()
		{
			for (int i = 0; i < strings.Length; i++)
			{
				//strings[i][0] = tuningList[whichTuning][i];
				strings[i][0] = tuningCreator[whichTuning].tuning[i];
			}
			for (int s = 0; s < strings.Length; s++)
			{
				for (int i = 1; i < 16; i++)
				{
					strings[s][i] = strings[s][i - 1] + 1;
					if (strings[s][i] > 11)
					{
						strings[s][i] -= 12;
					}
				}
			}

			/*for (int i = 0; i < strings[0].Length; i++)
			{
				Console.WriteLine(tones[strings[0][i]]);
			}*/
		}

		public static void ComputeScale(string key, int[] steps)
		{
			int tonePosition = 0;
			int startTone;

			//Array.Clear(currentScale, 0, currentScale.Length);
			//Array.Resize(ref MainWindow.scale, 32);
			Array.Resize(ref currentScale, steps.Length);
			startTone = FindTone(key);
			if (startTone < 0)
				return;
			if (steps.GetLength(0) != currentScale.GetLength(0))
			{
				Array.Resize(ref currentScale, steps.Length);
				startTone = FindTone(key);
				//Console.WriteLine(currentScale.Length);
				return;
			}
			tonePosition = startTone;
			for (int i = 0; i < steps.GetLength(0); i++)
			{
				Array.Resize(ref currentScale, steps.Length);
				currentScale[i] = tonePosition % tones.GetLength(0);
				tonePosition += steps[i];
			}
			//WriteScale(currentScale);
		}

		public static void WriteScale(int[] scale)
		{
			foreach (int i in scale)
			{
				Console.WriteLine(tones[i]);
			}
		}

		public static int IntervalFromC(String key) { return FindTone(key); }
		public static int IntervalFromC(String key, int offset) { return FindTone(key) + offset; }

		public static void Setup()
		{
			for (int i = 0; i < tuningCreator.Length; i++)
			{
				//Array.Reverse(tuningList[i]);
				Array.Reverse(tuningCreator[i].tuning);
				//tuningList[i] = tuningCreator[i].getTuning();
			}
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
			{
				if (strings[whichString][whichFret] == currentScale[i])
				{
					if (FindTone(tones[i]) == 0)
						WindowScale.noteBackgrounds[whichString][whichFret].Source = WindowScale.imgRoot;
					else
						WindowScale.noteBackgrounds[whichString][whichFret].Source = WindowScale.imgNote;


					for (int a = 0; a < chords[WindowChord.clickedChordRow].Length; a++)
					{
						if (currentScale[i] == chords[WindowChord.clickedChordRow][a] && WindowChord.clickedChord != null && WindowChord.toggle)
							WindowScale.noteBackgrounds[whichString][whichFret].Source = WindowScale.imgHighlight;
					}

					WindowScale.noteBackgrounds[whichString][whichFret].Visibility = Visibility.Visible;
					//Console.WriteLine("String " + whichString + " Fret " + strings[whichString][whichFret]);
				}
			}
		}

		public static void PlaceTones(int whichString, int whichFret, int[] scale)
		{
			if (WindowScale.noteTones[0][0] == null) return;
			for (int i = 0; i < scale.Length; i++)
			{
				if (strings[whichString][whichFret] == currentScale[i])
				{
					WindowScale.noteTones[whichString][whichFret].Source = WindowScale.imgTone[currentScale[i]];
					WindowScale.noteTones[whichString][whichFret].Visibility = Visibility.Visible;
				}
			}
		}

		public static void ClearNotes()
		{
			if (WindowScale.noteBackgrounds[0][0] == null) return;
			for (int i = 0; i < 7; i++)
			{
				for (int a = 0; a < 16; a++)
				{
					WindowScale.noteBackgrounds[i][a].Visibility = Visibility.Hidden;
					WindowScale.noteTones[i][a].Visibility = Visibility.Hidden;
				}
			}
		}

		public static void ClearChords()
		{
			//if (WindowChord.bNotes[0][0] == null) return;
			for (int i = 0; i < 7; i++)
			{
				for (int a = 0; a < chords[i].Length; a++)
				{
					//WindowChord.bNotes[i][a].Visibility = Visibility.Hidden;
					WindowChord.bNotes[i][a].Content = "";
					WindowChord.bNotes[i][a].IsEnabled = false;
				}
			}
		}

		public static void PlaceChords()
		{
			if (WindowChord.bNotes[0][0] == null) return;
			ClearChords();

			for (int i = 0; i < currentScale.Length; i++)
			{

				for (int a = 0; a < chords[i].Length; a++)
				{
					if (a * 2 + i < currentScale.Length)
						chords[i][a] = currentScale[(a * 2 + i)];
					else if (a * 2 + i < currentScale.Length * 2)
						chords[i][a] = currentScale[(a * 2 + i) - currentScale.Length];
					else
						chords[i][a] = currentScale[(a * 2 + i) - currentScale.Length * 2];

					//WindowChord.bNotes[i][a].Visibility = Visibility.Visible;
					WindowChord.bNotes[i][a].Content = tones[chords[i][a]];
					WindowChord.bNotes[i][a].IsEnabled = true;
				}
			}
		}
	}
}
