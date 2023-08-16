using Ukol02;

namespace Ukol02.UnitTests;

public class UnitTest1 {

    // Test na maximální prvek pole
    [Theory]
    [InlineData(null, null)]
    [InlineData(null)]
    [InlineData(10, 1, 2, 3, 4, 5, 5, 6, 10, -10)]
    [InlineData(0, -10, -10, -10, -10, 0)]
    [InlineData(-10, -10, -10, -10, -10)]
    public void MaxItemTest(int? expectedResult, params int[] array) {
        var result = MathLib.MaxItem(array);
        Assert.Equal(expectedResult, result);
    }

    // Test na mocninu
    [Theory]
    [InlineData(1, 0, true)]
    [InlineData(0, 0, true)]
    [InlineData(0, 5, false)]
    [InlineData(1, 1, true)]
    [InlineData(4, 2, true)]
    [InlineData(32, 2, true)]
    [InlineData(-32, -2, true)]
    [InlineData(32, -2, false)]
    [InlineData(16, -2, true)]
    [InlineData(-16, -2, false)]
    [InlineData(-15, -2, false)]
    [InlineData(15, -2, false)]
    [InlineData(5, 2, false)]
    [InlineData(1555, 15, false)]
    [InlineData(125, 5, true)]
    public void IsPowOfTest(int number, int bas, bool expectedResult) {
        bool result = MathLib.IsPowOf(number, bas);
        Assert.Equal(result, expectedResult);
    }

    // Test pro převod čísla do řetězce vyjadřující binární hodnotu čísla
    [Theory]
    [InlineData(0, "0")]
    [InlineData(1, "1")]
    [InlineData(10, "1010")]
    [InlineData(15, "1111")]
    [InlineData(1600, "11001000000")]
    [InlineData(68, "1000100")]
    public void DecToBinTest(uint number, string expectedResult) {
        string result = MathLib.DecToBib(number);
        Assert.Equal(result, expectedResult);
    }
    
    // Testy pro třídu Set
    // Test na metodu Contains
    [Theory]
    [MemberData(nameof(DataForContainsTest))]
    public void SetContainsTest(int testingItem, bool expectedResult, Set testingSet) {
        Assert.Equal(testingSet.Contains(testingItem), expectedResult);
    }
    
    // Popis dat sloužícíh pro testování metody Contains
    public static IEnumerable<object[]> DataForContainsTest =>
        new List<object[]>
        {
            new object[] { 1, true, new Set(5, 4, 2, 1) },
            new object[] { 20, false, new Set(4, 1, 2) },
            new object[] { 20, false, new Set(5, 21, 4) },
            new object[] { 21, false, new Set(5, 5, 21, 4) },
            new object[] { 1, true, new Set(1, 2) },
            new object[] {   },
        };

    // Metody pro testování přidání prvku
    // Přidání jednoho prvku do prázdné množiny
    [Theory]
    [InlineData(true, 5)]
    [InlineData(true, 3)]
    [InlineData(true, 2)]
    public void EmptySetSingleAddTest(bool expectedResult, int itemToAdd) {
        Set startingSet = new Set();
        Set resultSet = new Set(itemToAdd);
        bool success = startingSet.Add(itemToAdd);
        Assert.Equal(success && startingSet.Equals(resultSet), expectedResult);
    }
    
    // Přidání více prvků do prázdné množiny
    [Theory]
    [InlineData(true, 5, 2, 3, 1)]
    [InlineData(false, 3, 3, 2, 1)]
    [InlineData(true)]
    public void EmptySetMultipleAddTest(bool expectedResult, params int[] itemsToAdd) {
        Set startingSet = new Set();
        Set resultSet = new Set(itemsToAdd);

        bool success = true;
        foreach (var item in itemsToAdd) {
            if (!startingSet.Add(item)) {
                success = false;
            }
        }
        
        Assert.Equal(success && startingSet.Equals(resultSet), expectedResult);
    }

    // Přidání jednoho prvku do množiny, která už obsahuje prvky 
    [Theory]
    [MemberData(nameof(DataForAddTest))]
    public void SetSingleAddTest(int itemToAdd, bool expectedResult, Set startingSet, Set resultSet) {
        startingSet.Add(itemToAdd);
        bool isSetsEqual = startingSet.Equals(resultSet);
        Assert.Equal(expectedResult, isSetsEqual);
    }
    
