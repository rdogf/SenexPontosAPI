USE dbTeste;
GO
	Create Or Alter Procedure dbo.GetPessoa
		 @cpf			BigInt = 0
		,@id_pessoa		Int    = 0
	As
	Begin
		Set Nocount On;

		Select 
			 id_pessoa
			,id_empresa
			,cpf
			,nome
			,data_cadastro
		From 
			dbo.PESSOA
		Where 
				(@id_pessoa = 0 Or id_pessoa	= @id_pessoa)
			And (@cpf		= 0 Or cpf			= @cpf);
	End;
GO
