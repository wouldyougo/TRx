# TRx Algorithmic Trading
## TRx C# Algorithmic Trading and BackTesting Library
TRx C# Algorithmic Trading Library built for desktop.
TRx Trading Library has SmartCOM connect for MOEX market. 

-	TRL - public base core trading library
-	TRx - public indicators & strategies, new extentions, experiments and samples
-	TRS - private strategy
-	TLS - private TSLab indicators & strategies based on TRx

### TRx содержит 3 примера:

TRx.Strategy.Sample1
TRx.BackTest.Sample1
TRx.Program.Sample1

TRx.Strategy.Sample2
TRx.BackTest.Sample2
TRx.Program.Sample2

TRx.Strategy.Sample3
TRx.BackTest.Sample3
TRx.Program.Sample3

Sample3 - пример стратегии пересечения 2х скользящих средних.
В Strategy пишется сама стратегия 1 раз. 
Затем эта сборка без изменения используется в Program или BackTest,
где меняется только окружение в котом вызывается стратегия.
Program - боевой режим работы.
BackTest - сейчас однопроходный.
Параметры стратегии задаются в парметрах приложения.

### Порядок сборки решения, после скачивания с репозитория.
Правой кнопкой мыши на решении "Восстановить пакеты NuGet".
![Alt text](img-2016-03-16-13-25-40.png?raw=true "Восстановить пакеты NuGet")
Правой кнопкой мыши на решении "Собрать решение".
