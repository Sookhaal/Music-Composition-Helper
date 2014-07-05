using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCompositionHelper
{
	public class Tuning
	{
		public int[] tuning;
		public String name;

		public Tuning()
		{
		}

		public Tuning(int[] tuning, String name)
		{
			this.tuning = tuning;
			this.name = name;
		}

		public String getName()
		{
			return name;
		}

		public int[] getTuning()
		{
			return tuning;
		}
	}
}
