«Аск» — цена, по которой продавец готов продать.
«Бид» — цена, которую готов заплатить покупатель финансового инструмента.

throw new NotImplementedException();
this.Id = SerialIntegerFactory.Make();

Millisecond
Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fff"));
Microseconds
Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.ffffff"));
Tick
Console.WriteLine(DateTime.Now.ToString("HH:mm:ss.fffffff"));
Console.WriteLine(DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss.fffffff"));
//Console.WriteLine(DateTime.Now.ToString("yyyyMMddHHmmssfffffff"));

var d1 = DateTime.Now;
Console.WriteLine(d1.ToString("yyyy.MM.dd HH:mm:ss.fffffff"));
d1 = d1.AddTicks(1);
Console.WriteLine(d1.ToString("yyyy.MM.dd HH:mm:ss.fffffff"));
2016.01.12 17:12:00.2385833

//пример формата даты
DateTime DateTime = new DateTime(2015, 1, 8);
System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;
string result;
result = String.Format("{0:yyyyMMdd,HHmmss}", DateTime.ToString(ci));
Console.WriteLine(result);
result = String.Format("{0:yyyyMMdd,HHmmss}", DateTime);
Console.WriteLine(result);
result = String.Format("{0}", DateTime.ToString("yyyyMMdd,HHmmss"));
Console.WriteLine(result);
result = String.Format("{0}", DateTime);
Console.WriteLine(result);

/// <summary>
/// 9 часов утра какой-то даты Date
/// </summary>
DateTime.Date.AddHours(9);

var a = new List<double>(10) { 0, 10, 20, 30, 40, 50, 60, 70, 80, 90 };
var b = a.Where(x => x < 50).ToList();
var c = a.Where(x => x < 50);
public List<double> foo(List<double> c)
{
    c[0] = 1;
    return c;
}
foo(a)
a
b
c

Microsoft выпустила предварительную версию Visual studio 2015 и .Net 4.6 для разработчиков. В новом C# 6.0 несколько новых возможностей, которые могут облегчить кодинг.

В этой статье рассмотрены новые возможности языка C# 6.0. Скачать новую VS можно по ссылке:
Microsoft Visual Studio Ultimate 2015 Preview

#### Инициализация свойств со значениями

В C# 6.0 мы можем инициализировать свойства со значениями, написав справа от них их значение. Это поможет избежать ошибки с null и пустыми значениями свойства.

Раньше:

public int Id { get; set; }
public string FirstName { get; set; }


Теперь:

public int Id { get; set; } = 1001;
public string FirstName { get; set; } = "Srinivas";


Интерполяция строк

Каждый день нам приходится сталкиваться с конкатенацией строк. Кто-то в основном использует оператор “+”, кто-то — метод string.Format(). Мне лично по душе string.Format(). Но проблемы с ним всем известны: при слишком большом количестве параметров тяжело понимать, что означают каждое число – {1}, {2}, {3}. В C# 6.0 придумали новую возможность, которая должна объединить достоинства обоих методов.

Раньше:

name = string.Format("Employee name is {0}, located at {1}", emp.FirstName, emp.Location); 


Теперь:

name = $"Employee name is {emp.FirstName}, located at {emp.Location}";



По просьбе трудящихся IL код


IL_0000: nop
IL_0001: ldstr "Ivan"
IL_0006: stloc.0
IL_0007: ldstr "Moscow"
IL_000c: stloc.1
IL_000d: ldstr "Employee name is {0}, located at {1}"
IL_0012: ldloc.0
IL_0013: ldloc.1
IL_0014: call string [mscorlib]System.String::Format(string, object, object)
IL_0019: stloc.2
IL_001a: ret


Так же можно использовать условия:

name = $"Employee name is {emp.FirstName}, located at {emp.Location}. Age of employee is 
{(emp.Age > 0) ? emp.Age.ToString() : "N/A"}"; 



#### Использование лямбда-выражений

В C# 6.0 свойства и методы можно определять через лямбда-выражения. Это сильно уменьшает количество кода.

Раньше:

public string[] GetCountryList()
{
   return new string[] { "Russia", "USA", "UK" };
} 


Теперь:

public string[] GetCountryList() => new string[] { "Russia", "USA", "UK" };  



Импорт статических классов

Все статические члены класса могут быть определены с помощью другого статического класса. Но нам приходится постоянно повторять имя данного статического класса. При большом количестве свойств приходится много раз повторять одно и то же.
В C# 6.0 появилась возможность импортировать с помощью ключевого слова using статические классы. Рассмотрим все на примере использования библиотеки Math:

Раньше

double powerValue = Math.Pow(2, 3);
double roundedValue = Math.Round(10.6);


Теперь:

using System.Math;
double powerValue = Pow(2, 3);
double roundedValue = Round(10.6);


Это можно использовать не только внутри класса, но и при выполнении метода:

Раньше:

var employees = listEmployees.Where(i => i.Location == "Bangalore"); 


Теперь:

using System.Linq.Enumerable;
var employees = Where(listEmployees, i => i.Location == "Bangalore");



#### Null-условный оператор

C# 6.0 вводит новый так называемый Null-условный оператор (?.), который будет работать поверх условного оператора (?:). Он призван облегчить проверку на NULL значения.
Он возвращает null значения, если объект класса, к которому применен оператор, равен null:

var emp = new Employee()
{
     Id = 1,
     Age = 30,
     Location = "Bangalore",
     Department = new Department()
     {
        Id = 1,
        Name = "IT"
      }
};



Раньше:

string location = emp == null ? null : emp.Location;
string departmentName = emp == null ? null : emp.Department == null ? null : emp.Department.Name;


Теперь:

string location = emp?.Location;
string departmentName = emp?.Department?.Name;



#### nameof оператор

В C# 6.0 оператор nameof будет использоваться, чтобы избежать появления в коде строковых литералов свойств. Этот оператор возвращает строковый литерал передаваемого в него элемента. В качестве параметра можно передать любой член класса или сам класс.

var emp = new Employee()
{
     Id = 1,
     Age = 30,
     Location = "Moscow",
     Department = new Department()
     {
        Id = 1,
        Name = "IT"
     }
}; 
Response.Write(emp.Location); //result: Moscow
Response.Write(nameof(Employee.Location)); //result: Location



Await в catch и finally блоках

До C# 6.0 нельзя было использовать в блоках catch и final оператор await. Сейчас такая возможность появилась. Ее можно будет использовать для освобождения ресурсов или для ведения логов ошибок.

public async Task StartAnalyzingData()
{
   try
   {
      // код               
   }
  catch
  {
     await LogExceptionDetailsAsync();
   }
  finally
  {
    await CloseResourcesAsync();
  }
}



#### Фильтры исключений

Фильтры исключений были в CLR, и они доступны в VB, но их не было в C#. Теперь данная возможность появилась, и можно накладывать дополнительный фильтр на исключения:

try
{               
   //Вызываем исключение
}
catch (ArgumentNullException ex) if (ex.Source == "EmployeeCreation")
{
   //Нотификация об ошибке
}
catch (InvalidOperationException ex) if (ex.InnerException != null)
{
   //Нотификация об ошибке
}
catch (Exception ex) if (ex.InnerException != null)
{
    //Сохраняем данные в лог
}



#### Инициализация Dictionary

В C# 6.0 добавлена возможность инициализации Dictionary по ключу значения. Это должно упростить инициализацию словарей.
Например, для JSON объектов:

var country = new Dictionary<int, string>
{
    [0] = "Russia",
    [1] = "USA",
    [2] = "UK",
    [3] = "Japan"
};


В C# 6.0 много синтаксических изменений и новых возможностей. Также Microsoft улучшает новый компилятор в плане производительности.

P.S. Новые возможности описаны на текущую версию компилятора, к выходу финальной версии синтаксис может измениться. 

