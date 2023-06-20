Feature: StockMarketSearch
As I User I Want To Search Tickers of stock market companies


Scenario: Real Time Tickers
	Given I am registred
		| name    | email                    | password |
		| dimitri | dimitri_menezes@test.com | qwerty   |		
	And I Have Entreprises registred
		| name      | code |
		| Microsoft | MSFT |
		| Tesla     | TSLA |
		| CGI       | TSE  |
	When I search for Real Time Tickers
		| code |
		| TSE  |
		| TSLA |
	Then the result is
		| code | date             | low   | high | current | open |
		| TSE  | 2023-06-07 12:00 | 208   | 209  | 208.5   | 209  |
		| TSLA | 2023-06-07 14:00 | 199.8 | 205  | 201     | 200  |
	And A search log is saved
		

Scenario: Historical Tickers
	Given I am registred
		| name    | email                    | password |
		| dimitri | dimitri_menezes@test.com | qwerty   |
	And I Have Entreprises registred
		| name      | code |
		| Microsoft | MSFT |
		| Tesla     | TSLA |
		| CGI       | TSE  |
	When I search for Historical Tickers
		| code | startDate  | endDate             |
		| MSFT | 2023-06-01 | 2023-06-06 23:59:59 |
	Then the result is
		| code | date             | low   | high | close | volume |
		| MSFT | 2023-06-06 16:00 | 122.1 | 129  | 127   | 123    |
		| MSFT | 2023-06-05 16:00 | 126   | 127  | 126.2 | 543    |
		| MSFT | 2023-06-01 16:00 | 128   | 123  | 124   | 765    |
	And A search log is saved		