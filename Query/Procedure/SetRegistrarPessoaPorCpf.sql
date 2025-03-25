Use dbTeste;
Go
Create Or Alter Procedure dbo.SetRegistrarPessoaPorCpf
	 @cpf			BigInt			= 0
	,@nome			NVarchar(150)	= Null
	,@id_empresa	Int				= 0
As
Begin
	Set Nocount On;

	Declare @id_pessoa Int;

	-- Verifica se já existe a pessoa
	Select 
		@id_pessoa = id_pessoa
	From dbo.PESSOA
	Where 
			cpf			= @cpf 
		And id_empresa	= @id_empresa;

	-- Se não existir, insere
	If @id_pessoa Is Null
	Begin
		Insert Into dbo.PESSOA 
		(
			 id_empresa
			,cpf
			,nome
			,data_cadastro
		)
		Values (
			 @id_empresa
			,@cpf
			,@nome
			,GetDate()
		);

		Set @id_pessoa = SCOPE_IDENTITY();
	End

	-- Retorna o ID
	Select id_pessoa = @id_pessoa ;
End;
Go
