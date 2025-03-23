Use dbTeste
--==================================================================================================
--REGISTRAR EMPRESAS
-- Superbus
Exec dbo.SetRegistrarEmpresaPorCnpj
	 @cnpj = 12345678000190
	,@nome = 'Superbus Soluções de Fidelidade';

-- Varejão do Povo
Exec dbo.SetRegistrarEmpresaPorCnpj
	 @cnpj = 11222333000100
	,@nome = 'Varejão do Povo';

-- Empório Digital
Exec dbo.SetRegistrarEmpresaPorCnpj
	 @cnpj = 99887766000155
	,@nome = 'Empório Digital Ltda';

Select * From dbo.EMPRESA Order By id_empresa;


--==================================================================================================
--REGISTRAR PESSOAS
-- SUPERBUS
Declare @id_superbus Int = (Select id_empresa From dbo.EMPRESA Where cnpj = 12345678000190);

Exec dbo.SetRegistrarPessoaPorCpf @cpf = 12345678900, @nome = 'Maria Superbus', @id_empresa = @id_superbus;

-- VAREJÃO DO POVO
Declare @id_varejao Int = (Select id_empresa From dbo.EMPRESA Where cnpj = 11222333000100);

Exec dbo.SetRegistrarPessoaPorCpf @cpf = 11122233344, @nome = 'João Varejão', @id_empresa = @id_varejao;

-- EMPÓRIO DIGITAL
Declare @id_emporio Int = (Select id_empresa From dbo.EMPRESA Where cnpj = 99887766000155);

Exec dbo.SetRegistrarPessoaPorCpf @cpf = 99988877766, @nome = 'Carlos Empório', @id_empresa = @id_emporio;

Select * From dbo.PESSOA Order By id_pessoa;


--==================================================================================================
--Inserindo consumos simulados
-- Maria Superbus
Declare		 @id_pessoa Int = 0
			,@DataConsumo DateTime =  GetDate();

Set @id_pessoa = (Select id_pessoa From dbo.PESSOA Where cpf = 12345678900);

Exec dbo.SetRegistrarConsumoDireto 
	  @id_pessoa_que_consumiu	= @id_pessoa
	 ,@data_consumo				= @DataConsumo
	 ,@valor_total				= 200.00;

-- João Varejão
Set @id_pessoa = (Select id_pessoa From dbo.PESSOA Where cpf = 11122233344);
Set @DataConsumo = GetDate();

Exec dbo.SetRegistrarConsumoDireto 
	  @id_pessoa_que_consumiu	= @id_pessoa
	 ,@data_consumo				= @DataConsumo
	 ,@valor_total				= 75.50;

-- Carlos Empório
Set @id_pessoa = (Select id_pessoa From dbo.PESSOA Where cpf = 99988877766);
Set @DataConsumo = GetDate();

Exec dbo.SetRegistrarConsumoDireto 
	 @id_pessoa_que_consumiu	= @id_pessoa
	 ,@data_consumo				= @DataConsumo
	 ,@valor_total				= 143.30;


