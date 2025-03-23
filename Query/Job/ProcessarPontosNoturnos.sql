USE dbTeste;
GO
CREATE OR ALTER Procedure dbo.ProcessarPontosNoturnos
AS
BEGIN
	SET NOCOUNT ON;

	-- Tabela temporária com consumos pendentes (ainda não estão no MEMORIAL)
	Create Table #Pendentes (
		 RowNum			Int Primary Key Identity
		,id_consumo		Int
		,id_pessoa		Int
		,valor_total	Decimal(10,2)
	);

	Insert Into #Pendentes (id_consumo, id_pessoa, valor_total)
	Select 
		 C.id_consumo
		,C.id_pessoa_que_consumiu
		,C.valor_total
	From 
		dbo.CONSUMO C
	Left Join 
		dbo.MEMORIAL M On M.id_consumo = C.id_consumo
	Where 
		M.id_consumo Is Null;

	Declare 
		 @id_consumo		Int
		,@id_pessoa			Int
		,@valor_total		Decimal(10,2)
		,@valor_pontuavel	Decimal(10,2)
		,@pontos_obtidos	Int;

	While Exists (Select 1 From #Pendentes)
	Begin
		-- Pega o próximo pendente
		Select Top 1 
			 @id_consumo	= id_consumo
			,@id_pessoa		= id_pessoa
			,@valor_total	= valor_total
		From #Pendentes
		Order By RowNum;

		-- Regra de pontuação (1 ponto a cada R$10)
		Set @valor_pontuavel = @valor_total;
		Set @pontos_obtidos  = Floor(@valor_pontuavel / 10);

		-- Insere no MEMORIAL
		Insert Into dbo.MEMORIAL (
			 id_consumo
			,valor_total
			,valor_pontuavel
			,pontos_obtidos_na_transacao
			,data_criacao_registro_memorial
		)
		Values (
			 @id_consumo
			,@valor_total
			,@valor_pontuavel
			,@pontos_obtidos
			,GetDate()
		);

		-- Atualiza ou insere na tabela de pontos
		If Exists (Select 1 From dbo.PONTOS Where id_pessoa_que_consumiu = @id_pessoa)
		Begin
			Update dbo.PONTOS
				Set saldo_de_pontos  = saldo_de_pontos + @pontos_obtidos,
					data_atualizacao = GetDate()
			Where 
				id_pessoa_que_consumiu = @id_pessoa;
		End
		Else
		Begin
			Insert Into dbo.PONTOS (
				 id_pessoa_que_consumiu
				,saldo_de_pontos
				,data_atualizacao
			)
			Values (
				 @id_pessoa
				,@pontos_obtidos
				,GetDate()
			);
		End

		-- Remove o processado da temp
		Delete From #Pendentes 
		Where RowNum = (Select Min(RowNum) From #Pendentes);
	End

	Drop Table #Pendentes;
END;
GO
