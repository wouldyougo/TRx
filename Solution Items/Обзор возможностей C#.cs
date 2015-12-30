Обзор возможностей C# 6.0

О новых возможностях языка C# написано чуть более, чем достаточно, но мне тоже хочется внести свою лепту. Я тут пару недель хакатонил и в новом проекте активно использовался C# 6.0, поэтому появился дополнительный опыт, которым можно уже поделиться.

Итак, ниже представлены фичи языка C# 6.0 в порядке их полезности для меня на данный момент времени.
String Interpolation

Да, интерполяция строк мне показалась самой полезной возможностью.Ведь очень часто приходится формировать строки и впихивать в них дополнительные данные.Вот, например, нужно сгенерировать исключение и добавить в сообщение значение одного из аргументов:

class UserNotFoundException : Exception
{
    public UserNotFoundException(string userId)
        : base($"User '{userId}' was not found!")
    { }
}

Или же, сформировать сообщение нужно в момент генерации исключения:

public enum Operation
{
    Add,
    Remove
}

public static void ValidateOperation(Operation operation)
{
    if (operation != Operation.Add && operation != Operation.Remove)
    {
        throw new ArgumentException($"Operation '{operation}' is not supported!");
    }
}

И даже без исключений, часто бывает полезным получить строковое представление объекта:

class Person
{
    public int Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return $"Id: {Id}; Name: {Name}";
    }
}

Теперь можно нормально использовать StringBuilder, при работе с которым всегда приходилось выбирать, использовать AppendFormat, а потом AppendLine, или же вызывать AppendLine, но использовать string.Format.Теперь можно использовать AppendLine + String Interpolation:

public ICollection<Person> GetBestDevs(string source, int timeout)
{
    var stopwatch = Stopwatch.StartNew();
    ICollection<Person> devs = DoGetBestProgrammers();

    var message =
        new StringBuilder()
        .AppendLine($"Got {devs.Count} smartes devs by {stopwatch.ElapsedMilliseconds}ms.")
        .AppendLine($"Source: {source}")
        .AppendLine($"Timeout: {timeout}ms")
        .AppendLine($"Fist top 10 devs: {string.Join(", ", devs.Select(x => x.Name))}")
        .ToString();
    Console.WriteLine(message);
    return devs;
}

Да, на всякий случай, обращу внимание: внутри ‘{}’ можно использовать любые выражения, включая вызовы методов, а не только обращаться к переменным/полям/свойствам.
Улучшенные свойства

Вторая по полезности и применимости фича для меня сейчас – это набор улучшений при работе со свойствами.

Довольно часто хочется получить простой класс, с парой свойств и очень хочется впихнуть разумные значения по умолчанию.Хороший пример – это некоторые классы-конфиги с параметрами по умолчанию:

public class ElasticsearchConfiguration
{
    public const int DefaultTimeout = 42;
    public int Timeout { get; set; } = DefaultTimeout;

    public const string DefaultUrl = "http://localhost:9200";
    public string Url { get; set; } = DefaultUrl;
}

Может и мелочь, но когда таких свойств штук 7-8, то новый подход дает существенные преимущества, поскольку требует меньше усилий, а на выходе получается более короткий и простой в понимании код.

Или же нам может понадобиться простой дата-объект со свойством, например, типа List<T>.Понятно, что правильные мужики наружу списки не выставляют, но в реальности такое происходит сплошь и рядом. Тут отлично подойдут get-only авто-свойства:

public class IndexConfiguration
{
    public List<string> Aliases { get; } = new List<string>();
    public string Name { get; set; } = "Default";
    public int Timeout { get; set; } = 42;
}

Или же хочется получить вычисляемое свойство, или свойство, которое вытаскивает значение из другого объекта.В этом случае отлично помогут expression body:

public class BackendConfiguration
{
    private ElasticsearchConfiguration _configuration = new ElasticsearchConfiguration();
    public List<IndexConfiguration> Indices { get; } = new List<IndexConfiguration>();
    public int Timeout => _configuration.Timeout;
    public string Url => _configuration.Url;
    public int NumberOfIndices => Indices.Count;

    public override string ToString() => $"Url: {Url}; Timeout: {Timeout}";
}

Да, тут я сразу показал, что expression body применяется не только к свойствам, но и к методам.

Теперь, наконец-то, можно сделать полноценные get-only автосвойства, изменить которые можно будет только в конструкторе (или в месте инициализации):

public class Response
{
    public Response(string url, string result)
    {
        Url = url;
    }

    public string Url { get; }
    public string Result { get; }

    public static Response FromError(Exception e)
    { }
    public static Response FromResult(int result)
    { }
}

Все! И никаких дополнительных кастомных backing полей!
Elvis Operator(?.)

