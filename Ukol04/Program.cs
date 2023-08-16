using MyLib;
using Microsoft.FSharp.Collections;

class Program {
    const int SIZE = 50;

    static void Main() {
        // Vytvoření listu a pole náhodně 50 generovaných čísel
        List<int> numbersList = new List<int>();
        int[] numbersArray = new int[SIZE];
        Random generator = new Random();
    
        // Naplnění kolekcí náhodně generovanými čísly
        for(int i = 0; i < SIZE; i++) {
            int number = generator.Next(0, 1000);
            numbersList.Add(number);
            numbersArray[i] = number;
        }

        // Zavolání knihovny MySort
        MySort.quickSortImp(numbersArray);
        var sortedNumbers =  MySort.quickSortFun(ListModule.OfSeq(numbersList));
        
        // Vypsání výsledku
        Console.WriteLine("Výsledek setřízení pomocí funkce quickSortImp: ");
        foreach (var number in numbersArray) {
            Console.WriteLine(number);
        }
        
        Console.WriteLine("Výsledek setřízení pomocí funkce quickSortFun: ");
        foreach (var number in sortedNumbers) {
            Console.WriteLine(number);
        }
    }
}