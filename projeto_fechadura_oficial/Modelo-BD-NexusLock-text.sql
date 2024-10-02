-- MySQL Workbench Forward Engineering

SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

-- -----------------------------------------------------
-- Schema db_6D_fechadura
-- -----------------------------------------------------
DROP SCHEMA IF EXISTS `db_6D_fechadura`;

CREATE SCHEMA IF NOT EXISTS `db_6D_fechadura` DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci;
USE `db_6D_fechadura`;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`usuarios`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`usuarios`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`usuarios` (
  `id_funcionario` INT NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(100) NOT NULL,
  `email` VARCHAR(100) NOT NULL,
  `hash_senha` VARCHAR(256) NOT NULL,
  `codigo_pin` CHAR(4) NOT NULL,
  `tag_rfid` VARCHAR(50) NOT NULL,
  PRIMARY KEY (`id_funcionario`),
  UNIQUE INDEX `UQ_codigo_pin` (`codigo_pin` ASC),
  UNIQUE INDEX `UQ_email` (`email` ASC),
  UNIQUE INDEX `UQ_tag_rfid` (`tag_rfid` ASC)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`salas`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`salas`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`salas` (
  `id_sala` INT NOT NULL AUTO_INCREMENT,
  `nome` VARCHAR(100) NOT NULL,
  `descricao` TEXT NULL DEFAULT NULL,
  `status` BOOLEAN NOT NULL DEFAULT FALSE,
  `imagem` LONGBLOB NULL DEFAULT NULL,
  `ocupado_por_funcionario_id` INT NULL,
  PRIMARY KEY (`id_sala`),
  FOREIGN KEY (`ocupado_por_funcionario_id`) REFERENCES `usuarios`(`id_funcionario`) ON DELETE SET NULL
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`permissoes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`permissoes`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`permissoes` (
  `id_permissao` INT NOT NULL AUTO_INCREMENT,
  `chave_permissao` VARCHAR(50) NOT NULL,
  `descricao` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id_permissao`),
  UNIQUE INDEX `UQ_chave_permissao` (`chave_permissao` ASC)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`cargos`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`cargos`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`cargos` (
  `id_cargo` INT NOT NULL AUTO_INCREMENT,
  `nome_cargo` VARCHAR(50) NOT NULL,
  `descricao` TEXT NULL DEFAULT NULL,
  PRIMARY KEY (`id_cargo`),
  UNIQUE INDEX `UQ_nome_cargo` (`nome_cargo` ASC)
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`cargo_permissoes`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`cargo_permissoes`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`cargo_permissoes` (
  `id_permissao_cargo` INT NOT NULL AUTO_INCREMENT,
  `id_cargo` INT NOT NULL,
  `id_permissao` INT NOT NULL,
  PRIMARY KEY (`id_permissao_cargo`),
  FOREIGN KEY (`id_cargo`) REFERENCES `cargos`(`id_cargo`) ON DELETE CASCADE,
  FOREIGN KEY (`id_permissao`) REFERENCES `permissoes`(`id_permissao`) ON DELETE CASCADE
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`usuario_cargos`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`usuario_cargos`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`usuario_cargos` (
  `id_funcionario_cargo` INT NOT NULL AUTO_INCREMENT,
  `id_funcionario` INT NULL,
  `id_cargo` INT NULL,
  PRIMARY KEY (`id_funcionario_cargo`),
  FOREIGN KEY (`id_funcionario`) REFERENCES `usuarios`(`id_funcionario`) ON DELETE SET NULL,
  FOREIGN KEY (`id_cargo`) REFERENCES `cargos`(`id_cargo`) ON DELETE SET NULL
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`registros_de_acesso`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`registros_de_acesso`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`registros_de_acesso` (
  `id_registro` INT NOT NULL AUTO_INCREMENT,
  `id_funcionario` INT NULL DEFAULT NULL,
  `id_sala` INT NULL DEFAULT NULL,
  `tempo_acesso` DATETIME NULL DEFAULT CURRENT_TIMESTAMP,
  `acesso_concedido` BOOLEAN NULL DEFAULT NULL,
  PRIMARY KEY (`id_registro`),
  FOREIGN KEY (`id_funcionario`) REFERENCES `usuarios`(`id_funcionario`) ON DELETE CASCADE,
  FOREIGN KEY (`id_sala`) REFERENCES `salas`(`id_sala`) ON DELETE CASCADE
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`tokens_usuario`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`tokens_usuario`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`tokens_usuario` (
  `id_token` INT NOT NULL AUTO_INCREMENT,
  `id_funcionario` INT NOT NULL,
  `token` VARCHAR(1024) NOT NULL,
  `expiracao` DATETIME NOT NULL,
  `criado_em` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`id_token`),
  FOREIGN KEY (`id_funcionario`) REFERENCES `usuarios`(`id_funcionario`) ON DELETE CASCADE
)
ENGINE = InnoDB
AUTO_INCREMENT = 22
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

-- -----------------------------------------------------
-- Table `db_6D_fechadura`.`usuario_sala_acesso`
-- -----------------------------------------------------
DROP TABLE IF EXISTS `db_6D_fechadura`.`usuario_sala_acesso`;

CREATE TABLE IF NOT EXISTS `db_6D_fechadura`.`usuario_sala_acesso` (
  `id_acesso` INT NOT NULL AUTO_INCREMENT,
  `id_funcionario` INT NULL,
  `id_sala` INT NULL,
  PRIMARY KEY (`id_acesso`),
  FOREIGN KEY (`id_funcionario`) REFERENCES `usuarios`(`id_funcionario`) ON DELETE SET NULL,
  FOREIGN KEY (`id_sala`) REFERENCES `salas`(`id_sala`) ON DELETE SET NULL
)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8mb4
COLLATE = utf8mb4_0900_ai_ci;

SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;