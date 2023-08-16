using System.Globalization;

namespace Ukol02;

public class MathLib {
    // Konstanty
    private const uint BASE_ARRAY_LENGTH = 20;
    
    public MathLib(){}

    // Metoda pro nalezení největšího prvku v poli
    public static int? MaxItem(int[] array) {
        if (array == null || array.Length == 0) {
            return null;
        }
        else {
            int res = array[0];
            foreach (var number in array) {
                if (number > res) {
                    res = number;
                }
            }

            return res;
        }
    }

    // Metoda zjišťující, zda je číslo druhou odmocninou nějakého základu
    // Zatím neošetřuji vstup záporného čísla
    public static bool IsPowOf(int num, int bas) {
        // V metodě beru i případ "na nultou" -> tedy cokoli na nultou je 1
        if (num == 1 || num == bas || (bas == 0 && num == 1)) {
            return true;
        }
        else if (bas == 1 || bas == 0) {
            return false;
        }
        else {
            int pow = 1;
            int testingNumber = Math.Abs(num);

            while (pow < testingNumber) {
                pow = pow * bas;
                // Musím brát v potaz i záporné hodnoty
                if (Math.Abs(pow) == Math.Abs(num)) {
                    break;
                }
            }
            
            return pow == num;
        }
    }

    // Metoda pro převod čísla do binárního řetezce
    public static string DecToBib(uint number) {
        char[] strArray = new char[BASE_ARRAY_LENGTH];
        int index = 0;
        do {
            if (Convert.ToBoolean(number & 1)) {
                strArray[index] = '1';
            }
            else {
                strArray[index] = '0';
            }

            number = number >> 1;
            index++;
            if (index >= strArray.Length) {
                strArray = IncreaseArrayLength(strArray);
            }
        } while (number != 0);

        // Upravíme pole, tak aby bylo tak dlouhé, kolik ma znaků
        char[] almostResult = createIdealArray(strArray);

        // A pole musíme jeste otocit -> konstruktor stringu přidá na konec prázdný znak
        return new string(reverseCharArray(almostResult));
    }

    // Pomocná metoda, která zvětší velikost pole a vráti jej
    private static char[] IncreaseArrayLength(char[] array) {
        int newLength = array.Length * 2;
        char[] res = new char[newLength];

        // Překopírování prvků
        for (int index = 0; index < array.Length; index++) {
            res[index] = array[index];
        }

        return res;
    }

    // Pomocná metoda pro převrácení pole znaků
    private static char[] reverseCharArray(char[] array) {
        char[] res = new char[array.Length];
        for (int index = 0; index < array.Length; index++) {
            res[index] = array[array.Length - 1 - index];
        }

        return res;
    }

    // Pomocná metoda pro upravení délky pole
    private static char[] createIdealArray(char[] array) {
        int size = 0;
        foreach (var symbol in array) {
            if (symbol == '1' || symbol == '0') {
                size++;
            }
            else {
                break;
            }
        }
        
        char[] result = new char[size];
        for (int index = 0; index < size; index++) {
            result[index] = array[index];
        }

        return result;
    }
}