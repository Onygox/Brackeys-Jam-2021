﻿namespace ImpossibleOdds.Examples.Runnables
{
	using ImpossibleOdds.Runnables;

	public class FrameCounter : IRunnable, IFixedRunnable
	{
		private int updateCounter = 0;
		private int fixedUpdateCounter = 0;

		public int UpdateCounter
		{
			get { return updateCounter; }
		}

		public int FixedUpdateCounter
		{
			get { return fixedUpdateCounter; }
		}

		public void FixedUpdate()
		{
			fixedUpdateCounter++;
		}

		public void Update()
		{
			updateCounter++;
		}
	}
}
