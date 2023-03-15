**你应该掌握的10个 JavaScript Map 方法**

[toc]

> 基本翻译自 [10 JavaScript Map Methods You Should Master Today](https://www.makeuseof.com/javascript-map-methods-to-master/)

> 此外，还有 [8 JavaScript Set Methods You Should Master Today](https://www.makeuseof.com/javascript-set-methods-to-master/) 和 [15 JavaScript Array Methods You Should Master Today](https://www.makeuseof.com/tag/javascript-array-methods/) 介绍 Set 和 Array 的介绍。

Map 是一个得到 JavaScript 良好支持的强大的数据结构，学习关于如何使用它的重要方法。

![](img/20230315120124.png)

JavaScript Map 是一个将每个元素存储为键值对的集合。该数据类型也称为关联数组或字典（`associative array or dictionary`）。

可以使用任何数据类型（基元和对象）作为键或值，Map 对象会记住插入的原始顺序，尽管通常会通过键访问值。

在本文中，你将了解今天应该掌握的十种 JavaScript Map 方法。


# 1. 在 JavaScript 中如何创建一个新 Map

使用 `Map()` 构造器可以创建一个新的 Map 对象。该构造器接受一个可迭代对象作为参数，可迭代对象的元素是键值成对的（如果有多个元素，将取前两个作为键值对）。

```js
let mapObj = new Map([
   [1966, 'Batman: The Movie'],
   [1989, 'Batman'],
   [1992, 'Batman Returns'],
   [1995, 'Batman Forever'],
   [2005, 'Batman Begins'],
   [2008, 'The Dark Knight'],
   [2012, 'The Dark Knight Rises']
]);
console.log(mapObj);
```

输出：

```js
Map(7) {
   1966 => 'Batman: The Movie',
   1989 => 'Batman',
   1992 => 'Batman Returns',
   1995 => 'Batman Forever',
   2005 => 'Batman Begins',
   2008 => 'The Dark Knight',
   2012 => 'The Dark Knight Rises'
}
```

如果不提供任何参数，JavaScript将创建一个新的空Map：

```js
let mapObj = new Map();
console.log(mapObj);

// Map(0) {}
```

如果创建Map时有重复的键，每个重复的键将使用新的值覆盖之前的值。

```js
let mapObj = new Map([
   ['key1', 'value1'],
   ['key2', 'value2'],
   ['key2', 'newValue2']
]);
console.log(mapObj);
```

输出：

```js
Map(2) {
   'key1' => 'value1',
   'key2' => 'newValue2'
}
```

此处`key2`键存储的值是`newValue2`。


你也可以创建拥有混合数据类型的键值对的 Map 对象：

```js
let mapObj = new Map([
    ["string1", 1],
    [2, "string2"],
    ["string3", 433.234],
    [23.56, 45]
]);
console.log(mapObj);
```

输出：

```js
Map(4) {
    'string1' => 1,
    2 => 'string2',
    'string3' => 433.234,
    23.56 => 45
}
```


# 2. `set()` - 向 Map 对象添加新元素或重新赋值

使用 `set()` 方法可以向 Map 对象添加一个新元素。该方法接受一个key和一个value，然后会将新元素添加到Map对象中，同时，也会返回该Map对象自身（用于支持链式调用）。

如果 key 已经存在Map中，则新值将会替换存在的值。

```js
let mapObj = new Map();
let mapObj1 = mapObj.set(1966, 'Batman: The Movie');
let mapObj2 = mapObj.set(1989, 'Batman');
mapObj.set(1992, 'Batman Returns');
mapObj.set(1995, 'Batman Forever');

console.log(mapObj);
```

输出：

```js
Map(4) {
   1966 => 'Batman: The Movie',
   1989 => 'Batman',
   1992 => 'Batman Returns',
   1995 => 'Batman Forever'
}
```

还可以使用变量或常量作为key或value：

```js
const year1 = 1966;
const movieName1 = 'Batman: The Movie';

let year2 = 1989;
var movieName2 = 'Batman';

let mapObj = new Map();
mapObj.set(year1, movieName1);
mapObj.set(year2, movieName2);

console.log(mapObj);
```

输出：

```js
Map(2) {
   1966 => 'Batman: The Movie',
   1989 => 'Batman'
}
```

`set()` 方法支持链式调用。

```js
let mapObj = new Map();
mapObj.set(1966, 'Batman: The Movie')
     .set(1989, 'Batman')
     .set(1992, 'Batman Returns')
     .set(1995, 'Batman Forever');

console.log(mapObj);
```

输出：

```js
Map(4) {
   1966 => 'Batman: The Movie',
   1989 => 'Batman',
   1992 => 'Batman Returns',
   1995 => 'Batman Forever'
}
```

# 3. `clear()` - 从一个 Map 对象移除所有元素

可以使用 `clear()` 方法从Map对象移除所有元素，该方法返回 **undefined**。

```js
let mapObj = new Map([
   [1966, 'Batman: The Movie'],
   [1989, 'Batman']
]);
console.log("Size of the Map object: " + mapObj.size);
console.log(mapObj);
 
mapObj.clear();
 
console.log("Size of the Map object after clearing elements: " + mapObj.size);
console.log(mapObj);
```

输出：

```js
Size of the Map object: 2
Map(2) { 1966 => 'Batman: The Movie', 1989 => 'Batman' }
Size of the Map object after clearing elements: 0
Map(0) {}
```

# 4. `delete()` - 从 Map 中删除一个指定元素

使用 `delete()` 方法可以从一个 Map 对象中移除指定的元素，该方法的参数为Map中key。如果key存在，返回 true；否则，返回 false。

```js
let mapObj = new Map([
   [1966, 'Batman: The Movie'],
   [1989, 'Batman']
]);
console.log("Initial Map: ");
console.log(mapObj);
 
mapObj.delete(1966);
 
console.log("Map after deleting the element having key as 1966");
console.log(mapObj);
```

输出：

```js
Initial Map:
Map(2) { 1966 => 'Batman: The Movie', 1989 => 'Batman' }
Map after deleting the element having key as 1966
Map(1) { 1989 => 'Batman' }
```

# 5. `has()` - 检查 Map 中是否存在某个元素

使用 `has()` 方法可以检查 Map 对象中是否存在某个元素，该方法接受key作为参数。如果key存在，返回 true；否则，返回false。

```js
let mapObj = new Map([
   [1966, 'Batman: The Movie'],
   [1989, 'Batman'],
   [1992, 'Batman Returns'],
   [1995, 'Batman Forever'],
   [2005, 'Batman Begins'],
   [2008, 'The Dark Knight'],
   [2012, 'The Dark Knight Rises']
]);
 
console.log(mapObj.has(1966));
console.log(mapObj.has(1977));
```

输出：

```js
true
false
```

# 6. `get()` - 通过指定键访问值

`get()` 方法从 Map 对象中返回指定元素，该方法接受key作为参数。如果key在Map对象中存在，返回元素的值；否则，返回 **undefined**

```js
let mapObj = new Map([
    [1966, 'Batman: The Movie'],
    [1989, 'Batman'],
    [1992, 'Batman Returns'],
    [1995, 'Batman Forever'],
    [2005, 'Batman Begins'],
    [2008, 'The Dark Knight'],
    [2012, 'The Dark Knight Rises']
]);
 
console.log(mapObj.get(1966));
console.log(mapObj.get(1988));
```

输出：

```js
Batman: The Movie
undefined
```

# 7. `entries()` - 通过迭代器访问 Map 的 Entries（条目/项）

根据官方 [MDN Web Docs](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Map/entries)：

> The entries() method returns a new Iterator object that contains the [key, value] pairs for each element in the Map object in insertion order. In this particular case, this iterator object is also iterable, so the for-of loop can be used. When the protocol [Symbol.iterator] is used, it returns a function that, when invoked, returns this iterator itself.

使用 `Map.entries()` 迭代器 和 `for...of` 语句可以访问 Map 的每个元素：

```js
let mapObj = new Map([
    [1966, 'Batman: The Movie'],
    [1989, 'Batman'],
    [1992, 'Batman Returns'],
    [1995, 'Batman Forever'],
    [2005, 'Batman Begins'],
    [2008, 'The Dark Knight'],
    [2012, 'The Dark Knight Rises']
]);
 
for (let entry of mapObj.entries()) {
    console.log(entry);
}
```

输出：

```js
[ 1966, 'Batman: The Movie' ]
[ 1989, 'Batman' ]
[ 1992, 'Batman Returns' ]
[ 1995, 'Batman Forever' ]
[ 2005, 'Batman Begins' ]
[ 2008, 'The Dark Knight' ]
[ 2012, 'The Dark Knight Rises' ]
```

也可以使用 ES6 解构赋值的特性访问每个 key 和 value：

```js
let mapObj = new Map([
    [1966, 'Batman: The Movie'],
    [1989, 'Batman'],
    [1992, 'Batman Returns'],
    [1995, 'Batman Forever'],
    [2005, 'Batman Begins'],
    [2008, 'The Dark Knight'],
    [2012, 'The Dark Knight Rises']
]);
 
for (let [key, value] of mapObj.entries()) {
    console.log("Key: " + key + " Value: " + value);
}
```

输出：

```js
Key: 1966 Value: Batman: The Movie
Key: 1989 Value: Batman
Key: 1992 Value: Batman Returns
Key: 1995 Value: Batman Forever
Key: 2005 Value: Batman Begins
Key: 2008 Value: The Dark Knight
Key: 2012 Value: The Dark Knight Rises
```

# 8. `values()` - 如何迭代 Map 的值

`values()` 方法返回一个包含Map所有值组成的迭代器对象，由元素插入的顺序组成。

```js
let mapObj = new Map([
    [1966, 'Batman: The Movie'],
    [1989, 'Batman'],
    [1992, 'Batman Returns']
]);
 
const iteratorObj = mapObj.values();
 
for (let value of iteratorObj) {
    console.log(value);
}
```

输出：

```js
Batman: The Movie
Batman
Batman Returns
```

# 9. `keys()` - 迭代 Map 的键

`keys()` 方法返回Map所有的键组成的迭代器对象，由元素插入的顺序组成。

```js
let mapObj = new Map([
    [1966, 'Batman: The Movie'],
    [1989, 'Batman'],
    [1992, 'Batman Returns']
]);
 
const iteratorObj = mapObj.keys();
 
for (let key of iteratorObj) {
    console.log(key);
}
```

输出：

```js
1966
1989
1992
```

# `forEach()` - 使用回调迭代Map中的元素

`forEach()` 方法为 Map 对象的每个元素调用回调函数，该回调函数有两个参数：key 和 value。

```js
function printKeyValue(key, value) {
    console.log("Key: " + key + " Value: " + value);
}

let mapObj = new Map([
    [1966, 'Batman: The Movie'],
    [1989, 'Batman'],
    [1992, 'Batman Returns']
]);

mapObj.forEach(printKeyValue);
```

输出：

```js
Key: Batman: The Movie Value: 1966
Key: Batman Value: 1989
Key: Batman Returns Value: 1992
```

# 已经了解了 JavaScript 中的 Map

现在你有足够的知识掌握JavaScript中的 Map 概念。Map 数据结构广泛用于许多编程任务。一旦掌握了它，就可以继续了解其他原生的JavaScript对象，如Sets，Arrays等。
