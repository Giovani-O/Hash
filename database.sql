create database hash;
use hash;

create table usuarios(
	id int primary key auto_increment,
    nome varchar(50) unique not null,
    senha char(32) not null,
    senhaSalt char(32) not null,
    descricao varchar(100) not null
);

select * from usuarios;