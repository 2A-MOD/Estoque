create database if not exists Estoque;

use Estoque;

create table if not exists Produto (
    Id int auto_increment primary key,
    Nome varchar(100) not null,
    Preco decimal(6, 2) not null,
    Categoria varchar(50) default 'Geral',
    DataCadastro datetime default current_timestamp
);

create table if not exists Usuario (
    Id int auto_increment primary key,
    Nome varchar(100),
    Email varchar(100) unique,
    Senha varchar(50),
    NivelAcesso varchar(20)
);

insert into produto (Nome, Preco, Categoria) values 
('Teclado Mecânico RGB', 250.00, 'Periféricos'),
('Mouse Gamer 12000 DPI', 180.50, 'Periféricos'),
('Monitor 24 Polegadas 144Hz', 1200.00, 'Monitores'),
('Headset 7.1 Surround', 350.90, 'Áudio');

insert into usuario (Nome, Email, Senha, NivelAcesso) 
values 
('Administrador', 'admin@loja.com', '123456', 'Administrador'),
('Funcionário', 'usuario@loja.com', '123456', 'Operador');

select * from Produto;
select * from Usuario;