CREATE DATABASE IF NOT EXISTS `prueba` /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */;
USE `prueba`;

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

-- --------------------------------------------------------
-- Estructura de tabla para `pelicula`
-- --------------------------------------------------------

DROP TABLE IF EXISTS `pelicula`;
CREATE TABLE `pelicula` (
  `idPelicula` int NOT NULL AUTO_INCREMENT,
  `Titulo` varchar(45) NOT NULL,
  `Director` varchar(45) DEFAULT NULL,
  `Anio` int DEFAULT NULL,
  `Link` varchar(255) DEFAULT NULL,
  `DuracionHoras` double DEFAULT NULL,
  `Genero` varchar(45) DEFAULT NULL,
  `Calificacion` double DEFAULT 0,
  PRIMARY KEY (`idPelicula`),
  CONSTRAINT `chk_calificacion` CHECK ((`Calificacion` between 0 and 10))
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Datos para la tabla `pelicula`
-- --------------------------------------------------------

LOCK TABLES `pelicula` WRITE;
INSERT INTO `pelicula` VALUES 
(1,'El Padrino','Francis Ford Coppola',1972,'https://ejemplo.com/padrino',2.5,'Drama',4.2),
(2,'Interstellar','Christopher Nolan',2014,'https://ejemplo.com/interstellar',2.49,'Ciencia ficción',3.6);
UNLOCK TABLES;

-- --------------------------------------------------------
-- Estructura de tabla para `rol`
-- --------------------------------------------------------

DROP TABLE IF EXISTS `rol`;
CREATE TABLE `rol` (
  `idRol` int NOT NULL AUTO_INCREMENT,
  `TipoRol` varchar(45) NOT NULL,
  PRIMARY KEY (`idRol`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Datos para la tabla `rol`
-- --------------------------------------------------------

LOCK TABLES `rol` WRITE;
INSERT INTO `rol` VALUES 
(1,'Admin'),
(2,'Usuario');
UNLOCK TABLES;

-- --------------------------------------------------------
-- Estructura de tabla para `serie`
-- --------------------------------------------------------

DROP TABLE IF EXISTS `serie`;
CREATE TABLE `serie` (
  `idserie` int NOT NULL AUTO_INCREMENT,
  `Titulo` varchar(45) NOT NULL,
  `Director` varchar(45) DEFAULT NULL,
  `Anio` int DEFAULT NULL,
  `Link` varchar(255) DEFAULT NULL,
  `Temporadas` int DEFAULT 1,
  `DuracionPorCapitulo` double DEFAULT NULL,
  `Genero` varchar(45) DEFAULT NULL,
  `Calificacion` double DEFAULT 0,
  PRIMARY KEY (`idserie`),
  CONSTRAINT `chk_calificacion_serie` CHECK ((`Calificacion` between 0 and 10))
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Datos para la tabla `serie`
-- --------------------------------------------------------

LOCK TABLES `serie` WRITE;
INSERT INTO `serie` VALUES 
(1,'Breaking Bad','Vince Gilligan',2008,'https://ejemplo.com/breakingbad',5,47,'Drama',4.5),
(2,'Stranger Things','The Duffer Brothers',2016,'https://ejemplo.com/strangerthings',4,50,'Ciencia ficción',4.7);
UNLOCK TABLES;

-- --------------------------------------------------------
-- Estructura de tabla para `usuario`
-- --------------------------------------------------------

DROP TABLE IF EXISTS `usuario`;
CREATE TABLE `usuario` (
  `idusuario` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(45) NOT NULL,
  `Correo` varchar(45) NOT NULL,
  `Contrasena` varchar(100) NOT NULL,
  `idRol` int NOT NULL DEFAULT '2' COMMENT '1=admin, 2=usuario',
  PRIMARY KEY (`idusuario`),
  UNIQUE KEY `Correo_UNIQUE` (`Correo`),
  KEY `fk_usuario_rol_idx` (`idRol`),
  CONSTRAINT `fk_usuario_rol` FOREIGN KEY (`idRol`) REFERENCES `rol` (`idRol`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Datos para la tabla `usuario`
-- --------------------------------------------------------

LOCK TABLES `usuario` WRITE;
INSERT INTO `usuario` VALUES 
(3,'Administrador','admin@example.com','$2a$10$xJwL5vYb5UvD7hB6U7zZf.9v8TC5W5NZy7wqk9Q7d3bJ1cXrV6XaO',1),
(4,'Usuario Normal','user@example.com','$2a$10$yH9eL3vR7fT8wU6vX5WzR.2s3D4F5G6H7J8K9L0M1N2O3P4Q5R6S7T',2);
UNLOCK TABLES;

-- --------------------------------------------------------
-- Restaurar configuraciones
-- --------------------------------------------------------

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;
/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;