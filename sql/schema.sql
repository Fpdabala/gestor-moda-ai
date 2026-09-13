-- DDL DE CRIAÇÃO DO BANCO GESTORMODA AI

CREATE TABLE clientes (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(150) NOT NULL,
    whatsapp VARCHAR(20) NOT NULL,
    tamanho_preferido VARCHAR(10),
    estilo VARCHAR(50),
    ativo BOOLEAN DEFAULT TRUE NOT NULL,
    criado_em TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL
);

CREATE TABLE categorias (
    id SERIAL PRIMARY KEY,
    nome VARCHAR(100) NOT NULL,
    descricao VARCHAR(255)
);

CREATE TABLE pecas (
    id SERIAL PRIMARY KEY,
    categoria_id INT NOT NULL,
    nome VARCHAR(150) NOT NULL,
    tamanho VARCHAR(10) NOT NULL,
    preco_custo NUMERIC(10,2) NOT NULL,
    preco_venda NUMERIC(10,2) NOT NULL,
    status VARCHAR(30) DEFAULT 'DISPONIVEL' NOT NULL,
    ativo BOOLEAN DEFAULT TRUE NOT NULL,
    CONSTRAINT fk_pecas_categoria FOREIGN KEY (categoria_id) REFERENCES categorias(id)
);

CREATE TABLE condicionais (
    id SERIAL PRIMARY KEY,
    cliente_id INT NOT NULL,
    data_saida DATE NOT NULL,
    data_limite_devolucao DATE NOT NULL,
    status VARCHAR(30) DEFAULT 'EM_ANDAMENTO' NOT NULL,
    observacoes TEXT,
    CONSTRAINT fk_condicionais_cliente FOREIGN KEY (cliente_id) REFERENCES clientes(id)
);

CREATE TABLE itens_condicional (
    id SERIAL PRIMARY KEY,
    condicional_id INT NOT NULL,
    peca_id INT NOT NULL,
    status_item VARCHAR(30) DEFAULT 'PENDENTE' NOT NULL,
    CONSTRAINT fk_itens_condicional_parent FOREIGN KEY (condicional_id) REFERENCES condicionais(id) ON DELETE CASCADE,
    CONSTRAINT fk_itens_condicional_peca FOREIGN KEY (peca_id) REFERENCES pecas(id)
);

CREATE TABLE vendas (
    id SERIAL PRIMARY KEY,
    cliente_id INT NOT NULL,
    condicional_id INT,
    data_venda TIMESTAMP DEFAULT CURRENT_TIMESTAMP NOT NULL,
    valor_total NUMERIC(10,2) NOT NULL,
    forma_pagamento VARCHAR(50) NOT NULL,
    CONSTRAINT fk_vendas_cliente FOREIGN KEY (cliente_id) REFERENCES clientes(id),
    CONSTRAINT fk_vendas_condicional FOREIGN KEY (condicional_id) REFERENCES condicionais(id)
);

CREATE TABLE itens_venda (
    id SERIAL PRIMARY KEY,
    venda_id INT NOT NULL,
    peca_id INT NOT NULL,
    preco_unitario NUMERIC(10,2) NOT NULL,
    CONSTRAINT fk_itens_venda_parent FOREIGN KEY (venda_id) REFERENCES vendas(id) ON DELETE CASCADE,
    CONSTRAINT fk_itens_venda_peca FOREIGN KEY (peca_id) REFERENCES pecas(id)
);
