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
  `Imagen` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`idPelicula`),
  CONSTRAINT `chk_calificacion` CHECK ((`Calificacion` between 0 and 10))
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Datos para la tabla `pelicula`
-- --------------------------------------------------------

LOCK TABLES `pelicula` WRITE;
INSERT INTO `pelicula` VALUES 
(1,'El Padrino','Francis Ford Coppola',1972,'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4',2.5,'Drama',4.2,'https://i.imgur.com/i7k6F3j.jpeg'),
(2,'Interstellar','Christopher Nolan',2014,'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4',2.49,'Ciencia ficción',3.6,'https://i.imgur.com/agFiKS5.jpeg'),
(3, 'Avatar', 'James Cameron', 2009, 'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4', 2.69, 'Ciencia ficción', 4.1, 'https://i.imgur.com/wtJWExC.jpeg'),
(4, 'El Caballero de la Noche', 'Christopher Nolan', 2008, 'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4', 2.79, 'Acción', 4.8, 'https://i.imgur.com/90opWBh.jpeg'),
(5, 'Matrix', 'Lana Wachowski, Lilly Wachowski', 1999, 'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4', 2.29, 'Acción', 4.5, 'https://i.imgur.com/pymgzHB.jpeg');
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
  `Imagen` varchar(255) DEFAULT NULL,
  PRIMARY KEY (`idserie`),
  CONSTRAINT `chk_calificacion_serie` CHECK ((`Calificacion` between 0 and 10))
) ENGINE=InnoDB AUTO_INCREMENT=6 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

DROP TABLE IF EXISTS `episodio`;
CREATE TABLE `episodio` (
  `idEpisodio` int NOT NULL AUTO_INCREMENT,
  `Nombre` varchar(100) NOT NULL,
  `Temporada` varchar(10) NOT NULL,
  `Link` varchar(255) NOT NULL,
  `idSerie` int NOT NULL,
  `DuracionMin` int DEFAULT NULL,
  PRIMARY KEY (`idEpisodio`),
  KEY `fk_episodio_serie_idx` (`idSerie`),
  CONSTRAINT `fk_episodio_serie` 
    FOREIGN KEY (`idSerie`) 
    REFERENCES `serie` (`idserie`)
    ON DELETE CASCADE
    ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Datos para la tabla `episodio`
-- --------------------------------------------------------
LOCK TABLES `episodio` WRITE;
INSERT INTO `episodio` (`Nombre`, `Temporada`, `Link`, `idSerie`, `DuracionMin`) VALUES
('Breaking Bad - Episodio 1', '1', 'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4', 1, 47),
('Stranger Things - Episodio 1', '1', 'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4', 2, 50),
('Dark - Episodio 1', '1', 'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4', 3, 26),
('El Mandaloriano - Episodio 1', '1', 'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4', 4, 24),
('Juego De Tronos - Episodio 1', '1', 'http://localhost:5050/uploads/InventarioEquipos/sdaiuhfsadiuohfoiasdhjfiojsdafiojsdoaifjdksmlfldjsaf.mp4', 5, 73);
UNLOCK TABLES;


-- --------------------------------------------------------
-- Datos para la tabla `serie`
-- --------------------------------------------------------

LOCK TABLES `serie` WRITE;
INSERT INTO `serie` VALUES 
(1,'Breaking Bad','Vince Gilligan',2008,'https://myflixerz.to/watch-tv/breaking-bad-39506.4858942',5,47,'Drama',4.5,'https://i.imgur.com/NoWb09t.jpegS'),
(2,'Stranger Things','The Duffer Brothers',2016,'https://myflixerz.to/watch-tv/stranger-things-39444.4874236',4,50,'Ciencia ficción',4.7,'https://i.imgur.com/h2cWb01.jpeg'),
(3,'Dark','Baran bo Odar, Jantje Friese',2017,'https://myflixerz.to/watch-tv/dark-38935.4970251',3,26,'Ciencia ficción',4.6,'https://i.imgur.com/k5irtvT.jpeg'),
(4,'El Mandaloriano','Jon Favreau',2019,'https://myflixerz.to/watch-tv/the-mandalorian-32386.5252932',3,24,'Acción y aventura',4.4,'https://i.imgur.com/D6OMWTn.jpeg'),
(5,'Juego De Tronos','David Benioff, D. B. Weiss',2011,'https://myflixerz.to/watch-tv/game-of-thrones-39539.4846588',8,73,'Fantasía',4.8,'https://i.imgur.com/WuHlz3T.jpeg');
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
  KEY `fk_usuario_rol_idx` (`idRol`)
) ENGINE=InnoDB AUTO_INCREMENT=9 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- --------------------------------------------------------
-- Datos para la tabla `usuario`
-- --------------------------------------------------------

LOCK TABLES `usuario` WRITE;
INSERT INTO `usuario` VALUES 
(1,'Administrador','admin@example.com','$2a$11$BbbrAzWRNqcWHtwPH78ViO6ngdkrxcRh67i1CEGHED9CZuHTy46SW',1),
(2,'Usuarionormal','user@example.com','$2a$10$yH9eL3vR7fT8wU6vX5WzR.2s3D4F5G6H7J8K9L0M1N2O3P4Q5R6S7T',2);
UNLOCK TABLES;

-- Crear tabla pelicula_log
CREATE TABLE IF NOT EXISTS `pelicula_log` (
  `idLog` INT AUTO_INCREMENT PRIMARY KEY,
  `idPelicula` INT,
  `Titulo` VARCHAR(100),
  `FechaInsertado` DATETIME
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;

-- Crear trigger
DELIMITER //

CREATE TRIGGER after_insert_pelicula
AFTER INSERT ON pelicula
FOR EACH ROW
BEGIN
  INSERT INTO pelicula_log (idPelicula, Titulo, FechaInsertado)
  VALUES (NEW.idPelicula, NEW.Titulo, NOW());
END;
//

DELIMITER ;

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