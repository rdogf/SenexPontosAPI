USE dbTeste;
GO
CREATE OR ALTER PROCEDURE ProcessarIngestaoConsumo
AS
BEGIN
    SET NOCOUNT ON;

    -- Insere apenas os registros ainda não processados
    INSERT INTO CONSUMO 
	(
         id_pessoa_que_consumiu
        ,data_consumo
        ,valor_total
    )
    SELECT
         id_pessoa_que_consumiu
        ,data_consumo
        ,valor_total
    FROM TEMP_CONSUMO
    WHERE 
		processado = 0;

    -- Atualiza os registros como processados
    UPDATE TEMP_CONSUMO
    SET 
		processado = 1
    WHERE 
		processado = 0;
END;
