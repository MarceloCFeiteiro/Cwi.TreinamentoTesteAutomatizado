Funcionalidade: Criar funcionários com steps genéricos

Cenário: Criação de funcionário com sucesso
	Dado que a base de dados esteja limpa
	E que o usuário esteja autenticado
	E seja feita uma chamda do tipo 'POST' para o endpoint 'v1/employees' como o corpo da requisição
		"""
			{
				"name": "Funcionário 1",
				"email": "funcionario1@empresa.com"
			}
		"""
	Entao o código de retorno será '201'
	E o registro estará disponivel na tabela 'Employee' da base de dados
		| Id | Name            | Email                      | Active |
		| 1  | 'Funcionário 1' | 'funcionario1@empresa.com' | True   |