-- MySQL dump 10.13  Distrib 8.0.19, for Win64 (x86_64)
--
-- Host: localhost    Database: onlineroulette
-- ------------------------------------------------------
-- Server version	8.0.19

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `bets`
--

DROP TABLE IF EXISTS `bets`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `bets` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `SpinId` int unsigned NOT NULL,
  `UserId` int unsigned NOT NULL,
  `BetAmount` decimal(12,2) NOT NULL,
  `BetString` text NOT NULL,
  `BetStatus` tinyint NOT NULL,
  `WonAmount` decimal(12,2) DEFAULT NULL,
  `JackpotAmount` decimal(12,2) DEFAULT NULL,
  `IpAddress` varchar(39) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_BetUser_idx` (`UserId`) /*!80000 INVISIBLE */,
  KEY `FK_BetSpin_idx` (`SpinId`),
  CONSTRAINT `FK_BetSpin` FOREIGN KEY (`SpinId`) REFERENCES `spins` (`Id`),
  CONSTRAINT `FK_BetUser` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `bets`
--

LOCK TABLES `bets` WRITE;
/*!40000 ALTER TABLE `bets` DISABLE KEYS */;
INSERT INTO `bets` VALUES (2,1,1,2.00,'[{\"T\":\"v\",\"I\":10,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":11,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":8,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":5,\"C\":6,\"K\":1},{\"T\":\"n\",\"I\":19,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":16,\"C\":1,\"K\":1},{\"T\":\"n\",\"I\":14,\"C\":1,\"K\":1},{\"T\":\"s\",\"I\":18,\"C\":1,\"K\":1}]',1,45.00,100.02,'::1','2020-01-21 17:31:40');
/*!40000 ALTER TABLE `bets` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `jackpot`
--

DROP TABLE IF EXISTS `jackpot`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `jackpot` (
  `Id` int NOT NULL AUTO_INCREMENT,
  `Amount` decimal(12,2) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `jackpot`
--

LOCK TABLES `jackpot` WRITE;
/*!40000 ALTER TABLE `jackpot` DISABLE KEYS */;
INSERT INTO `jackpot` VALUES (1,100.02,1,'2020-01-21 00:00:00','2020-01-21 17:31:50');
/*!40000 ALTER TABLE `jackpot` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `spins`
--

DROP TABLE IF EXISTS `spins`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `spins` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `SpinStatus` tinyint DEFAULT NULL,
  `WinningNumber` int DEFAULT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedAt` datetime DEFAULT NULL,
  PRIMARY KEY (`Id`),
  KEY `Spin` (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=5 DEFAULT CHARSET=tis620;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `spins`
--

LOCK TABLES `spins` WRITE;
/*!40000 ALTER TABLE `spins` DISABLE KEYS */;
INSERT INTO `spins` VALUES (1,2,16,'2020-01-21 15:25:12','2020-01-21 17:31:40'),(2,1,NULL,'2020-01-21 15:25:40',NULL),(3,1,NULL,'2020-01-21 15:31:35',NULL),(4,1,NULL,'2020-01-21 15:32:01',NULL);
/*!40000 ALTER TABLE `spins` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `Id` int unsigned NOT NULL AUTO_INCREMENT,
  `Email` varchar(200) NOT NULL,
  `FirstName` varchar(250) NOT NULL,
  `LastName` varchar(250) NOT NULL,
  `Password` text NOT NULL,
  `Salt` text NOT NULL,
  `Balance` decimal(12,2) DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAt` datetime NOT NULL,
  `UpdatedAt` datetime NOT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Email_UNIQUE` (`Email`),
  KEY `Email_INDEX` (`Email`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'test@test.net','გიორგი','იობაშვილი','4d98cc31fae35dcc5174d0ff2ef565d9ecb0144f811cd35a4e737076f9455036b3fd03b7dbdd4793b7c165cadffb2c85a0e9200e5fd64c9620280e2479cbb152','GeaW9i6BBN',8.00,1,'2020-01-20 14:39:00','2020-01-21 17:31:40');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2020-01-22  0:30:51
