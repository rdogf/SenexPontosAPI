USE dbTeste;
GO
Create Or Alter Procedure dbo.GetPontosPorEmpresa
	 @cnpj			BigInt	= 0
	,@id_empresa	Int		= 0
As
Begin
	Set Nocount On;

	-- Soma total de pontos e valores por pessoa
	With Totais As (
		Select 
			 p.id_pessoa
			,id_empresa		= e.id_empresa
			,TotalPontos	= Sum(COALESCE(m.pontos_obtidos_na_transacao, 0))
			,TotalValor		= Sum(COALESCE(m.valor_total, 0))
			,UltimoConsumo	= Convert(DateTime,Max(c.data_consumo))
		From 
					dbo.EMPRESA		e
		INNER Join	dbo.PESSOA		p On p.id_empresa				= e.id_empresa
		INNER Join	dbo.CONSUMO		c On c.id_pessoa_que_consumiu	= p.id_pessoa
		INNER Join	dbo.MEMORIAL	m On m.id_consumo				= c.id_consumo
		Where 
				(@id_empresa	= 0 Or e.id_empresa = @id_empresa)
			And (@cnpj			= 0 Or e.cnpj		= @cnpj)
		Group By 
			p.id_pessoa, e.id_empresa
	)

	Select 
		 p.id_pessoa
		,p.nome
		,p.cpf
		,pt.saldo_de_pontos
		,pt.data_atualizacao
		,t.TotalValor
		,t.TotalPontos
		,t.UltimoConsumo
		,nome_empresa				= e.nome
	From 
				dbo.PESSOA	p
	Inner Join	Totais		t	On t.id_pessoa					= p.id_pessoa
	Inner Join dbo.EMPRESA	e	On e.id_empresa					= t.id_empresa
	Left  Join dbo.PONTOS	pt	On pt.id_pessoa_que_consumiu	= p.id_pessoa
	
End;
GO
