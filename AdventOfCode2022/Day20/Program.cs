using AdventOfCode2022.SharedKernel;
using System.Diagnostics;

namespace Day20
{
    internal class Program
    {
        class Element
        {
            public int Value { get; }
            public Element(int value)
            {
                Value = value;
            }
        }

        static void Main(string[] args)
        {
            Console.WriteLine(PuzzleOutputFormatter.getPuzzleCaption("Day 20: Grove Positioning System "));
            Console.WriteLine("encrypted file: ");
            PuzzleInput puzzleInput = new(PuzzleOutputFormatter.getPuzzleFilePath(), true);

            List<Element> elements = GetFileElements(puzzleInput.Lines);
            List<Element> rearragendElements = RearrageFileElements(elements);

            Console.WriteLine("Sum of grove coordinates: {0}", GetElementAtPositionAfterZero(rearragendElements, 1000) + GetElementAtPositionAfterZero(rearragendElements, 2000) 
                                                                    + GetElementAtPositionAfterZero(rearragendElements, 3000));
        }

        private static List<Element> RearrageFileElements(List<Element> elements)
        {
            List<Element> rearrengedList = new List<Element>(elements);

            foreach (Element element in elements)
            {
                int oldPosition = rearrengedList.IndexOf(element);
                int newPosition = GetNewPositionForElement(oldPosition, elements.Count, element.Value);
                
                if(newPosition > oldPosition)
                {
                    rearrengedList.Insert(newPosition + 1, element);
                    rearrengedList.RemoveAt(oldPosition);
                }
                else
                {
                    rearrengedList.Insert(newPosition, element);
                    rearrengedList.RemoveAt(oldPosition + 1);
                }
                
                //foreach(Element r in rearrengedList)
                //{
                //    Console.Write(r.Value + ",");
                //}
                //Console.Write("\n");
            }

            return rearrengedList;
        }

        private static int GetNewPositionForElement(int oldPosition, int elementCount, int distance)
        {
            int newPosition = oldPosition;
            if(distance > 0)
            {
                for(int step = 1; step <= distance; step++)
                {
                    newPosition++;
                    if(newPosition == elementCount)
                    {
                        newPosition= 1;
                    }
                }
            }
            else if (distance < 0)
            {
                for (int step = 1; step <= Math.Abs(distance); step++)
                {
                    newPosition--;
                    if (newPosition == 0)
                    {
                        newPosition = elementCount - 1;
                    }
                    else if (newPosition == -1)
                    {
                        newPosition = elementCount - 2;
                    }
                }
            }
            return newPosition;
        }

        private static List<Element> GetFileElements(List<string> lines)
        {
            List<Element> elements = new List<Element>();

            foreach (string line in lines)
            {
                elements.Add(new Element(Int32.Parse(line)));
            }

            return elements;
        }

        private static int GetElementAtPositionAfterZero(List<Element> rearragendElements, int positionAfterZero)
        {
            int index = rearragendElements.FindIndex(element => element.Value == 0);
            
            for(int i = 1; i <= positionAfterZero; i++)
            {
                index++;
                if(index == rearragendElements.Count)
                {
                    index = 0;
                }
            }

            return rearragendElements[index].Value;
        }
    }
}