-- Opcional: Limpa as tabelas antigas se existirem para recriar do zero (cuidado em produção!)
DROP TABLE IF EXISTS Festa;
DROP TABLE IF EXISTS Cliente;
DROP TABLE IF EXISTS Endereco;
DROP TABLE IF EXISTS PagamentoPrev;
DROP TABLE IF EXISTS Aniversariante;

-- 1. Tabela Endereço
CREATE TABLE Endereço (
id_endereco SERIAL PRIMARY KEY,
	cep VARCHAR(10) NOT NULL,
	logradouro VARCHAR(150),
    numumero VARCHAR(10),
    complemento VARCHAR(100),

	created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
)

-- 1. Tabela Cliente
CREATE TABLE Cliente (
    id_cliente SERIAL,
    cpf VARCHAR(14) NOT NULL,
    nome VARCHAR(100) NOT NULL,
    sobrenome VARCHAR(100) NOT NULL,
    celular VARCHAR(20) NOT NULL,
    email VARCHAR(150),
	id_endereco INTEGER NOT NULL,
    
    -- Constraints (Regras)
    CONSTRAINT pk_cliente PRIMARY KEY (id_cliente),
    CONSTRAINT uq_cliente_cpf UNIQUE (cpf) -- Garante que não existam 2 clientes com mesmo CPF
	
	CONSTRAINT fk_endereco FOREIGN KEY (id_endereco)
	REFERENCES Endereco (id_endereco)
	ON DELETE RESTRICT
);

-- 3. Tabela Aniversariante
CREATE TABLE Aniversariante (
    id_aniversariante SERIAL,
    nome VARCHAR(100) NOT NULL,
    sobrenome VARCHAR(100) NOT NULL,
    nascimento DATE NOT NULL, -- Tipo DATE é melhor que TEXT
    
    CONSTRAINT pk_aniversariante PRIMARY KEY (id_aniversariante)
);

-- 4. Tabela PagamentoPrev
CREATE TABLE PagamentoPrev (
    id_pagamento SERIAL,
    forma_pagamento VARCHAR(50) NOT NULL,
    entrada DECIMAL(10, 2) NOT NULL DEFAULT 0, -- DECIMAL é melhor para dinheiro
    parcelas INTEGER NOT NULL DEFAULT 1,
    desconto DECIMAL(10, 2) NOT NULL DEFAULT 0,
    valor_total DECIMAL(10, 2) NOT NULL,
    valor_final DECIMAL(10, 2) NOT NULL,
    
    CONSTRAINT pk_pagamentoprev PRIMARY KEY (id_pagamento)
);

-- 5. Tabela Festa
CREATE TABLE Festa (
    id_festa SERIAL,
    aniversariante VARCHAR(100) NOT NULL,
    idade INTEGER NOT NULL,
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
    
    -- Chaves Estrangeiras (Relacionamentos)
    id_cliente INTEGER NOT NULL,
    id_pagamento INTEGER NOT NULL,

    CONSTRAINT pk_festa PRIMARY KEY (id_festa),
    
    -- Constraint FK: Relaciona Festa com Cliente
    CONSTRAINT fk_festa_cliente FOREIGN KEY (id_cliente)
        REFERENCES Cliente (id_cliente)
        ON DELETE RESTRICT, -- Impede deletar um cliente se ele tiver festas cadastradas
        
    -- Constraint FK: Relaciona Festa com Pagamento
    CONSTRAINT fk_festa_pagamento FOREIGN KEY (id_pagamento)
        REFERENCES PagamentoPrev (id_pagamento)
        ON DELETE CASCADE -- Se deletar o pagamento, deleta a festa (ajuste conforme sua regra)
);