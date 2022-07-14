# VkPostsParserApi

Это демонстративный проект, который позволяет спарсить вашу стену в вк и 
получить количество вхождений букв в пяти последних постах.

Для работы проекта необходимо создать приложение в вк, узнать его id и защищённый ключ,
а также ввести доверенный redirect URI 
(по умолчанию ``https://localhost:7005/VkPosts/GetNumberOccurrencesLetters``). 
Далее установить в конфиге `appId` и `SecretKey`, можно воспользоваться secret manager и 
выполнить следующие команды:
```
dotnet user-secrets set "VkApi:AppId" "{AppId}"
dotnet user-secrets set "VkApi:SecretKey" "{SecretKey}"
```

Так же необходимо установить PostgreSQL, сконфигурировать ``ConnectionStrings:UserResultsDB`` и сделать миграции.

Что бы получить результат нужно запросить разрешения в вк для доступа к персональным данным 
(личной стены вк) и переадрессовать код доступа на нужный url. 
Для этого необходимо отправить подобный get-запрос:

``https://oauth.vk.com/authorize?client_id={AppId}&display=page&redirect_uri=https://localhost:7005/VkPosts/GetNumberOccurrencesLetters&scope=walls&response_type=code&v=5.131``

В случае успеха результат работы будет представлен в формате json.
