// See https://aka.ms/new-console-template for more information


// aggregate聚合函数 的 第一个参数表示聚合值，第二个参数表示当前元素

// 计算长度最长的元素
string[] fruits = { "apple", "mango", "orange", "passionfruit", "grape" };

// Aggregate 个参数：aggregate聚合函数
//  aggregate聚合函数 的参数为 aggregated value（聚合值），当前元素；第一次调用时，第一个元素作为聚合值传入
string longestName1 =  fruits.Aggregate((aggre, curr) => curr.Length > aggre.Length ? curr : aggre);

Console.WriteLine(
    "The fruit with the longest name is {0}.",
    longestName1);

// --------------------------------

// 反转 元素
string sentence = "the quick brown fox jumps over the lazy dog";

// Split the string into individual words.
string[] words = sentence.Split(' ');

// Prepend each word to the beginning of the
// new sentence to reverse the word order.
// 每个元素放到之前结果前面，即后面的元素一次放在前面，实现反转
string reversed = words.Aggregate((workingSentence, curr) =>
                                      curr + " " + workingSentence);

Console.WriteLine(reversed);

//-----------------------------

// Aggregate 三个参数：初始种子，aggregate聚合函数，最终结果处理函数
string longestName2 =
    fruits.Aggregate("banana",
                    (longest, curr) =>
                        curr.Length > longest.Length ? curr : longest,
                    // Return the final result as an upper case string.
                    fruit => fruit.ToUpper());

Console.WriteLine(
    "The fruit with the longest name is {0}.",
    longestName2);



Console.ReadLine();