Знаменитый в узких кругах «монадический» null-coalescing operator в моем списке оказался лишь на третьем месте.Штука очень полезная, хотя и не так часто применимая, как предыдущие возможности.

Теперь, когда у вас на собеседовании спросят, как зажигать события, нужно давать такой ответ:

public event EventHandler SomethingHappened;

private void FireSomethingHappened()
{
    SomethingHappened?.Invoke(this, EventArgs.Empty);
}

Коротко, и выразительно!

Но эта возможность полезна не только для зажигания событий.Как ни крути, но null-ы приходят и уходят, и условный вызов метода, или условное получение значения по цепочке объектов происходит постоянно.Теперь, добавляем null-coalescing оператор(a.k.a. ‘??’) и получаем очень даже выразительный код:

static BackendConfiguration TryReadBackendConfiguration()
{ }

//static void Main(string []args)
static void RunSomeStuff()
{
    const int DefaultTimeout = 42;
    var timeout =
        TryReadBackendConfiguration()
            ?.Indices.FirstOrDefault(x => x.Name == "default")
            ?.Timeout ?? DefaultTimeout;
}

Тут очень важно не перегибать палку.Во-первых, если отсутствие значения таки является проблемой, то будет очень сложно сказать, почему же его не удалось получить и какой из объектов в цепочке оказался равен null. Тут поможет дебагер, который покажет, кто именно был не прав, но на продкшне, как правило, с дебагерами есть небольшие проблемы – их там нет.Во-вторых, хождение вглубь объектов зачастую говорит о нарушении закона Деметры и не слишком хорошо характеризует текущий дизайн.
Using static

Теперь, можно импортировать статические функции в текущую область видимости и вызывать их без явного указания имени типа.Это очень удобно, когда у тебя есть набор фабричных методов, но очень не хочется постоянно указывать имя фабрики:

using static Response;
 
public class Response
{
    public static Response FromError(Exception e)
    { }
    public static Response FromResult(int result)
    { }
}

public class Repository
{
    public Response GetData()
    {
        try
        {
            int result = DoGetResult();
            return FromResult(result);
        }
        catch (Exception e)
        {
            FromError(e);
        }
    }

    private int DoGetResult()
    { }
}

Тут, правда, есть как положительные, так и отрицательные моменты.

С одной стороны, код получается более читабельным, поскольку можно сосредоточиться на сути решаемой задачи и не захламлять код именами типов, в которых находятся вспомогательные методы.Теперь можно легко сваять внутренний DSL и получить весьма читаемый код.С другой стороны, мы привыкли, что отсутствие явного указания типов говорит о том, что метод находится где-то в нашем классе, что может попервой немного смущать.

Также интересная особенность происходит с рефакторингом. Он упрощается, поскольку теперь можно вынести метод за пределы класса, а реализация текущего класса останется точно такой же. Но если перенести метод в другой класс вручную, то придется добавлять using static самостоятельно, поскольку не одна тула его сама найти не сможет(в отличие от импорта простых using директив, которые могут быть добавлены автоматически R# или самой студией).
Index Initializer

Новая инициализация индексаторов полезна, но применимость будет полностью определяться типом решаемых задач.При обильной работе с тем же Json-ом, необходимость в этой возможности будет возникать часто.Помимо этой области, я этой штукой уже неоднократно пользовался при реализации разных фабрик:

private readonly Dictionary<Operation, Func<Operation, string>> _parsers =
    new Dictionary<Operation, Func<Operation, string>>()
    {
        [Operation.Add] = ProcessAdd,
        [Operation.Remove] = ProcessRemove,
    };

private static string ProcessAdd(Operation operation) { }
private static string ProcessRemove(Operation operation) { }

Остальные возможности

Что еще осталось? Да, это возможность использования await-ов в блоках catch и finally. Полезно, хотя теперь это не столько новая возможность, сколько доработка старой возможности до ума.

Еще, расширились возможности инициализаторов коллекций, теперь не обязательно, чтобы метод Add был экземплярным.Достаточно, чтобы класс реализовывал интерфейс IEnumerable, и был доступен метод Add – не важно, в этом же классе, или через метод расширения.

Ну и появились фильтры исключений.Вещь полезная, но едва ли сверх часто используемая. 



The 3 Ways a WCF RT Notification Service can use SignalR

    The WCF service uses the SignalR Persistent Connections feature to push data to clients.
    The WCF service uses one or more SignalR Hubs through a “direct reference” to the Hub on the server, calling the Hub’s “Client Methods” that invoke a method on the client as the means to push data.This is the simplest implementation of the 3 Ways.
    The WCF service uses one or more Hubs as a client of SignalR, connecting to the Hub(s) via SignalR’s HTTP endpoint “connection”.  Then, through the HTTP connection as a client, the WCF service calls the Hub’s “Server Methods” as the means to push data. 