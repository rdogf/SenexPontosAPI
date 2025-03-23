USE dbTeste;
GO
Create Or Alter Procedure dbo.GetPontosPorPessoa
	 @id_pessoa		Int		= 0
	,@cpf			BigInt	= 0
As
Begin
	Set Nocount On;

	SELECT 
		 P.id_pessoa
		,P.nome
		,P.cpf
		,PT.saldo_de_pontos
		,PT.data_atualizacao
		,C.id_consumo
		,C.data_consumo
		,C.valor_total
		,M.pontos_obtidos_na_transacao
		,data_criacao_registro				= M.data_criacao_registro_memorial 
		,nome_empresa						= E.nome 
	FROM		dbo.PESSOA	P
	JOIN		dbo.EMPRESA E ON E.id_empresa				= P.id_empresa
	LEFT JOIN	dbo.PONTOS PT ON PT.id_pessoa_que_consumiu	= P.id_pessoa
	LEFT JOIN	dbo.CONSUMO C ON C.id_pessoa_que_consumiu	= P.id_pessoa
	LEFT JOIN (
		SELECT 
				id_consumo
			   ,pontos_obtidos_na_transacao		= MAX(pontos_obtidos_na_transacao) 
			   ,data_criacao_registro_memorial	= MAX(data_criacao_registro_memorial) 
		FROM MEMORIAL
		GROUP BY 
			id_consumo
	) M ON M.id_consumo = C.id_consumo
	WHERE 
			(@id_pessoa		= 0 OR P.id_pessoa	= @id_pessoa)
		And  (@cpf			= 0 OR P.cpf		= @cpf)

End;
GO
