# Car-parking-web
ASP.NET Core Web API додаток, який симулює роботу парковки

## REST API: Cars

1. Список всіх машин (GET)
    *  _http://localhost:53577/api/cars/all_

2. Деталі по одній машині (GET)
    * _http://localhost:53577/api/cars/id_
        * [id - порядковий номер машини на парковці]

3. Видалити машину (DELETE) 
    * _http://localhost:53577/api/cars/delete/id_
      * [id - порядковий номер машини на парковці]

4. Додати машину (POST) 
    * _http://localhost:53577/api/cars/add_
    * _http://localhost:53577/api/cars/add/type;balance_
        * [type - приймає наступні значення: 1, 2, 3, 4, Passenger, Truck, Bus, Motorcycle]
        * [balance - приймає значення в діапазоні від 50 до 1000]

5. Додати певне число машин (POST) 
   * _http://localhost:53577/api/cars/addrange/count_
      * [count - кількість машин для додавання на парковку]

## REST API: Parking

1. Кількість вільних місць (GET) 
    * _http://localhost:53577/api/parking/free_

2. Кількість зайнятих місць (GET) 
    * _http://localhost:53577/api/parking/occupied_

3. Загальний дохід (GET) 
    * _http://localhost:53577/api/parking/balance_

4. Загальна статистика (GET) 
    * _http://localhost:53577/api/parking/all_

## REST API: Transactions

1. Вивести Transactions.log (GET) 
    * _http://localhost:53577/api/transactions/log_

2. Вивести транзакції за останню хвилину (GET)
    * _http://localhost:53577/api/transactions/history_

3. Вивести транзакції за останню хвилину по одній конкретній машині (GET) 
    * _http://localhost:53577/api/transactions/history/id_
        * [id - порядковий номер машини на парковці]

4. Поповнити баланс машини (PUT) 
    * _http://localhost:53577/api/transactions/recharge/id;amount_
        * [id - порядковий номер машини на парковці]
        * [amount - приймає значення в діапазоні від 50 до 1000]
