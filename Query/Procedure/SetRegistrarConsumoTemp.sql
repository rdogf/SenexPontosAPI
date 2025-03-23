USE dbTeste;
GO
CREATE OR ALTER PROCEDURE  dbo.SetRegistrarConsumoTemp
     @id_pessoa		INT
    ,@data_consumo	DATETIME
    ,@valor_total	DECIMAL(10,2)
AS
BEGIN
    SET NOCOUNT ON;
	
    -- Validações para evitar registros inválidos
    IF @id_pessoa <= 0
    BEGIN
        RAISERROR ('O id_pessoa deve ser maior que zero.', 16, 1);
        RETURN;
    END

    IF @valor_total <= 0
    BEGIN
        RAISERROR ('O valor_total deve ser maior que zero.', 16, 1);
        RETURN;
    END

	  -- Verifica se já existe um consumo idêntico para essa pessoa na mesma data e valor
    IF EXISTS (
        Select 1 From TEMP_CONSUMO
        Where 
				id_pessoa_que_consumiu	= @id_pessoa
			AND data_consumo			= @data_consumo
			AND valor_total				= @valor_total
    )
    BEGIN
        RAISERROR ('Esse consumo já foi registrado.', 16, 1);
        RETURN;
    END


    INSERT INTO TEMP_CONSUMO 
	(
		 id_pessoa_que_consumiu
		,data_consumo
		,valor_total
	)
    VALUES 
	(
		 @id_pessoa
		,@data_consumo
		,@valor_total
	);
END;


--Select * FRom TEMP_CONSUMO