USE dbTeste;
GO
Create Or Alter Procedure  dbo.SetRegistrarEmpresaPorCnpj
	 @cnpj		BigInt
	,@nome		Varchar(150)
As
Begin
	Set Nocount On;

	Declare @id_empresa Int;

	-- Verifica se já existe
	Select 
		@id_empresa = id_empresa
	From dbo.EMPRESA
	Where 
		cnpj = @cnpj;

	-- Se não existir, insere
	If @id_empresa Is Null
	Begin
		Insert Into dbo.EMPRESA 
		(
			 cnpj
			,nome
			,data_criacao
		)
		Values (
			 @cnpj
			,@nome
			,GetDate()
		);

		Set @id_empresa = SCOPE_IDENTITY();
	End

	-- Retorna o ID
	Select @id_empresa As id_empresa;
End;
GO
