USE dbTeste;
GO
CREATE OR ALTER PROCEDURE SetCreditarPontosTempoReal
    @id_consumo		INT				= 0
   ,@id_pessoa		INT				= 0
   ,@valor_total	DECIMAL(10,2)	= 0
AS
BEGIN
    SET NOCOUNT ON;

    -- Verifica se já foi processado (existe no MEMORIAL)
    IF EXISTS (SELECT 1 FROM MEMORIAL WHERE id_consumo = @id_consumo)
    BEGIN
        RAISERROR ('Esse consumo já foi processado.', 16, 1);
        RETURN;
    END

    -- Verifica se o consumo existe
    IF NOT EXISTS (
        SELECT 1 FROM CONSUMO
        WHERE 
				id_consumo				= @id_consumo 
			AND id_pessoa_que_consumiu	= @id_pessoa
    )
    BEGIN
        RAISERROR ('Consumo não encontrado para esta pessoa.', 16, 1);
        RETURN;
    END

    -- Calcula os pontos
    DECLARE @pontos_obtidos		INT				= @valor_total / 10;
	DECLARE @valor_pontuavel	DECIMAL(10,2)	= @valor_total;

    -- Atualiza ou insere pontos
    UPDATE PONTOS
    SET 
		 saldo_de_pontos	= saldo_de_pontos + @pontos_obtidos
		,data_atualizacao	= GETDATE()
    WHERE 
		id_pessoa_que_consumiu = @id_pessoa;

    IF @@ROWCOUNT = 0
    BEGIN
        INSERT INTO PONTOS 
		(
			 id_pessoa_que_consumiu
			,saldo_de_pontos
			,data_atualizacao
		)
        VALUES 
		(
			 @id_pessoa
			,@pontos_obtidos
			,GETDATE()
		);
    END

    -- Insere no MEMORIAL
    INSERT INTO MEMORIAL 
	(
		 id_consumo
		,valor_total
		,valor_pontuavel
		,pontos_obtidos_na_transacao
		,data_criacao_registro_memorial
	)
    VALUES 
	(
		 @id_consumo
		,@valor_total
		,@valor_pontuavel
		,@pontos_obtidos
		,GETDATE()
	);
END;

GO
