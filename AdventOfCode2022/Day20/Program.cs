using AdventOfCode2022.SharedKernel;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Day20
{
    internal class Program
    {
        class Element
        {
            public Int64 Value { get; set; }
            public Element(Int64 value)
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
            List<Element> rearragendElements = RearrangeFileElements(elements, elements);

            Console.WriteLine("Sum of grove coordinates: {0}", GetElementAtPositionAfterZero(rearragendElements, 1000) + GetElementAtPositionAfterZero(rearragendElements, 2000) 
                                                                    + GetElementAtPositionAfterZero(rearragendElements, 3000));

            foreach(Element element in elements) { element.Value *= 811589153; }
            rearragendElements = elements;
            for (int i = 1; i <= 10; i++)
            {
                rearragendElements = RearrangeFileElements(rearragendElements, elements);
            }
            Console.WriteLine("Sum of grove coordinates with decryption key: {0}", GetElementAtPositionAfterZero(rearragendElements, 1000) + GetElementAtPositionAfterZero(rearragendElements, 2000)
                                                                    + GetElementAtPositionAfterZero(rearragendElements, 3000));
        }

        private static List<Element> RearrangeFileElements(List<Element> elementsToRearrange, List<Element> originalList)
        {
            List<Element> rearrengedList = new List<Element>(elementsToRearrange);

            foreach (Element element in originalList)
            {
                Int64 oldIndex = rearrengedList.IndexOf(element);
                Int64 newIndex = (oldIndex + element.Value) % (originalList.Count - 1);

                if (newIndex < 0)
                    newIndex = originalList.Count + newIndex - 1;

                rearrengedList.Remove(element);
                rearrengedList.Insert((int)newIndex, element);
            }

            return rearrengedList;
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

        private static Int64 GetElementAtPositionAfterZero(List<Element> rearragendElements, int positionAfterZero)
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