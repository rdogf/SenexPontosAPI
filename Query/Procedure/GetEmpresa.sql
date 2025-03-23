USE dbTeste;
GO
Create Or Alter Procedure dbo.GetEmpresa
	 @cnpj			BigInt	= 0
	,@id_empresa	Int		= 0
As
Begin
	Set Nocount On;

	Select 
		 id_empresa
		,cnpj
		,nome
		,data_criacao
	From dbo.EMPRESA
	Where 
			(@id_empresa	= 0 Or id_empresa	= @id_empresa)
		And (@cnpj			= 0 Or cnpj			= @cnpj);
End;
Go
