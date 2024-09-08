
namespace Labb1_CsProg_ITHS.NET;

internal class Program
{
	static void Main(string[] args)
	{
		Console.WriteLine("Hello, World!");
		Console.WriteLine("Please enter a text string to be processed:");
		string input = Console.ReadLine()!;

		var timeNow = DateTime.Now;

		var ranges = FindValidRangesInText(input);
		var sequences = GenerateSequences(input, ranges);

		long sum = 0;
		foreach(var sequence in sequences)
		{
			sequence.Print(ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White);
			sum += sequence.GetNumber();
		}

		var timeEnd = DateTime.Now;

		Console.WriteLine();
		Console.WriteLine($"The sum of all numbers in the sequences is: {sum}");
		Console.WriteLine($"Time taken: {timeEnd - timeNow}");
	}

	static Dictionary<int, int> FindValidRangesInText(string input)
	{
		Dictionary<int, int> completedSequences = new();

		Dictionary<char, int> activeSequenceStarts = new();

		for(int currentIndex = 0 ; currentIndex < input.Length ; currentIndex++)
		{
			char c = input[currentIndex];
			if(char.IsDigit(c))
			{
				if(activeSequenceStarts.TryGetValue(c, out var completingStart))
				{
					completedSequences.Add(completingStart, currentIndex-completingStart+1);
					activeSequenceStarts.Remove(c);
				}
				activeSequenceStarts.Add(c, currentIndex);
			}
			else
			{
				activeSequenceStarts.Clear();
			}
		}
		return completedSequences;
	}

	static Sequence[] GenerateSequences(string input, Dictionary<int, int> sequenceRanges)
	{
		Sequence[] sequences = new Sequence[sequenceRanges.Count];
		int sequenceCount = 0;

		for(int i = 0 ; sequenceCount < sequences.Length ; i++)
		{
			if(sequenceRanges.TryGetValue(i, out var length))
			{
				sequences[sequenceCount] = new Sequence(i, length, input.Length);
				sequenceCount++;
			}
		}

		for(int inputIndex = 0 ; inputIndex < input.Length ; inputIndex++)
		{
			for(int sequenceIndex = 0 ; sequenceIndex < sequences.Length ; sequenceIndex++)
			{
				sequences[sequenceIndex].Add(input[inputIndex]);
			}
		}

		return sequences;
	}

	private class Sequence
	{
		char[][] _chars = new char[3][];

		private int _currentChars;
		private int _currentIndex = 0;

		public Sequence(int start, int length, int fullLength)
		{
			if(start < 0 || length < 0 || fullLength < 0 || start + length > fullLength)
			{
				throw new ArgumentException("Invalid arguments");
			}

			_chars[0] = new char[start];
			_chars[1] = new char[length];
			_chars[2] = new char[fullLength - start - length];
			_currentChars = start == 0 ? 1 : 0;
		}

		public void Add(char c)
		{
			var arr = _chars[_currentChars];
			if(_currentIndex < arr.Length)
			{
				arr[_currentIndex] = c;
				_currentIndex++;
			}
			else
			{
				_currentChars++;
				_currentIndex = 0;
				arr = _chars[_currentChars];
				arr[_currentIndex] = c;
				_currentIndex++;
			}
		}

		public long GetNumber()
		{
			return long.Parse(_chars[1]);
		}

		public void Print(ConsoleColor color1, ConsoleColor color2, ConsoleColor color3)
		{
			Console.ForegroundColor = color1;
			Console.Write(_chars[0]);
			Console.ForegroundColor = color2;
			Console.Write(_chars[1]);
			Console.ForegroundColor = color3;
			Console.WriteLine(_chars[2]);
			Console.ResetColor();
		}

		public override string ToString()
		{
			return $"{_chars[0]}{_chars[1]}{_chars[2]}";
		}

	}
}
