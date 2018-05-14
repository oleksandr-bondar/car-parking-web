# Car-parking-web
ASP.NET Core Web API додаток, який симулює роботу парковки

## REST API: Cars

-Список всіх машин (GET) 
    http://localhost:53577/api/cars/all

-Деталі по одній машині (GET) 
    http://localhost:53577/api/cars/id
    [id - порядковий номер машини на парковці]

-Видалити машину (DELETE) 
    http://localhost:53577/api/cars/delete/id
    [id - порядковий номер машини на парковці]

-Додати машину (POST) 
    http://localhost:53577/api/cars/add
    http://localhost:53577/api/cars/add/type;balance
    [type - приймає наступні значення: 1, 2, 3, 4, Passenger, Truck, Bus, Motorcycle]
    [balance - приймає значення в діапазоні від 50 до 1000]

-Додати певне число машин (POST) 
    http://localhost:53577/api/cars/addrange/count
    [count - кількість машин для додавання на парковку]

## REST API: Parking

-Кількість вільних місць (GET) 
    http://localhost:53577/api/parking/free

-Кількість зайнятих місць (GET) 
    http://localhost:53577/api/parking/occupied

-Загальний дохід (GET) 
    http://localhost:53577/api/parking/balance

-Загальна статистика (GET) 
    http://localhost:53577/api/parking/all

## REST API: Transactions

-Вивести Transactions.log (GET) 
    http://localhost:53577/api/transactions/log

-Вивести транзакції за останню хвилину (GET)
    http://localhost:53577/api/transactions/history

-Вивести транзакції за останню хвилину по одній конкретній машині (GET) 
    http://localhost:53577/api/transactions/history/id
    [id - порядковий номер машини на парковці]

-Поповнити баланс машини (PUT) 
    http://localhost:53577/api/transactions/recharge/id;amount
    [id - порядковий номер машини на парковці]
    [amount - приймає значення в діапазоні від 50 до 1000]
