#Find missing references tool

Инструмент получает пути ко всем ассетам проекта, а затем рекурсивно проходит по каждому дочернему объекту ассета, проверяя каждый компонент на наличие протухших ссылок. Поиск работает в апдейте, новые найденные добавляются по мере нахождения. Нажатие на найденный объект перекидывает на родительский компонент ассета с компонентом-потеряшкой.
Выбран такой способ проверки на потерянные ссылки, потому что поверхностный поиск выдал только этот результат, а искать алгоритм побыстрее и понадёжнее для тестового нерационально.

### TODO
-------------

- Добавить имена полностью отстутствующих компонентов.
- Добавить поиск по сабкомпонентам, так как это реализовано для аниматоров.
- Осуществлять поиск в многопоточном режиме.

### Скриншоты
-------------
![](https://github.com/AfroKakTyC/MissingReferencesTool/raw/72cb55e71fb59c96c9979e4154744541c4a0c79e/Screenshots/FindMissingReferencesToolScreenshot1.png)
-------------
![](https://github.com/AfroKakTyC/MissingReferencesTool/raw/72cb55e71fb59c96c9979e4154744541c4a0c79e/Screenshots/FindMissingReferencesToolScreenshot2.png)
