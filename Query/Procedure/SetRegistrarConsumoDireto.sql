USE dbTeste;
GO
CREATE OR ALTER PROCEDURE SetRegistrarConsumoDireto
     @id_pessoa_que_consumiu	INT				= 0
    ,@data_consumo				DATETIME		= Null
    ,@valor_total				DECIMAL(10,2)	= 0
AS
BEGIN
      SET NOCOUNT ON;

    -- 🔐 Verifica se o mesmo consumo já existe (mesmo cliente, valor e data)
    IF EXISTS	(
					SELECT 1
					FROM dbo.CONSUMO
					WHERE 
							id_pessoa_que_consumiu	= @id_pessoa_que_consumiu
						AND data_consumo			= @data_consumo
						AND valor_total				= @valor_total
				)
    BEGIN
        RAISERROR ('Consumo já registrado anteriormente.', 16, 1);
        RETURN;
    END

    -- ✅ Insere o consumo e retorna o ID
    INSERT INTO dbo.CONSUMO 
	(
         id_pessoa_que_consumiu
        ,data_consumo
        ,valor_total
    )
    OUTPUT INSERTED.id_consumo
    VALUES 
	(
         @id_pessoa_que_consumiu
        ,@data_consumo
        ,@valor_total
    );
END;
