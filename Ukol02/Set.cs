namespace Ukol02; 

public class Set {
    private const int BASE_ARRAY_LENGTH = 20;
    private int[] setArray;
    private int index;

    public Set() {
        setArray = new int[BASE_ARRAY_LENGTH];
        index = 0;
    }

    public Set(params int[] items) {
        if (ContainsDuplicitItems(items) || items.Length == 0) {
            setArray = new int[BASE_ARRAY_LENGTH];
            index = 0;
        }
        else {
            setArray = items;
            index = items.Length;
        }
    }

    public int[] GetArrayOfItems() {
        int[] result = new int[index];
        for (int i = 0; i < index; i++) {
            result[i] = setArray[i];
        }

        return result;
    }
    
    public bool Contains(int item) {
        for (int i = 0; i < index; i++) {
            if (setArray[i] == item) {
                return true;
            }
        }

        return false;
    }
    

    public bool Add(int item) {
        if (Contains(item)) {
            return false;
        }
        else {
            if (index < setArray.Length) {
                setArray[index] = item;
                index++;
                return true;
            }
            else {
                setArray = IncreaseArrayLength(setArray);
                return Add(item);
            }
        }
    }

    public bool Remove(int item) {
        if (Contains(item)) {
            int[] newArray = new int[setArray.Length];
            int newIndex = 0;
            for (int idx = 0; idx < setArray.Length; idx++) {
                if (item == setArray[idx]) {
                    continue;
                }
                else {
                    newArray[newIndex] = setArray[idx];
                    newIndex++;
                }
            }
            setArray = newArray;
            index = newIndex;
            return true;
        }
        else {
            return false;
        }
    }

    public int Size() {
        return index;
    }
    
    // Matoda pro sjednocení
    public Set Union(Set otherSet) {
        int[] array1 = GetArrayOfItems();
        int[] array2 = otherSet.GetArrayOfItems();

        Set result = new Set(array1);

        foreach (var item in array2) {
            if (!result.Contains(item)) {
                result.Add(item);
            }
        }

        return result;
    }
    
    
    // Metoda pro průnik
    public Set Intersect(Set otherSet) {
        Set result = new Set();
        
        for (int i = 0; i < index; i++) {
            int item = setArray[i];
            if (otherSet.Contains(item)) {
                result.Add(item);
            }
        }

        return result;
    }
    
    // Metoda pro rozdíl množin -> odčítám od množiny, na kterou je metoda zavolaná
    public Set Subtract(Set otherSet) {
        Set result = new Set();

        for (int i = 0; i < index; i++) {
            int item = setArray[i];
            if (!otherSet.Contains(item)) {
                result.Add(item);
            }
        }

        return result;
    }
    
    // Metoda pro zjištění, zda je množina podmnožinou jiné množiny (té dosazené v argumentu)
    public bool IsSubset(Set otherSet) {
        bool result = true;

        for (int i = 0; i < index; i++) {
            int item = setArray[i];
            if (!otherSet.Contains(item)) {
                result = false;
            }
        }

        return result;
    }
    
    // Pomocné statické metody
    private static int[] IncreaseArrayLength(int[] array) {
        int newLength = array.Length * 2;
        int[] res = new int[newLength];

        // Překopírování prvků
        for (int index = 0; index < array.Length; index++) {
            res[index] = array[index];
        }

        return res;
    }

    // Metoda zjištující, zda pole obsahuje duplicitní prvky
    private static bool ContainsDuplicitItems(int[] array) {
        for (int i = 0; i < array.Length; i++) {
            int number = array[i];
            for (int j = 0; j < array.Length; j++) {
                if (number == array[j] && i != j) {
                    return true;
                }
            }
        }

        return false;
    }
    
    // Metoda pro zjištění, zda jsou 2 množiny stejné -> obsahují stejné prvky
    public override bool Equals(object? obj) {
        // Zkontorlujeme typ
        if (obj.GetType() == typeof(Set)) {
            Set set = (Set)obj;
            
            // Musí také sedět velikosti polí
            if (Size() == set.Size()) {
                for (int i = 0; i < Size(); i++) {
                    int item = setArray[i];
                    if (!set.Contains(item)) {
                        return false;
                    }
                }

                return true;
            }
            else {
                return false;
            }

        }
        else {
            return false;
        }
    }
}