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
	And I Have Real Time Tickers registred
		| code | date             | low   | high | current | open |
		| MSFT | 2023-06-07 15:00 | 180.8 | 189  | 185     | 180  |
		| TSLA | 2023-06-07 14:00 | 199.8 | 205  | 201     | 200  |
		| TSE  | 2023-06-07 12:00 | 208   | 209  | 208.5   | 209  |
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
	And I Have Historical Tickers registred
		| code | date             | low   | high  | close | volume |
		| MSFT | 2023-06-07 16:00 | 123.8 | 130   | 124   | 200456 |
		| MSFT | 2023-06-06 16:00 | 122.1 | 129   | 127   | 123    |
		| MSFT | 2023-06-05 16:00 | 126   | 127   | 126.2 | 543    |
		| MSFT | 2023-06-01 16:00 | 128   | 123   | 124   | 765    |
		| MSFT | 2023-05-31 16:00 | 124   | 126.9 | 125   | 754    |
		| MSFT | 2023-05-30 16:00 | 122   | 130   | 127   | 645    |
		| TSLA | 2023-06-07 16:00 | 199.8 | 205   | 201   | 201    |
		| TSE  | 2023-06-07 16:00 | 208   | 209   | 208.4 | 208.4  |
	When I search for Historical Tickers
		| code | startDate  | endDate             |
		| MSFT | 2023-06-01 | 2023-06-06 23:59:59 |
	Then the result is
		| code | date             | low   | high | close | volume |
		| MSFT | 2023-06-06 16:00 | 122.1 | 129  | 127   | 123    |
		| MSFT | 2023-06-05 16:00 | 126   | 127  | 126.2 | 543    |
		| MSFT | 2023-06-01 16:00 | 128   | 123  | 124   | 765    |
	And A search log is saved		