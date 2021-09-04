Funcionalidade: Obter todos os funcionários

Cenario: Obter os funcionários sem registro na base
	Dado que a base de dados esteja limpa
	E que o usuário esteja autenticado
	E seja feita uma chamada do tipo 'GET' para o endpoint 'v1/employees'
	Entao o código de retorno será '204'

Cenario: Obter os funcionários registrados na base versão
	Dado que a base de dados esteja limpa
	E os registros sejam inseridos na tabela 'employee' da base de dados
		| Name              | Email            | Active |
		| Funcionario teste | teste@teste.com  | True   |
		| Funcionario test2 | teste@teste2.com | True   |
		| Funcionario test3 | teste@teste3.com | False  |
	E que o usuário esteja autenticado
	E seja feita uma chamada do tipo 'GET' para o endpoint 'v1/employees'
	Entao o código de retorno será '200'
	E o conteúdo retornado será
		| Name              | Email            | Active |
		| Funcionario teste | teste@teste.com  | True   |
		| Funcionario test2 | teste@teste2.com | True   |
		| Funcionario test3 | teste@teste3.com | False  |

Cenario: Obter os funcionários registrados na base versão2
	Dado que a base de dados esteja limpa
	E os registros sejam inseridos na tabela 'employee' da base de dados
		| Id | Name              | Email            | Active |
		| 1  | Funcionario teste | teste@teste.com  | True   |
		| 2  | Funcionario test2 | teste@teste2.com | True   |
		| 3  | Funcionario test3 | teste@teste3.com | False  |
	E que o usuário esteja autenticado
	E seja feita uma chamada do tipo 'GET' para o endpoint 'v1/employees'
	Entao o código de retorno será '200'
	E o conteúdo retornado será generico
		| id | name              | email            | active |
		| 1  | Funcionario teste | teste@teste.com  | True   |
		| 2  | Funcionario test2 | teste@teste2.com | True   |
		| 3  | Funcionario test3 | teste@teste3.com | False  |