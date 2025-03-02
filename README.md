## Создание базы данных

    1. Установите PostgreSQL.
    2. Создайте базу данных с именем `RegistrationDB`.
    3. Выполните скрипт `ScriptsBD/init.sql` для создания таблиц:
 ```bash
    psql -U <имя_пользователя> -d RegistrationDB -f ScriptsBD/init.sql
    ```