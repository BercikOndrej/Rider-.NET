open System
// Funkce pro spočítání největšího společného dělitele
let rec gcd  (number1: int) (number2: int) =
    if (number2 = 0) then number1
    else gcd number2 (number1 % number2)
    
// Fukce, která vrátí zlomek (dvojící čísel) v základním tvaru
let baseForm (a, b) =
    let commonDevider = gcd a b
    (a / commonDevider, b / commonDevider)

// Funkce na sčítání zlomků
let addFrac (numerator1, denominator1) (numerator2, denominator2) =
    let devider = gcd denominator1 denominator2
    let resultDenominator = (denominator1 * denominator2) / devider
    let first = numerator1 * (resultDenominator / denominator1)
    let second = numerator2 * (resultDenominator / denominator2)
    baseForm (first + second, resultDenominator)
    
// Funkce na odčítání zlomků
let subFrac (numerator1, denominator1) (numerator2, denominator2) =
    let devider = gcd denominator1 denominator2
    let resultDenominator = (denominator1 * denominator2) / devider
    let first = numerator1 * (resultDenominator / denominator1)
    let second = numerator2 * (resultDenominator / denominator2)
    baseForm (first - second, resultDenominator)
    
// Funkce na nasobení zlomků
let mulFrac (numerator1, denominator1) (numerator2, denominator2) =
    let first = numerator1 * numerator2
    let second = denominator1 * denominator2
    baseForm (first, second)
    
// Funkce na dělení zlomků
let divFrac (numerator1, denominator1) (numerator2, denominator2) =
    mulFrac (numerator1, denominator1) (denominator2, numerator2)

// Pomocná funkce pro výpočet faktorialu -> neošetřuji případ zavolání funkce se záporným číslem
// Vše probíhá v rozahu typu int
let rec fac number =
    match number with
    | 0 -> 1
    | 1 -> 1
    | _ -> number * fac (number - 1)
    

// Funkce, která vrátí kombinační číslo -> probíhá správně pouze do rozsahu typu int
let comb (upperNumber, bottomNumber) =
    let numerator = fac upperNumber
    let denominator = (fac bottomNumber) * fac (upperNumber - bottomNumber)
    baseForm (numerator, denominator)

// Testy
// Dělitel -> platí i pro obrácená čísla
let deviderTest1 = gcd 8 22
let deviderTest2 = gcd 22 8

// Základní tvar zlomku
let baseFormTest = baseForm(12, 6)

// Sčítání zlomků
let addTest1 = addFrac (5, 8) (9, 12)
let addTest2 = addFrac (5, 9) (4, 3)
let addTest3 = addFrac (-4, -1) (-1, 1)

// Odčítání zlomků
let subTest1 = subFrac (1, 1) (1, 1)
let subTest2 = subFrac (5, 9) (4, 3)

// Násobení zlomků
let mulTest1 = mulFrac (5, 4) (5, 4)
let mulTest2 = mulFrac (-5, 2) (-5, 2)
let mulTest3 = mulFrac (10, 2) (-1, 2)

// Dělení zlomků
let divTest1 = divFrac (-3, 1) (-3, 1)
let divTest2 = divFrac (30, 5) (3, 5)
let divTest3 = divFrac (15, 2) (6, 4)

// Faktorial
let facTest1 = fac 4
let facTest2 = fac 5
let facTest3 = fac 9

// Kombinační číslo
let combTest1 = comb (5, 5)
let combTest2 = comb (7, 4)
let combTest3 = comb (9, 3)