---------------------------------------------------------------
-- 1. Função: Cadastrar Cliente + Endereço (Tudo de uma vez) --
---------------------------------------------------------------
CREATE OR REPLACE FUNCTION cadastrar_cliente_completo(
    -- Dados do Cliente
    p_cpf VARCHAR,
    p_nome VARCHAR,
    p_sobrenome VARCHAR,
    p_celular VARCHAR,
    p_email VARCHAR,
	
    -- Dados do Endereço
    p_cep VARCHAR,
    p_logradouro VARCHAR,
    p_numero VARCHAR,
    p_complemento VARCHAR,
    p_bairro VARCHAR,
    p_cidade VARCHAR
)
RETURNS INTEGER AS $$
DECLARE
	v_id_endereco INTEGER;
	v_id_cliente INTEGER;
BEGIN
	-- Insert do endereço, retorna seu id
	INSERT INTO Endereco (cep, logradouro, numero, complemento, bairro, cidade)
	VALUES (p_cep, p_logradouro, p_numero, p_complemento, p_bairro, p_cidade)
	RETURNING id_endereco INTO v_id_endereco;

	-- Insert do Cliente, retorna seu id
	INSERT INTO Cliente (cpf_cliente, nome_cliente, sobrenome_cliente, celular_cliente, email_cliente, id_endereco)
	VALUES (p_cpf, p_nome, p_sobrenome, p_celular, p_email, v_id_endereco)
	RETURNING id_cliente INTO v_id_cliente;

	RETURN v_id_cliente;
END;
$$ LANGUAGE plpgsql;

	

------------------------------------------
-- 2. Função: Agendar Festa + Pagamento --
------------------------------------------
CREATE OR REPLACE FUNCTION agendar_festa_completa(
	-- Vinculo
    p_cpf_cliente INTEGER,
    
	-- Dados do Aniversariante
    p_cpf_aniversariante VARCHAR,
    p_nome VARCHAR,
    p_sobrenome VARCHAR,
	p_nascimento DATE,
	
    -- Dados do Pagamento
    p_forma_pagamento VARCHAR,
    p_entrada DECIMAL,
    p_parcelas INTEGER,
    p_desconto DECIMAL,
    p_valor_total DECIMAL,
	p_valor_final DECIMAL,

    -- Dados da Festa
    p_tipo VARCHAR,
    p_tema VARCHAR,
    p_pacote VARCHAR,
    p_data_festa DATE,
    p_hora_festa TIME,
    p_convidados INTEGER,
    p_convidados_np INTEGER,
    p_criancas INTEGER,
    p_extras_desc TEXT,
    p_extras_vlr DECIMAL
)
RETURNS INTEGER AS $$
DECLARE
	v_id_pagamento INTEGER;
	v_id_aniversariante INTEGER;
	v_id_festa INTEGER;
	v_id_cliente INTEGER;
BEGIN
	-- Insert do Pagamento Previsto, retorna seu id
	INSERT INTO PagamentoPrev (forma_pagamento, entrada, parcelas, desconto, valor_total, valor_final)
	VALUES (p_forma_pagamento, p_entrada, p_parcelas, p_desconto, p_valor_total, p_valor_total)
	RETURNING id_pagamento INTO v_id_pagamento;

	INSERT INTO Aniversariante (cpf_aniversariante, nome_aniversariante, sobrenome_aniversariante, nascimento_aniversariante)
	VALUES (p_cpf_aniversariante, p_nome, p_sobrenome, p_nascimento)
	RETURNING id_aniversariante INTO v_id_aniversariante;

	SELECT id_cliente
	INTO v_id_cliente
	FROM Clientes
	WHERE cpf_cliente = p_cpf_cliente;

	IF NOT FOUND THEN
		RAISE EXCEPTION 'Cliente com CPF % não encontrado!', p_cpf_cliente;
	END IF;
	
	-- Insert da Festa Previsto, retorna seu id
	INSERT INTO Festa (tipo, tema, pacote, data_festa, hora_festa,
        convidados, convidados_np, criancas, extras_desc, extras_vlr,
        id_aniversariante, id_cliente, id_pagamento)
	VALUES (p_tipo, p_tema, p_pacote, p_data_festa, p_hora_festa,
        p_convidados, p_convidados_np, p_criancas, p_extras_desc, p_extras_vlr,
        v_id_aniversariante, v_id_cliente, v_id_pagamento)
	RETURNING id_festa INTO v_id_festa;
END;
$$ LANGUAGE plpgsql;



	