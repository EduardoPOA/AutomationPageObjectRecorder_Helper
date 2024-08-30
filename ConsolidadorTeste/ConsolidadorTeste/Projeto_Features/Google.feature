Feature: Google

Scenario: Validar as nomeclaturas
	Given acesso ao site 'https://www.google.com.br/'
	When valido botao pesquisa com o nome de 'Pesquisa Google'
	Then valido tbm botao sorte com o nome de 'Estou com sorte'
