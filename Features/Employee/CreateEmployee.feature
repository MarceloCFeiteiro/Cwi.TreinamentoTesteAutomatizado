Funcionalidade: Cadastro de funcionários
	Sendo um usuário com as devidas permissões
	Quero poder cadastrar um novo funcionário

@Employee @CreateEmployee
Cenário: Cadastro de funcionário sem autenticação
	Dado que o usuário não esteja autenticado
	E que seja solicitado a criação de um novo funcionário
	Então o funcionário não será cadastrado
	E será retornado uma mensagem de falha de autenticação

@Employee @CreateEmployee
Cenário: Cadastro de funcionário sem preencher os campos obrigatórios
	Dado que a base de dados esteja limpa
	E que o usuário esteja autenticado
	E que seja solicitado a criação de um novo funcionário sem o preenchimento do campos obigatórios
	Então o funcionário não será cadastrado
	E será retornado uma mensagem de falha de preenchimento de campos obrigatórios

#@Employee @CreateEmployee
#Cenário: Cadastro de funcionário sem preencher os campos obrigatórios
#	Dado que a base de dados esteja limpa
#	E que o usuário esteja autenticado
#	E que seja solicitado a criação de um novo funcionário com os campos ultrpassando o limite de caracteres
#	Então o funcionário não será cadastrado
#	E será retornado uma mensagem de falha de preenchimento de campos obrigatórios

@Employee @CreateEmployee
Cenário: Cadastro de funcionário com sucesso
	Dado que a base de dados esteja limpa
	E que o usuário esteja autenticado
	E que seja solicitado a criação de um novo funcionário
	Então o funcionário será cadastrado
	E o registro estará disponivel na tabela 'Employee' da base de dados
		| Id | Name            | Email                      | Active |
		| 1  | 'Funcionário 1' | 'funcionario1@empresa.com' | True   |