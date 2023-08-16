namespace MyLib

module MySort =
    
    // Imperativní verze
    // Funkce napsána dle algoritmu zmíněného v předmětu ALG1
    let partition(array:int array, leftIndex:int, rightIndex:int) =
        let x = array[rightIndex]
        let mutable i = leftIndex - 1
        
        for j in leftIndex .. rightIndex - 1 do
            if array[j] <= x then
                i <- i + 1
                let tmp = array[i]
                array[i] <- array[j]
                array[j] <- tmp
        
        let tmp = array[i+1]
        array[i+1] <- array[rightIndex]
        array[rightIndex] <- tmp
        i + 1
        
    let rec quickSortImpWithIndexs(array:int array, leftIndex:int, rightIndex:int) =
        if leftIndex < rightIndex then
            let pivot = partition (array, leftIndex, rightIndex)
            quickSortImpWithIndexs(array, leftIndex, pivot - 1)
            quickSortImpWithIndexs(array, pivot, rightIndex)
            
    // Funkce, která pouze zajišťuje volání bez zadávání indexů
    let quickSortImp(array:int array) =
        let leftIndex = 0
        let rightIndex = array.Length - 1
        quickSortImpWithIndexs(array, leftIndex, rightIndex)
        
        
    // Funkcionální verze
    let rec quickSortFun(list:int list) =
        match list with
        | [] -> []
        | first::others ->
            let smallerNumbers =
                quickSortFun (List.filter (fun number -> number < first) others)
            let biggerNumbers =
                quickSortFun (List.filter (fun number -> number >= first) others)
            List.concat[smallerNumbers; [first]; biggerNumbers;]
            