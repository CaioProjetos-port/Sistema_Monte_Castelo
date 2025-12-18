-- Opcional: Limpa as tabelas antigas se existirem para recriar do zero (cuidado em produção!)
DROP TABLE IF EXISTS Festa;
DROP TABLE IF EXISTS Cliente;
DROP TABLE IF EXISTS Endereco;
DROP TABLE IF EXISTS PagamentoPrev;
DROP TABLE IF EXISTS Aniversariante;

-- 1. Tabela Endereço
CREATE TABLE Endereco (
id_endereco SERIAL PRIMARY KEY,
	cep VARCHAR(10) NOT NULL,
	logradouro VARCHAR(150),
    numero VARCHAR(10),
    complemento VARCHAR(100),
	bairro VARCHAR(100),
    cidade VARCHAR(100),

	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- 2. Tabela Cliente
CREATE TABLE Cliente (
    id_cliente SERIAL,
    cpf_cliente VARCHAR(14) NOT NULL,
    nome_cliente VARCHAR(100) NOT NULL,
    sobrenome_cliente VARCHAR(100) NOT NULL,
    celular_cliente VARCHAR(20) NOT NULL,
    email_cliente VARCHAR(150),
	id_endereco INTEGER NOT NULL,
    
    -- Constraints (Regras)
    CONSTRAINT pk_cliente PRIMARY KEY (id_cliente),
    CONSTRAINT uq_cliente_cpf UNIQUE (cpf_cliente), -- Garante que não existam 2 clientes com mesmo CPF
	
	CONSTRAINT fk_endereco FOREIGN KEY (id_endereco)
		REFERENCES Endereco (id_endereco)
		ON DELETE RESTRICT
);

-- 3. Tabela Aniversariante
CREATE TABLE Aniversariante (
    id_aniversariante SERIAL,
	cpf_aniversariante VARCHAR(14) NOT NULL,
    nome_aniversariante VARCHAR(100) NOT NULL,
    sobrenome_aniversariante VARCHAR(100) NOT NULL,
    nascimento_aniversariante DATE NOT NULL,
	
    CONSTRAINT pk_aniversariante PRIMARY KEY (id_aniversariante),
	CONSTRAINT uq_aniversariante_cpf UNIQUE (cpf_aniversariante) -- Garante que não existam 2 aniversariantes com mesmo CPF
);

-- 4. Tabela PagamentoPrev
CREATE TABLE PagamentoPrev (
    id_pagamento SERIAL,
    forma_pagamento VARCHAR(50) NOT NULL,
    entrada DECIMAL(10, 2) NOT NULL DEFAULT 0,
    parcelas INTEGER NOT NULL DEFAULT 1,
    desconto DECIMAL(10, 2) NOT NULL DEFAULT 0,
    valor_total DECIMAL(10, 2) NOT NULL,
    valor_final DECIMAL(10, 2) NOT NULL,
    
    CONSTRAINT pk_pagamentoprev PRIMARY KEY (id_pagamento)
);

-- 5. Tabela Festa
CREATE TABLE Festa (
    id_festa SERIAL,
    tipo VARCHAR(50) NOT NULL,
    tema VARCHAR(100) NOT NULL,
    pacote VARCHAR(100) NOT NULL,
    data_festa DATE NOT NULL,
    hora_festa TIME NOT NULL,
    convidados INTEGER NOT NULL,
    convidados_np INTEGER NOT NULL DEFAULT 0,
    criancas INTEGER NOT NULL DEFAULT 0,
    extras_desc TEXT,
    extras_vlr DECIMAL(10, 2),
	id_aniversariante INTEGER NOT NULL,
    id_cliente INTEGER NOT NULL,
    id_pagamento INTEGER NOT NULL,

    CONSTRAINT pk_festa PRIMARY KEY (id_festa),

	CONSTRAINT fk_festa_aniversariante FOREIGN KEY (id_aniversariante)
		REFERENCES Aniversariante (id_aniversariante)
		ON DELETE RESTRICT,
	
    CONSTRAINT fk_festa_cliente FOREIGN KEY (id_cliente)
        REFERENCES Cliente (id_cliente)
        ON DELETE RESTRICT, -- Impede deletar um cliente se ele tiver festas cadastradas
        
    CONSTRAINT fk_festa_pagamento FOREIGN KEY (id_pagamento)
        REFERENCES PagamentoPrev (id_pagamento)
);