    // Popis dat sloužícím k testování metody Add
    public static IEnumerable<object[]> DataForAddTest =>
        new List<object[]>
        {
            new object[] { 5, true, new Set(1, 2, 3, 4), new Set(1, 2, 3, 4, 5) },
            new object[] { 5, false, new Set(1, 2, 3, 4), new Set(1, 2) },
            new object[] { 5, true, new Set(), new Set(5) },
        };
    
    // Přidání více prvků do množiny, která už obsahuje prvky
    [Theory
    [MemberData(nameof(DataForMultipleAddTest))]
    public void SetMultipleAddTest(bool expectedResult, int[] itemsToAdd, Set startingSet, Set resultSet) {

        foreach (var item in itemsToAdd) {
            startingSet.Add(item);
        }
        
        bool isSetsEqual = startingSet.Equals(resultSet);
        Assert.Equal(expectedResult, isSetsEqual);
    }
    
    // Popis dat sloužících k testování metody Add, kterou použijeme opakovaně
    public static IEnumerable<object[]> DataForMultipleAddTest =>
        new List<object[]>
        {
            new object[] { true, new int[] {5, 6, 7, 8, 9, 10}, new Set(1, 2, 3, 4), new Set(1, 2, 3, 4, 5, 6, 7, 8, 9, 10) },
            new object[] { true, new int[] {9, 44}, new Set(1, 2, 3, 4), new Set(1, 2, 44, 9, 3, 4) },
            new object[] { true, new int[] {}, new Set(4), new Set(4) },
            new object[] { false, new int[] {10, 5, 18, 3}, new Set(4), new Set(4) },
        };
    
    // Metody pro odstranění prvku -> testy pouze pro jeden prvek... pro více by to fungovalo podobně
    [Theory]
    [MemberData(nameof(DataForRemoveTest))]
    public void SetRemoveTest(bool expectedResult, int newSize, int itemToRemove, Set startingSet, Set resultSet) {
        startingSet.Remove(itemToRemove);
        Assert.Equal(startingSet.Equals(resultSet), expectedResult);
        Assert.Equal(newSize == startingSet.Size(), expectedResult);
    }
    
    // Popis dat složících k testování metody Remove
    public static IEnumerable<object[]> DataForRemoveTest =>
        new List<object[]>
        {
            new object[] { true, 4, 5, new Set(2, 3, 4, 5, 6), new Set(2, 3, 4, 6) },
            new object[] { false, 5, 6, new Set(2, 3, 4, 5, 6), new Set(2, 3, 5, 6) },
            new object[] { true, 0, 5, new Set(), new Set() },
            new object[] { true, 0, 5, new Set(5), new Set() },
            new object[] { false, 3, 10, new Set(10, 4, 2), new Set(4, 2, 5) }
        };
    
    // Test na metodu, která vrací velikost množiny
    [Theory]
    [MemberData(nameof(DataForSizeTest))]
    public void SetSizeTest(bool expectedResult, int expectedSize, Set testSet) {
        Assert.Equal(expectedSize == testSet.Size(), expectedResult);
    }
    
    // Popis dat sloužících k testování metody size
    public static IEnumerable<object[]> DataForSizeTest =>
        new List<object[]>
        {
            new object[] { true, 0, new Set() },
            new object[] { true, 1, new Set(0) },
            new object[] { false, 4, new Set(0) },
            new object[] { true, 7, new Set(0, 1, 2, 3, 4, 5, 6) }
        };
    
    // Test na metodu, která vrací velikost množiny po odstranění prvku
    [Theory]
    [MemberData(nameof(DataForSizeTestAfterRemove))]
    public void SetSizeAfterRemoveTest(bool expectedResult, int expectedSize, int itemToRemove, Set testSet) {
        testSet.Remove(itemToRemove);

        Assert.Equal(expectedSize ==testSet.Size(), expectedResult);
    }
    
    // Popis dat sloižících k testu na metodu size po odebrání prvku
    public static IEnumerable<object[]> DataForSizeTestAfterRemove =>
        new List<object[]>
        {
            new object[] { true, 1, 5, new Set(10, 5) },
            new object[] { false, 5, 1, new Set() },
            new object[] { true, 0, 1, new Set() },
            new object[] { true, 3, 1, new Set(1, 2, 3, 4) },
        };
    
    // Test na metodu, která vrací velikost množiny po přidání prvku
    [Theory]
    [MemberData(nameof(DataForSizeTestAfterAdd))]
    public void SetSizeAfterAddTest(bool expectedResult, int expectedSize, int itemToAdd, Set testSet) {
        testSet.Add(itemToAdd);

        Assert.Equal(expectedSize == testSet.Size(), expectedResult);
    }
    
    // Popis dat sloužících k testu na metodu size po odebrání prvku
    public static IEnumerable<object[]> DataForSizeTestAfterAdd =>
        new List<object[]>
        {
            new object[] { true, 2, 5, new Set(10, 5) },
            new object[] { false, 3, 5, new Set(10, 5) },
            new object[] { false, 5, 1, new Set() },
            new object[] { true, 1, 1, new Set() },
            new object[] { true, 5, 0, new Set(1, 2, 3, 4) },
        };
    
    // Test pro metodu spojení
    [Theory]
    [MemberData(nameof(DataForUnionTest))]
    public void UnionTest(bool expectedResult, Set set1, Set set2, Set resultSet) {
        Set result = set1.Union(set2);
        Assert.Equal(expectedResult, resultSet.Equals(result));
    }
    
    // Popis dat sloužících k testu na metodu Union
    public static IEnumerable<object[]> DataForUnionTest =>
        new List<object[]>
        {
            new object[] { true, new Set(1, 2, 3), new Set(4, 5, 6), new Set(1, 2, 3, 4, 5, 6)},
            new object[] { true, new Set(), new Set(), new Set()},
            new object[] { true, new Set(1, 2, 3), new Set( 2, 3), new Set(1, 2, 3)},
            new object[] { true, new Set(1, 2, 3, 4, 5), new Set( ), new Set(1, 2, 3, 4, 5)},
            new object[] { false, new Set(1, 2, 3, 4, 5), new Set(7, 8), new Set(1, 2, 3, 4, 5)},
        };
    
    // Test na průnik množin
    [Theory]
    [MemberData(nameof(DataForIntersectTest))]
    public void IntersectTest(bool expectedResult, Set set1, Set set2, Set resultSet) {
        Set intersectSet = set1.Intersect(set2);
        
        Assert.Equal(expectedResult, intersectSet.Equals(resultSet));
    }
    
    // Popis dat sloužících k testu na metodu Intersect
    public static IEnumerable<object[]> DataForIntersectTest =>
        new List<object[]>
        {
            new object[] { true, new Set(1, 2, 3), new Set(4, 5, 6), new Set()},
            new object[] { true, new Set(), new Set(), new Set()},
            new object[] { true, new Set(1, 2, 3), new Set( 2, 3), new Set( 2, 3)},
            new object[] { true, new Set(1, 2, 3, 4, 5), new Set( ), new Set()},
            new object[] { false, new Set(1, 2, 3, 4, 5), new Set(3, 5), new Set(1, 2)},
        };

    // Test na rozdíl množin
    [Theory]
    [MemberData(nameof(DataForSubtractTest))]
    public void SubtractTest(bool expectedResult, Set set1, Set set2, Set resultSet) {
        Set subtractSet = set1.Subtract(set2);
        Assert.Equal(expectedResult, subtractSet.Equals(resultSet));
    }
    
    // Popis dat sloužících k testu na metodu Subtract
    public static IEnumerable<object[]> DataForSubtractTest =>
        new List<object[]>
        {
            new object[] { true, new Set(1, 2, 3), new Set(3), new Set(1, 2)},
            new object[] { false, new Set( 3), new Set(1, 2), new Set(1, 2)},
            new object[] { true, new Set(1, 2, 3), new Set(1, 2, 3), new Set()},
            new object[] { true, new Set(), new Set( ), new Set()},
            new object[] { true, new Set(1, 2, 3, 4, 5), new Set( 78, 100), new Set(1, 2, 3, 4, 5)},
            new object[] { false, new Set(1, 2, 3, 4, 5), new Set( 1, 2, 3, 4, 5), new Set(1, 2, 3, 4, 5)},
        };
    
    
    // Test na metodu IsSubset
    [Theory]
    [MemberData(nameof(DataForIsSubsetTest))]
    public void IsSubsetTest(bool expectedResult, Set set1, Set set2) {
        bool subsetResult = set1.IsSubset(set2);
        Assert.Equal(expectedResult, subsetResult);
    }
    
    // Popis dat sloužících k testu na metodu IsSubset
    public static IEnumerable<object[]> DataForIsSubsetTest =>
        new List<object[]>
        {
            new object[] { true, new Set(1, 2, 3), new Set(1, 2, 3, 4, 5)},
            new object[] { true, new Set(), new Set()},
            new object[] { true, new Set(1, 2, 3), new Set(1, 2, 3)},
            new object[] { false, new Set(1, 2, 3, 4, 5), new Set(1, 2, 3)},
            new object[] { false, new Set(1, 2, 3, 4, 5), new Set()},
        };
}