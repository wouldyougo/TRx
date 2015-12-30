// Основные сведения о пустом шаблоне см. в следующей документации:
// http://go.microsoft.com/fwlink/?LinkId=232509
(function () {
    "use strict";

    WinJS.Binding.optimizeBindingReferences = true;

    var app = WinJS.Application;
    var activation = Windows.ApplicationModel.Activation;

    app.onactivated = function (args) {
        if (args.detail.kind === activation.ActivationKind.launch) {
            if (args.detail.previousExecutionState !== activation.ApplicationExecutionState.terminated) {
                // TODO: Это приложение было вновь запущено. Инициализируйте
                // приложение здесь.
            } else {
                // TODO: Это приложение вновь активировано после приостановки.
                // Восстановите состояние приложения здесь.
            }
            args.setPromise(WinJS.UI.processAll());
        }
    };

    app.oncheckpoint = function (args) {
        // TODO: Это приложение будет приостановлено. Сохраните все состояния,
        // которые необходимо сохранять во время приостановки, здесь. Можно использовать
        // объект WinJS.Application.sessionState, который автоматически
        // сохраняется и восстанавливается при приостановке. Если перед приостановкой приложения необходимо
        // выполнить асинхронную операцию, вызовите метод
        // args.setPromise().
    };

    app.start();
})();
