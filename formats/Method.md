**Text inside blockquotes is not included in format.**

### PubMethod01

`void` PubMethod01()

> Above is an example of the simplest method definition and no comments 😔.

### PubMethod02

```cs
void PubMethod02(int number)
```

- *@param* `int` ***number***, Consectetur adipiscing elit.

> Example of no return, but one parameter.

### PubMethod03

`char` PubMethod03(`string` *str*, `int` *_value*)

- *@param* `string` ***str***, Consectetur adipiscing elit.
- *@param* `int` ***_value***, Sed do eiusmod tempor incididunt
- *@returns* `char`, Dolor in reprehenderit in voluptate.

Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.Duis aute irure dolor in reprehenderit in voluptate.

> Example with with parameters and a return type.

### PubMethod04

```cs
int PubMethod04<T>(string str, T _value)
```
`int` PubMethod04<`T`>(`string` *str*, `T` *_value*)

- *@typeparam* `T`, Tempor incididunt ut labore et.
- *@param* `string` ***str***, Consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
- *@param* `T` ***_value***, Elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.
- *@returns* `int`, Adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.

Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.Duis aute irure dolor in reprehenderit in voluptate.

> Example with a type parameter, parameters, and a return type.

### PubMethod05

`char` PubMethod05(`string` *str*, `int` *_value*)

- *@param* `string` ***,str***
- *@param* `int` ***,_value***, Sed do eiusmod tempor incididunt
- *@returns* `char`

> Example of partial documentation for parameters and return type.

### PubMethod06

(`char`, `long`) PubMethod06(`string` *str*)

- *@param* `string` ***,str***
- *@returns* (`char`, `long`), Ut labore et dolore magna aliqua.Duis aute irure dolor in reprehenderit in voluptate. Adipiscing elit, sed do eiusmod tempor incididunt ut labore et.

> Example of a documented tuple return type that does not have named items.

**Not currently supporting named arguments in a return `Tuple`**.
