Funcionalidade: Criar Empresa

Esquema do Cenário: Criação de uma empresa com sucesso
	Dado que a base de dados esteja limpa
	E que o usuário esteja autenticado
	E seja feita uma chamda do tipo 'POST' para o endpoint 'v1/companies' como o corpo da requisição
		"""
			{
				"name": "<Name>",
				"code": "001",
				"maxEmployeesNumber": 5
			}
		"""
	Entao o código de retorno será '201'
	E o registro estará disponivel na tabela 'Company' da base de dados
		| Id | Name     | Code  | MaxEmployeesNumber | Active |
		| 1  | '<Name>' | '001' | 5                  | True   |

	Exemplos:
		| Name      |
		| Empresa 1 |