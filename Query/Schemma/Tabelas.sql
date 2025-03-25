USE dbTeste;
GO
--  EMPRESA
If Object_Id('dbo.EMPRESA') Is Not Null Drop Table dbo.EMPRESA
Go
Create Table dbo.EMPRESA 
(
     id_empresa		Int				Primary Key Identity
    ,cnpj			BigInt			Not Null Unique
    ,nome			Varchar(150)	Not Null
    ,data_criacao	DateTime		Not Null Default(GetDate())
);
Go
--  PESSOA
If Object_Id('dbo.PESSOA') Is Not Null Drop Table dbo.PESSOA
Go
Create Table dbo.PESSOA 
(
     id_pessoa		Int				Primary Key Identity
    ,id_empresa		Int				Not Null References dbo.EMPRESA(id_empresa)
    ,cpf			BigInt			Not Null Unique
    ,nome			NVarchar(150)	Not Null
    ,data_cadastro	DateTime		Not Null Default(GetDate())
);
Go
-- Garantir unicidade do CNPJ e CPF
Create Unique Index UX_EMPRESA_CNPJ On dbo.EMPRESA(cnpj);
Create Unique Index UX_PESSOA_CPF   On dbo.PESSOA(cpf);


--  CONSUMO
IF OBJECT_ID('dbo.CONSUMO') IS NOT NULL DROP TABLE dbo.CONSUMO;
GO
CREATE TABLE dbo.CONSUMO 
(
     id_consumo							INT             PRIMARY KEY IDENTITY
    ,id_pessoa_que_consumiu				INT             NOT NULL REFERENCES dbo.PESSOA(id_pessoa)
    ,data_consumo						DATETIME        NOT NULL DEFAULT(GETDATE())
    ,valor_total						DECIMAL(10,2)   NOT NULL
);
GO

--  MEMORIAL
IF OBJECT_ID('dbo.MEMORIAL') IS NOT NULL DROP TABLE dbo.MEMORIAL;
GO
CREATE TABLE dbo.MEMORIAL 
(
     id_registro_memorial				INT             PRIMARY KEY IDENTITY
    ,id_consumo							INT             NOT NULL REFERENCES dbo.CONSUMO(id_consumo)
    ,valor_total						DECIMAL(10,2)   NOT NULL
    ,valor_pontuavel					DECIMAL(10,2)   NOT NULL
    ,pontos_obtidos_na_transacao		INT             NOT NULL
    ,data_criacao_registro_memorial		DATETIME        NOT NULL DEFAULT(GETDATE())
);
GO

-- 🧹 PONTOS
IF OBJECT_ID('dbo.PONTOS') IS NOT NULL DROP TABLE dbo.PONTOS;
GO
CREATE TABLE dbo.PONTOS 
(
     id_cartela_pontos					INT             PRIMARY KEY IDENTITY
    ,id_pessoa_que_consumiu				INT             NOT NULL UNIQUE REFERENCES dbo.PESSOA(id_pessoa)
    ,saldo_de_pontos					INT             NOT NULL DEFAULT(0)
    ,data_atualizacao					DATETIME        NOT NULL DEFAULT(GETDATE())
);
Go

--🧹 TESTES

	SELECT * FROM dbo.PONTOS;
	
	SELECT * FROM dbo.MEMORIAL;

	SELECT * FROM dbo.CONSUMO WHERE id_consumo = 5;

	
	SELECT * FROM dbo.EMPRESA;

	
	SELECT * FROM dbo.PESSOA;
	/*
	Delete FROM dbo.CONSUMO WHERE id_consumo = 7;
	Delete FROM dbo.CONSUMO WHERE id_consumo = 7;
	Delete FROM dbo.CONSUMO WHERE id_consumo = 7;
	*/



