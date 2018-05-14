# Car-parking-web
ASP.NET Core Web API додаток, який симулює роботу парковки

## REST API: Cars

1. Список всіх машин (GET) 
    http://localhost:53577/api/cars/all

2. Деталі по одній машині (GET) 
    http://localhost:53577/api/cars/id
    [id - порядковий номер машини на парковці]

3. Видалити машину (DELETE) 
    http://localhost:53577/api/cars/delete/id
    [id - порядковий номер машини на парковці]

4. Додати машину (POST) 
    http://localhost:53577/api/cars/add
    http://localhost:53577/api/cars/add/type;balance
    [type - приймає наступні значення: 1, 2, 3, 4, Passenger, Truck, Bus, Motorcycle]
    [balance - приймає значення в діапазоні від 50 до 1000]

5. Додати певне число машин (POST) 
    http://localhost:53577/api/cars/addrange/count
    [count - кількість машин для додавання на парковку]

## REST API: Parking

1. Кількість вільних місць (GET) 
    http://localhost:53577/api/parking/free

2. Кількість зайнятих місць (GET) 
    http://localhost:53577/api/parking/occupied

3. Загальний дохід (GET) 
    http://localhost:53577/api/parking/balance

4. Загальна статистика (GET) 
    http://localhost:53577/api/parking/all

## REST API: Transactions

1. Вивести Transactions.log (GET) 
    http://localhost:53577/api/transactions/log

2. Вивести транзакції за останню хвилину (GET)
    http://localhost:53577/api/transactions/history

3. Вивести транзакції за останню хвилину по одній конкретній машині (GET) 
    http://localhost:53577/api/transactions/history/id
    [id - порядковий номер машини на парковці]

4. Поповнити баланс машини (PUT) 
    http://localhost:53577/api/transactions/recharge/id;amount
    [id - порядковий номер машини на парковці]
    [amount - приймає значення в діапазоні від 50 до 1000]
